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
    public class DALReporteEstadoCuentaLocal
    {
        private readonly string connectionString;

        public DALReporteEstadoCuentaLocal()
        {
            var connection = Conexion.ConexionDB();
            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        public async Task<List<ReporteEstadoCuentaLocalEntity>> ObtenerEstadosCuentaLocal(FiltroEstadoCuentaLocal model)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_EstadoCuentaLocal";
                    var parametros = new
                    {
                        @FechaInicio = model.FechaInicio,
                        @FechaFin = model.FechaFin,
                        @Ruta = model.Ruta,
                        @Proveedor = model.Proveedor,
                        @Destino = model.Destino,
                        @Factura = model.Factura
                    };
                    var resultado = await connection.QueryAsync<ReporteEstadoCuentaLocalEntity>
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
                throw new Exception("Error al obtener datos del estado de cuenta local ", ex);
            }
        }

        public async Task<List<ProveedorEntityCuentaImportacion>> ObtenerProveedores()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_EstadoCuentaLocalProveedores";
                var resultado = await connection.QueryAsync<ProveedorEntityCuentaImportacion>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }

        public async Task<List<ListadoTiendasEstadoCuentaEntity>> ObtenerTiendas()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_EstadoCuentaLocalTienda";
                var resultado = await connection.QueryAsync<ListadoTiendasEstadoCuentaEntity>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }
    }
}
