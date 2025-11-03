using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Entity;

namespace PORTALI.Services
{
    public static class MailServices
    {
        public static Response EnviarCorreoElectronico(EnvioCorreoGestionEmpleados correo)
        {
            Response reply = new Response();

            MailMessage msg = new MailMessage();

            msg.Subject = correo.Asunto;
            msg.To.Add(correo.Correos); /*gerente.it@eltejar.com.gt*/ /*programador.jr@eltejar.com.gt*/ /*katherine.campos@eltejar.com.gt*/
            msg.Body = correo.Cuerpo;
            msg.IsBodyHtml = false;


            msg.From = new MailAddress("eltejarferreteria@gmail.com", $"Gestion de empleados - {correo.Nombre}", System.Text.Encoding.UTF8);
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.BodyEncoding = System.Text.Encoding.UTF8;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("eltejarferreteria@gmail.com", "htymvkzuxsgtrwej");

            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
                reply.result = true;
                reply.message = "Correo envíado exitosamente.";
            }
            catch (Exception ex)
            {
                reply.result = false;
                reply.message = ex.Message;
            }
            return reply;
        }
    }
}