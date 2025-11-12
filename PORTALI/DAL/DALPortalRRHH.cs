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

namespace DAL
{
    public class DALPortalRRHH
    {
        private readonly string connectionString;

        public DALPortalRRHH()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<List<MarcajeEntity>> ObtenerIncumplimiento(MarcajeFiltrosEntiry model, SessionLoginEntity user)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_obtener_incumplimientos_marcaje";

                    return (List<MarcajeEntity>)await connection.QueryAsync<MarcajeEntity>(sp, new { @FechaI = model.FechaInicio, @FechaF = model.FechaFin, @Area = model.area, @Departamento = user.Depto, @Usuario = user.CodeEmpleado, @UserT360 = user.UserId, @Nivel = user.Nivel }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<MarcajeEntity>{
                     new MarcajeEntity{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<List<DetalleMarcaje>> ObtenerDetalleMarcaje(MarcajeFiltrosEntiry model, SessionLoginEntity user)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_obtener_incumplimientos_marcaje_detalle";

                    return (List<DetalleMarcaje>)await connection.QueryAsync<DetalleMarcaje>(sp, new { @FechaI = model.FechaInicio, @FechaF = model.FechaFin, @Departamento = model.CodigoDepartamento, @Area = model.areaDescripcion, @Usuario = user.CodeEmpleado, @UserT360 = user.UserId, @Nivel = user.Nivel }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<DetalleMarcaje>
                {
                    new DetalleMarcaje
                    {
                        ErrorMessage = ex.Message
                    }
                };
            }
        }

        public async Task<List<DetalleMarcaje>> ObtenerDetalleMarcajePorEmpleado(MarcajeFiltrosEntiry model)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_obtener_incumplimientos_marcaje_detalle_por_empleado";

                    return (List<DetalleMarcaje>)await connection.QueryAsync<DetalleMarcaje>(sp, new { @FechaI = model.FechaInicio, @FechaF = model.FechaFin, @IdEmpleado = model.CodigoEmpleado }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<DetalleMarcaje>
                {
                    new DetalleMarcaje
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }

        public async Task<List<DetalleVacaciones>> ObternerDetalleVacaciones(FiltroVacaciones model)
        {
            try
            {
                var Fechas = new
                {
                    FechaInicio = (model.FechaInicio < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : model.FechaInicio,
                    FechaFin = (model.FechaFin < (DateTime)SqlDateTime.MinValue) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1) : model.FechaFin
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_vaciones_empleados";

                    return (List<DetalleVacaciones>)await connection.QueryAsync<DetalleVacaciones>(sp, new { @FechaInicio = Fechas.FechaInicio, @FechaFin = Fechas.FechaFin, @Area = model.Area, @Departamento = model.Departamento, @CodEmpleado = model.CodEmpleado, @Tipo = model.TipoPermiso }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<DetalleVacaciones>
                {
                    new DetalleVacaciones
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }
        public async Task<List<AreaEntity>> ObtenerAreas()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT FldValue AS Codigo,Descr AS Descripcion FROM UFD1 WHERE TableID = 'OHEM' AND FieldID = 2 ";

                    return (List<AreaEntity>)await connection.QueryAsync<AreaEntity>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<AreaEntity>
                {
                    new AreaEntity
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }
        public async Task<List<DepartamentosEntity>> ObtenerDepartamentos(int codigoArea)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $@"DECLARE @CodigoArea INT = {codigoArea}
                     SELECT DISTINCT
                     ISNULL(T7.FldValue, 1) AS CodigoArea,
                     ISNULL(T7.Descr, 'Administrativo') AS Area,
                     ISNULL(T8.Code, ISNULL(T7.FldValue, 1)) AS CodigoDepartamento,
                     ISNULL(T8.Name, ISNULL(T7.Descr, 'Administrativo')) AS Departamento
                     FROM OHEM e
                     LEFT OUTER JOIN OUDP T8 ON e.dept = T8.Code
                     LEFT OUTER JOIN UFD1 T7 ON T7.TableID = 'OHEM' AND FieldID = 2 AND T7.FldValue = e.U_Area
                     WHERE(@CodigoArea = 0 OR T7.FldValue = @CodigoArea)";

                    return (List<DepartamentosEntity>)await connection.QueryAsync<DepartamentosEntity>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<DepartamentosEntity>
                {
                    new DepartamentosEntity
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }
        public async Task<List<EmpleadoEntity>> BuscarEmpleadosPorNombre(string nombre, SessionLoginEntity user)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_buscar_empleado_por_nombre";

                    return (List<EmpleadoEntity>)await connection.QueryAsync<EmpleadoEntity>(sp, new { @NombreBuscado = nombre, @UserId = user.CodeEmpleado, @Departamento = user.Depto, @UserT360 = user.UserId, @Nivel = user.Nivel }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<EmpleadoEntity>
                {
                    new EmpleadoEntity
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }
        public async Task<List<GetPermisoEntity>> ObtenerPermisosPorEmpleado(SessionLoginEntity User, MarcajeFiltrosEntiry fechas)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_obterner_permisos";

                    return (List<GetPermisoEntity>)await connection.QueryAsync<GetPermisoEntity>(query, new { @Departamento = User.Depto, @Usuario = User.CodeEmpleado, @FechaInicial = fechas.FechaInicio, @FechaFinal = fechas.FechaFin, @Estado = fechas.estado, @UserT360 = User.UserId, @Nivel = User.Nivel }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<GetPermisoEntity>
                {
                    new GetPermisoEntity
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }
        public async Task<List<FirmaDigitalEntity>> ObtenerFirmaDigital(int Solicitante, int LineId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_verifiacar_firma_de_documento";

                    return (List<FirmaDigitalEntity>)await connection.QueryAsync<FirmaDigitalEntity>(query, new { @Solicitante = Solicitante, @LineId = LineId }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<FirmaDigitalEntity>
                {
                    new FirmaDigitalEntity
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }

        public async Task<int> ExisteRegistroPermiso(int user)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT top 1 1 FROM [@PERMISOEMPLEADO_DET] where code != '*' and Code = @code";

                    return await connection.QuerySingleAsync<int>(query, new { @code = user }, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<TiposPermisosEntity>> ObternerTiposPermisos()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM [@TIPOS_PERMISOS]";

                    return (List<TiposPermisosEntity>)await connection.QueryAsync<TiposPermisosEntity>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<TiposPermisosEntity>
                {
                    new TiposPermisosEntity
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }
        public async Task<List<AsistenciaModel>> ObternerAsistencia(DateTime? fechaI , DateTime? fechaF, string nombre = "", int userId = 0)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ver_horarios_ingreso_por_sucursal";

                    return (List<AsistenciaModel>)await connection.QueryAsync<AsistenciaModel>(sp,new { @FechaI = fechaI, @FechaF = fechaF, @Nombre = nombre, @UserId = userId} , commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<AsistenciaModel>
                {
                    new AsistenciaModel
                    {
                        ErrorMessage =  ex.Message
                    }
                };
            }
        }

        public async Task<int> EsJefe(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $"IF({userId} = 0) BEGIN SELECT 1 END ELSE BEGIN SELECT COUNT(*) FROM OHEM WHERE U_JefeInmediato = {userId} END";

                    return await connection.QuerySingleAsync<int>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return 0;

            }
        }

        public async Task<int> LineIdPermiso(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $"SELECT COUNT(*) + 1 FROM [@PERMISOEMPLEADO_DET] WHERE Code != '*' and  Code = {userId}";

                    return await connection.QuerySingleAsync<int>(query, commandType: CommandType.Text, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return 0;

            }
        }


    }
}
