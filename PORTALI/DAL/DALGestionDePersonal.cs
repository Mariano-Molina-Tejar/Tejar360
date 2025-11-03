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
    }
}
