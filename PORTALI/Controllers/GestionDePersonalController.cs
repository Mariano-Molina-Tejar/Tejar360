using DAL;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity;
using Microsoft.AspNet.SignalR.Hosting;
using Newtonsoft.Json;
using PORTALI.Helpers.EmailHelper;
using PORTALI.Services;
using PORTALI.Services;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Controllers
{
    public class GestionDePersonalController : Controller
    {
        // GET: GestionDePersonal
        private DALGestionDePersonal _dal = new DALGestionDePersonal();
        private UserLockService _servicesLockUser = new UserLockService();

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
        public async Task<JsonResult> EnviarSolicitudDeBaja(int id, int motivo, string observaciones, string causas, HttpPostedFileBase carta, string nombre, string motivoCadena, bool solicitarRequisicion, string observacionRequisicion, DateTime FechaRetiro)
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
                solicitud.Name = solicitarRequisicion ? Guid.NewGuid().ToString().Substring(0, 7) : null;
                solicitud.U_ObservecionesRequisicion = observacionRequisicion;
                solicitud.U_FechaDeRetiro = FechaRetiro;

                correo.Asunto = "Se a realizado una solicitud de baja de personal";
                correo.Cuerpo = Templates.BodyMailSolicitud(nombre, sessions.DeptoName, motivoCadena, observaciones, nombreUsuario);
                correo.Correos = "programador@eltejar.com.gt";
                correo.Nombre = "Baja de personal";
                correo.isHTML = true;

                string response = DAL_API.enviarDatosSL(url, solicitud);

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
                            string responseCausa = DAL_API.enviarDatosSL(url, causa);
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

        public bool SubirDocumento(HttpPostedFileBase archivo, int empleadoId, int solicitudId, string Extencion = "pdf", string carta = "Carta")
        {
            if (archivo == null || archivo.ContentLength == 0)
                return false;

            try
            {
                string path = @"\\172.31.99.76\SAPDocs\GestionDePersonal\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rutaCompleta = Path.Combine(path, $"{carta}-{solicitudId}{empleadoId}.{Extencion}");
                archivo.SaveAs(rutaCompleta);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public JsonResult ExistenciaDeDocumento(int solicitud, int id)
        {
            string carpeta = @"\\172.31.99.76\SAPDocs\GestionDePersonal\";
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
                    Procesos = await _dal.ObtenerProcesoDeBaja(),
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

        [HttpPost]
        public async Task<JsonResult> AutorizarBaja(int id,
                                                    int estado,
                                                    string nombreEmpleado,
                                                    string departamento,
                                                    string motivo,
                                                    string observaciones,
                                                    string nombreSolicitante,
                                                    string comentariosGTH,
                                                    int departamentoId,
                                                    int puestoId,
                                                    int solicitaId,
                                                    string observacionesRequisicion
            )
        {
            try
            {
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
                int response = await _dal.CambiarEstadoSolicitudBaja(id, estado);
                if (response != 1)
                    return Json(new { success = "success", message = "Ocurrio un error inesperado al realiazar la solicitud" });

                string correos = await _dal.ObtenerCorreosSolicitud(id, estado);

                var correo = new EnvioCorreoGestionEmpleados
                {
                    Asunto = "Respuesta GTH sobre la baja de personal solicitada",
                    Cuerpo = Templates.BodyMailResponseGTH(nombreEmpleado, departamento, motivo, observaciones, nombreSolicitante, estado, comentariosGTH),
                    Correos = "programador@eltejar.com.gt",
                    Nombre = "Baja de personal",
                    isHTML = true
                };

                int respuestaObservaciones = _servicesLockUser.guardarProcesoDeBaja(sessions.UserId, id, estado, comentariosGTH);

                if (estado == 1)
                {
                    var requisicion = await _dal.TieneRequisicion(id);

                    if (requisicion)
                        await CrearSolicitudDeAlta(id, solicitaId, puestoId, departamentoId, observacionesRequisicion);

                    if (motivo == "Abandono" || motivo == "Renuncia")
                        _servicesLockUser.guardarProcesoDeBaja(sessions.UserId, id, 3, "Bloqueo de usuarios operativos automaticamente por el sistema");
                }

                MailServices.EnviarCorreoElectronico(correo);
                return Json(new { success = "success", message = "Solicitud realizada con exito" });
            }
            catch (Exception ex)
            {
                return Json(new { success = "error", message = "Ocurrio un error inesperado al realizar la solicitud" });
            }
        }

        public async Task<int> CrearSolicitudDeAlta(
            int IdSolicitudBaja,
            int IdSolicitante,
            int IdPosicion,
            int departamento,
            string observacionesRequisicion
            )
        {
            string url = "GestionDePersonal/CrearSolicitudDeAlta";
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return 0;
            }
            try
            {
                var ObjectSend = new
                {
                    U_IdSolicitudBaja = IdSolicitudBaja,
                    U_FechaCreacion = DateTime.Now,
                    U_IdSolicitante = IdSolicitante,
                    U_Estado = "A",
                    U_IdPosicion = IdPosicion,
                    U_IdDepartamento = departamento,
                    U_IdPerfil = 0,
                    U_NuevaPlaza = "N",
                    U_Observaciones = observacionesRequisicion
                };

                string response = DAL_API.enviarDatosSL(url, ObjectSend);

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
                string response = DAL_API.enviarDatosSL(url, new { idEmployees = sessions.CodeEmpleado, eMail = email });

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

        public ActionResult VerDocumentos(int solicitud, int id, string carta = "Carta")
        {
            string carpeta = @"\\172.31.99.76\SAPDocs\GestionDePersonal\";
            string nombreArchivo = $"{carta}-{solicitud}{id}.pdf";

            string ruta = Path.Combine(carpeta, nombreArchivo);

            if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
                return Content("Error al mostrar el pdf");

            string contentType = MimeMapping.GetMimeMapping(ruta);

            return File(ruta, contentType);
        }

        public async Task<JsonResult> EnviarSolicitudNuevoPuesto(string puesto, int puestoId, string observaciones, string observacionesReclu, bool nuevo)
        {
            try
            {
                string host = "https://matthias-proadoption-vapouringly.ngrok-free.dev/api/AutorizacionGerencia/AutorizacionNuevoPuesto";
                string url = "GestionDePersonal/CrearSolicitudDeAlta";
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];

                if (sessions == null)
                {
                    return Json(new { success = false });
                }

                var nombreCompleto = await _dal.ObtenerNombreCompleto(sessions.UserId);

                var Solicitud = new
                {
                    U_FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd"),
                    U_IdSolicitante = sessions.CodeEmpleado,
                    U_IdPosicion = puestoId,
                    U_IdDepartamento = sessions.Depto,
                    U_NuevaPlaza = "Y",
                    U_Observaciones = observacionesReclu
                };

                string response = DAL_API.enviarDatosSL(url, Solicitud);

                var reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                int code = (int)json.Code;

                if (reply.result != 1)
                    return Json(new { success = false });

                observaciones = string.IsNullOrWhiteSpace(observaciones) ? "No se agregaron observaciones" : observaciones;

                string TemplateCorreoAutorizacion = Templates.BodyMailSolicitudAutorizacion(
                    nombreCompleto,
                    puesto.ToUpper(),
                    DateTime.Now.ToString("dd/MM/yyyy"),
                    observaciones,
                     $"{host}?code={HttpUtility.UrlEncode(code.ToString())}&aut=1&puesto={HttpUtility.UrlEncode(puesto)}&puestoId={HttpUtility.UrlEncode(puestoId.ToString())}",
                     $"{host}?code={HttpUtility.UrlEncode(code.ToString())}&aut=-1&puesto={HttpUtility.UrlEncode(puesto)}&puestoId={HttpUtility.UrlEncode(puestoId.ToString())}"
                    );

                var responseMail = MailServices.EnviarCorreoElectronico
                    (
                    new EnvioCorreoGestionEmpleados
                    {
                        Asunto = "Solicitud de nuevo puesto",
                        Cuerpo = TemplateCorreoAutorizacion,
                        Correos = "programador@eltejar.com.gt",
                        Nombre = "Solicitud de nuevo puesto",
                        isHTML = true
                    }
                    );

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public async Task<JsonResult> ObtenerProcesoDeAutorizacion()
        {
            try
            {
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];

                if (sessions == null)
                {
                    return Json(new { success = false });
                }

                var procesos = await _dal.ObtenerProcesoDeAutorizaciones(sessions.CodeEmpleado);
                if (procesos.Any())
                    return Json(new { success = true, data = procesos }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> ObtenerTrackingDeBaja(int empId)
        {
            try
            {
                var traking = await _dal.ObtenerTrackingBaja(empId) ?? new TrackingDeBaja();

                if (traking != null)
                    return Json(new { success = true, data = traking }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> CargarCartaDespido(HttpPostedFileBase carta, int idEpleado)
        {
            try
            {
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];

                var IdSolicitudPendiente = await _dal.ObtenerIdProcesoDeBaja(idEpleado);
                var responseCargarArchivo = SubirDocumento(carta, idEpleado, IdSolicitudPendiente, "pdf", "CartaDespido");
                if (responseCargarArchivo)
                {
                    _servicesLockUser.guardarProcesoDeBaja(sessions.UserId, 2, IdSolicitudPendiente, "Se cargo la carta de despido");
                    return Json(new { success = true });
                }
                return Json(new { success = false });

            }
            catch
            {
                return Json(new { success = false });
            }


        }

        public JsonResult VerExistenciaDeDocumento(int solicitud, int id, string carta = "Carta")
        {

            string carpeta = @"\\172.31.99.76\SAPDocs\GestionDePersonal\";
            string nombreArchivo = $"{carta}-{solicitud}{id}.pdf";

            string ruta = Path.Combine(carpeta, nombreArchivo);

            if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
                return Json(new { success = false, carta = false }, JsonRequestBehavior.AllowGet);

            return Json(new { success = true, carta = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> VerificarSiExistePerfil(int idPuesto)
        {
            try
            {
                var existe = await _dal.ExistePerfil(idPuesto);
                if (existe)
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> guardarVerificacionNuevaPlaza(
            string U_Puesto,
            int U_ExperienciaMinima,
            string U_Sexo,
            string U_NivelEstudio,
            int U_RangoEdadMin,
            int U_RangoEdadMax,
            double U_SalarioMin,
            double U_SalarioMax,
            string U_Observaciones,
            string U_JustificacionGerencia

            )
        {
            try
            {
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
                var url = "GestionDePersonal/AgregarPerfilPuesto";
                var urlGuardarGTH = "GestionDePersonal/CrearSolicitudDeAlta";
                var nuevoPerfil = new
                {
                    Name = U_Puesto.ToUpper(),
                    U_IdPuesto = -500,
                    U_ExperienciaMinima = U_ExperienciaMinima,
                    U_RangoEdadMin = U_RangoEdadMin,
                    U_RangoEdadMax = U_RangoEdadMax,
                    U_Sexo = U_Sexo,
                    U_NivelEstudio = U_NivelEstudio,
                    U_SalarioMin = U_SalarioMin,
                    U_SalarioMax = U_SalarioMax,
                    U_Observaciones = U_Observaciones,
                    U_FechaCreacion = DateTime.Now.ToString("yyyy-MM-dd"),
                    U_UsuarioCreacion = sessions.CodeEmpleado
                };

                var response = DAL_API.enviarDatosSL(url, nuevoPerfil);
                var reply = JsonConvert.DeserializeObject<Reply>(response);
                if (reply.result == 1)
                {
                    var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                    int code = (int)json.Code;

                    var ObjectSend = new
                    {
                        U_IdSolicitudBaja = 0,
                        U_FechaCreacion = DateTime.Now,
                        U_IdSolicitante = sessions.CodeEmpleado,
                        U_Estado = "P",
                        U_IdPosicion = -500,
                        U_IdDepartamento = sessions.Depto,
                        U_IdPerfil = code,
                        U_NuevaPlaza = "Y",
                        U_Observaciones = U_Observaciones,
                        U_JustificacionGerencia = U_JustificacionGerencia
                    };

                    response = DAL_API.enviarDatosSL(urlGuardarGTH, ObjectSend);
                    reply = JsonConvert.DeserializeObject<Reply>(response);

                    if (reply.result == 1)
                        return Json(new { success = true });
                    return Json(new { success = false });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        public async Task<JsonResult> guardarCartaLlamadaAtencion(int tipoDeFalta, DateTime fechaDeIncidente, HttpPostedFileBase carta, string observaciones, int empId)
        {
            try
            {
                var url = "GestionDePersonal/LlamadaAtencion";
                var correlativo = await _dal.ObtenerCorrelativoLlamadaAtencion(empId);
                var subirCarta = subirCartaLlamadaAtencion(carta, $"Llamada_de_atencion_{empId}_{correlativo}.pdf");

                if (subirCarta)
                {
                    var Datos = new
                    {
                        U_TipoDeFalta = tipoDeFalta,
                        U_Fecha = fechaDeIncidente,
                        U_NombreArchivo = $"Llamada_de_atencion_{empId}_{correlativo}.pdf",
                        U_Observaciones = observaciones,
                        U_EmpId = empId
                    };

                    var response = DAL_API.enviarDatosSL(url, Datos);

                    var reply = JsonConvert.DeserializeObject<Reply>(response);

                    if (reply.result == 1)
                    {
                        return Json(new { success = true });
                    }

                    return Json(new { success = false });

                }

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public bool subirCartaLlamadaAtencion(HttpPostedFileBase archivo, string nombre)
        {
            if (archivo == null || archivo.ContentLength == 0)
                return false;

            try
            {
                string path = @"\\172.31.99.76\SAPDocs\GestionDePersonal\CartasDeAtencion\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rutaCompleta = Path.Combine(path, nombre);
                archivo.SaveAs(rutaCompleta);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<JsonResult> ObtenerLlamadasDeAtencion(int empId)
        {
            try
            {
                var llamadasDeAtencion = await _dal.ObtenerLlamadasDeAtencionPorUsuario(empId);

                if (llamadasDeAtencion.Any())
                    return Json(new { success = true, data = llamadasDeAtencion }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VerCartaLlamadaAtencion(string direccion)
        {
            string carpeta = @"\\172.31.99.76\SAPDocs\GestionDePersonal\CartasDeAtencion\";
            string nombreArchivo = direccion;

            string ruta = Path.Combine(carpeta, nombreArchivo);

            if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
                return Content("Error al mostrar el pdf");

            string contentType = MimeMapping.GetMimeMapping(ruta);

            return File(ruta, contentType);
        }

        public async Task<JsonResult> verSolvenciaAdministrativaJefe(int empId)
        {
            try
            {
                var solvencia = await _dal.ObtenerSolvenciaAdministrativaJefe(empId);

                if (solvencia.FechaSolvenciaJefe != null)
                    return Json(new { success = true, data = solvencia }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}