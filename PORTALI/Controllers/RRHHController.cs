using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json;
using SelectPdf;
using PORTALI.Services;
using System.Net;

namespace PORTALI.Controllers
{
    public class RRHHController : Controller
    {
        DALPortalRRHH service = new DALPortalRRHH();
        FirmaDigitalService firmaService = new FirmaDigitalService();

        FiltroVacaciones filtro = new FiltroVacaciones();
        public async Task<ActionResult> Index(MarcajeFiltrosEntiry model = null)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            List<MarcajeEntity> listaMarcaje = new List<MarcajeEntity>();

            var fechas = new MarcajeFiltrosEntiry
            {
                FechaInicio = (model.FechaInicio < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : model.FechaInicio,
                FechaFin = (model.FechaFin < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : model.FechaFin,
                area = model.area
            };

            ViewBag.FechaInicio = fechas.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechas.FechaFin.ToString("yyyy-MM-dd");

            try
            {
                listaMarcaje = await service.ObtenerIncumplimiento(fechas, sessions);
                ViewBag.Areas = await service.ObtenerAreas();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(listaMarcaje);
        }

        public async Task<JsonResult> ObtenerDetalleIncumplimientos(MarcajeFiltrosEntiry model)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            List<DetalleMarcaje> listaMarcaje = new List<DetalleMarcaje>();

            try
            {
                listaMarcaje = await service.ObtenerDetalleMarcaje(model, sessions);
                return Json(new { success = true, data = listaMarcaje });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //[HttpPost]
        public async Task<JsonResult> ObtenerDetalleIncumplimientosPorEmpleado(MarcajeFiltrosEntiry model)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            List<DetalleMarcaje> listaMarcaje = new List<DetalleMarcaje>();

            try
            {
                listaMarcaje = await service.ObtenerDetalleMarcajePorEmpleado(model);
                return Json(new { success = true, data = listaMarcaje });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ExportarDetalleExcel(MarcajeFiltrosEntiry model)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            var service = new DALPortalRRHH();
            var data = await service.ObtenerDetalleMarcaje(model, sessions); // Puede ser async si necesitas

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Detalle Marcaje");

                // Encabezados
                ws.Cell(1, 1).Value = "Fecha";
                ws.Cell(1, 2).Value = "Empleado";
                ws.Cell(1, 3).Value = "Puesto";
                ws.Cell(1, 4).Value = "Hora Entrada";
                ws.Cell(1, 5).Value = "Hora Salida";
                ws.Cell(1, 6).Value = "ROL Entrada";
                ws.Cell(1, 7).Value = "ROL Salida";
                ws.Cell(1, 8).Value = "Incumplimiento";

                int row = 2;
                foreach (var item in data)
                {
                    ws.Cell(row, 1).Value = item.Fecha.ToString("dd/MM/yyyy");
                    ws.Cell(row, 2).Value = item.Empleado;
                    ws.Cell(row, 3).Value = item.Posicion;
                    ws.Cell(row, 4).Value = item.HoraEntrada;
                    ws.Cell(row, 5).Value = item.HoraSalida;
                    ws.Cell(row, 6).Value = item.HoraEntradaROL;
                    ws.Cell(row, 7).Value = item.HoraSalidaROL;
                    ws.Cell(row, 8).Value = item.Name;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"DetalleMarcaje_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public async Task<ActionResult> DetalleVacaciones(FiltroVacaciones model)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            List<DetalleVacaciones> listaVacaciones = new List<DetalleVacaciones>();
            filtro = model;

            var fechas = new FiltroVacaciones
            {
                FechaInicio = (model.FechaInicio < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : model.FechaInicio,
                FechaFin = (model.FechaFin < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : model.FechaFin
            };

            ViewBag.FechaInicio = fechas.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechas.FechaFin.ToString("yyyy-MM-dd");
            ViewBag.Areas = await service.ObtenerAreas();
            ViewBag.Departamentos = await service.ObtenerDepartamentos(model.CodDepartamento);

            try
            {
                listaVacaciones = await service.ObternerDetalleVacaciones(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(listaVacaciones);
        }

        public async Task<ActionResult> ExportarVacacionesExcel()
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            var service = new DALPortalRRHH();
            var data = await service.ObternerDetalleVacaciones(filtro);

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Detalle Marcaje");

                ws.Cell(1, 1).Value = "Tipo De Accion";
                ws.Cell(1, 2).Value = "Fecha";
                ws.Cell(1, 3).Value = "Empleado";
                ws.Cell(1, 4).Value = "Tipo Ausencia";
                ws.Cell(1, 5).Value = "Fecha_rige";
                ws.Cell(1, 6).Value = "Fecha_vence";
                ws.Cell(1, 7).Value = "Dias acción";

                int row = 2;
                foreach (var item in data)
                {
                    ws.Cell(row, 1).Value = "VACG";
                    ws.Cell(row, 2).Value = item.Fecha.ToString("dd/MM/yyyy");
                    ws.Cell(row, 3).Value = item.U_CodEmpleado;
                    ws.Cell(row, 4).Value = "VAC";
                    ws.Cell(row, 5).Value = item.FechaInicio.ToString("dd/MM/yyyy");
                    ws.Cell(row, 6).Value = item.FechaFin.ToString("dd/MM/yyyy");
                    ws.Cell(row, 7).Value = item.DiasAccion;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"Incidencia_Gose_Vacaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public async Task<ActionResult> Permisos(MarcajeFiltrosEntiry model = null)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            var fechas = new MarcajeFiltrosEntiry
            {
                FechaInicio = (model.FechaInicio < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : model.FechaInicio,
                FechaFin = (model.FechaFin < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : model.FechaFin,
                area = model.area,
                estado = model.estado
            };

            ViewBag.FechaInicio = fechas.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechas.FechaFin.ToString("yyyy-MM-dd");

            string localIP = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                // Filtra por direcciones IPv4
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            ViewBag.EsJefe = await service.EsJefe(sessions.CodeEmpleado);
            ViewBag.TiposPermiso = await service.ObternerTiposPermisos();
            ViewBag.UserNombre = sessions.UserName;
            ViewBag.UserCodigo = sessions.CodeEmpleado;

            var permisos = await service.ObtenerPermisosPorEmpleado(sessions,fechas);
            return View(permisos);
        }


        [HttpPost]
        public async Task<JsonResult> ObtenerDepartamentosPorArea(int codigoArea)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            var departamentos = await service.ObtenerDepartamentos(codigoArea);

            var resultado = departamentos.Select(d => new
            {
                Codigo = d.CodigoDepartamento,
                Nombre = d.Departamento
            });

            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> BuscarEmpleadosPorNombre(string nombre)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            var empleados = await service.BuscarEmpleadosPorNombre(nombre, sessions); // Devuelve List<EmpleadoDto>

            var resultado = empleados.Select(e => new
            {
                CodigoEmpleado = e.CodigoEmpleado,
                Nombre = e.Nombre,
                Departamento = e.Departamento
            });

            return Json(resultado, JsonRequestBehavior.AllowGet); // Usa solo Json(...) si estás en .NET Core
        }

        [HttpPost]
        public async Task<ActionResult> GuardarPermisos(detallePermiso permisos)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            if (sessions == null)
            {
                return View();
            }

            int LineId = await service.LineIdPermiso(permisos.CodigoEmpleado);
            int existe = await service.ExisteRegistroPermiso(permisos.CodigoEmpleado);
            string url = existe == 1 ? "Permisos/Update/" : "Permisos/New/";
            PermisosEntity permiso = new PermisosEntity
            {
                CodigoEmpleado = permisos.CodigoEmpleado,
                IdFirmante = sessions.CodeEmpleado,
                Existe = existe,
                Detalle = new List<detalle> {
                    new detalle {
                    FechaInicio = permisos.FechaInicio,
                    FechaFin = permisos.FechaFin,
                    FechaRegistro = DateTime.Now,
                    Tipo = permisos.Tipo,
                    Observaciones = permisos.Observaciones,
                    Solicita = permisos.Solicita,
                    LineId = LineId
                    }
                }
            };

            if (permiso != null)
            {
                string contenido = DAL_API.CrearPermiso(url, permiso);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
                return Json(datos);
            }

            return Json(new { success = false, message = "Hubo un error al intentar guardar el permiso." });
        }
        [HttpPost]
        public async Task<ActionResult> AgregarVoBoRh(detallePermiso permisos)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            if (sessions == null)
            {
                return View();
            }

            string url = "Permisos/VoBoRH/";

            PermisosEntity permiso = new PermisosEntity
            {
                CodigoEmpleado = permisos.CodigoEmpleado,
                IdFirmante = sessions.CodeEmpleado,
                Detalle = new List<detalle> {
                    new detalle {
                    FechaInicio = permisos.FechaInicio,
                    FechaFin = permisos.FechaFin,
                    FechaRegistro = DateTime.Now,
                    Tipo = permisos.Tipo,
                    Observaciones = permisos.Observaciones,
                    Solicita = permisos.Solicita,
                    rrhh = permisos.rrhh,
                    LineId = permisos.LineId
                    }
                }
            };

            if (permiso != null)
            {
                string contenido = DAL_API.CrearPermiso(url, permiso);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
                return Json(datos);
            }

            return Json(new { success = false, message = "Hubo un error al intentar guardar el permiso." });
        }

        [HttpPost]
        public async Task<ActionResult> AutorizarRechazarPermisos(int CodigoEmpleado, int LineId, int Id)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }

            PermisosEntity permiso = new PermisosEntity
            {
                CodigoEmpleado = CodigoEmpleado,
                IdFirmante = sessions.CodeEmpleado,
                Detalle = new List<detalle> {
                    new detalle {
                    LineId = LineId,
                    Id = Id
                    }
                }
            };

            string url = "Permisos/AutorizarRechazarPermiso/";

            if (permiso != null)
            {
                string contenido = DAL_API.AutorizarRechazarPermiso(url, permiso);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
                return Json(datos);
            }

            return Json(new { success = false, message = "Hubo un error al procesar la cotización." });
        }


        [HttpPost]
        public async Task<ActionResult> GeneratePdfAjax(GetPermisoEntity permiso)
        {
            var htmlFilePath = Server.MapPath("~/Html/PermisoVacaciones.html");
            var htmlContent = System.IO.File.ReadAllText(htmlFilePath);
            List<FirmaDigitalEntity> FimaDigital = await service.ObtenerFirmaDigital(permiso.CodigoEmpleado, permiso.LineId);

            htmlContent = htmlContent.Replace("@Nombre", permiso.Nombre);
            htmlContent = htmlContent.Replace("@Puesto", permiso.Puesto);
            htmlContent = htmlContent.Replace("@Departamento", permiso.Departamento);
            htmlContent = htmlContent.Replace("@CodigoEmpleado", permiso.CodigoEmpleado.ToString());
            htmlContent = htmlContent.Replace("@FechaRegistro", permiso.FechaRegistro.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@FechaInicio", permiso.FechaInicio.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@FechaFin", permiso.FechaFin.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@DiasGozadados", permiso.DiasGozados.ToString());
            htmlContent = htmlContent.Replace("@FechaPresentarse", permiso.FechaPresentarse.ToString("dd/MM/yyyy"));
            
            if (FimaDigital.Count >= 1)
            {
                htmlContent = htmlContent.Replace("@ComentarioFirmaSolicitante", FimaDigital[0].Comentario);
                htmlContent = htmlContent.Replace("@FirmaSolicitante", FimaDigital[0].FirmaDigital);
            }
            else
            {
                htmlContent = htmlContent.Replace("@ComentarioFirmaSolicitante", "<br>");
                htmlContent = htmlContent.Replace("@FirmaSolicitante", "");
            }
            if (FimaDigital.Count >= 2)
            {
                htmlContent = htmlContent.Replace("@ComentarioAutorizador", FimaDigital[1].Comentario);
                htmlContent = htmlContent.Replace("@FirmaAutorizador", FimaDigital[1].FirmaDigital);
            }
            else
            {
                htmlContent = htmlContent.Replace("@ComentarioAutorizador", "<br>");
                htmlContent = htmlContent.Replace("@FirmaAutorizador", "");
            }

            if (FimaDigital.Count == 3)
            {
                htmlContent = htmlContent.Replace("@ComentarioRRHH", FimaDigital[2].Comentario);
                htmlContent = htmlContent.Replace("@FirmaRRHH", FimaDigital[2].FirmaDigital);
            }
            else
            {
                htmlContent = htmlContent.Replace("@ComentarioRRHH", "");
                htmlContent = htmlContent.Replace("@FirmaRRHH", "");
            }

            var htmlContentTotal = htmlContent;
            // Configuración del convertidor
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginTop = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginBottom = 20;
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 70;
            converter.Footer.DisplayOnEvenPages = false;
            //converter.Footer.Add(new PdfHtmlSection(foot(), string.Empty));

            using (var memoryStream = new MemoryStream())
            {
                PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContentTotal);
                pdfDocument.Save(memoryStream);
                pdfDocument.Close();

                return File(memoryStream.ToArray(), "application/pdf", "Permiso.pdf");
            }
        }
        public async Task<ActionResult> GeneratePdfAjaxPE(GetPermisoEntity permiso)
        {
            var htmlFilePath = Server.MapPath("~/Html/PermisosEspeciales.html");
            List<FirmaDigitalEntity> FimaDigital = await service.ObtenerFirmaDigital(permiso.CodigoEmpleado, permiso.LineId);

            var htmlContent = System.IO.File.ReadAllText(htmlFilePath);
            string[] checkedCheck = new string[3];

            checkedCheck[0] = permiso.TipoPermiso == "Enfermedad con constancia IGSS" ? "Checked" : "";
            checkedCheck[1] = permiso.TipoPermiso == "Emergencia personal o familiar" ? "Checked" : "";
            checkedCheck[2] = permiso.TipoPermiso == "Permiso especial" ? "Checked" : "";

            string checkContainer = @"<div class='checkbox-container'>" +
                    " <div class='checkbox-item'>" +
                    $"     <input type='checkbox' id='check1' {checkedCheck[0]}>" +
                    "     <label for='check1'>Enfermedad con constancia IGSS</label>" +
                    " </div>" +
                    " <div class='checkbox-item'>" +
                    $"     <input type='checkbox' id='check3' {checkedCheck[1]}>" +
                    "     <label for='check3'>Emergencia personal o familiar</label>" +
                    " </div>" +
                    " <div class='checkbox-item'>" +
                    $"     <input type='checkbox' id='check4' {checkedCheck[2]}>" +
                    "     <label for='check4'>Permiso especial (especificar)</label>" +
                    " </div>" +
                "</div>";


            htmlContent = htmlContent.Replace("@Nombre", permiso.Nombre);
            htmlContent = htmlContent.Replace("@Puesto", permiso.Puesto);
            htmlContent = htmlContent.Replace("@Departamento", permiso.Departamento);
            //htmlContent = htmlContent.Replace("@CodigoEmpleado", permiso.CodigoEmpleado.ToString());
            htmlContent = htmlContent.Replace("@FechaRegistro", permiso.FechaRegistro.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@FechaInicio", permiso.FechaInicio.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@FechaFin", permiso.FechaFin.ToString("dd/MM/yyyy"));
            htmlContent = htmlContent.Replace("@ContainerCheck", checkContainer);
            htmlContent = htmlContent.Replace("@U_Observacion", permiso.U_Observacion);

            if (FimaDigital.Count >= 1)
            {
                htmlContent = htmlContent.Replace("@ComentarioFirmaSolicitante", FimaDigital[0].Comentario);
                htmlContent = htmlContent.Replace("@FirmaSolicitante", FimaDigital[0].FirmaDigital);
            }
            else
            {
                htmlContent = htmlContent.Replace("@ComentarioFirmaSolicitante", "<br>");
                htmlContent = htmlContent.Replace("@FirmaSolicitante", "");
            }
            if (FimaDigital.Count >= 2)
            {
                htmlContent = htmlContent.Replace("@ComentarioAutorizador", FimaDigital[1].Comentario);
                htmlContent = htmlContent.Replace("@FirmaAutorizador", FimaDigital[1].FirmaDigital);
            }
            else
            {
                htmlContent = htmlContent.Replace("@ComentarioAutorizador", "<br>");
                htmlContent = htmlContent.Replace("@FirmaAutorizador", "");
            }

            if (FimaDigital.Count == 3)
            {
                htmlContent = htmlContent.Replace("@ComentarioRRHH", FimaDigital[2].Comentario);
                htmlContent = htmlContent.Replace("@FirmaRRHH", FimaDigital[2].FirmaDigital);
            }
            else
            {
                htmlContent = htmlContent.Replace("@ComentarioRRHH", "");
                htmlContent = htmlContent.Replace("@FirmaRRHH", "");
            }



            var htmlContentTotal = htmlContent;
            // Configuración del convertidor
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.Letter;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginTop = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginBottom = 20;

            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 70;
            converter.Footer.DisplayOnEvenPages = false;
            //converter.Footer.Add(new PdfHtmlSection(foot(), string.Empty));

            using (var memoryStream = new MemoryStream())
            {
                PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContentTotal);
                pdfDocument.Save(memoryStream);
                pdfDocument.Close();

                return File(memoryStream.ToArray(), "application/pdf", "Permiso.pdf");
            }
        }

        [HttpPost]
        public async Task<JsonResult> GuardarCompensacionDeTiempo(int IdEmpleado, DateTime Fecha, string HoraInicioCompensada, string HoraFinCompensada, string Comentario)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            List<DetalleMarcaje> listaMarcaje = new List<DetalleMarcaje>();

            try
            {
                //listaMarcaje = await service.ObtenerAreas;
                return Json(new { success = true, data = listaMarcaje });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}