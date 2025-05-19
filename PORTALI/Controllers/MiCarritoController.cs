using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DAL;
using Entity;
using Newtonsoft.Json;

namespace PORTALI.Controllers
{
    public class MiCarritoController : Controller
    {
        [System.Web.Http.HttpGet]
        public ActionResult ListadoCarrito()
        {
            // Se envía un modelo vacío inicialmente
            var carrito = new CarritoComprasEntity
            {
                Detalle = new List<DetalleCarritoEntity>()
            };
            return View(carrito);
        }

        [System.Web.Http.HttpPost]
        public ActionResult GuardarCarrito([FromBody] CarritoComprasEntity carrito,int PriceList, int Btn)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];

            if (carrito != null && carrito.Detalle.Any())
            {
                if(Btn == 1)
                {
                    string data = string.Join(",", carrito.Detalle.Select(item => item.ItemCode));
                    List<ItemsCotizacionEntity> _lista = DAL.DALPortalCarritoCompras.CambioPrecioCotizacion(PriceList, data, sessions.WhsCode);
                    foreach (var detalle in carrito.Detalle)
                    {
                        // Buscar el item correspondiente en la lista de cotización (_lista)
                        var itemCotizacion = _lista.FirstOrDefault(i => i.ItemCode == detalle.ItemCode);

                        // Si se encuentra el item en _lista, actualizar el precio
                        if (itemCotizacion != null)
                        {
                            detalle.Price = itemCotizacion.PreciUnitAlto;
                            detalle.Dscto = itemCotizacion.DescuentoQ;
                            detalle.DescuentoU = detalle.DescuentoQtz;
                            detalle.LineTotal = (detalle.Price - (itemCotizacion.DescuentoQ + detalle.DescuentoU)) * detalle.Quantity;
                        }
                    }
                }
                return PartialView("_ModalDetalle", carrito);
            }
            else
            {   
                return Json(new { success = false, message = "No se recibieron datos válidos." });
            }
        }

        public ActionResult BuscarNitClientes(string Nit) 
        {
            ClienteSATEntity clienteSATEntity = new ClienteSATEntity();
            clienteSATEntity = DALPortalGenerales.ConsultarNIT(Nit);
            if (clienteSATEntity != null)
            {
                clienteSATEntity.Nit = clienteSATEntity.Nit;
                clienteSATEntity.Nombre = clienteSATEntity.Nombre;
                clienteSATEntity.Direccion = clienteSATEntity.Direccion;                
            }

            return Json(clienteSATEntity, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarSocioNegocio(string Nit)
        {
            ClienteSATEntity clienteSATEntity = new ClienteSATEntity();
            CarritoComprasSocioEntity carritoComprasSocioEntity = DAL.DALPortalCarritoCompras.getSingleSocio(Nit);

            clienteSATEntity = DALPortalGenerales.ConsultarNIT(Nit);
            if (clienteSATEntity != null)
            {
                carritoComprasSocioEntity.CardName = VoltearNombres(clienteSATEntity.Nombre).ToUpper().Replace(",", " ").Replace("  ", " ").Trim();
                carritoComprasSocioEntity.FacturarNit = clienteSATEntity.Nit.ToUpper();
                carritoComprasSocioEntity.FacturarNombre = clienteSATEntity.Nombre.ToUpper();
                carritoComprasSocioEntity.FacturarDireccion = clienteSATEntity.Direccion;
                carritoComprasSocioEntity.ExisteNit = "Y";
            }

            return Json(carritoComprasSocioEntity, JsonRequestBehavior.AllowGet);
        }

        protected string VoltearNombres(string input)
        {
            string[] partes = input.Split(new string[] { ",," }, StringSplitOptions.None);
            string resultado = "";

            if (partes.Length == 2)
            {
                // Rearmar la cadena con la segunda parte al inicio
                resultado = partes[1] + ",," + partes[0];                
            }
            return resultado;
        }

        [System.Web.Http.HttpGet]
        public PartialViewResult LoadPartialView(int step)
        {
            switch (step)
            {
                case 1:
                    return PartialView("MiCarrito");
                case 2:
                    return PartialView("SocioNegocio");
                case 3:
                    var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
                    return PartialView("Resumen", sessions);
                default:
                    return PartialView("MiCarrito");
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult EnviarCotizacionSAP(CarritoComprasEntity carrito)
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];
            if (carrito != null)
            {
                //Realizar alguna lógica para enviar la cotización a SAP o realizar cualquier otra acción
                carrito.SlpCode = sessions.SlpCode;
                carrito.UserCode = sessions.UserCode;
                string contenido = DAL_API.CrearCotizacionVenta("CarritoCompras/New/", carrito);
                Reply datos = JsonConvert.DeserializeObject<Reply>(contenido);
                return Json(datos, JsonRequestBehavior.AllowGet);
                //return Json("", JsonRequestBehavior.AllowGet);
            }

            // Si el carrito es nulo o hay algún problema, retornar un mensaje de error
            return Json(new { success = false, message = "Hubo un error al procesar la cotización." });
        }

        public ActionResult TablaDescuentos(string ItemCode)
        {
            List<ListaPreciosEntity> listaPreciosEntities = DALPortalInventario.ListaPreciosVentas(ItemCode);
            return Json(listaPreciosEntities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllTiendas()
        {
            List<PortalTiendasEntity> listaTiendas = DALPortalCotizaciones.getTiendas();
            return Json(listaTiendas, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        public JsonResult getAsesoresPorTienda(string WhsCode)
        {
            if (string.IsNullOrEmpty(WhsCode))
            {
                return Json(new { error = "WhsCode es requerido" }, JsonRequestBehavior.AllowGet);
            }

            List<ListaAsesoresEntity> listaUsuarios = DALPortalCotizaciones.getAllUsers(WhsCode);
            return Json(listaUsuarios, JsonRequestBehavior.AllowGet);
        }
    }
}