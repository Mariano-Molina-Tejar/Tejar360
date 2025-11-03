using Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.Filtros;
using Dapper;
using System.Data;
using Entity.Utilitario.LlenadoDropDown;
using System.Data.SqlClient;

namespace DAL
{
    public class DALReporteEstadoCuentaGastos
    {
        private readonly string connectionString;

        public DALReporteEstadoCuentaGastos()
        {
            var connection = Conexion.ConexionDB();

            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        public async Task<List<ReporteEstadoCuentaGastosEntity>> ObtenerEstadoCuentaGastos(FiltroEstadoCuentaGastos model)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_EstadoCuentaGastos";

                    var parametros = new
                    {
                        @FechaI = model.FechaInicio,
                        @FechaF = model.FechaFin,
                        @Proveedor = model.Proveedor,
                        @CodArticulo = model.CodArticulo
                    };

                    var resultado = await connection.QueryAsync<ReporteEstadoCuentaGastosEntity>
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
                throw new Exception("Error al obtener datos del estado de cuenta de servicios adicionales", ex);
            }
        }

        public async Task<List<ProveedorEntityCuentaImportacion>> ObtenerProveedores()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_ProveedoresEstadoCuentaGastos";
                var resultado = await connection.QueryAsync<ProveedorEntityCuentaImportacion>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }
    }
}
