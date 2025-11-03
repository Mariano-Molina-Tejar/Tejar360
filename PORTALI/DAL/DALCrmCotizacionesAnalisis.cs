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
        public static List<CrmCotizacionesEntity> CrmCotizaciones(DateTime FechaI, DateTime FechaF)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            var lista = new List<CrmCotizacionesEntity>();
            string connectionString = $"Provider=SQLOLEDB;Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
            using (OleDbConnection iConnection = new OleDbConnection(connectionString))
            using (OleDbCommand iCommand = new OleDbCommand("sp_crm_cotizaciones", iConnection))
            {
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.Add("@FechaI", OleDbType.Date).Value = FechaI;
                iCommand.Parameters.Add("@FechaF", OleDbType.Date).Value = FechaF;

                try
                {
                    iConnection.Open();
                    using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                    {
                        DataTable dt = new DataTable();
                        iDAResult.Fill(dt);

                        lista = (from row in dt.AsEnumerable()
                                 select new CrmCotizacionesEntity()
                                 {
                                     WhsCode = row["WhsCode"]?.ToString() ?? string.Empty,
                                     WhsName = row["WhsName"]?.ToString() ?? string.Empty,
                                     Region = row["Region"]?.ToString() ?? string.Empty,
                                     TotalCoti = row["TotalCoti"] != DBNull.Value ? Convert.ToInt32(row["TotalCoti"]) : 0,
                                     LineTotalCoti = row["LineTotalCoti"] != DBNull.Value ? Convert.ToDouble(row["LineTotalCoti"]) : 0.0,
                                     LineTotalCobiAb = row["LineTotalCobiAb"] != DBNull.Value ? Convert.ToDouble(row["LineTotalCobiAb"]) : 0.0,
                                     TotalFac = row["TotalFac"] != DBNull.Value ? Convert.ToInt32(row["TotalFac"]) : 0,
                                     LineTotalFac = row["LineTotalFac"] != DBNull.Value ? Convert.ToDouble(row["LineTotalFac"]) : 0.0,
                                     TasaCierre = row["TasaCierre"] != DBNull.Value ? Convert.ToDouble(row["TasaCierre"]) : 0.0,
                                     MetaCoti = row["MetaCoti"] != DBNull.Value ? Convert.ToInt32(row["MetaCoti"]) : 0,
                                     TasaGeneracion = row["TasaGeneracion"] != DBNull.Value ? Convert.ToDouble(row["TasaGeneracion"]) : 0.0,
                                     ClientesNuevos = row["ClientesNuevos"] != DBNull.Value ? Convert.ToInt32(row["ClientesNuevos"]) : 0,
                                     TotalClie = row["TotalClie"] != DBNull.Value ? Convert.ToInt32(row["TotalClie"]) : 0,
                                     totalClieRetenido = row["ClientesRetenidos"] != DBNull.Value ? Convert.ToInt32(row["ClientesRetenidos"]) : 0,
                                     TasaRetencion = row["TasaRetencion"] != DBNull.Value ? Convert.ToDouble(row["TasaRetencion"]) : 0.0,
                                     Cerradas = row["Cerradas"] != DBNull.Value ? Convert.ToInt32(row["Cerradas"]) : 0,
                                     Abiertos = row["Abiertos"] != DBNull.Value ? Convert.ToInt32(row["Abiertos"]) : 0,
                                     Perdida = row["Perdida"] != DBNull.Value ? Convert.ToInt32(row["Perdida"]) : 0,
                                     Seguimiento = row["Seguimiento"] != DBNull.Value ? Convert.ToInt32(row["Seguimiento"]) : 0,
                                     SinEstatus = row["SinEstatus"] != DBNull.Value ? Convert.ToInt32(row["SinEstatus"]) : 0
                                 }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<CrmCotizacionesEntity>();
                }
            }

            return lista;
        }



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
