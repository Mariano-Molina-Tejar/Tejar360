using Connection;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Entity.Utilitario.LlenadoDropDown;
using Entity;

namespace DAL
{
    public class DALFormularioCheckList
    {
        private readonly string connectionString;

        public DALFormularioCheckList()
        {
            var connection = Conexion.ConexionDB();
            connectionString = $"Server={connection.ServerName};Database={connection.DataBase};Uid={connection.User};Pwd={connection.Password};";
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

        public async Task<List<ListasDesplegableCheckList>> ObtenerSupervisoresBodega()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_CheckListObtenerSupervisorBodega";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }

        public async Task<List<ListasDesplegableCheckList>> ObtenerSupervisoresVenta()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_CheckListObtenerSupervisorVenta";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);

                return resultado.ToList();
            }
        }

        public async Task<List<ListasDesplegableCheckList>> ObtenerArea()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_CheckListObtenerArea";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);
                return resultado.ToList();
            }

        }

        public async Task<List<ListasDesplegableCheckList>> ObtenerGerenteTiendas()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sp = "sp_CheckListObtenerGerenteTienda";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);
                return resultado.ToList();
            }
        }
            
        public async Task<List<ListasDesplegableCheckList>> ObtenerEncargadosBodega()
        {
            using (var connection = new SqlConnection(connectionString))    
            {
                string sp = "sp_CheckListObtenerEncargadoBodega";
                var resultado = await connection.QueryAsync<ListasDesplegableCheckList>(sp, commandType: CommandType.StoredProcedure);
                return resultado.ToList();
            }
        }
        public async Task<List<CheckListInfo>> ObtenerDetalleCheckList(DateTime? FechaI, DateTime? FechaF, int Estado, int User360, string Tienda, int Region)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ver_check_sup_procesos_v2";     
                    return (List<CheckListInfo>)await connection.QueryAsync<CheckListInfo>(sp, new { @FechaI = FechaI, @FechaF = FechaF, @Estado = Estado, @USER360 = User360, @Tienda = Tienda, @RegionT = Region }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                return new List<CheckListInfo>
                {
                    new CheckListInfo {
                        Error = ex.Message
                    }
                };
            }
        }
        public async Task<List<ReporteEvaluacion>> ObtenerDetalleCheckListExcel(DateTime? FechaI, DateTime? FechaF, int Estado, int User360, string Tienda, int Region)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ver_check_sup_procesos_excel";
                    return (List<ReporteEvaluacion>)await connection.QueryAsync<ReporteEvaluacion>(sp, new { @FechaI = FechaI, @FechaF = FechaF, @Estado = Estado, @USER360 = User360, @Tienda = Tienda, @RegionT = Region }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                return new List<ReporteEvaluacion>
                {
                    new ReporteEvaluacion {
                        Error = ex.Message
                    }
                };
            }
        }
        public async Task<FormularioCheckList1Entity> ObtenerFormuarioCheckList(string IdFormulario)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $"SELECT * FROM [@CHECKLIST] WHERE U_IdFormulario = '{IdFormulario}'";
                    return await connection.QuerySingleAsync<FormularioCheckList1Entity>(query, commandType: CommandType.Text);
                }
            }
            catch
            {
                return new FormularioCheckList1Entity();
            }
        }
        public async Task<IdFormulario> ObtenerNuevoNumeroDeFormulario()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "ObtenerNuevoNumeroFormulario";
                    return await connection.QuerySingleAsync<IdFormulario>(query, commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                return new IdFormulario();
            }
        }
        public async Task<string> ObtenerNombreResponsable(string empId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $"sp_obtener_nombre_checklist";
                    return await connection.QuerySingleAsync<string>(query,new {@UserId = empId } ,commandType: CommandType.StoredProcedure);
                }
            }
            catch
            {
                return "No se encontro el nombre";
            }
        }
        public async Task<int> FormularioPendiente(string empId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = $"IF EXISTS (SELECT * FROM [@CHECKLIST] WHERE U_IdUsuario = {empId} AND ISNULL(U_Estado,0) = 0) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                    return await connection.QuerySingleAsync<int>(query, commandType: CommandType.Text);
                }
            }
            catch
            {
                return -1;
            }
        }
        public async Task<List<TiendasRegionesCheckListEntity>> TiendasCheckLick(int User360)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sp = "sp_ver_tiendas_check_list";
                    return (List<TiendasRegionesCheckListEntity>)await connection.QueryAsync<TiendasRegionesCheckListEntity>(sp, new { @USER360 = User360 }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                return new List<TiendasRegionesCheckListEntity>
                {
                    new TiendasRegionesCheckListEntity
                    {
                        Error = ex.Message
                    }
                };
            }
        }
    }
}
