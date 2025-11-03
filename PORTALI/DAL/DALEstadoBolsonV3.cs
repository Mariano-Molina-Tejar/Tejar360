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
    public class DALEstadoBolsonV3
    {
        public static List<EstadoBolsonV3Entity> BolsonActivoDetalleV3(string WhsCode, int Anio, int Semana, int? Tipo = null, DateTime? FechaI = null, DateTime? FechaF = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<EstadoBolsonV3Entity> listado = new List<EstadoBolsonV3Entity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_estado_bolson_detalle_v4", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);
                    iCommand.Parameters.AddWithValue("@Anio", Anio);
                    iCommand.Parameters.AddWithValue("@Semana", Semana);
                    iCommand.Parameters.AddWithValue("@Tipo", Tipo);
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
                                   select new EstadoBolsonV3Entity
                                   {
                                       WhsCode = row["WhsCode"]?.ToString(),
                                       WhsName = row["WhsName"]?.ToString(),
                                       IdRegion = row["IdRegion"] != DBNull.Value ? Convert.ToInt32(row["IdRegion"]) : 0,
                                       Region = row["Region"]?.ToString(),
                                       Semana = row["Semana"] != DBNull.Value ? Convert.ToInt32(row["Semana"]) : 0,

                                       SlpCode = row["SlpCode"] != DBNull.Value ? Convert.ToInt32(row["SlpCode"]) : 0,
                                       SlpName = row["SlpName"]?.ToString(),

                                       CotGen_TotalU = row["CotGen_TotalU"] != DBNull.Value ? Convert.ToInt32(row["CotGen_TotalU"]) : 0,
                                       CotGen_TotalQ = row["CotGen_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["CotGen_TotalQ"]) : 0,
                                       TicketPromedio = row["TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["TicketPromedio"]) : 0,

                                       CotiFac_TotalU = row["CotiFac_TotalU"] != DBNull.Value ? Convert.ToInt32(row["CotiFac_TotalU"]) : 0,
                                       CotiFac_TotalQ = row["CotiFac_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["CotiFac_TotalQ"]) : 0,
                                       CotiFac_TicketPromedio = row["CotiFac_TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["CotiFac_TicketPromedio"]) : 0,
                                       CotiFac_Tasa = row["CotiFac_Tasa"] != DBNull.Value ? Convert.ToDouble(row["CotiFac_Tasa"]) : 0,

                                       CotiPer_TotalU = row["CotiPer_TotalU"] != DBNull.Value ? Convert.ToInt32(row["CotiPer_TotalU"]) : 0,
                                       CotiPer_TotalQ = row["CotiPer_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["CotiPer_TotalQ"]) : 0,
                                       CotiPer_TicketPromedio = row["CotiPer_TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["CotiPer_TicketPromedio"]) : 0,
                                       CotiPer_Tasa = row["CotiPer_Tasa"] != DBNull.Value ? Convert.ToDouble(row["CotiPer_Tasa"]) : 0,

                                       TotalAb_TotalU = row["TotalAb_TotalU"] != DBNull.Value ? Convert.ToInt32(row["TotalAb_TotalU"]) : 0,
                                       TotalAb_TotalQ = row["TotalAb_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["TotalAb_TotalQ"]) : 0,
                                       TotalAb_TicketPromedio = row["TotalAb_TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["TotalAb_TicketPromedio"]) : 0,
                                       TotalAb_Tasa = row["TotalAb_Tasa"] != DBNull.Value ? Convert.ToDouble(row["TotalAb_Tasa"]) : 0,
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        return new List<EstadoBolsonV3Entity>();
                    }
                }
            }
        }
        public static List<EstadoBolsonV3Entity> BolsonActivoV3(int Anio, int NoSemana, int? IdRegion, string WhsCode,int Tipo, DateTime? FechaI, DateTime? FechaF)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<EstadoBolsonV3Entity> listado = new List<EstadoBolsonV3Entity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_estado_bolson_v4", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@Semana", (object)NoSemana ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@Anio", (object)Anio ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);                    
                    iCommand.Parameters.AddWithValue("@Tipo", (object)Tipo ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaI", (object)FechaI ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaF", (object)FechaF ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new EstadoBolsonV3Entity
                                   {
                                       WhsCode = row["WhsCode"]?.ToString(),
                                       WhsName = row["WhsName"]?.ToString(),
                                       IdRegion = row["IdRegion"] != DBNull.Value ? Convert.ToInt32(row["IdRegion"]) : 0,
                                       Region = row["Region"]?.ToString(),
                                       Semana = row["Semana"] != DBNull.Value ? Convert.ToInt32(row["Semana"]) : 0,

                                       CotGen_TotalU = row["CotGen_TotalU"] != DBNull.Value ? Convert.ToInt32(row["CotGen_TotalU"]) : 0,
                                       CotGen_TotalQ = row["CotGen_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["CotGen_TotalQ"]) : 0,
                                       TicketPromedio = row["TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["TicketPromedio"]) : 0,

                                       CotiFac_TotalU = row["CotiFac_TotalU"] != DBNull.Value ? Convert.ToInt32(row["CotiFac_TotalU"]) : 0,
                                       CotiFac_TotalQ = row["CotiFac_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["CotiFac_TotalQ"]) : 0,
                                       CotiFac_TicketPromedio = row["CotiFac_TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["CotiFac_TicketPromedio"]) : 0,
                                       CotiFac_Tasa = row["CotiFac_Tasa"] != DBNull.Value ? Convert.ToDouble(row["CotiFac_Tasa"]) : 0,

                                       CotiPer_TotalU = row["CotiPer_TotalU"] != DBNull.Value ? Convert.ToInt32(row["CotiPer_TotalU"]) : 0,
                                       CotiPer_TotalQ = row["CotiPer_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["CotiPer_TotalQ"]) : 0,
                                       CotiPer_TicketPromedio = row["CotiPer_TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["CotiPer_TicketPromedio"]) : 0,
                                       CotiPer_Tasa = row["CotiPer_Tasa"] != DBNull.Value ? Convert.ToDouble(row["CotiPer_Tasa"]) : 0,

                                       TotalAb_TotalU = row["TotalAb_TotalU"] != DBNull.Value ? Convert.ToInt32(row["TotalAb_TotalU"]) : 0,
                                       TotalAb_TotalQ = row["TotalAb_TotalQ"] != DBNull.Value ? Convert.ToDouble(row["TotalAb_TotalQ"]) : 0,
                                       TotalAb_TicketPromedio = row["TotalAb_TicketPromedio"] != DBNull.Value ? Convert.ToDouble(row["TotalAb_TicketPromedio"]) : 0,
                                       TotalAb_Tasa = row["TotalAb_Tasa"] != DBNull.Value ? Convert.ToDouble(row["TotalAb_Tasa"]) : 0,
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        return new List<EstadoBolsonV3Entity>();
                    }
                }
            }
        }
    }
}
