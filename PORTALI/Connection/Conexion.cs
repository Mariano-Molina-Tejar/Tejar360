using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Entity;

namespace Connection
{
    public class Conexion
    {
        public static ConnectionEntity ConexionDB()
        {
            try
            {
                ConnectionEntity Cnn = new ConnectionEntity
                {
                    User = System.Configuration.ConfigurationManager.AppSettings["User"].ToString(),
                    Password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString(),
                    DataBase = System.Configuration.ConfigurationManager.AppSettings["DataBase"].ToString(),
                    ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"].ToString()
                };

                return Cnn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public static string ConsumirAPI(string Url, Object objecto, string parametros)
        {
            try
            {
                using (HttpClient _httpClient = new HttpClient())
                {
                    HttpResponseMessage respuesta = new HttpResponseMessage();
                    string Server = System.Configuration.ConfigurationManager.AppSettings["APITEJAR"].ToString();
                    string apiUrl = Server + Url;

                    string usuario = System.Configuration.ConfigurationManager.AppSettings["UserApi"].ToString();
                    string contraseña = System.Configuration.ConfigurationManager.AppSettings["PassApi"].ToString();

                    var credenciales = Encoding.ASCII.GetBytes($"{usuario}:{contraseña}");
                    var encabezadoAutorizacion = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credenciales));
                    _httpClient.DefaultRequestHeaders.Authorization = encabezadoAutorizacion;

                    if (objecto != null)
                    {
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(objecto);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        respuesta = _httpClient.PostAsync(apiUrl, content).Result;
                    }
                    else
                    {
                        string par = apiUrl + parametros;
                        respuesta = _httpClient.PostAsync(par, null).Result;
                    };

                    if (respuesta.IsSuccessStatusCode)
                    {
                        var datos = respuesta.Content.ReadAsStringAsync().Result;
                        return datos;
                    }
                    else
                    {
                        return "Error al consumir el API";
                    }
                }
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }
    }
}