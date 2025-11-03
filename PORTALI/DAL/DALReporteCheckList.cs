using Connection;
using Dapper;
using Entity;
using Entity.Filtros;
using Entity.Utilitario.LlenadoDropDown;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALReporteCheckList
    {
        private readonly string connectionString;

        public DALReporteCheckList()
        {
            var connection = Conexion.ConexionDB();
            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";

        }

        public async Task<List<ReporteCheckListEntity>> ObtenerCheckList(FiltroReporteCheckList filtro)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_ReporteChechkListBodega";

                var parametros = new
                {
                    FechaInicio = filtro.FechaInicio,
                    FechaFin = filtro.FechaFin,
                    CodigoTienda = filtro.CodigoTienda,
                    CodigoArea = filtro.CodigoArea,
                    CodigoRegion = filtro.CodigoRegion
                };

                var resultado = await connection.QueryAsync<ReporteCheckListEntity>
                    (
                        sp,
                        parametros,
                        commandType: CommandType.StoredProcedure
                    );

                return resultado.ToList();
            }
        }

        public async Task<List<ListasDesplegableCheckList>> ObtenerArea()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "Sp_CheckListObtenerArea";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);
                return resultado.ToList();
            }
        }

        public async Task<List<ListasDesplegableCheckList>> ObtenerSucursal()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_CheckListObtenerSucursales";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }

        public async Task<List<ListasDesplegableCheckList>> ObtenerRegion()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_ObtenerRegiones";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }


        /*
         Campos a usar en el tablero de checklist
       ---- Filtros -----
        Sucursal
        Area
        Region
        Seccion (Almacenamiento, despacho, recepcion, dañado y 5S)

        ---- Tarjetas -----
        PuntajeAlmacenaje
        PuntajeRecepcion
        PuntajeDesapacho
        PuntajeDaniado
        Puntaje5S

        Calificacion total 


         
         
         */

    }
}
