using DAL;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using Entity;
using Microsoft.AspNet.SignalR.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Ocsp;
using PORTALI.Helpers.EmailHelper;
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
    public class ReclutamientoController : Controller
    {
        private DALReclutamiento _dal = new DALReclutamiento();
        private readonly EmpleadosDAL _dalGestionPersonal = new EmpleadosDAL();

        public async Task<ActionResult> Index()
        {
            try
            {
                ReclutamientoViewModel reclutamiento = new ReclutamientoViewModel();

                reclutamiento.Reclutamiento = await _dal.VerSolicitudesDePersonal();
                reclutamiento.ReclutamientoFinalizadas = await _dal.VerSolicitudesDePersonalFinalizadas();
                reclutamiento.ReclutamientoVerificacionNuevaPlaza = await _dal.VerVerificacionDeNuevosPuestos();
                reclutamiento.Documentos = await _dal.VerDocumentosRequeridos();
                reclutamiento.Tiendas = await _dal.ObtenerTiendas();
                var gerentes = await _dal.ObtenerGerentes();

                if (reclutamiento.Reclutamiento != null)
                    return View(reclutamiento);

                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.GetType();
                return View();
            }
        }

        [HttpPost]
        public async Task<JsonResult> CrearUsuarioLanding(string usuario, int idSolicitudAlta, string nombreAspirante, string correoAspirante)
        {
            EnvioCorreoGestionEmpleados correo = new EnvioCorreoGestionEmpleados();
            Response replyEmail = new Response();

            string claveGenerada = GenerarClaveAleatoria();
            int response = await _dal.InsertUsuarioLanding(usuario, claveGenerada, idSolicitudAlta, correoAspirante, nombreAspirante);

            if (response == 1)
            {
                correo.Asunto = "Llenado de información";
                correo.Cuerpo = Templates.BodyMailAspirante(nombreAspirante, usuario, claveGenerada, DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"), "https://localhost:44313/");
                correo.Correos = correoAspirante;
                correo.Nombre = "Llenado de información";
                correo.isHTML = true;
                replyEmail = MailServices.EnviarCorreoElectronico(correo);

                if (replyEmail.result)
                    return Json(new { success = true, message = $"Creación de usuario y envio de correo realizado con exito para el aspirante {nombreAspirante}" });

                return Json(new { success = true, message = $"Se creo el usuario correctamente para el aspirante {nombreAspirante} pero ocurrio un error al enviar el correo" });
            }

            return Json(new { success = false, message = $"Ocurrio un erro al guardar el usuario" });
        }

        public async Task<JsonResult> ObtenerCodigoSiguiente(string userId)
        {
            try
            {
                int response = await _dal.ObtenerCodigoSiguienUsuario(userId);
                if (response == 0)
                    return Json(-1, JsonRequestBehavior.AllowGet);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public static string GenerarClaveAleatoria()
        {
            Random random = new Random();
            int numero = random.Next(1000, 9999);
            return numero.ToString();
        }

        public async Task<JsonResult> ObtenerDetalleAspirantes(string userName)
        {
            try
            {
                var detalleAspirantes = await _dal.ObtenerDetalleAspirantes(userName);

                if (detalleAspirantes.Any())
                    return Json(new { success = true, message = "", data = detalleAspirantes }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false, message = "No se encontraron datos" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "No se encontraron datos" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public async Task<JsonResult> VerClaveUsuario(string userName)
        {
            try
            {
                string clave = await _dal.verClaveUsuario(userName);

                if (string.IsNullOrEmpty(clave))
                    return Json(new { success = false, clave = "" }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, clave = clave }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, clave = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> ObtenerDatosPersonales(string userName)
        {
            try
            {
                var datosPersonales = await _dal.ObtenerDatosPersonalesAspirante(userName);

                if (!string.IsNullOrWhiteSpace(datosPersonales.DatosPersonalesVM.PrimerNombre))
                    return Json(new { success = true, data = datosPersonales }, JsonRequestBehavior.AllowGet);

                var code = await _dal.ObtenerCodigoUsuario(userName);
                datosPersonales.DatosPersonalesVM.Code = code;
                if (datosPersonales.DatosPersonalesVM.Code != 0)
                    return Json(new { success = true, data = datosPersonales }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CargarVistaParcial(string nombreDocumento, int tipo, string extencion, string descripcion)
        {
            ViewModelDocumento documento = new ViewModelDocumento()
            {
                NombreDocumento = nombreDocumento,
                TipoDocumento = tipo,
                Extencion = extencion,
                Descripcion = descripcion
            };

            return PartialView($"_DetalleDocumento", documento);
        }

        public JsonResult ExistenciaDocumentos(List<CVDOCREQ> Documentos, int idUser)
        {
            Reply reply = new Reply();

            List<dynamic> response = new List<dynamic>();
            string carpeta = @"\\172.31.99.76\SAPDocs\DocumentosAspirantes\";
            foreach (var Lista in Documentos)
            {
                string nombreArchivo = $"Documento_{Lista.DocEntry}{idUser}.{Lista.Extension}";
                string ruta = Path.Combine(carpeta, nombreArchivo);

                if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
                {
                    response.Add(new { DocEntry = Lista.DocEntry, Existe = 0 });
                }
                else
                {
                    response.Add(new { DocEntry = Lista.DocEntry, Existe = 1 });
                }
            }
            return Json(new { Response = 1, Objet = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ReenviarCorreoAspirante(string usuario)
        {
            EnvioCorreoGestionEmpleados correo = new EnvioCorreoGestionEmpleados();
            Response replyEmail = new Response();
            try
            {
                var aspirante = await _dal.ObtenerDetalleAspirante(usuario);

                if (!string.IsNullOrWhiteSpace(aspirante.UserName) || aspirante != null)
                {
                    if (DateTime.Now > aspirante.Created.AddDays(2))
                        return Json(new { success = false, message = $"La fecha para llenar el formulario ya vencio" });

                    correo.Asunto = "Llenado de información";
                    correo.Cuerpo = Templates.BodyMailAspirante(aspirante.NombreAspirante, usuario, aspirante.PassWord, aspirante.Created.AddDays(2).ToString("dd/MM/yyyy"), "bolsaempleoseltejar.com");
                    correo.Correos = aspirante.Correo;
                    correo.Nombre = "Llenado de información";
                    correo.isHTML = true;
                    replyEmail = MailServices.EnviarCorreoElectronico(correo);

                    if (replyEmail.result)
                        return Json(new { success = true, message = $"El correo se re-envio correctamente" });

                    return Json(new { success = false, message = $"Ocurrio un error al re-enviar el correo" });
                }

                return Json(new { success = false, message = $"Ocurrio un error al obtener los datos del aspirante" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Ocurrio un error al re-enviar el correo" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AgregarComentario(int code, string comentario)
        {
            string url = "GestionDePersonal/AgregarComentario";
            var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            var datosComentario = new
            {
                U_Usuario = code,
                U_Comentario = comentario,
                U_Fecha = DateTime.Now,
                U_UsuarioComentario = sessions.UserId
            };

            string response = DAL_API.enviarDatosSL(url, datosComentario);

            var reply = JsonConvert.DeserializeObject<Reply>(response);

            if (reply.result == 1)
            {
                return Json(new { success = true });
            }
            return Json(new { });
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerComentariosAspirantes(int usuario)
        {
            try
            {
                var comentarios = await _dal.ObtenerComentariosAspitantes(usuario);
                if (comentarios.Any())
                    return Json(new { success = true, data = comentarios }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPerfilPuestoPorId(int idPuesto, string nombrePuesto = "")
        {
            try
            {
                string puesto = idPuesto == 0 ? nombrePuesto : idPuesto.ToString();
                var perfil = await _dal.ObtenerPerfilDePuestoPorId(puesto);

                if (perfil.U_IdPuesto == 0)
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, data = perfil }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GuardarPerfilPuesto(PerfilPuestoModel perfil)
        {
            try
            {
                bool existe = await _dal.VerificarExistenciaDePerfil(perfil.Code);

                perfil.U_IdPuesto = perfil.U_IdPuesto == 0 ? -500 : perfil.U_IdPuesto;

                string url = !existe ? "GestionDePersonal/AgregarPerfilPuesto" : "GestionDePersonal/ActualizarPerfilPuesto";
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
                perfil.U_UsuarioCreacion = sessions.UserId;
                perfil.U_FechaCreacion = DateTime.Now;

                string response = DAL_API.enviarDatosSL(url, perfil);

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                {
                    var responseMail = EnviarSolicitudNuevoPuesto(perfil.descriptio, -500, perfil.Justificacion, true, perfil.Solicita, int.Parse(perfil.IdSolicitudAlta), perfil.Code);

                    if (responseMail)
                    {
                        var respuesta = await _dal.actualizarEstadoSolicitudDeAlta(int.Parse(perfil.IdSolicitudAlta));
                        if (respuesta == 1)
                            return Json(new { success = true });
                        return Json(new { success = false });
                    }

                    return Json(new { success = false });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }

        }

        public bool EnviarSolicitudNuevoPuesto(string puesto, int puestoId, string observaciones, bool nuevo, string solicita, int code, int idPerfil)
        {
            try
            {
                string host = "https://matthias-proadoption-vapouringly.ngrok-free.dev/api/AutorizacionGerencia/AutorizarNuevaPlaza";
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return false;
                }

                observaciones = string.IsNullOrWhiteSpace(observaciones) ? "No se agregaron observaciones" : observaciones;

                string TemplateCorreoAutorizacion = Templates.BodyMailSolicitudAutorizacion(
                             solicita,
                             puesto.ToUpper(),
                             DateTime.Now.ToString("dd/MM/yyyy"),
                             observaciones,
                             $"{host}?code={HttpUtility.UrlEncode(code.ToString())}&aut=1&puesto={HttpUtility.UrlEncode(puesto)}&puestoId={HttpUtility.UrlEncode(puestoId.ToString())}&idPerfil={HttpUtility.UrlEncode(idPerfil.ToString())}",
                             $"{host}?code={HttpUtility.UrlEncode(code.ToString())}&aut=-1&puesto={HttpUtility.UrlEncode(puesto)}&puestoId={HttpUtility.UrlEncode(puestoId.ToString())}&idPerfil={HttpUtility.UrlEncode(idPerfil.ToString())}"

                         );


                var response = new Entity.Response();

                response = MailServices.EnviarCorreoElectronico
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

                return response.result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<JsonResult> ActualizarCorreo(string usuario, string correo)
        {
            try
            {
                var resultado = await _dal.ActualizarCorreo(usuario, correo);

                if (resultado == 0)
                    return Json(new { success = false });

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public async Task<JsonResult> ObtenerDatosPuestoDepartamentoGerentes()
        {
            try
            {
                var gerentes = await _dal.ObtenerGerentes();
                var puestos = await _dalGestionPersonal.ObtenerPosiciones();
                var departamentos = await _dalGestionPersonal.ObtenerTodosLosDepartamentos();

                var Informacion = new
                {
                    Posiciones = puestos,
                    Departamentos = departamentos,
                    Gerentes = gerentes
                };

                if (!gerentes.Any() && !puestos.Any() && !departamentos.Any())
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, data = Informacion }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GuardarSolicitudDePersonal(int posicion, int departamento, int solicita)
        {
            try
            {
                string url = "GestionDePersonal/CrearSolicitudDeAlta";

                var solicitudAlta = new
                {
                    U_FechaCreacion = DateTime.Now,
                    U_IdSolicitante = solicita,
                    U_Estado = "A",
                    U_IdPosicion = posicion,
                    U_IdDepartamento = departamento,
                    U_IdPerfil = 0,
                    U_NuevaPlaza = "N"
                };

                string response = DAL_API.enviarDatosSL(url, solicitudAlta);

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult DarDeAltaAAspirante
            (
            DatosPersonales datosPersonales,
            int puestoId,
            int departamentoId,
            int solicitaId
            )
        {
            try
            {
                string url = "Reclutamiento/DatoMaestroEmpleado";

                var datosEmpleado = new
                {
                    DatosPersonales = datosPersonales,
                    PuestoId = puestoId,
                    DepartamentoId = departamentoId,
                    SolicitaId = solicitaId
                };

                string response = DAL_API.enviarDatosSL(url, datosEmpleado);
                var reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                string employeeID = (string)json.EmployeeID;

                if (reply.result == 1)
                {
                    return Json(new { success = true, empId = employeeID });
                }
                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CrearProveedor(bool proveedor, DatosPersonales datosPersonales, int empId)
        {
            try
            {
                var url = proveedor ? "Reclutamiento/Proveedor" : "Reclutamiento/Cliente";

                var datosSN = new
                {
                    DatosPersonales = datosPersonales,
                    EmpId = empId
                };

                var response = DAL_API.enviarDatosSL(url, datosSN);
                var reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                string cardCode = (string)json.CardCode;
                if (reply.result == 1)
                {
                    return Json(new { success = true, proveedor = cardCode });
                }

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CrearUsuarioSAP(string departamento, DatosPersonales datosPersonales, bool usuario360, int empId, int departamentoId)
        {
            try
            {
                var url = "Reclutamiento/UsuarioSAP";
                var usuarioParcial = departamento.Length > 6 ? $"t{departamento.Substring(0, 6).ToLower()}" : $"t{departamento.ToLower()}";
                var usuarioFinal = "";
                int contador = 0;
                var existe = true;

                do
                {
                    existe = await _dal.ExisteUsuario(contador == 0 ? usuarioParcial : $"{usuarioParcial}{contador}");
                    usuarioFinal = contador == 0 ? usuarioParcial : $"{usuarioParcial}{contador}";
                    contador++;
                } while (existe);

                var user = new
                {
                    userName = $"{datosPersonales.PrimerNombre} {datosPersonales.PrimerApellido}",
                    userCode = usuarioFinal,
                    usuario360 = usuario360,
                    EmpId = empId,
                    departamento = departamentoId
                };

                var response = DAL_API.enviarDatosSL(url, user);
                var reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                string UserCode = (string)json.UserCode;

                return Json(new { success = true, userCode = UserCode });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CrearEmpleadoDeVentas(DatosPersonales datosPersonales, string whsCode, string whsName, int empId)
        {
            try
            {
                var url = "Reclutamiento/EmpleadoDeVentas";
                string SalesEmployeeName = "";
                int SalesEmployeeCode = 0;

                var EmpleadoDeVentas = new
                {
                    Nombre = $"{datosPersonales.PrimerNombre} {datosPersonales.SegundoNombre} {datosPersonales.PrimerApellido} {datosPersonales.SegundoApellido}",
                    WhsCode = whsCode,
                    WhsName = whsName,
                    EmpId = empId
                };

                var response = DAL_API.enviarDatosSL(url, EmpleadoDeVentas);

                var reply = JsonConvert.DeserializeObject<Reply>(response);
                if (reply.result == 1)
                {
                    var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                    SalesEmployeeName = (string)json.SalesEmployeeName;
                    SalesEmployeeCode = (int)json.SalesEmployeeCode;
                }

                if (reply.result == 1)
                {
                    return Json(new { success = true, salesEmployeeName = SalesEmployeeName, salesEmployeeCode = SalesEmployeeCode });
                }
                else
                {
                    return Json(new { success = false, message = reply.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CrearUsuarioSAPAsesor(DatosPersonales datosPersonales, string whsCode, int empId)
        {
            try
            {
                var prefijo = whsCode.Substring(whsCode.IndexOf("-") + 2);
                var url = "Reclutamiento/UsuarioSAPAsesor";
                var usuarioParcial = $"V{prefijo}-Asesor";
                var usuarioFinal = "";
                int contador = 1;
                var existe = true;

                do
                {
                    existe = await _dal.ExisteUsuario($"{usuarioParcial}{contador}");
                    usuarioFinal = contador == 0 ? usuarioParcial : $"{usuarioParcial}{contador}";
                    contador++;
                } while (existe);

                var user = new
                {
                    userName = $"{datosPersonales.PrimerNombre} {datosPersonales.PrimerApellido}",
                    userCode = usuarioFinal,
                    whsCode = whsCode,
                    empId = empId
                };

                var response = DAL_API.enviarDatosSL(url, user);
                var reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                string UserCode = (string)json.UserCode;

                return Json(new { success = true, userCode = UserCode });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AsignarMetaAsesor(int codigoEmpVentas, string tienda)
        {
            try
            {
                var url = "Reclutamiento/AsignarMeta";

                var data = new
                {
                    empeDeVentas = codigoEmpVentas,
                    whsCode = tienda
                };

                var response = DAL_API.enviarDatosSL(url, data);
                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return Json(new { success = true, metaAsesor = "Q150,000" });

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CerrarSolicitudDeAlta(int idSolicitud, int empId, string user)
        {
            try
            {
                var url = "Reclutamiento/CerrarSolicitudDeAlta";

                var response = DAL_API.enviarDatosSL(url, new { idSolicitud = idSolicitud });
                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                {

                    int result = await _dal.AgregarEmpleadoAlAspirante(empId, user);

                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public ActionResult VerImagen(string archivo)
        {
            string carpeta = @"\\172.31.99.76\SAPDocs\GestionDePersonal\fotosPerfil\";

            // Sanitizar para evitar rutas peligrosas
            archivo = Path.GetFileName(archivo);

            string ruta = Path.Combine(carpeta, archivo);

            if (!System.IO.File.Exists(ruta))
            {
                string sinExtension = Path.GetFileNameWithoutExtension(archivo);

                var posiblesExtensiones = new[] { ".jpg", ".jpeg", ".png" };
                ruta = posiblesExtensiones
                    .Select(ext => Path.Combine(carpeta, sinExtension + ext))
                    .FirstOrDefault(f => System.IO.File.Exists(f));
            }

            // Si no encuentra ninguna, usa NoPhoto.jpg
            if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
            {
                ruta = Path.Combine(carpeta, "NoProfile.png");
                if (!System.IO.File.Exists(ruta))
                {
                    return HttpNotFound("No se encontró la imagen ni el archivo NoPhoto.jpg");
                }
            }
            string contentType = MimeMapping.GetMimeMapping(ruta);
            return File(ruta, contentType);
        }

        public async Task<JsonResult> ObtenerInformaciónCreadaEmpleado(int empId)
        {
            try
            {
                var datos = await _dal.ObtenerDatosEmpleadoCreado(empId);

                if (datos != null)
                    return Json(new { success = true, data = datos }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VerDocumetos(int Usuario, string Tipo, int VerExistencia = 0, string Extencion = "pdf")
        {
            string carpeta = @"\\172.31.99.76\SAPDocs\DocumentosAspirantes\";
            string nombreArchivo = $"Documento_{Tipo}{Usuario}.{Extencion}";
            string ruta = Path.Combine(carpeta, nombreArchivo);

            if (string.IsNullOrEmpty(ruta) || !System.IO.File.Exists(ruta))
                return Content("-1");

            if (VerExistencia == 0)
            {
                string contentType = MimeMapping.GetMimeMapping(ruta);
                return File(ruta, contentType);
            }
            else
            {
                return Content("1");
            }
        }


        [HttpPost]
        public JsonResult SubirDocumentos(HttpPostedFileBase archivo, int Tipo, string Extencion, int usuario)
        {
            //var usuario = Session["Usuario"] as UsuarioEntity;

            if (archivo == null || archivo.ContentLength == 0)
                return Json(new { Message = "Debes seleccionar un archivo" });

            if (usuario == null)
                return Json(new { Message = "La sesión ha expirado. Por favor, recargue la página e inicie sesión nuevamente." });

            try
            {
                string path = @"\\172.31.99.76\SAPDocs\DocumentosAspirantes\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rutaCompleta = Path.Combine(path, $"Documento_{Tipo}{usuario}.{Extencion}");
                archivo.SaveAs(rutaCompleta);

                return Json(new { Message = "Documento cargado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message });
            }
        }

        public async Task<JsonResult> ObtenerAnalisisMixtral(int idSolicitud, int idPuesto)
        {
            try
            {
                var mixtralService = new MixtralService("Bearer M7jFFXCpKeecatV6ZSMI18mNQVLfkP2L");

                string perfilJson = _dal.ObtenerPerfilJson(idSolicitud, idPuesto);
                string aspiranteJson = _dal.ObtenerAspirantesJson(idSolicitud);

                string apiResponse = await mixtralService.AnalizarPerfilAsync(perfilJson, aspiranteJson);

                var root = JObject.Parse(apiResponse);
                string contenidoJson = root["choices"][0]["message"]["content"].ToString().Trim();

                // 4️⃣ Validar que sea JSON válido
                ResultadoEvaluacionIA resultadoIA;

                try
                {
                    resultadoIA = JsonConvert.DeserializeObject<ResultadoEvaluacionIA>(contenidoJson);
                    GuardarDatosIAenDB(resultadoIA, idSolicitud);
                    return Json(new { success = true });

                }
                catch (Exception ex)
                {
                    // fallback defensivo
                    resultadoIA = new ResultadoEvaluacionIA
                    {
                        evaluacion = new List<EvaluacionAspirante>(),
                        recomendacionFinal = "Error al procesar la evaluación de IA"
                    };

                    return Json(new { success = false });
                    // aquí puedes loguear ex + contenidoJson
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        private bool GuardarDatosIAenDB(ResultadoEvaluacionIA resultado, int solicitudID)
        {
            var urlEvalucion = "Reclutamiento/evaluacionIA";

            var response = DAL_API.enviarDatosSL(urlEvalucion, new { resultado = resultado, SolicitudId = solicitudID });
            return true;
        }

        public async Task<JsonResult> obtenerAnalisisIA(int userId)
        {
            try
            {
                var response = await _dal.ObtenerAnalisisAspirantesIA(userId);
                if (response.Any())
                    return Json(new { success = true, data = response }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}