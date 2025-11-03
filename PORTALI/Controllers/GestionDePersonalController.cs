using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Threading.Tasks;
using PORTALI.Services;
using Newtonsoft.Json;
using System.IO;

namespace PORTALI.Controllers
{
    public class GestionDePersonalController : Controller
    {
        // GET: GestionDePersonal
        DALGestionDePersonal _dal = new DALGestionDePersonal();

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                ViewData["Error"] = "Su sesion ha expirado!!";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var Personal = await _dal.ObtenerColaboradoresACargo(sessions.UserId);

                if (!Personal.Any() || Personal == null)
                {
                    ViewData["Error"] = "No se encontraron datos";
                    return View();
                }

                return View(Personal);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.GetType();
                return View();
            }
        }

        [Authorize]
        public async Task<JsonResult> ObtenerPuestos()
        {
            try
            {
                var puestos = await _dal.ObtenerPuestos();

                if (!puestos.Any() || puestos == null)
                    return Json(new { success = false, message = "No se encontraron datos" }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, message = "", data = puestos }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { success = false, message = "Ocurrio un error al obtener los puestos" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public async Task<JsonResult> ObtenerCausasDespido()
        {
            try
            {
                var Causas = await _dal.ObtenerCausasDespido();
                if (!Causas.Any() || Causas == null)
                    return Json(new { success = false, message = "No se encontraron datos" }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, message = "", data = Causas }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Ocurrio un error al obtener los puestos" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EnviarSolicitudDeBaja(int id, int motivo, string observaciones, int[] causas, HttpPostedFileBase carta)
        {
            EnvioCorreoGestionEmpleados correo = new EnvioCorreoGestionEmpleados();
            Reply reply = new Reply();
            Response replyEmail = new Response();
            Causa causa = new Causa();


            bool successSolicitud = false;
            bool successEmail = false;  

            SolicitudDeBaja solicitud = new SolicitudDeBaja();

            string url = "GestionDePersonal/SolicitudDeBaja";
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            try
            {
                solicitud.U_EmpleadoId = id;
                solicitud.U_SolicitanteId = sessions.UserId;
                solicitud.U_Observaciones = observaciones;
                solicitud.U_AutorizadoGTH = "N";
                solicitud.U_Motivo = motivo;
                solicitud.U_Estado = 0;
                solicitud.U_FechaSolicitud = DateTime.Now;

                correo.Asunto = "Se a realizado una solicitud de baja de personal";
                correo.Cuerpo = "Se a realizado una solicitud de baja de personal";
                correo.Correos = "programador@eltejar.com.gt";
                correo.Nombre = "Baja de personal";

                string response = DAL_API.NotasPpto(url, solicitud);     

                reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                int code = (int)json.Code;

                if (reply.result == 1)
                {

                    if (carta != null)
                    {
                        var responseCargarArchivo= SubirDocumento(carta, solicitud.U_EmpleadoId, code);
                    }

                    if (causas != null && code != 0)
                    {
                        url = "GestionDePersonal/CausaSolicitudDeBaja";
                        causa.U_IdSolicitud = code;
                        foreach (var c in causas)
                        {
                            causa.U_IdCausa = c;
                            string responseCausa = DAL_API.NotasPpto(url, causa);
                        }
                    }

                    replyEmail = MailServices.EnviarCorreoElectronico(correo);
                }

                if (reply.result == 1)
                    successSolicitud = true;

                if (replyEmail.result)
                    successEmail = true;

                return Json(new { successS = successSolicitud, successE = successEmail });
            }
            catch(Exception ex)
            {
                return Json(new { successS = successSolicitud, successE = successEmail });
            }
        }
        public string SubirDocumento(HttpPostedFileBase archivo,int empleadoId, int solicitudId, string Extencion = "pdf")
        {
            //var usuario = Session["Usuario"] as UsuarioEntity;

            if (archivo == null || archivo.ContentLength == 0)
                return "Debes seleccionar un archivo";

            //if (usuario == null)
            //    return Json(new { Message = "La sesión ha expirado. Por favor, recargue la página e inicie sesión nuevamente." });

            try
            {
                string path = @"\\SRVSAPTQ2\SAPDocs\GestionDePersonal\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rutaCompleta = Path.Combine(path, $"Carta-{solicitudId}{empleadoId}.{Extencion}");
                archivo.SaveAs(rutaCompleta);

                return "Documento cargado correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}