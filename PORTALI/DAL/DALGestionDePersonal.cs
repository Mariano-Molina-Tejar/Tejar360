using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Connection;
using Entity;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DALGestionDePersonal
    {
        private readonly string connectionString;
        public DALGestionDePersonal()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<IEnumerable<DatosEmpleados>> ObtenerColaboradoresACargo(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string sp = "sp_obtener_colaboradores_por_gerente";

                    return await conn.QueryAsync<DatosEmpleados>
                        (
                        sp,
                        new { @User360 = userId },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Puestos>> ObtenerPuestos()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT posID as Id, descriptio as Nombre FROM OHPS ORDER BY posID";

                    return await conn.QueryAsync<Puestos>
                        (
                        query,
                        commandType: CommandType.Text,
                        commandTimeout: 60
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<AutorizacionDeBaja>> ObtenerAutorizacinesDeBaja()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = "sp_ver_autorizaciones_baja_de_personal";

                    return await conn.QueryAsync<AutorizacionDeBaja>
                        (
                        query,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 60
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<CausasDespido>> ObtenerCausasDespido()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_CAUSAS]";

                    return await conn.QueryAsync<CausasDespido>
                        (
                        query,
                        commandType: CommandType.Text,
                        commandTimeout: 60
                        );
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CausasDespido>> ObtenerCausasPorSolicitud()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = $@"SELECT U_IdSolicitud as Code, T1.Name FROM [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_DET_CAU] T0
                                      INNER JOIN [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_CAUSAS] T1 ON T0.U_IdCausa = T1.Code";

                    return await conn.QueryAsync<CausasDespido>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CambiarEstadoSolicitudBaja(int Solicitud, int Estado)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string update = $@"UPDATE [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_B]
                                    SET U_Estado = {Estado}
                                    WHERE CODE = {Solicitud}";
                    return await conn.ExecuteAsync(update, commandType: CommandType.Text, commandTimeout: 60);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> VerificarSolicitudDeBajaPendiente(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string select = $@"IF(EXISTS (SELECT 1 FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_B] WHERE U_EmpleadoId = {userId} AND U_Estado >= 0))
                                        BEGIN
                                        SELECT 1
                                        END
                                        ELSE
                                        BEGIN
                                        SELECT 0
                                        END	
                                        ";

                    return await conn.QuerySingleAsync<int>(select, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> VerificarExistenciaDeCorreo(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string select = $@"IF((SELECT email FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].OHEM WHERE userId = {userId}) is not null)
                                        BEGIN
                                        SELECT 1
                                        END 
                                        ELSE 
                                        BEGIN
                                        SELECT 0
                                        END
                                        ";

                    return await conn.QuerySingleAsync<int>(select, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> ObtenerNombreDeUsuario(int UserId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = $"SELECT TOP 1 CONCAT(firstName, ' ' , lastName) FROM OHEM WHERE userId = {UserId}";

                    return await conn.QuerySingleAsync<string>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> ObtenerCorreosSolicitud(int idSolicitud, int estado)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string sp = "sp_ver_correos_solicitud_baja";

                    var parametros = new
                    {
                        IdSolicitud = idSolicitud,
                        Estado = estado
                    };

                    return await conn.QuerySingleOrDefaultAsync<string>(
                        sp, 
                        parametros, 
                        commandType: CommandType.StoredProcedure, 
                        commandTimeout: 60)
                        .ConfigureAwait(false) ?? "";
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener los correos de la solicitud", ex);
            }
        }
    }
}
