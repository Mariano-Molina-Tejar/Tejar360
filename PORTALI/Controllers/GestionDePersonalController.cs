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
using PORTALI.Helpers.EmailHelper;

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
                var Personal = (List<DatosEmpleados>)await _dal.ObtenerColaboradoresACargo(sessions.UserId);

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
        public async Task<JsonResult> EnviarSolicitudDeBaja(int id, int motivo, string observaciones, string causas, HttpPostedFileBase carta, string nombre, string motivoCadena)
        {
            EnvioCorreoGestionEmpleados correo = new EnvioCorreoGestionEmpleados();
            Reply reply = new Reply();
            Response replyEmail = new Response();
            Causa causa = new Causa();
            int[] listaCausas = JsonConvert.DeserializeObject<int[]>(causas);
            bool successSolicitud = false;
            bool successEmail = false;

            SolicitudDeBaja solicitud = new SolicitudDeBaja();

            int solicitudPendient = await _dal.VerificarSolicitudDeBajaPendiente(id);
            if (solicitudPendient != 0)
                return Json(new { success = false, message = "Ya existe una solicitud pendiente para este empleado" });

            string url = "GestionDePersonal/SolicitudDeBaja";
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            string nombreUsuario = await _dal.ObtenerNombreDeUsuario(sessions.UserId);

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
                correo.Cuerpo = Templates.BodyMailSolicitud(nombre, sessions.DeptoName, motivoCadena, observaciones, nombreUsuario);
                correo.Correos = "programador@eltejar.com.gt";
                correo.Nombre = "Baja de personal";
                correo.isHTML = true;

                string response = DAL_API.NotasPpto(url, solicitud);

                reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                int code = (int)json.Code;

                if (reply.result == 1)
                {
                    if (carta != null)
                    {
                        var responseCargarArchivo = SubirDocumento(carta, solicitud.U_EmpleadoId, code);
                    }

                    if (listaCausas != null && code != 0)
                    {
                        url = "GestionDePersonal/CausaSolicitudDeBaja";
                        causa.U_IdSolicitud = code;
                        foreach (var c in listaCausas)
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
            catch (Exception ex)
            {
                return Json(new { successS = successSolicitud, successE = successEmail });
            }
        }
        public string SubirDocumento(HttpPostedFileBase archivo, int empleadoId, int solicitudId, string Extencion = "pdf")
        {
            if (archivo == null || archivo.ContentLength == 0)
                return "Debes seleccionar un archivo";

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

        public JsonResult ExistenciaDeDocumento(int solicitud, int id)
        {
            string carpeta = @"\\SRVSAPTQ2\SAPDocs\GestionDePersonal\";
            string nombreArchivo = $"Carta-{solicitud}{id}.pdf";
            string ruta = Path.Combine(carpeta, nombreArchivo);
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            try
            {
                if (System.IO.File.Exists(ruta))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> AutorizacionGTH()
        {
            try
            {

                AutorizacionesGTHViewModel autorizacionesViewModel = new AutorizacionesGTHViewModel
                {
                    Autorizaciones = await _dal.ObtenerAutorizacinesDeBaja(),
                    Causas = await _dal.ObtenerCausasPorSolicitud()
                };

                return View(autorizacionesViewModel);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.GetType();
                return View();
            }
        }

        public async Task<JsonResult> AutorizarBaja(int id,
                                                    int estado,
                                                    string nombreEmpleado,
                                                    string departamento,
                                                    string motivo,
                                                    string observaciones,
                                                    string nombreSolicitante,
                                                    string comentariosGTH
            )
        {
            try
            {
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
                int response = await _dal.CambiarEstadoSolicitudBaja(id, estado);
                if (response != 1)
                    return Json(new { success = false, message = "Ocurrio un error inesperado al realiazar la solicitud" }, JsonRequestBehavior.AllowGet);

                string correos = await _dal.ObtenerCorreosSolicitud(id, estado);

                var correo = new EnvioCorreoGestionEmpleados
                {
                    Asunto = "Respuesta GTH sobre la baja de personal solicitada",
                    Cuerpo = Templates.BodyMailResponseGTH(nombreEmpleado, departamento, motivo, observaciones, nombreSolicitante, estado, comentariosGTH),
                    Correos = correos.Trim(','),
                    Nombre = "Baja de personal",
                    isHTML = true
                };

                int respuestaObservaciones = guardarObservaciones(sessions.UserId, id, estado, comentariosGTH);

                MailServices.EnviarCorreoElectronico(correo);

                return Json(new { success = true, message = "Solicitud realizada con exito" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrio un error inesperado al realizar la solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> VerificarSolicitudBajaPendiente(int id)
        {
            try
            {
                int solicitudPendient = await _dal.VerificarSolicitudDeBajaPendiente(id);

                if (solicitudPendient == 0)
                    return Json(new { pendiente = 0, error = false }, JsonRequestBehavior.AllowGet);

                return Json(new { pendiente = 1, error = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { pendiente = 1, error = true }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public async Task<JsonResult> guardarObservaciones(int id, int motivo, string observaciones, string causas, HttpPostedFileBase carta)
        //{
        //    Reply reply = new Reply();

        //    SolicitudDeBaja solicitud = new SolicitudDeBaja();


        //    string url = "GestionDePersonal/guardarObservacionesSolicitud";
        //    var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
        //    if (sessions == null)
        //    {
        //        return Json(new { success = false, message = "Sesión expirada" });
        //    }
        //    try
        //    {
        //        string response = DAL_API.NotasPpto(url, solicitud);

        //        reply = JsonConvert.DeserializeObject<Reply>(response);

        //        if (reply.result == 1)
        //        {

        //        }

        //        return Json(new { successS = true, successE = "Hola" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { successS = false, successE = "" });
        //    }
        //}

        [HttpPost]
        public JsonResult AgregarEmail(string email)
        {
            string url = "GestionDePersonal/agregarCorreo";
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            try
            {
                string response = DAL_API.NotasPpto(url, new { idEmployees = sessions.CodeEmpleado, eMail = email });

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        public async Task<JsonResult> VerificarExistenciaDeCorreo()
        {
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            try
            {
                int resultado = await _dal.VerificarExistenciaDeCorreo(sessions.UserId);
                if (resultado == 1)
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VerDocumentos(int solicitud, int id)
        {
            string carpeta = @"\\SRVSAPTQ2\SAPDocs\GestionDePersonal\";
            string nombreArchivo = $"Carta-{solicitud}{id}.pdf";

            string ruta = Path.Combine(carpeta, nombreArchivo);

            if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
                return Content("Error al mostrar el pdf");


            string contentType = MimeMapping.GetMimeMapping(ruta);

            return File(ruta, contentType);
        }

        public int guardarObservaciones(int userId, int IdSolicitud, int IdEstado, string Observaciones)
        {
            string url = "GestionDePersonal/guardarObservacionesSolicitud";
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return 0;
            }
            try
            {
                var ObjectSend = new
                {
                    Name = userId,
                    U_IdSolicitud = IdSolicitud,
                    U_IdEstado = IdEstado,
                    U_FechaObservacion = DateTime.Now,
                    U_Observaciones = Observaciones
                };

                string response = DAL_API.NotasPpto(url, ObjectSend);

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return 1;

                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}