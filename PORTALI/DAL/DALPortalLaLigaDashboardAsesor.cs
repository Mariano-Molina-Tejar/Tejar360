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
    public class DALPortalLaLigaDashboardAsesor
    {
        public static DashboardAsesoresEntity DashboardAsesores(int SlpCode, DateTime FechaI, DateTime FechaF)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            DashboardAsesoresEntity laligaDashboardAsesoresEntity = new DashboardAsesoresEntity();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_laliga_dashboard_asesores", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@SlpCode", SlpCode);
                iCommand.Parameters.AddWithValue("@FechaI", FechaI);
                iCommand.Parameters.AddWithValue("@FechaF", FechaF);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        laligaDashboardAsesoresEntity.SlpCode = int.Parse(dt.Rows[0]["SlpCode"].ToString());
                        laligaDashboardAsesoresEntity.MetaHoy = double.Parse(dt.Rows[0]["MetaHoy"].ToString());
                        laligaDashboardAsesoresEntity.VentasHoy = double.Parse(dt.Rows[0]["VentasHoy"].ToString());
                        laligaDashboardAsesoresEntity.ProyectadoHoy = double.Parse(dt.Rows[0]["ProyectadoHoy"].ToString());
                        laligaDashboardAsesoresEntity.IndiceVentaHoy = double.Parse(dt.Rows[0]["IndiceVentaHoy"].ToString());
                        laligaDashboardAsesoresEntity.MetaUtilidad = double.Parse(dt.Rows[0]["MetaUtilidad"].ToString());
                        laligaDashboardAsesoresEntity.UtilidadHoy = double.Parse(dt.Rows[0]["UtilidadHoy"].ToString());
                        laligaDashboardAsesoresEntity.ProyectadoUtilidad = double.Parse(dt.Rows[0]["ProyectadoUtilidad"].ToString());
                        laligaDashboardAsesoresEntity.IndiceUtilidadHoy = double.Parse(dt.Rows[0]["IndiceUtilidadHoy"].ToString());
                        laligaDashboardAsesoresEntity.MetaVentaMes = double.Parse(dt.Rows[0]["MetaVentaMes"].ToString());
                        laligaDashboardAsesoresEntity.VentaMes = double.Parse(dt.Rows[0]["VentaMes"].ToString());
                        laligaDashboardAsesoresEntity.IndiceVentaMes = double.Parse(dt.Rows[0]["IndiceVentaMes"].ToString());
                        laligaDashboardAsesoresEntity.MetaUtilidadMes = double.Parse(dt.Rows[0]["MetaUtilidadMes"].ToString());
                        laligaDashboardAsesoresEntity.UtilidadMes = double.Parse(dt.Rows[0]["UtilidadMes"].ToString());
                        laligaDashboardAsesoresEntity.IndiceUtilidadMes = double.Parse(dt.Rows[0]["IndiceUtilidadMes"].ToString());

                        return laligaDashboardAsesoresEntity;
                    }

                    return laligaDashboardAsesoresEntity;
                }
                catch (Exception ex)
                {
                    return laligaDashboardAsesoresEntity;
                }
            }
        }
    }
}
