using DAL;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace PORTALI.Controllers
{
    public class FormularioCheckListController : Controller
    {
        private readonly DALFormularioCheckList _dal = new DALFormularioCheckList();

        private async Task CargarListasDesplegables()
        {
            ViewBag.Sucursales = new SelectList(await _dal.ObtenerSucursal(), "Codigo", "Nombre");
            ViewBag.SupBodega = new SelectList(await _dal.ObtenerSupervisoresBodega(), "Codigo", "Nombre");
            ViewBag.SupVenta = new SelectList(await _dal.ObtenerSupervisoresVenta(), "Codigo", "Nombre");
            ViewBag.Area = new SelectList(await _dal.ObtenerArea(), "Codigo", "Nombre");
            ViewBag.GerenteTienda = new SelectList(await _dal.ObtenerGerenteTiendas(), "Codigo", "Nombre");
            ViewBag.EncargadoBodega = new SelectList(await _dal.ObtenerEncargadosBodega(), "Codigo", "Nombre");
        }

        public ActionResult CargarSeccion(int seccion)
        {
            var modelo = new FormularioCheckList1Entity();

            switch (seccion)
            {
                case 1:
                    return PartialView("_Seccion1DatosGenerales", modelo);
                case 2:
                    return PartialView("_Seccion2Almacenamiento", modelo);
                case 3:
                    return PartialView("_Seccion3Recepcion", modelo);
                case 4:
                    return PartialView("_Seccion4Despacho", modelo);
                case 5:
                    return PartialView("_Seccion5Daniado", modelo);
                case 6:
                    return PartialView("_Seccion6_5S", modelo);
                default:
                    return PartialView("_Seccion1DatosGenerales", modelo);
            }
        }
        public JsonResult EnviarSeccion(FormularioCheckList1Entity modelo)
        {
            modelo.U_Alma_ADPA1 = null;
            modelo.U_Alma_ADPA2 = null;
            modelo.U_Alma_ADPA3 = null;
            modelo.U_Alma_ADPA4 = null;
            modelo.U_Alma_ADPA5 = null;
            modelo.U_Alma_ADPA6 = null;
            modelo.U_Alma_ADPA7 = null;

            modelo.U_Recep_ADPR1 = null;
            modelo.U_Recep_ADPR2 = null;
            modelo.U_Recep_ADPR3 = null;
            modelo.U_Recep_ADPR4 = null;
            modelo.U_Recep_ADPR5 = null;
            modelo.U_Recep_ADPR6 = null;
            modelo.U_Recep_ADPR7 = null;

            modelo.U_Desp_ADPH1 = null;
            modelo.U_Desp_ADPH2 = null;
            modelo.U_Desp_ADPH3 = null;
            modelo.U_Desp_ADPH4 = null;
            modelo.U_Desp_ADPH5 = null;
            modelo.U_Desp_ADPH6 = null;
            modelo.U_Desp_ADPH7 = null;

            modelo.U_Dan_ADPD1 = null;
            modelo.U_Dan_ADPD2 = null;
            modelo.U_Dan_ADPD3 = null;
            modelo.U_Dan_ADPD4 = null;
            modelo.U_Dan_ADPD5 = null;
            modelo.U_Dan_ADPD6 = null;
            modelo.U_Dan_ADPD7 = null;

            modelo.U_5S_ADPS1 = null;
            modelo.U_5S_ADPS2 = null;
            modelo.U_5S_ADPS3 = null;
            modelo.U_5S_ADPS4 = null;
            modelo.U_5S_ADPS5 = null;
            modelo.U_5S_ADPS6 = null;
            modelo.U_5S_ADPS7 = null;

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            try
            {
                string url = "FormCheckList/Set/";
                string contenido = DAL_API.CrearFormulario(url, modelo);

                Reply datos;
                datos = JsonConvert.DeserializeObject<Reply>(contenido);

                if (datos.result == 1)
                {
                    return Json(new { success = true, data = datos.message });
                }
                else
                {
                    return Json(new { success = false, data = datos.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message });
            }
        }

        public async Task<ActionResult> Checklist(int nuevo = 0, string NumeroFormulario = "")
        {
            //0   Nuevo
            //-1  Incompleto
            //1   Completo
            FormularioCheckList1Entity formulario = new FormularioCheckList1Entity();
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            formulario.U_Alma_ADPA1 = null;
            Reply response = new Reply();
            string url = "FormCheckList/Set/";
            string _nombreResponsable = "";
            try
            {
                await CargarListasDesplegables();
                //var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }

                if (!string.IsNullOrEmpty(NumeroFormulario))
                {
                    formulario = await _dal.ObtenerFormuarioCheckList(NumeroFormulario);
                    if (nuevo == -1)
                    {
                        response.result = 2;
                        response.message = $"Formulario {NumeroFormulario} pendiente de completar";
                    }
                    else
                    {
                        response.result = 1;
                        response.message = $"Formulario {NumeroFormulario} completo";
                    }
                }
                else
                {
                    int _formularioPendiente = await _dal.FormularioPendiente(sessions.UserId.ToString());


                    if(_formularioPendiente != 0)
                    {
                        return RedirectToAction("Index",new {Pendiente = 1});   
                        //return Json(new {Result = -1 , Message = "Tienes un formulario pendiente, debes te"});
                    }

                    IdFormulario IdFormulario = await CrearNuevoNumeroDeFormulario();
                    formulario.Code = IdFormulario.Code;
                    formulario.Name = IdFormulario.Code;
                    formulario.U_IdFormulario = IdFormulario.Numero;
                    formulario.U_FechaCreado = DateTime.Now;
                    formulario.U_IdUsuario = sessions.UserId;

                    

                    string contenido = DAL_API.CrearFormulario(url, formulario);

                    response = JsonConvert.DeserializeObject<Reply>(contenido);

                    if (response.result == 1)
                    {
                        response.message = $"Formulario {IdFormulario.Numero} creado correctamente";
                    }
                }

                _nombreResponsable = await _dal.ObtenerNombreResponsable(formulario.U_IdUsuario.ToString());
            }
            catch (Exception ex)
            {
                formulario = new FormularioCheckList1Entity();
                response.result = -1;
                response.message = ex.Message;
            }

            ViewBag.nombreResponsable = _nombreResponsable;
            ViewBag.disabled = nuevo == 1 ? true : false;
            ViewBag.Error = response;
            return View(formulario);
        }

        public async Task<ActionResult> Index(DateTime? FechaInicial = null, DateTime? FechaFinal = null, int Estado = -1, int Pendiente = 0,string Tienda = null, int Region = -1)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<CheckListInfo> ListaCheckList = new List<CheckListInfo>();
            List <TiendasRegionesCheckListEntity> TiendasRegiones = new List<TiendasRegionesCheckListEntity>();
            DateTime FechaI = (DateTime)((FechaInicial == null) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : FechaInicial);
            DateTime FechaF = (DateTime)((FechaFinal == null) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : FechaFinal);
            Tienda = string.IsNullOrEmpty(Tienda) ? null : Tienda;
            try
            {
                if (sessions == null)
                {
                    return Json(new { success = false, message = "Sesión expirada" });
                }
                TiendasRegiones = await _dal.TiendasCheckLick(sessions.UserId);
                ListaCheckList = await _dal.ObtenerDetalleCheckList(FechaI, FechaF, Estado, sessions.UserId,Tienda,Region);

                if (ListaCheckList.Count == 0)
                {
                    ListaCheckList.Add(new CheckListInfo { Error = "No se encontraron datos" });
                }
            }
            catch (Exception ex)
            {
                ListaCheckList.Add(new CheckListInfo { Error = ex.Message });
            }
            

            int CantTiendas = TiendasRegiones.Count;
            int CantRegiones = TiendasRegiones.GroupBy(x => new { x.Code, x.Location }).Select(g => g.First()).ToList().Count;

            Tienda = CantTiendas == 1 ? TiendasRegiones[0].WhsCode : Tienda;
            Region = CantRegiones == 1 ? TiendasRegiones.GroupBy(x => new { x.Code, x.Location }).Select(g => g.First()).ToList()[0].Code : Region;
            ViewBag.DisabledTienda = CantTiendas == 1 ? true : false;
            ViewBag.DisabledRegion = CantRegiones == 1 ? true : false;
            ViewBag.Tienda = Tienda == null ? "" : Tienda;
            ViewBag.Region = Region;
            ViewBag.FechaI = FechaI.ToString("yyyy-MM-dd");
            ViewBag.FechaF = FechaF.ToString("yyyy-MM-dd");
            ViewBag.Estado = Estado;
            ViewBag.TiendasRegiones = TiendasRegiones;
            ViewBag.Pendient = await _dal.FormularioPendiente(sessions.UserId.ToString());
            return View(ListaCheckList);
        }

        private async Task<IdFormulario> CrearNuevoNumeroDeFormulario()
        {
            IdFormulario Id = new IdFormulario();

            try
            {
                Id = await _dal.ObtenerNuevoNumeroDeFormulario();
            }
            catch
            {
                Id.Code = "-1";
            }
            return Id;
        }

        [HttpPost]
        public JsonResult MoverImagen(string Id, string Name, string IdFormulario, HttpPostedFileBase Imagen, string Code)
        {
            try
            {
                if (Imagen != null && Imagen.ContentLength > 0)
                {

                    string path = @"\\SRVSAPTQ2\SAPDocs\Imagenes\";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string extension = Path.GetExtension(Imagen.FileName);

                    // Guardar en el servidor
                    string rutaCompleta = Path.Combine(path, $"{IdFormulario}-{Id}{extension}");
                    Imagen.SaveAs(rutaCompleta);

                    GuardarImagenFormulario(Name, Code,Id,IdFormulario);

                    return Json(new { exito = true, nombre = $"{IdFormulario}-{Id}{extension}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
            return Json(new { });
        }

        private void GuardarImagenFormulario(string name, string code, string Id,string IdFormulario)
        {
            Reply response = new Reply();
            string url = "FormCheckList/Set/";
            FormularioCheckList1Entity formulario = new FormularioCheckList1Entity();
            var propiedad = typeof(FormularioCheckList1Entity).GetProperty(name);
            if (propiedad != null && propiedad.CanWrite)
            {
                formulario.Code = code;
                formulario.Name = code;
                propiedad.SetValue(formulario, $"{IdFormulario}-{Id}");

                string contenido = DAL_API.CrearFormulario(url, formulario);

                response = JsonConvert.DeserializeObject<Reply>(contenido);
            }
        }
        public ActionResult VerImagen(string archivo)
        {
            string carpeta = @"\\SRVSAPTQ2\SAPDocs\Imagenes\";

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
                ruta = Path.Combine(carpeta, "NoPhoto.png");
                if (!System.IO.File.Exists(ruta))
                {
                    return HttpNotFound("No se encontró la imagen ni el archivo NoPhoto.jpg");
                }
            }

            string contentType = MimeMapping.GetMimeMapping(ruta);
            return File(ruta, contentType);
        }


        public async Task<ActionResult> ExportarExcel(DateTime? FechaInicial = null, DateTime? FechaFinal = null, int Estado = -1, int Pendiente = 0, string Tienda = null, int Region = -1)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (sessions == null)
            {
                return Json(new { success = false, message = "Sesión expirada" });
            }
            DateTime FechaI = (DateTime)((FechaInicial == null) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : FechaInicial);
            DateTime FechaF = (DateTime)((FechaFinal == null) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : FechaFinal);


            var data = await _dal.ObtenerDetalleCheckListExcel(FechaI, FechaF, Estado, sessions.UserId,Tienda,Region); // Puede ser async si necesitas

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Detalle Marcaje");

                // Encabezados
                ws.Cell(1, 1).Value = "Correlativo";
                ws.Cell(1, 2).Value = "Fecha";
                ws.Cell(1, 3).Value = "Nombre";
                ws.Cell(1, 4).Value = "Tienda";
                ws.Cell(1, 5).Value = "Bodega";
                ws.Cell(1, 6).Value = "Estado";
                ws.Cell(1, 7).Value = "Alm1";
                ws.Cell(1, 8).Value = "Alm2";
                ws.Cell(1, 9).Value = "Alm3";
                ws.Cell(1, 10).Value = "Alm4";
                ws.Cell(1, 11).Value = "Alm5";
                ws.Cell(1, 12).Value = "Alm6";
                ws.Cell(1, 13).Value = "Alm7";
                ws.Cell(1, 14).Value = "Rec1";
                ws.Cell(1, 15).Value = "Rec2";
                ws.Cell(1, 16).Value = "Rec3";
                ws.Cell(1, 17).Value = "Rec4";
                ws.Cell(1, 18).Value = "Rec5";
                ws.Cell(1, 19).Value = "Rec6";
                ws.Cell(1, 20).Value = "Rec7";
                ws.Cell(1, 21).Value = "Des1";
                ws.Cell(1, 22).Value = "Des2";
                ws.Cell(1, 23).Value = "Des3";
                ws.Cell(1, 24).Value = "Des4";
                ws.Cell(1, 25).Value = "Des5";
                ws.Cell(1, 26).Value = "Des6";
                ws.Cell(1, 27).Value = "Des7";
                ws.Cell(1, 28).Value = "Dan1";
                ws.Cell(1, 29).Value = "Dan2";
                ws.Cell(1, 30).Value = "Dan3";
                ws.Cell(1, 31).Value = "Dan4";
                ws.Cell(1, 32).Value = "Dan5";
                ws.Cell(1, 33).Value = "Dan6";
                ws.Cell(1, 34).Value = "Dan7";
                ws.Cell(1, 35).Value = "5S1";
                ws.Cell(1, 36).Value = "5S2";
                ws.Cell(1, 37).Value = "5S3";
                ws.Cell(1, 38).Value = "5S4";
                ws.Cell(1, 39).Value = "5S5";
                ws.Cell(1, 40).Value = "5S6";
                ws.Cell(1, 41).Value = "5S7";
                ws.Cell(1, 42).Value = "Puntaje Almacenamiento";
                ws.Cell(1, 43).Value = "Puntaje Recepcion";
                ws.Cell(1, 44).Value = "Puntaje Despacho";
                ws.Cell(1, 45).Value = "Puntaje Dañado";
                ws.Cell(1, 46).Value = "Puntaje 5S";
                ws.Cell(1, 47).Value = "Puntaje General";

                int row = 2;
                foreach (var item in data)
                {
                    ws.Cell(row, 1).Value = item.Correlativo;
                    ws.Cell(row, 2).Value = item.Fecha.ToString("dd/MM/yyyy");
                    ws.Cell(row, 3).Value = item.Nombre;
                    ws.Cell(row, 4).Value = item.Tienda;
                    ws.Cell(row, 5).Value = item.Bodega;
                    ws.Cell(row, 6).Value = item.Estado;
                    ws.Cell(row, 7).Value = item.Alm1;
                    ws.Cell(row, 8).Value = item.Alm2;
                    ws.Cell(row, 9).Value = item.Alm3;
                    ws.Cell(row, 10).Value = item.Alm4;
                    ws.Cell(row, 11).Value = item.Alm5;
                    ws.Cell(row, 12).Value = item.Alm6;
                    ws.Cell(row, 13).Value = item.Alm7;
                    ws.Cell(row, 14).Value = item.Rec1;
                    ws.Cell(row, 15).Value = item.Rec2;
                    ws.Cell(row, 16).Value = item.Rec3;
                    ws.Cell(row, 17).Value = item.Rec4;
                    ws.Cell(row, 18).Value = item.Rec5;
                    ws.Cell(row, 19).Value = item.Rec6;
                    ws.Cell(row, 20).Value = item.Rec7;
                    ws.Cell(row, 21).Value = item.Des1;
                    ws.Cell(row, 22).Value = item.Des2;
                    ws.Cell(row, 23).Value = item.Des3;
                    ws.Cell(row, 24).Value = item.Des4;
                    ws.Cell(row, 25).Value = item.Des5;
                    ws.Cell(row, 26).Value = item.Des6;
                    ws.Cell(row, 27).Value = item.Des7;
                    ws.Cell(row, 28).Value = item.Dan1;
                    ws.Cell(row, 29).Value = item.Dan2;
                    ws.Cell(row, 30).Value = item.Dan3;
                    ws.Cell(row, 31).Value = item.Dan4;
                    ws.Cell(row, 32).Value = item.Dan5;
                    ws.Cell(row, 33).Value = item.Dan6;
                    ws.Cell(row, 34).Value = item.Dan7;
                    ws.Cell(row, 35).Value = item.S1;
                    ws.Cell(row, 36).Value = item.S2;
                    ws.Cell(row, 37).Value = item.S3;
                    ws.Cell(row, 38).Value = item.S4;
                    ws.Cell(row, 39).Value = item.S5;
                    ws.Cell(row, 40).Value = item.S6;
                    ws.Cell(row, 41).Value = item.S7;
                    ws.Cell(row, 42).Value = item.U_PuntajeAlmacenaje;
                    ws.Cell(row, 43).Value = item.U_PuntajeRecepcion;
                    ws.Cell(row, 44).Value = item.U_PuntajeDespacho;
                    ws.Cell(row, 45).Value = item.U_PuntajeDaniado;
                    ws.Cell(row, 46).Value = item.U_Puntaje5S;
                    ws.Cell(row, 47).Value = item.PuntajeGeneral;
                    
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"CheckList{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

    }
}