using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Connection;
using System.Data.SqlClient;
using Entity;
using System.Data;

namespace DAL
{
    public class DALFlujoCD
    {
        private readonly string connectionString;
        public DALFlujoCD()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }
        public async Task<List<FlujoCD_Entity>> ObtenerFLujoEntradasSalidas(
            DateTime fechaI,
            DateTime fechaF,
            string almacen,
            int unidad
            )
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_flujo_ingreso_egresos_cajas_metros";

                    return (List<FlujoCD_Entity>)await connection.QueryAsync<FlujoCD_Entity>(
                        sp,
                        new
                        {
                            @FechaI = fechaI,
                            @FechaF = fechaF,
                            @Almacen = almacen,
                            @UnidadMedida = unidad
                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 120
                        );
                }
            }
            catch (Exception ex)
            {
                return new List<FlujoCD_Entity>
                {
                    new FlujoCD_Entity
                    {
                        ErrorMessage = ex.GetType().Name
                    }
                };
            }
        }

        public async Task<List<AlmacenesFLujoCD>> ObtenerAlmacenes()
        {
            try
            {
                using (var Connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT WhsCode, WhsName FROM OWHS WHERE WhsName like 'Tienda%' OR WhsCode = 'BTP-CD'";

                    return (List<AlmacenesFLujoCD>)await Connection.QueryAsync<AlmacenesFLujoCD>(
                        query,
                        commandType: CommandType.Text,
                        commandTimeout: 120
                        );
                }
            }
            catch (Exception ex)
            {
                return new List<AlmacenesFLujoCD> { new AlmacenesFLujoCD { MessageError = ex.Message } };
            }
        }

    }
}
