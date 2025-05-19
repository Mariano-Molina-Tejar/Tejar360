using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Connection;
using System.Data.OleDb;
using System.Data;

namespace DAL
{
    public class DALPortalDashboardGt
    {
        public static PortalDashboardGtEntity getDashboardVentasGt(string WhsCode, DateTime FechaI, DateTime FechaF)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            PortalDashboardGtEntity portalDashboardGtEntity = new PortalDashboardGtEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_portal_gt_dashboard", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@WhsCode", WhsCode);
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);
                    if(dt.Rows.Count > 0) 
                    {
                        portalDashboardGtEntity.MetaTienda = double.Parse(dt.Rows[0]["MetaTienda"].ToString());
                        portalDashboardGtEntity.VentaTienda = double.Parse(dt.Rows[0]["VentaTienda"].ToString());
                        portalDashboardGtEntity.Indice = double.Parse(dt.Rows[0]["IndiceTienda"].ToString());
                        portalDashboardGtEntity.VentaTiendaProy = double.Parse(dt.Rows[0]["VentaTiendaProy"].ToString());
                        portalDashboardGtEntity.IndiceProyectado = double.Parse(dt.Rows[0]["IndiceProy"].ToString());
                        portalDashboardGtEntity.MetaUtilidadTienda = double.Parse(dt.Rows[0]["MetaUtilidadTienda"].ToString());
                        portalDashboardGtEntity.UtilidadIndice = double.Parse(dt.Rows[0]["IndiceUtilidadTienda"].ToString());
                        portalDashboardGtEntity.UtilidadTiendaProy = double.Parse(dt.Rows[0]["UtilidadTiendaProy"].ToString());
                        portalDashboardGtEntity.IndiceUtilidadProy = double.Parse(dt.Rows[0]["IndiceUtilidadProy"].ToString());
                    }

                    portalDashboardGtEntity.ListaAsesores = (from row in dt.AsEnumerable()
                                    select new PortalDashboardDetalleAsesorEntity
                                    {
                                        SlpCode = int.Parse(row["SlpCode"].ToString()),
                                        SlpName = row["SlpName"].ToString(),
                                        MetaAsesor = double.Parse(row["MetaAsesor"].ToString()),
                                        VentaAsesor = double.Parse(row["VentaAsesor"].ToString()),
                                        Indice = double.Parse(row["IndiceAsesor"].ToString()),
                                        Peso = double.Parse(row["PesoAsesor"].ToString())                                        
                                    }).ToList();

                    PortalChartPieEntity portalChartPieEntity = new PortalChartPieEntity();
                    List<string> labels = new List<string>();
                    labels.Add("Meta");
                    labels.Add("Venta");
                    portalChartPieEntity.labels = labels;

                    List<string> backgroundColor = new List<string>();
                    backgroundColor.Add("#de1f21");
                    backgroundColor.Add("#00aae4");

                    List<double> data = new List<double>();
                    data.Add(double.Parse(dt.Rows[0]["MetaTienda"].ToString()));
                    data.Add(double.Parse(dt.Rows[0]["VentaTienda"].ToString()));

                    DtSets dtSets = new DtSets();
                    dtSets.data = data;
                    dtSets.backgroundColor = backgroundColor;

                    List<DtSets> datasets = new List<DtSets>();
                    datasets.Add(dtSets);
                    portalChartPieEntity.datasets = datasets;

                    portalDashboardGtEntity.PieChartData = portalChartPieEntity;
                    return portalDashboardGtEntity;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
