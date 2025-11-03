using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PORTALI.Controllers
{
    public class ProspAgendaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListadoDeptos(int IdRegion, string WhsCode)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<DepartamentosGtEntity> lista = DALPortalGenerales.getAllDepartamentosgt(IdRegion, WhsCode);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListadoMunicipios(int IdDepto)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<MunicipiosEntity> lista = DALPortalGenerales.getAllMunicipios(IdDepto);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListadoZonas()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            List<ZonasEntity> lista = DALPortalGenerales.getAllZonas();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarData()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            List<ListadoAgendaEntity> lista = DALObras.ListadoAgenda(sessions.Region, sessions.WhsCode, sessions.SlpCode, null, null, sessions.UserId);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarProgramacion(AgendaObrasEntity agendaObrasEntity)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            string contenido = DAL_API.CrearCotizacionVenta("Obras/Agenda/", agendaObrasEntity);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarPuntoDePartida(RecorridoEntity recorridoEntity, bool validar)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            string ruta = "";
            if (!validar)
            {
                ruta = "Obras/PuntoDePartida/";
            }
            else 
            {
                ruta = "Obras/PuntoFinal/";
                recorridoEntity.U_LongitudFinal = recorridoEntity.U_Longitud;
                recorridoEntity.U_LatitudFinal = recorridoEntity.U_Latitud;
                recorridoEntity.U_LugarFinal = recorridoEntity.U_Lugar;
            }
            
            string contenido = DAL_API.CrearCotizacionVenta(ruta, recorridoEntity);
            Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> CargarDetalle(int IdPlan)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            // Esperamos el resultado de forma asíncrona
            List<PlanificacionDetalleModel> lista = await DALObras.DetallePlanificacion(IdPlan);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}