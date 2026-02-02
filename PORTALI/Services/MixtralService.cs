using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace PORTALI.Services
{
    public class MixtralService
    {
        private const string MIXTRAL_URL = "https://api.mistral.ai/v1/chat/completions";
        private readonly string _apiKey;

        public MixtralService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> AnalizarPerfilAsync(string perfilJson,string aspiranteJson)
        {
            var requestBody = ConstruirRequest(perfilJson, aspiranteJson);

            var jsonRequest = JsonConvert.SerializeObject(requestBody);

            using (var client = new HttpClient())
            {
                // Headers
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "M7jFFXCpKeecatV6ZSMI18mNQVLfkP2L");

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(MIXTRAL_URL, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error Mixtral: {response.StatusCode} - {responseContent}");
                }

                return responseContent;
            }
        }

        private object ConstruirRequest(string perfilJson, string aspiranteJson)
        {
            const string SYSTEM_PROMPT = @"
Eres un asistente experto en reclutamiento y análisis de perfiles profesionales.

REGLAS ESTRICTAS E INQUEBRANTABLES:
- Devuelve ÚNICAMENTE un JSON válido
- NO uses Markdown
- NO uses ``` ni bloques de código
- NO agregues texto antes ni después del JSON
- NO incluyas explicaciones, comentarios ni encabezados
- NO encierres el JSON en comillas
- La respuesta DEBE comenzar EXACTAMENTE con { y terminar EXACTAMENTE con }
- Si no puedes cumplir estrictamente el formato, devuelve {}

OBJETIVO:
Evaluar una lista de aspirantes comparándolos contra un perfil de puesto.

Estructura OBLIGATORIA de salida:
{
  ""evaluacion"": [
    {
      ""UserId"": number,
      ""UserName"": string,
      ""cumplePerfil"": boolean,
      ""score"": number,
      ""fortalezas"": string[],
      ""debilidades"": string[],
      ""observacion"": string
    }
  ],
  ""recomendacionFinal"": string
}
";



            string userPrompt = $@"
Perfil del puesto (requisición):
{perfilJson}

Aspirantes al puesto:
{aspiranteJson}

Instrucciones:
- Evalúa cada aspirante contra el perfil del puesto
- Asigna un score de 0 a 100
- Indica si cumple o no el perfil
- Sé objetivo y técnico
";


            return new
            {
                model = "mistral-small",
                temperature = 0.2,
                messages = new[]
                {
                new
                {
                    role = "system",
                    content = SYSTEM_PROMPT
                },
                new
                {
                    role = "user",
                    content = userPrompt
                }
            }
            };
        }
    }

}
