using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity;
using Connection;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using Entity.Utilitario;
using Entity.Filtros;

namespace DAL
{
    public class DALReporteDañado
    {
        private readonly string connectionString;

        public DALReporteDañado()
        {
            var connection = Conexion.ConexionDB();

            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        public async Task<List<ReporteDañadoTransferencia>> ObtenerTransferenciaDañados(FechaFiltroEntity model)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_TransferenciaDañado";

                    var parametros = new
                    {
                        @FechaInicio = model.FechaInicio,
                        @FechaFin = model.FechaFin
                    };

                    var resultado = await connection.QueryAsync<ReporteDañadoTransferencia>
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
                return new List<ReporteDañadoTransferencia>
                {
                   new ReporteDañadoTransferencia{ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<List<ReporteDañadoEntity>>ObtenerDañados(FiltroDañadoEntity filtro)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString) )
                {
                    string sp = "sp_DañadoConNC";

                    var parametros = new
                    {
                        @FechaInicio = filtro.FechaInicio,
                        @FechaFin = filtro.FechaFin,
                        @Tienda = filtro.AlmacenSeleccionado,
                        @Transportista = filtro.TranportistaSeleccionado

                    };

                    var datos = await connection.QueryAsync<ReporteDañadoEntity>
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
                return new List<ReporteDañadoEntity>
                {
                     new ReporteDañadoEntity {ErrorMessage = ex.Message}
                };
            }
        }

        public List<string> ObtenerAlmacenes(List<ReporteDañadoEntity> datos)
        {
            return datos
                .Where(d => !string.IsNullOrEmpty(d.Tienda))
                .Select(d => d.Tienda)
                .Distinct()
                .ToList();
        }

        public List<string> ObtenerTransportistas(List<ReporteDañadoEntity> datos)
        {
            return datos
                .Where(d => !string.IsNullOrEmpty(d.Transportista))
                .Select(d => d.Transportista)
                .Distinct()
                .ToList();
        }



    }
}
