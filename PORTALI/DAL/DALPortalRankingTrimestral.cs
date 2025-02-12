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
    public class DALPortalRankingTrimestral
    {
        public static List<RankingV2Entity> RankingV2(int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                if (SlpCode == -1)
                {
                    return new List<RankingV2Entity>();
                }

                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_ranking_v3", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", -1);

                List<RankingV2Entity> lista = new List<RankingV2Entity>();
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    lista = (from row in dt.AsEnumerable()
                             select new RankingV2Entity()
                             {
                                 SlpCode = int.Parse(row["SlpCode"].ToString()),
                                 SlpName = row["SlpName"].ToString(),
                                 Mes1 = double.Parse(row.ItemArray[2].ToString()),
                                 Mes2 = double.Parse(row.ItemArray[3].ToString()),
                                 Mes3 = double.Parse(row.ItemArray[4].ToString()),
                                 Total = double.Parse(row["Total"].ToString()),
                                 WhsName = row["WhsName"].ToString()
                             }).ToList();

                    return lista;
                }
                catch (Exception ex)
                {
                    return new List<RankingV2Entity>();
                }
            }
        }

        public static List<PortalGerencialRankingTrimestralEntity> Ranking(int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                if(SlpCode == -1) 
                {
                    List<PortalGerencialRankingTrimestralEntity> lista2 = new List<PortalGerencialRankingTrimestralEntity>();                    
                    return lista2;
                }

                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_ranking_trimestral", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", -1);

                List<PortalGerencialRankingTrimestralEntity> lista = new List<PortalGerencialRankingTrimestralEntity>();
                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    List<PortalTrankingTrimestralAsesorEntity> ranking = new List<PortalTrankingTrimestralAsesorEntity>();
                    ranking = DALPortalRankingTrimestral.RankingPorAsesor(SlpCode).ToList();

                    lista = (from row in dt.AsEnumerable()
                                select new PortalGerencialRankingTrimestralEntity()
                                {
                                    SlpName = row["SlpName"].ToString(),
                                    Tienda = row["Tienda"].ToString(),
                                    MetaMensual = double.Parse(row["MetaMensual"].ToString()),
                                    VentaMensual = double.Parse(row["VentaTotalMensual"].ToString()),
                                    VentaProyectada = double.Parse(row["VentaProyectada"].ToString()),
                                    Indice = double.Parse(row["Indice"].ToString()),
                                    No = int.Parse(row["No"].ToString()),
                                    ListaTrimestral = ranking
                                }).ToList();

                    
                    return lista;
                }
                catch (Exception ex)
                {
                    return new List<PortalGerencialRankingTrimestralEntity>();
                }
            }
        }
        public static List<PortalTrankingTrimestralAsesorEntity> RankingPorAsesor(int SlpCode)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                if (SlpCode == -1)
                {
                    return new List<PortalTrankingTrimestralAsesorEntity>();
                }
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_ranking_trimestral_asesor", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode.ToString() == "" ? "-1" : SlpCode.ToString());

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    var bono = (from row in dt.AsEnumerable()
                                select new PortalTrankingTrimestralAsesorEntity()
                                {
                                    Id = int.Parse(row["Id"].ToString()),
                                    Nombre = row["Mes"].ToString(),
                                    Posicion = double.Parse(row["Posicion"].ToString())                                    
                                }).ToList();

                    return bono;
                }
                catch (Exception ex)
                {
                    return new List<PortalTrankingTrimestralAsesorEntity>();
                }
            }
        }
    }
}
