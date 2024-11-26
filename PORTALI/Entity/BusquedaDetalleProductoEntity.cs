using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BusquedaDetalleProductoEntity
    {
        public int Rw { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string WhsCode { get; set; }
        public double Stock { get; set; }
        public string UrlImg { get; set; }
        public string Cuenta { get; set; }
        public string NombreCuenta { get; set; }
        public double Prespuesto { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public string NotasLine { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }

    public class GetAllProductsEntity 
    {
        public List<BusquedaDetalleProductoEntity> ListaUno { get; set; }
        public List<ResumenPresupuestoEntity> ListaDos { get; set; }
    }

    public class ResumenPresupuestoEntity 
    {
        public string Cuenta { get; set; }
        public string Nombre { get; set; }
        public double Presupuesto { get; set; }
        public double Gasto { get; set; }
        public double Saldo { get; set; }
    }
}
