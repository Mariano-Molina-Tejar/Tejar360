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
using System.Web.ModelBinding;

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
        public async Task<IEnumerable<AutorizacionDeBaja>> ObtenerProcesoDeBaja()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    string query = "sp_ver_procesos_baja_de_personal";

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
                    string query = @"SELECT * FROM [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_CAUSAS]
                                     where U_despido = 'Y'";

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

        public async Task<int> ObtenerIdProcesoDeBaja(int idEmpleado)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT TOP 1 Code FROM [ELTEJAR_PRUEBAS_R1_5.1].dbo.[@GESTION_EMP_B]
                                WHERE U_EmpleadoId = @IdUsuario
                                ORDER BY Code DESC";

                return await conn.QuerySingleAsync<int>(query, new { IdUsuario  = idEmpleado});
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
                        @IdSolicitud = idSolicitud,
                        @IdEstado = estado
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

        public async Task<string> ObtenerNombreCompleto(int userId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT	CASE 
		                        WHEN CONCAT(OHEM.firstName, ' ' ,OHEM.LastName) = '' THEN OUSR.U_NAME
		                        ELSE CONCAT(OHEM.firstName, ' ' ,OHEM.LastName) END Nombre
                        FROM OUSR
                        LEFT JOIN OHEM ON OUSR.USERID = OHEM.userId
                        WHERE OUSR.USERID = @UserId";

                return await conn.ExecuteScalarAsync<string>
                    (
                    query,
                    new { UserId = userId }
                    );
            }
        }

        public async Task<IEnumerable<Autorizaciones>> ObtenerProcesoDeAutorizaciones(int userId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT	T0.U_FechaCreacion AS FechaDeSolicitud,
		                        ISNULL(T1.descriptio,t2.Name) AS Posicion,
		                        ISNULL(T0.U_Estado,'P') AS Estado,
		                        CASE WHEN T0.U_Estado IS NULL THEN 'Pendiente - Gerencia'
			                         WHEN T0.U_Estado = 'P' Then 'verificando perfil - GTH'
			                         WHEN T0.U_Estado = 'R' THEN 'Rechazado gerencia'
			                         ELSE 'Autorizado'
		                        END AS Observaciones
	                        FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_A] T0
	                        LEFT JOIN [ELTEJAR_PRUEBAS_R1_5.1].[DBO].OHPS T1 ON T0.U_IdPosicion = T1.posID  
	                        LEFT JOIN [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_PERFIL] T2 ON T0.U_IdPerfil = t2.Code
	                        WHERE U_IdSolicitante = @userId";

                return await conn.QueryAsync<Autorizaciones>
                    (
                    query,
                    new
                    {
                        UserId = userId
                    }
                    );
            }
        }

        public async Task<TrackingDeBaja> ObtenerTrackingBaja(int empId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var sp = "sp_ver_proceso_de_baja";

                return await conn.QuerySingleAsync<TrackingDeBaja>
                    (
                    sp,
                    new { EmpId = empId }
                    );
            }
        }

        public async Task<bool> TieneRequisicion(int code)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var sql = @"SELECT CASE WHEN [Name] IS NULL THEN 0 ELSE 1 END AS Requisicion FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_B] WHERE code = @Code";

                return await conn.ExecuteScalarAsync<bool>(sql, new { Code = code});
            }
        }

        public async Task<bool> ExistePerfil(int idPuesto)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"IF(EXISTS(SELECT * FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_PERFIL] WHERE U_IdPuesto = @IdPuesto))
                                SELECT 1
                                ELSE SELECT 0";

                return await conn.ExecuteScalarAsync<bool>(query, new { IdPuesto  = idPuesto });
            }
        }

        public async Task<int> ObtenerCorrelativoLlamadaAtencion(int empId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT COUNT(*) + 1 AS Correlativo FROM [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_LLA_ATE]
                                WHERE U_EmpId = @EmpId";
                return await conn.ExecuteScalarAsync<int>(query, new { EmpId = empId });
            }
        }

        public async Task<IEnumerable<LlamadaDeAtencion>> ObtenerLlamadasDeAtencionPorUsuario(int empId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = @"SELECT	T1.Name AS Falta,
		                                CONVERT(DATE,T0.U_Fecha) AS Fecha,
		                                T0.U_Observaciones AS Observaciones,
		                                T0.U_NombreArchivo AS NombreArchivo
                                FROM [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_LLA_ATE] T0
                                INNER JOIN [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_CAUSAS] T1 ON T0.U_TipoDeFalta = T1.Code
                                WHERE U_EmpId = @EmpId";

                return await conn.QueryAsync<LlamadaDeAtencion>(query, new { EmpId = empId });
            }
        }

        public async Task<SolvenciaAdministrativaJefe> ObtenerSolvenciaAdministrativaJefe(int empleadoId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT TOP 1 T0.Code,
			                        T0.U_JefeInmediato,
			                        U_Herramientas,
                                    U_Gafete, 
			                        U_FechaSolvenciaJefe,
			                        U_HoraSolvenciaJefe,
			                        U_ObsevacionesJefeInmediato
	                        FROM [ELTEJAR_PRUEBAS_R1_5.1].DBO.[@GESTION_EMP_SOL_ADM] T0
	                        INNER JOIN [ELTEJAR_PRUEBAS_R1_5.1].DBO.[@GESTION_EMP_B] T1 ON t0.Code = t1.code
	                        WHERE T1.U_EmpleadoId = @EmpleadoId";

                    return await conn.QuerySingleOrDefaultAsync<SolvenciaAdministrativaJefe>(query, new { EmpleadoId = empleadoId }) ?? new SolvenciaAdministrativaJefe();
                }
            }
            catch(Exception ex)
            {
                throw;
            } 
        }
    }
}
