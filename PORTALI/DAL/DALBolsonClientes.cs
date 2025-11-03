using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection;
using Dapper;
using Entity;

namespace DAL
{
    public class DALBolsonClientes
    {
        private readonly string connectionString;

        public DALBolsonClientes()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<List<ReporteVentas>> ObtenerBolsonClientes(string SlpCode)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_bolson_clientes";

                    return (List<ReporteVentas>)await connection.QueryAsync<ReporteVentas>(sp, new { @WhsCode = "", @Region = "", @SlpCode = SlpCode }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<ReporteVentas>{
                     new ReporteVentas{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<BolsonClientesInfo> ObtenerBolsonClientesEncabezado(string SlpCode)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_bolson_clientes_encabezado";

                    return (BolsonClientesInfo)await connection.QuerySingleAsync<BolsonClientesInfo>(sp, new { @WhsCode = "", @Region = "", @SlpCode = SlpCode }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new BolsonClientesInfo { ErrorMessage = ex.Message };

            }
        }
    }
}
