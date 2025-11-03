using Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.Filtros;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Entity.Utilitario.LlenadoDropDown;

namespace DAL
{
    public class DALReporteEstadoCuentaProveedor
    {
        private readonly string connectionString;

        public DALReporteEstadoCuentaProveedor()
        {
            var connection = Conexion.ConexionDB();

            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        public async Task<List<ReporteEstadoCuentaImportacionEntity>>ObtenerEstadoCuentaImportacion(FiltroEstadoCuentaEntity model) 
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_EstadoCuentaImportacion";

                    var parametros = new
                    {
                        @FechaI = model.FechaInicio,
                        @FechaF = model.FechaFin,
                        @Proveedor = model.Proveedor
                    };

                    var resultado = await connection.QueryAsync<ReporteEstadoCuentaImportacionEntity>
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
                     return new List<ReporteEstadoCuentaImportacionEntity> { };
            }
        }

        public async Task<List<ProveedorEntityCuentaImportacion>> ObtenerProveedoresEstadoCuenta()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_ObtenerProveedoresEstadoCuentaImportacion";
                var resultado = await connection.QueryAsync<ProveedorEntityCuentaImportacion>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }

      
    }
}
