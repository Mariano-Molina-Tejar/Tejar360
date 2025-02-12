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
    public class DALPortalBonoLiga
    {
        public static PortalComisionAsesoresEntity BonificacionLiga(int Anio, int Mes, int SlpCode, string Usuario)
        {
            PortalComisionAsesoresEntity Liga = new PortalComisionAsesoresEntity();

            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                //iCommand = new OleDbCommand("sp_portal_liga_asesor_main_v2", iConnection);
                iCommand = new OleDbCommand("sp_portal_liga_asesor_main_v5", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Anio", Anio);
                iCommand.Parameters.AddWithValue("@Mes", Mes);
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);

                try
                {
                    if (SlpCode != -1)
                    {
                        OleDbDataAdapter iDAResult = null;
                        DataTable dt = new DataTable();
                        iDAResult = new OleDbDataAdapter();
                        iDAResult.SelectCommand = iCommand;
                        iDAResult.Fill(dt);
                        Liga = (from row in dt.AsEnumerable()
                                select new PortalComisionAsesoresEntity()
                                {
                                    SlpCode = int.Parse(row["SlpCode"].ToString()),
                                    WhsCode = row["WhsCode"].ToString(),
                                    MetaAsesor = double.Parse(row["MetaAsesor"].ToString()),
                                    VentaAsesor = double.Parse(row["VentaAsesor"].ToString()),
                                    VentaAsesorPro = double.Parse(row["VentaProyectaAsesor"].ToString()),
                                    IndiceAsesor = double.Parse(row["IndiceAsesor"].ToString()),
                                    MetaTienda = double.Parse(row["MetaTienda"].ToString()),
                                    VentaTienda = double.Parse(row["VentaTienda"].ToString()),
                                    VentaTiendaPro = double.Parse(row["VentaProyectadaTienda"].ToString()),
                                    IndiceTienda = double.Parse(row["IndiceTienda"].ToString()),
                                    MetaBanio = double.Parse(row["MetaBanio"].ToString()),
                                    VentaBanio = double.Parse(row["VentaBanio"].ToString()),
                                    VentaBanioPro = double.Parse(row["VentaBanioProy"].ToString()),
                                    IndiceBanio = double.Parse(row["IndiceBanio"].ToString()),
                                    MetaCcp = double.Parse(row["MetaCcp"].ToString()),
                                    VentaCcp = double.Parse(row["VentaCcp"].ToString()),
                                    VentaCcpPro = double.Parse(row["VentaCcpProy"].ToString()),
                                    IndiceCcp = double.Parse(row["IndiceCcp"].ToString()),
                                    IndiceAsesorPro = double.Parse(row["IndiceAsesorPro"].ToString()),
                                    IndiceBanioPro = double.Parse(row["IndiceBanioPro"].ToString()),
                                    IndiceCcpPro = double.Parse(row["IndiceCcpPro"].ToString()),
                                    IndiceTiendaPro = double.Parse(row["IndiceTiendaPro"].ToString()),
                                    ComisionBase = double.Parse(row["ComisionBase"].ToString()),
                                    ComisionPlus = double.Parse(row["ComisionPlus"].ToString()),
                                    BonoBase = double.Parse(row["BonoBase"].ToString()),
                                    BonoPersonal = double.Parse(row["BonoPersonal"].ToString()),
                                    BonoBanio = double.Parse(row["BonoBanio"].ToString()),
                                    BonoCcp = double.Parse(row["BonoCcp"].ToString()),
                                    BonoTienda = double.Parse(row["BonoTienda"].ToString()),
                                    TotalRecibir = double.Parse(row["TotalRecibir"].ToString())
                                }).FirstOrDefault();

                        Liga.ListaBonos = DALPortalBonoLiga.ListaBonos(Liga.VentaAsesor, Liga.MetaAsesor);                        
                    }
                    else
                    {
                        Liga.ListaBonos = new List<ListaBonos>();
                    }
                    Liga.ListadoAsesores = DALPortalGenerales.getAllAsesores(SlpCode);
                    Liga.ListaMeses = DALPortalGenerales.ListadoMeses();

                    return Liga;
                }
                catch (Exception ex)
                {
                    Liga.ListaBonos = new List<ListaBonos>();                    
                    return Liga;
                }
            }
        }
        public static List<ListaBonos> ListaBonos(double VentaActual, double Meta)
        {
            List<ListaBonos> Lista = new List<ListaBonos>();

            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_listado_bonos_liga", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@VentaActual", VentaActual);
                iCommand.Parameters.AddWithValue("@Meta", Meta);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    Lista = (from row in dt.AsEnumerable()
                             select new ListaBonos()
                             {
                                 Escala = row["Escala"].ToString(),
                                 Bono = double.Parse(row["Bono"].ToString()),
                                 Meta = double.Parse(row["Meta"].ToString()),
                                 Color = row["Color"].ToString(),
                                 Flag = row["Flag"].ToString(),
                                 Icono = row["Icono"].ToString(),
                                 Oculto = row["Ocultar"].ToString()
                             }).ToList();

                    return Lista;
                }
                catch (Exception ex)
                {
                    return Lista;
                }
            }
        }        
    }
}