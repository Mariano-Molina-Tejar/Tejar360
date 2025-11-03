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
    public class DALCrmVentasFacturas
    {
        public static List<CrmVentasEntity> CrmVentasFacturas(int? IdRegion = null, string WhsCode = null, int? SlpCode = null, DateTime? FechaI = null, DateTime? FechaF = null, int? ReferidoSac = null)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<CrmVentasEntity> listado = new List<CrmVentasEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_facturas", iConnection))
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
                                   select new CrmVentasEntity
                                   {
                                       DocEntry = Convert.ToInt32(row["DocEntry"]),
                                       DocNumFac = int.Parse(row["DocNumFac"].ToString()),
                                       DocDate = Convert.ToDateTime(row["DocDate"]),
                                       DocNumCot = row["DocNumCot"].ToString(),
                                       Folio = row["Folio"].ToString(),
                                       CardCode = row["CardCode"].ToString(),
                                       CardName = row["CardName"].ToString(),
                                       WhsCode = row["WhsCode"].ToString(),
                                       WhsName = row["WhsName"].ToString(),
                                       SlpCode = int.Parse(row["SlpCode"].ToString()),
                                       SlpName = row["SlpName"].ToString(),
                                       IdRegion = Convert.ToInt32(row["IdRegion"]),
                                       Region = row["Region"].ToString(),
                                       LineTotal = Convert.ToDouble(row["LineTotal"])
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        // Puedes loggear ex.Message si deseas
                        return new List<CrmVentasEntity>();
                    }
                }
            }
        }
    }
}
