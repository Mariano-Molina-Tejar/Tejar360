using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALMargenTiendas
    {
        public static List<MargenTiendasEntity> ListadoDetalleMargen(DateTime FechaI, DateTime FechaF)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<MargenTiendasEntity> listado = new List<MargenTiendasEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_margen_utilidad_tienda_v2", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                    iCommand.Parameters.AddWithValue("@FechaF", FechaF);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                       select new MargenTiendasEntity
                                       {
                                           WhsCode = row["WhsCode"].ToString(),
                                           WhsName = row["WhsName"].ToString(),
                                           IdRegion = Convert.ToInt32(row["IdRegion"]),
                                           Region = row["Region"].ToString(),

                                           VentaActual = Convert.ToDouble(row["VentaActual"]),
                                           VentaDescuento = Convert.ToDouble(row["Venta/Descuento"]),
                                           VentaPromocion = Convert.ToDouble(row["Venta/Promocion"]),
                                           VentaProyectada = Convert.ToDouble(row["VentaProyectada"]),

                                           Rentabilidad = Convert.ToDouble(row["Rentabilidad"]),
                                           RentabilidadDescuento = Convert.ToDouble(row["Rentabilidad/Descuento"]),
                                           RentabilidadPromocion = Convert.ToDouble(row["Rentabilidad/Promocion"]),

                                           Facturas = Convert.ToInt32(row["Facturas"])
                                       }).ToList();


                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<MargenTiendasEntity>();
                    }
                }
            }
        }
    }
}
