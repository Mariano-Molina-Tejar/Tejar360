using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection;
using Dapper;
using Entity;

namespace DAL
{
    public class EmpleadosDAL
    {
        private readonly string connectionString;

        public EmpleadosDAL()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<IEnumerable<EmpleadosEntity>> ObtenerListaEmpledos()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_datos_empleados";

                    return await conn.QueryAsync<EmpleadosEntity>(
                        sp,
                        new { Nombre = "" },
                        commandType: System.Data.CommandType.StoredProcedure
                        );
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Posicion>> ObtenerPosiciones()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT	posID AS NoPosicion,
		                                    T0.[name] AS Nombre,
		                                    descriptio AS Descripcion,
		                                    CASE WHEN T1.Code IS NULL THEN 0 ELSE 1 END AS Perfil
                                    FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].OHPS T0
                                    LEFT JOIN [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_PERFIL] T1 ON T0.posID = T1.U_IdPuesto
                                ";

                    return await conn.QueryAsync<Posicion>(
                        query
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<EmpleadoSL> ObtenerInformacionEmpleado(int empId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"  SELECT	T1.firstName,
		                                    T1.middleName,
		                                    T1.lastName,
		                                    T1.jobTitle,
		                                    T1.position,
		                                    T1.dept,
		                                    T1.empID,
		                                    T1.Active,
		                                    T1.mobile,
		                                    T1.hometel,
		                                    T1.email,
                                            T1.U_JefeInmediato,
                                            T1.U_Tienda
                                    FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].OHEM T1
                                    WHERE T1.empID = @EmpId";

                    return await conn.QuerySingleAsync<EmpleadoSL>(
                        query,
                        new { EmpId = empId }
                        ) ?? new EmpleadoSL();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Departamentos>> ObtenerTodosLosDepartamentos()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT T0.Code,T0.[Name], Remarks, t2.empID, CONCAT(T2.FIRSTNAME,'',T2.lastName) AS Gerente
                                    FROM OUDP T0
                                    LEFT JOIN [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[@GESTION_EMP_GER_DEP] T1 ON T0.Code = T1.CODE
                                    LEFT JOIN OHEM T2 ON T1.U_IdEmpleado = T2.empID";

                    return await conn.QueryAsync<Departamentos>(
                        query
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Tiendas>> ObtenerListadoDeTiendas()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT WhsCode, WhsName FROM  [ELTEJAR_PRUEBAS_R1_5.1].[DBO].OWHS T0
                                  WHERE T0.WhsName LIKE 'Tienda%' OR T0.WhsCode = 'BTP-CD'";

                    return await conn.QueryAsync<Tiendas>(
                        query
                        );
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> ObtenerAreas()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"SELECT FldValue, Descr FROM UFD1
                                  WHERE TableID = 'OHEM' AND FieldID = 2";

                    return await conn.QueryAsync<dynamic>(
                        query
                        );
                }
                ;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ExisteDatoGerente(int code)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var query = @"IF EXISTS (SELECT 1
                                               FROM [ELTEJAR_PRUEBAS_R1_5.1].[dbo].[@GESTION_EMP_GER_DEP]
                                               WHERE Code = @Codigo)
                                        SELECT 1 AS Existe;
                                    ELSE
                                        SELECT 0 AS Existe;";

                    return await conn.ExecuteScalarAsync<bool>(
                        query,
                        new { Codigo = code }
                        );
                }
            }
            catch
            {
                throw;
            }
        }
    }
}