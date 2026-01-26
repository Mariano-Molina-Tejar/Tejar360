using Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection;
using System.Data.SqlClient;
using Entity;
using Dapper;

namespace DAL
{
    public class LockServiceDAL
    {
        private readonly string connectionString;

        public LockServiceDAL()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        private SqlConnection conexionDB ()=> new SqlConnection (connectionString);

        public async Task<LockUserOperativos> ObtenerUsuarios(int empId)
        {
            using (var conn = conexionDB())
            {
                var query = @"SELECT T1.USERID AS SAP,t1.U_Contrasena AS Tejar360,T0.salesPrson AS EmpleadoDeVentas 
                                FROM [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[OHEM] T0 
                                LEFT JOIN [ELTEJAR_PRUEBAS_R1_5.1].[DBO].[OUSR] T1 ON T0.UserId = T1.USERID
                                WHERE T0.EmpId = @EmpId";

                return await conn.QuerySingleAsync<LockUserOperativos>(query, new { EmpId = empId });
            }
        }
    }
}
