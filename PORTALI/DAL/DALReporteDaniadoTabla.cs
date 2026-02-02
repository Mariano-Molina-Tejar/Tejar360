using Connection;
using Dapper;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALReporteDaniadoTabla
    {
        private readonly string connectionString;

        public DALReporteDaniadoTabla()
        {
            var connection = Conexion.ConexionDB();
            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
        }

        //Para Obtener ventas, montos dañado y porcentajes de Bodega Central 
        public async Task<List<TablaCDVentasDaniadoEntity>> ObtenerDatosCD(DateTime fechaI, DateTime fechaF)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_DaniadoBodegaCentral";

                    var parametros = new
                    {
                        FechaI = fechaI,
                        FechaF = fechaF
                    };


                    var resultado = await connection.QueryAsync<TablaCDVentasDaniadoEntity>
                   (
                       sp,
                       parametros,
                       commandType: CommandType.StoredProcedure,
                       commandTimeout: 120
                   );

                    return resultado.ToList();
                }
            }
            catch (Exception ex)
            {

                //throw new Exception("DAL: Error al ejecutar sp_crm_venta_diaria_cd_v2", ex);
                return new List<TablaCDVentasDaniadoEntity>
                {
                    new TablaCDVentasDaniadoEntity { ErrorMessage = ex.Message }
                };
            }
        }


        ////Para Obtener ventas, montos dañado y porcentajes de Tiendas
        public async Task<List<TablaTiendasVentasDaniadoEntity>> ObtenerDatosTiendas(DateTime fechaI, DateTime fechaF)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_DaniadoTiendas";

                    var parametros = new
                    {
                        FechaI = fechaI,
                        FechaF = fechaF
                    };


                    var resultado = await connection.QueryAsync<TablaTiendasVentasDaniadoEntity>
                   (
                       sp,
                       parametros,
                       commandType: CommandType.StoredProcedure,
                       commandTimeout: 120
                   );

                    return resultado.ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<TablaTiendasVentasDaniadoEntity>
                {
                    new TablaTiendasVentasDaniadoEntity { ErrorMessage = ex.Message }
                };
            }
        }

       

    }

    #region Entidades

    public class TablaTiendasVentasDaniadoEntity
    {
        public int Region { get; set; }
        public string Localizacion { get; set; }
        public string CodTienda { get; set; }
        public string Tienda { get; set; }
        public decimal Venta { get; set; }
        public decimal TotalDaniado { get; set; }  //igual a Daño en almace
        public decimal PorcentajeDaniadoAlmacen { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TablaCDVentasDaniadoEntity
    {
        public int Region { get; set; }
        public string  Localizacion { get; set; }
        public string CodTienda { get; set; }
        public string Tienda { get; set; }
        
        //Montos
        public decimal Venta { get; set; }
        public decimal Danado_Salida_CD { get; set; }
        public decimal Danado_NotaCredito { get; set; }
        public decimal Danado_Recuperado { get; set; }
        public decimal MontoTotalDaniado { get; set; }

        //Porcentajes
        public decimal PorcentajeNC { get; set; }
        public decimal PorcentajeAlmacen { get; set; }
        public decimal  PorcentajeTotalDaniado { get; set; }

        public string ErrorMessage { get; set; }

    }


    #endregion
}
