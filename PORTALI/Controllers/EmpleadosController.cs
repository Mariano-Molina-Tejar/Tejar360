using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL;
using DocumentFormat.OpenXml.VariantTypes;
using Entity;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class EmpleadosController : Controller
    {
        private EmpleadosDAL _dal = new EmpleadosDAL();

        public async Task<ActionResult> Index()
        {
            try
            {
                var empleados = await _dal.ObtenerListaEmpledos();
                var posicion = await _dal.ObtenerPosiciones();
                var departamentos = await _dal.ObtenerTodosLosDepartamentos();
                var tiendas = await _dal.ObtenerListadoDeTiendas();
                var areas = await _dal.ObtenerAreas();

                var empleadosViewModel = new EmpleadosViewModel
                {
                    Empleados = empleados.OrderBy(x => x.Nombre),
                    Posicion = posicion,
                    Departamentos = departamentos,
                    Tiendas = tiendas,
                    Areas = areas
                };

                return View(empleadosViewModel);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GuardarPosicion(string nombre, string posicion)
        {
            try
            {
                string url = "GestionDePersonal/AgregarPosicion";
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];

                string response = DAL_API.NotasPpto(url, new { name = nombre, description = posicion });

                var reply = JsonConvert.DeserializeObject<Reply>(response);
                var json = JsonConvert.DeserializeObject<dynamic>(reply.data.ToString());
                string PositionID = (string)json.PositionID ?? "";

                if (reply.result == 1)
                    return Json(new { success = true, positionID = PositionID });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public async Task<JsonResult> ObtenerInforacionEmpleado(int empId)
        {
            try
            {
                var infoEmpleado = await _dal.ObtenerInformacionEmpleado(empId);

                if (string.IsNullOrEmpty(infoEmpleado.firstName))
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, data = infoEmpleado }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GuardarInformacionEmpleado(EmpleadoActualizar infoEmpleado)
        {
            try
            {
                string url = "GestionDePersonal/ActualizarInfoEmpleado";
                var sessions = (SessionLoginEntity)Session["PropertiesEntity"];

                string response = DAL_API.NotasPpto(url, infoEmpleado);

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return Json(new { success = true });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GuardarGerenteDepartamento(int gerente, int departamento)
        {
            try
            {
                string url = "GestionDePersonal/GuardarGerenteDepartamento";

                var existe = await _dal.ExisteDatoGerente(departamento);

                string response = DAL_API.NotasPpto(url, new { Departamento = departamento, Gerente = gerente, Update = existe });

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return Json(new { success = true });

                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}