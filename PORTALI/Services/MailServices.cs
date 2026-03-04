using Entity;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace PORTALI.Services
{
    public static class MailServices
    {
        private static readonly string mail = "eltejarferreteria@gmail.com";
        private static readonly string APP_PASSWORD = "htymvkzuxsgtrwej";
        public static Response EnviarCorreoElectronico(EnvioCorreoGestionEmpleados correo)
        {
            Response reply = new Response();
            correo.Correos = "programador.sr@eltejar.com.gt";
            using (MailMessage msg = new MailMessage())
            {
                msg.Subject = correo.Asunto;
                msg.To.Add(correo.Correos);
                msg.Body = correo.Cuerpo;
                msg.IsBodyHtml = correo.isHTML;
                msg.BodyEncoding = Encoding.UTF8;
                msg.SubjectEncoding = Encoding.UTF8;
                msg.ReplyToList.Add("eltejarferreteria@gmail.com");

                msg.From = new MailAddress("eltejarferreteria@gmail.com", "Gestion de empleados - EL TEJAR", System.Text.Encoding.UTF8);
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.BodyEncoding = System.Text.Encoding.UTF8;

                try
                {
                    using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                    {
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential(mail, APP_PASSWORD);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Timeout = 20000;

                        client.Send(msg);
                        reply.result = true;
                        reply.message = "Correo envíado exitosamente.";
                    }
                }
                catch (Exception ex)
                {
                    reply.result = false;
                    reply.message = ex.Message;
                }
            }
            return reply;
        }

        //public static Response EnviarCorreoElectronico(EnvioCorreoGestionEmpleados correo)
        //{
        //    Response reply = new Response();

        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Gestión de Empleados", "eltejarferreteria@gmail.com"));
        //    message.To.Add(MailboxAddress.Parse(correo.Correos));
        //    message.Subject = correo.Asunto;

        //    var bodyBuilder = new BodyBuilder { HtmlBody = correo.Cuerpo };
        //    message.Body = bodyBuilder.ToMessageBody();

        //    try
        //    {
        //        using (var client = new SmtpClient())
        //        {
        //            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        //            client.Authenticate("eltejarferreteria@gmail.com", APP_PASSWORD); // ⚠️ App Password
        //            client.Send(message);
        //            client.Disconnect(true);
        //        };

        //        reply.result = true;
        //        reply.message = "Correo enviado exitosamente.";
        //    }
        //    catch (Exception ex)
        //    {
        //        reply.result = false;
        //        reply.message = ex.Message;
        //    }
        //    return reply;
        //}

    }
}