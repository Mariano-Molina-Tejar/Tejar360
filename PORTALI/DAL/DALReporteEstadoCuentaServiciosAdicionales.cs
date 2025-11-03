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
    public class DALReporteEstadoCuentaServiciosAdicionales
    {
        private readonly string connectionString;

        public DALReporteEstadoCuentaServiciosAdicionales()
        {
            var connection = Conexion.ConexionDB();

            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        public async Task<List<ReporteEstadoCuentaServiciosAdicionales>> ObtenerServiciosAdicionales(FiltroServiciosAdicionalesEntity model)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_EstadoCuentaServiciosAdicionales";

                    var parametros = new
                    {
                        @FechaI = model.FechaInicio,
                        @FechaF = model.FechaFin,
                        @Proveedor = model.Proveedor,
                        @OrdenCompra = model.OrdenCompra,
                        @Factura = model.Factura
                    };

                    var resultado = await connection.QueryAsync<ReporteEstadoCuentaServiciosAdicionales>
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
                //return new List<ReporteEstadoCuentaServiciosAdicionales> { };
                throw new Exception("Error al obtener datos del estado de cuenta de servicios adicionales", ex);
            }
        }

        public async Task<List<ProveedorEntityCuentaImportacion>> ObtenerProveedores()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_ProveedoresServiciosAdicionales";
                var resultado = await connection.QueryAsync<ProveedorEntityCuentaImportacion>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }


    }
}
