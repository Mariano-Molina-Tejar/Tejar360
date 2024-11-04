using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Delivery
        public ActionResult Recepcion()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<RecepcionProductosEntity> _listado = new List<RecepcionProductosEntity>();
            _listado = DALPortalRecepcionProductos.getAll(sessions.Depto, 0);
            return View(_listado);
        }
        public ActionResult DetalleModal(int DocNum)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            RecepcionProductosModalDetalleEntity _data = new RecepcionProductosModalDetalleEntity();
            _data = DALPortalRecepcionProductos.getSingle(sessions.Depto, DocNum);
            _data.Detalle = DALPortalRecepcionProductos.getDetalleProducts(DocNum);
            _data.ListaEstados = DALPortalRecepcionProductos.getAllEstadosRecepcion(_data.IdEstado.ToString());
            _data.logRecepcion = DALPortalRecepcionProductos.getLogRecepcion(_data.DocNumOc.ToString());
            return PartialView("_ModalDetalle", _data);
        }
        public ActionResult AceptarMercaderia()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            var archivos = Request.Files;
            List<HttpPostedFileBase> listaArchivos = new List<HttpPostedFileBase>();
            for (int i = 0; i < archivos.Count; i++)
            {
                listaArchivos.Add(archivos[i]);
            }
            var jsonData = Request.Form["jsonData"];
            AceptarMercaderiaEntity delivery = JsonConvert.DeserializeObject<AceptarMercaderiaEntity>(jsonData);
            delivery.User = sessions.UserCode;
            if (listaArchivos.Count > 0)
            {
                delivery.Files = listaArchivos;
            }

            string contenido = DALPortalSolicitudCompra.CrearSolicitud("Recepcion/Registro", delivery);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        public class AceptarMercaderiaEntity
        {
            public int DocEntry { get; set; }
            public int DocNum { get; set; }
            public string Comments { get; set; }
            public string User { get; set; }
            public string IdEstado { get; set; }

            [JsonIgnore]
            public List<HttpPostedFileBase> Files { get; set; }
            public List<FileDto> FilesInfo
            {
                get
                {
                    return Files?.Select(file => new FileDto
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        FileSize = file.ContentLength,
                        FileBase64 = DALEntity.GetFileBase64(file)
                    }).ToList();
                }
            }
        }      
    }
}