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
    public class DALPortalPresupuesto
    {
        private readonly string connectionString;

        public DALPortalPresupuesto()
        {
            var pConnection = Conexion.ConexionDB();
            connectionString = $"Server={pConnection.ServerName};Database={pConnection.DataBase};Uid={pConnection.User};Pwd={pConnection.Password};";
        }

        public async Task<List<DetallePresupuestoN1>> ObtenrDetallePresupuestoN1(int? mes, int? area)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_detalle_presupuesto_n1";

                    return (List<DetallePresupuestoN1>)await connection.QueryAsync<DetallePresupuestoN1>(sp, new { @Mes = mes, @AcctCode = area }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<DetallePresupuestoN1>{
                     new DetallePresupuestoN1{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<List<DetallePresupuestoN1>> ObtenrDetallePresupuestoN2(int cuenta, int? mes)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_detalle_presupuesto_n2";

                    return (List<DetallePresupuestoN1>)await connection.QueryAsync<DetallePresupuestoN1>(sp, new { @AcctCode = cuenta, @Mes = mes }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<DetallePresupuestoN1>{
                     new DetallePresupuestoN1{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<List<DetallePresupuestoN3>> ObtenrDetallePresupuestoN3(string cuenta, int? mes)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_detalle_presupuesto_n3";

                    return (List<DetallePresupuestoN3>)await connection.QueryAsync<DetallePresupuestoN3>(sp, new { @AcctCode = cuenta, @Mes = mes }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

                }
            }
            catch (Exception ex)
            {
                return new List<DetallePresupuestoN3>{
                     new DetallePresupuestoN3{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<List<DetalleDocumento>> ObtenrDetalleFactura(int DocNum, string Cuenta, int Id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ver_detalle_factura_presupuesto";

                    return (List<DetalleDocumento>)await connection.QueryAsync<DetalleDocumento>(sp, new { @DocNum = DocNum, @AcctCode = Cuenta, @Tipo = Id }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

                }
            }
            catch (Exception ex)
            {
                return new List<DetalleDocumento>{
                     new DetalleDocumento{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<List<AreasPresupuesto>> ObtenerAreasPresupuesto(int Usuario)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_obtener_arear_ppto";

                    return (List<AreasPresupuesto>)await connection.QueryAsync<AreasPresupuesto>(query, new { @Usuario = Usuario }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

                }
            }
            catch (Exception ex)
            {
                return new List<AreasPresupuesto>{
                     new AreasPresupuesto{ ErrorMessage = ex.Message}
                };
            }
        }
        public async Task<double> ObtenerSaldoFuturo(int? cuenta, int? mes)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_obtener_saldo_futuro_ppto";

                    return await connection.QuerySingleAsync<int>(query, new { @AcctCode = cuenta, @Mes = mes }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<List<ReporteFinanciero>> ObtenerRepoteFInanciero()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_crm_venta_diaria_V2";

                    return (List<ReporteFinanciero>)await connection.QueryAsync<ReporteFinanciero>(query, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<ReporteFinanciero> { new ReporteFinanciero { ErrorMessage = ex.Message } };
            }
        }
        public async Task<List<ReporteFinanciero>> ObtenerRepoteFInancier_cd()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_crm_venta_diaria_cd_v2";

                    return (List<ReporteFinanciero>)await connection.QueryAsync<ReporteFinanciero>(query, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<ReporteFinanciero> { new ReporteFinanciero { ErrorMessage = ex.Message } };
            }
        }
        public async Task<List<ReporteFinanciero>> ObtenerRepoteFInancier_inter()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "sp_crm_venta_diaria_inter";

                    return (List<ReporteFinanciero>)await connection.QueryAsync<ReporteFinanciero>(query, commandType: CommandType.StoredProcedure, commandTimeout: 120);
                }
            }
            catch (Exception ex)
            {
                return new List<ReporteFinanciero> { new ReporteFinanciero { ErrorMessage = ex.Message } };
            }
        }
    }
}
