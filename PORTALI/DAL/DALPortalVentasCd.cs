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
    public class DALPortalVentasCd
    {
        public static List<VentasDetalleEntity> getDetalleSingle(DateTime FechaI, DateTime FechaF, string CardCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_detalle_venta_cd", iConnection);

                iCommand.CommandTimeout = 120;
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                iCommand.Parameters.AddWithValue("@CardCode", CardCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new VentasDetalleEntity()
                                {
                                    WhsCode  = row["WhsCode"].ToString(),
                                    WhsName = row["WhsName"].ToString(),
                                    V_Venta = double.Parse(row["V-Venta"].ToString())
                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return new List<VentasDetalleEntity>();
                }
            }
        }
        public static List<VentasEmpresaEntity> getAllData(DateTime FechaI, DateTime FechaF, int UserId)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_venta_diaria_cd", iConnection);

                iCommand.CommandTimeout = 120;
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                iCommand.Parameters.AddWithValue("@UserId", UserId);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new VentasEmpresaEntity()
                                {
                                    Region = row["Location"].ToString(),
                                    WhsCode = row["WhsCode"].ToString(),
                                    WhsName = row["WhsName"].ToString(),
                                    DiasLaborados = double.Parse(row["DiasLab"].ToString()),
                                    DiasTranscurridos = double.Parse(row["DiasTrans"].ToString()),
                                    V_Meta = double.Parse(row["V-Meta"].ToString()),
                                    V_Venta = double.Parse(row["V-Venta"].ToString()),
                                    V_PromDiario = double.Parse(row["V-PromDiario"].ToString()),
                                    V_Proyeccion = double.Parse(row["V-Proyeccion"].ToString()),
                                    V_MetaDeVentaPorcentaje = double.Parse(row["V-MetaDeVenta%"].ToString()),
                                    U_Utilidad = double.Parse(row["U-Utilidad"].ToString()),
                                    U_PromDiario = double.Parse(row["U-PromDiario"].ToString()),
                                    U_Proyeccion = double.Parse(row["U-Proyeccion"].ToString()),
                                    U_Margen = double.Parse(row["U-Margen"].ToString()),
                                    D_Venta = double.Parse(row["D-Venta"].ToString()),
                                    D_Utilidad = double.Parse(row["D-Utilidad"].ToString()),
                                    D_Margen = double.Parse(row["D-Margen"].ToString()),
                                    NivelEntrada = int.Parse(row["NivelEntrada"].ToString())
                                }).ToList();
                    return bono;
                }
                catch (Exception ex)
                {
                    return new List<VentasEmpresaEntity>();
                }
            }
        }
    }
}
