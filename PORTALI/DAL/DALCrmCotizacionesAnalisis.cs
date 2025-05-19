using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DAL
{
    public class DALCrmCotizacionesAnalisis
    {
        public static List<ListadoSeguimientoEntity> listadoSeguimientoCotizaciones(int DocEntry)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            CrmCotiAnalisisEntity cotizacionCrm = new CrmCotiAnalisisEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_listado_seguimientos", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@DocEntry", DocEntry);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var lista = (from row in dt.AsEnumerable()
                                 select new ListadoSeguimientoEntity()
                                 {
                                     Creado = row["Creado"].ToString(),
                                     Code = int.Parse(row["Code"].ToString()),
                                     DocEntry = int.Parse(row["DocEntry"].ToString()),
                                     DocNum = int.Parse(row["DocNum"].ToString()),
                                     IdEstado = int.Parse(row["IdEstado"].ToString()),
                                     DescrEstado = row["DescrEstado"].ToString(),
                                     FechaSeg = DateTime.Parse(row["FechaSeg"].ToString()),
                                     FechaVencimiento = DateTime.Parse(row["FechaVencimiento"].ToString()),
                                     TipoDeContacto = row["TipoDeContacto"].ToString(),
                                     Accion = row["Accion"].ToString(),
                                     Notas = row["Notas"].ToString(),
                                     Usuario = row["Usuario"].ToString()
                                 }).ToList();
                    return lista;

                }
                catch (Exception ex)
                {
                    return new List<ListadoSeguimientoEntity>();
                }
            }
        }
        public static CrmCotiAnalisisEntity CrmCotizaciones(DateTime FechaI, DateTime FechaF, int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            CrmCotiAnalisisEntity cotizacionCrm = new CrmCotiAnalisisEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_crm_asesores_cotizaciones", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        cotizacionCrm.SlpCode = int.Parse(dt.Rows[0]["SlpCode"].ToString());
                        cotizacionCrm.SlpName = dt.Rows[0]["SlpName"].ToString();
                        cotizacionCrm.TotalCotAbiertas = double.Parse(dt.Rows[0]["TotalCotAbiertas"].ToString());
                        cotizacionCrm.TotalCotCerradas = double.Parse(dt.Rows[0]["TotalCotCerradas"].ToString());
                        cotizacionCrm.Indice = double.Parse(dt.Rows[0]["Indice"].ToString());
                        cotizacionCrm.MetaCoti = double.Parse(dt.Rows[0]["MetaCoti"].ToString());                        
                        cotizacionCrm.IndiceMetaCoti = double.Parse(dt.Rows[0]["IndiceMetaCoti"].ToString());
                    }

                    return cotizacionCrm;
                }
                catch (Exception ex)
                {
                    return new CrmCotiAnalisisEntity();
                }
            }
        }
    }
}
