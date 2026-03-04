using Connection;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace DAL
{
    public class SolvenciaDAL
    {
        private readonly string _connectionString;
        public SolvenciaDAL()
        {
            ConnectionEntity pConnection = Conexion.ConexionDB();
            _connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        private SqlConnection getConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<EmpleadosSolvencia>> GetEmployedSolvency()
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT	T1.EmpId,
		                        CONCAT(T1.FirstName,' ',T1.LastName) AS Nombre,
		                        T2.Remarks AS Departamento,
		                        T3.Descriptio AS Puesto,
		                        T3.posID AS PuestoId,
		                        CASE WHEN T5.U_FechaSolvenciaIT IS NULL THEN 0 ELSE 1 END AS EmisioDeSolvencia
                        FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T0
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHEM T1 ON T0.U_EmpleadoId = t1.empId
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OUDP T2 ON T1.dept = t2.code
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHPS T3 ON T1.position = T3.PosId
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T4 ON t4.U_EmpleadoId = t1.empID
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T5 ON T4.Code = T5.Code";

                return await conn.QueryAsync<EmpleadosSolvencia>(query);
            }
        }
        public async Task<IEnumerable<EmpleadosSolvencia>> GetEmployedSolvencyFinance()
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT	T1.EmpId,
	                            CONCAT(T1.FirstName,' ',T1.LastName) AS Nombre,
	                            T2.Remarks AS Departamento,
	                            T3.Descriptio AS Puesto,
	                            T3.posID AS PuestoId,
	                            CASE WHEN T5.U_FechaSolvenciaFinanzas IS NULL THEN 0 ELSE 1 END AS EmisioDeSolvencia
                            FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T0
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHEM T1 ON T0.U_EmpleadoId = t1.empId
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OUDP T2 ON T1.dept = t2.code
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHPS T3 ON T1.position = T3.PosId
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T4 ON t4.U_EmpleadoId = t1.empID
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T5 ON T4.Code = T5.Code";

                return await conn.QueryAsync<EmpleadosSolvencia>(query);
            }
        }
        public async Task<IEnumerable<EmpleadosSolvencia>> GetEmployedSolvencyAudit()
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT	T1.EmpId,
	                            CONCAT(T1.FirstName,' ',T1.LastName) AS Nombre,
	                            T2.Remarks AS Departamento,
	                            T3.Descriptio AS Puesto,
	                            T3.posID AS PuestoId,
	                            CASE WHEN T5.U_FechaSolvenciaAuditoria IS NULL THEN 0 ELSE 1 END AS EmisioDeSolvencia
                            FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T0
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHEM T1 ON T0.U_EmpleadoId = t1.empId
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OUDP T2 ON T1.dept = t2.code
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHPS T3 ON T1.position = T3.PosId
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T4 ON t4.U_EmpleadoId = t1.empID
                            LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T5 ON T4.Code = T5.Code";

                return await conn.QueryAsync<EmpleadosSolvencia>(query);
            }
        }
        public async Task<IEnumerable<EmpleadosSolvencia>> GetEmployedSolvencyPaysheet()
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT	T1.EmpId,
	                        CONCAT(T1.FirstName,' ',T1.LastName) AS Nombre,
	                        T2.Remarks AS Departamento,
	                        T3.Descriptio AS Puesto,
	                        T3.posID AS PuestoId,
	                        CASE WHEN T5.U_FechaSolvenciaNomina IS NULL THEN 0 ELSE 1 END AS EmisioDeSolvencia
                        FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T0
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHEM T1 ON T0.U_EmpleadoId = t1.empId
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OUDP T2 ON T1.dept = t2.code
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.OHPS T3 ON T1.position = T3.PosId
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T4 ON t4.U_EmpleadoId = t1.empID
                        LEFT JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T5 ON T4.Code = T5.Code";

                return await conn.QueryAsync<EmpleadosSolvencia>(query);
            }
        }

        public async Task<SolvenciaIT> GetSolvencyIT(int empId)
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT TOP 1 T0.Code,
	                            U_EquipoDeComputo,
                                U_TelefonoYAccesorios, 
	                            U_FechaSolvenciaIT,
	                            U_HoraSolvenciaIT,
	                            U_ObeservacionesIT
                            FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T0
                            INNER JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T1 ON t0.Code = t1.code
                            WHERE T1.U_EmpleadoId = @EmpId";

                return await conn.QuerySingleOrDefaultAsync<SolvenciaIT>(query, new { EmpId = empId }) ?? new SolvenciaIT();
            }
        }
        public async Task<SolvenciaNomina> GetSolvencyPaySheet(int empId)
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT TOP 1 T0.Code,
	                            U_AnticiposSalarios,
                                U_PrestamoBancario, 
	                            U_Embargos,
	                            U_DescuentoUniforme,
	                            U_DevolucionISR,
	                            U_Uniformes,
	                            U_NotificacionAreasInteresadas,
	                            U_FechaSolvenciaNomina,
	                            U_HoraSolvenciaNomina,
	                            U_ObservacionesNomina
                            FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T0
                            INNER JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T1 ON t0.Code = t1.code
                            WHERE T1.U_EmpleadoId = @EmpId";

                return await conn.QuerySingleOrDefaultAsync<SolvenciaNomina>(query, new { EmpId = empId }) ?? new SolvenciaNomina();
            }
        }
        public async Task<SolvenciaAditoria> GetSolvencyAudit(int empId)
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT TOP 1 T0.Code,
	                            U_CompraEmpleados,
                                U_FaltantesBodega, 
	                            U_FechaSolvenciaAuditoria,
	                            U_HoraSolvenciaAuditoria,
	                            U_ObservacionesAuditoria
                            FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T0
                            INNER JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T1 ON t0.Code = t1.code
                            WHERE T1.U_EmpleadoId = @EmpId";

                return await conn.QuerySingleOrDefaultAsync<SolvenciaAditoria>(query, new { EmpId = empId }) ?? new SolvenciaAditoria();
            }
        }
        public async Task<SolvenciaContabilidad> GetSolvencyContabilidad(int empId)
        {
            using (var conn = getConnection())
            {
                var query = @"SELECT TOP 1 T0.Code,
			                    U_CuentaEspecial,
			                    U_LiquidacionDeViaticos, 
			                    U_FechaSolvenciaFinanzas,
			                    U_HoraSolvenciaFinanzas,
			                    U_ObeservacionesFinanzas
	                    FROM [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_SOL_ADM] T0
	                    INNER JOIN [ELTEJAR_PRUEBAS_21022026].DBO.[@GESTION_EMP_B] T1 ON t0.Code = t1.code
	                    WHERE T1.U_EmpleadoId = @EmpId";

                return await conn.QuerySingleOrDefaultAsync<SolvenciaContabilidad>(query, new { EmpId = empId }) ?? new SolvenciaContabilidad();
            }
        }
    }
}
