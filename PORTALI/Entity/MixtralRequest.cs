using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class MixtralRequest
    {
        public string model { get; set; } = "mistral-small";
        public double temperature { get; set; } = 0.2;
        public List<MixtralMessage> messages { get; set; }
    }
    public class MixtralMessage
    {
        public string role { get; set; }   // system | user
        public string content { get; set; }
    }

    public class ResultadoEvaluacionIA
    {
        public List<EvaluacionAspirante> evaluacion { get; set; }
        public string recomendacionFinal { get; set; }
    }
    public class EvaluacionAspirante
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool cumplePerfil { get; set; }
        public int score { get; set; }
        public List<string> fortalezas { get; set; }
        public List<string> debilidades { get; set; }
        public string observacion { get; set; }
    }

}
