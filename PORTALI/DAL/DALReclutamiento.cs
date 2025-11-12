using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection;
using Entity;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DALReclutamiento
    {
        private readonly string connectionString;
        public DALReclutamiento()
        {
            ConnectionEntity pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<IEnumerable<ReclutemientoEntity>> VerSolicitudesDePersonal()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var sp = "sp_ver_puestos_solicitados";

                    return await conn.QueryAsync<ReclutemientoEntity>(sp, commandType: CommandType.Text, commandTimeout: 60);
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
