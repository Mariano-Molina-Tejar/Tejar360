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
    public class DALBolsonPerdidas
    {
        public static List<BolsonPerdidoEntity> BolsonPerdidas(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, DateTime? FechaI = null, DateTime? FechaF = null, int? ReferidoSac = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<BolsonPerdidoEntity> listado = new List<BolsonPerdidoEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_boslon_perdidas", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@IdRegion", (object)IdRegion ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@WhsCode", (object)WhsCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@SlpCode", (object)SlpCode ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaI", (object)FechaI ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaF", (object)FechaF ?? DBNull.Value);
                    iCommand.Parameters.AddWithValue("@ReferidoSac", (object)ReferidoSac ?? DBNull.Value);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new BolsonPerdidoEntity
                                   {
                                       TipoTabla = row["TipoTabla"].ToString(),
                                       DocEntry = Convert.ToInt32(row["DocEntry"]),
                                       DocNum = Convert.ToInt32(row["DocNum"]),
                                       SlpCode = Convert.ToInt32(row["SlpCode"]),
                                       SlpName = row["SlpName"].ToString(),
                                       Nit = row["Nit"].ToString(),
                                       Nombre = row["Nombre"].ToString(),
                                       Telefono = row["Telefono"] == DBNull.Value ? null : row["Telefono"].ToString(),
                                       DocDate = row["DocDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["DocDate"]),
                                       DocTotal = row["DocTotal"] == DBNull.Value ? 0 : Convert.ToDouble(row["DocTotal"]),
                                       Estado = row["Estado"].ToString(),
                                       IdMotivo = row["IdMotivo"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdMotivo"]),
                                       MotivoPerdida = row["MotivoPerdida"] == DBNull.Value ? null : row["MotivoPerdida"].ToString(),
                                       FechaPerdida = row["FechaPerdida"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FechaPerdida"]),
                                       NotaPerdida = row["NotaPerdida"] == DBNull.Value ? null : row["NotaPerdida"].ToString(),
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString(),
                                       IdRegion = row["IdRegion"] == DBNull.Value ? 0 : Convert.ToInt32(row["IdRegion"]),
                                       Region = row["Region"].ToString(),
                                       Tipo = row["Tipo"] == DBNull.Value ? 0 : Convert.ToInt32(row["Tipo"]),
                                       ReferidoSac = row["ReferidoSac"] == DBNull.Value ? null : row["ReferidoSac"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<BolsonPerdidoEntity>();
                    }
                }
            }
        }
    }
}
