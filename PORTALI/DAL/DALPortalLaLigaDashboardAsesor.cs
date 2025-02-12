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
        public static List<DashboardAsesorGraficoEntity> DashboardVentasPorSemana()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<DashboardAsesorGraficoEntity> datos = new List<DashboardAsesorGraficoEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_dashboard_ventas_por_dia_asesor", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        datos = (from row in dt.AsEnumerable()
                                 select new DashboardAsesorGraficoEntity()
                                 {
                                     labels = row["DiaSemana"].ToString(),
                                     data = double.Parse(row["Total"].ToString())
                                 }).ToList();
                        return datos;
                    }

                    return new List<DashboardAsesorGraficoEntity>();
                }
                catch (Exception ex)
                {
                    return new List<DashboardAsesorGraficoEntity>();
                }
            }
        }

        public static async Task<List<DashboardAsesorGraficoEntity>> DashboardVentasPorMesAsync()
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<DashboardAsesorGraficoEntity> datos = new List<DashboardAsesorGraficoEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = new OleDbCommand("sp_dashboard_ventas_por_mes_asesor", iConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    await iConnection.OpenAsync();  // Conexión asincrónica

                    OleDbDataAdapter iDAResult = new OleDbDataAdapter
                    {
                        SelectCommand = iCommand
                    };

                    DataTable dt = new DataTable();
                    await Task.Run(() => iDAResult.Fill(dt));  // Ejecución asincrónica de la consulta

                    if (dt.Rows.Count > 0)
                    {
                        datos = (from row in dt.AsEnumerable()
                                 select new DashboardAsesorGraficoEntity()
                                 {
                                     labels = row["Mes"].ToString(),
                                     data = double.Parse(row["Total"].ToString())
                                 }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return new List<DashboardAsesorGraficoEntity>();
                }
            }

            return datos;
        }


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
                        laligaDashboardAsesoresEntity.RestanteHoy = double.Parse(dt.Rows[0]["RestanteHoy"].ToString());
                        laligaDashboardAsesoresEntity.IndiceVentaHoy = double.Parse(dt.Rows[0]["IndiceVentaHoy"].ToString());
                        laligaDashboardAsesoresEntity.MetaUtilidad = double.Parse(dt.Rows[0]["MetaUtilidad"].ToString());
                        laligaDashboardAsesoresEntity.UtilidadHoy = double.Parse(dt.Rows[0]["UtilidadHoy"].ToString());
                        laligaDashboardAsesoresEntity.RestanteUtilidadHoy = double.Parse(dt.Rows[0]["RestanteUtilidadHoy"].ToString());
                        laligaDashboardAsesoresEntity.IndiceUtilidadHoy = double.Parse(dt.Rows[0]["IndiceUtilidadHoy"].ToString());
                        laligaDashboardAsesoresEntity.MetaVentaMes = double.Parse(dt.Rows[0]["MetaVentaMes"].ToString());
                        laligaDashboardAsesoresEntity.VentaMes = double.Parse(dt.Rows[0]["VentaMes"].ToString());
                        laligaDashboardAsesoresEntity.IndiceVentaMes = double.Parse(dt.Rows[0]["IndiceVentaMes"].ToString());
                        laligaDashboardAsesoresEntity.ProyeccionVentaMes = double.Parse(dt.Rows[0]["ProyeccionVentaMes"].ToString());
                        laligaDashboardAsesoresEntity.MetaUtilidadMes = double.Parse(dt.Rows[0]["MetaUtilidadMes"].ToString());
                        laligaDashboardAsesoresEntity.UtilidadMes = double.Parse(dt.Rows[0]["UtilidadMes"].ToString());
                        laligaDashboardAsesoresEntity.IndiceUtilidadMes = double.Parse(dt.Rows[0]["IndiceUtilidadMes"].ToString());
                        laligaDashboardAsesoresEntity.ProyeccionUtilidadMes = double.Parse(dt.Rows[0]["ProyeccionUtilidadMes"].ToString());
                        laligaDashboardAsesoresEntity.Ranking = int.Parse(dt.Rows[0]["Ranking"].ToString());
                        laligaDashboardAsesoresEntity.TotalFacturas = int.Parse(dt.Rows[0]["TotalFacturas"].ToString());
                        laligaDashboardAsesoresEntity.ClientesAtendidos = int.Parse(dt.Rows[0]["ClientesAtendidos"].ToString());
                        laligaDashboardAsesoresEntity.DiasTrans = int.Parse(dt.Rows[0]["DiasTrans"].ToString());
                        laligaDashboardAsesoresEntity.DiasMes = int.Parse(dt.Rows[0]["DiasMes"].ToString());
                        laligaDashboardAsesoresEntity.DiasRestantes = int.Parse(dt.Rows[0]["DiasRestantes"].ToString());

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
