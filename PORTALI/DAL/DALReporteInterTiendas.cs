using Connection;
using Dapper;
using Entity;
using Entity.Filtros;
using Entity.Utilitario.LlenadoDropDown;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALReporteInterTiendas
    {
        private readonly string connectionString;

        public DALReporteInterTiendas()
        {
            var connection = Conexion.ConexionDB();
            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        public async Task<List<ReporteInterTiendasEntity>> ObtenerInterTiendas(FiltroReporteInterTiendas model)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ReporteInterTiendas";
                    var parametros = new
                    {
                        @FechaI = model.FechaInicio,
                        @FechaF = model.FechaFin,
                        @Ruta = model.Ruta,
                        @Transporte = model.Transporte,
                        @OrdenVenta = model.NoOrdenVenta,
                        @OrdenCompra = model.NoOrdenCompra
                    };
                    var resultado = await connection.QueryAsync<ReporteInterTiendasEntity>
                   (
                           sp,
                           parametros,
                           commandType: CommandType.StoredProcedure,
                           commandTimeout: 120
                   );
                    return resultado.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos del procedimiento Almacenado InterTiendas ", ex);
            }
        }

        public async Task<List<ReporteInterTiendasEstatusRutaEntity>> ObtenerEstatusRuta(FiltroReporteInterTiendas filtro)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ReporteInterTiendasEstatusRuta";

                    var parametros = new
                    {
                        @FechaI = filtro.FechaInicio,
                        @FechaF = filtro.FechaFin,
                        @Ruta = filtro.Ruta

                    };

                    var datos = await connection.QueryAsync<ReporteInterTiendasEstatusRutaEntity>
                        (
                            sp,
                            parametros,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 120
                        );
                    return datos.ToList();
                    //return Resultado<ReporteDañadoEntity>.ConDatos(datos.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos del procedimiento almacenado: Reporte InterTiendas Estatus Rutas ", ex);
            }
        }


        public async Task<List<ListadoTransporteInterTiendas>> ObtenerTransportes()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_ReporteIntertiendasTransportes";
                var resultado = await connection.QueryAsync<ListadoTransporteInterTiendas>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }

        

    }
}
