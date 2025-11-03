using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace PORTALI.Controllers
{
    public class ProspVisitaController : Controller
    {
        // GET: ProspVisita
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Guardar(List<HttpPostedFileBase> fotos, List<string> nombresFotos, string jsonData, string listaContactos)
        {
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            ProspeccionDataEntity prospeccionDataEntity = new ProspeccionDataEntity();

            var prospeccionVisitaEntity = JsonConvert.DeserializeObject<ProspeccionVisitaEntity>(jsonData);
            var contactos = JsonConvert.DeserializeObject<List<VisitasContactosEntity>>(listaContactos);
            prospeccionVisitaEntity.U_Usuario = sessions.UserId;


            prospeccionDataEntity.Fotografias = new List<VisitasFotografiasEntity>();
            if (fotos != null && fotos.Count > 0)
            {
                for (int i = 0; i < fotos.Count; i++)
                {
                    var file = fotos[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = "Foto_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads/Prospeccion/"), fileName);

                        // Guardar la foto (puedes incluir optimización si quieres)
                        file.SaveAs(path);

                        // Capturar el nombre enviado desde el front
                        string nombreFoto = (nombresFotos != null && nombresFotos.Count > i) ? nombresFotos[i] : "";

                        prospeccionDataEntity.Fotografias.Add(new VisitasFotografiasEntity
                        {
                            U_Url = fileName,       // nombre físico en el servidor
                            U_Notas = nombreFoto     // nombre personalizado de la foto
                        });
                    }
                }
            }

            
            prospeccionDataEntity.Encabezado = prospeccionVisitaEntity;
            prospeccionDataEntity.Contactos = contactos;

            string contenido = DAL_API.CrearCotizacionVenta("Obras/Visita/", prospeccionDataEntity);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);

            return Json(datos, JsonRequestBehavior.AllowGet);
        }



        public ActionResult getAllList(int Tipo)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ListadosProspeccionEntity> lista = DALProspeccion.getAllList(Tipo);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}