using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DAL
{
    public class DALOrdenDeCompra
    {
        public static OcPrintEntity ocImprimir(int DocNum)
        {
            OcPrintEntity datos = new OcPrintEntity();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_oc_imprimir", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@DocNum", DocNum);

                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);
                            OcPrintEntity enc = new OcPrintEntity
                            {   
                                NombreSucursal = dt.Rows[0]["NombreSucursal"].ToString(),
                                DocNum = Convert.ToInt32(dt.Rows[0]["DocNum"].ToString()),
                                CardCode = dt.Rows[0]["CardCode"].ToString(),
                                CardName = dt.Rows[0]["CardName"].ToString(),
                                DocDate = Convert.ToDateTime(dt.Rows[0]["DocDate"].ToString()),
                                E_Mail = dt.Rows[0]["E_Mail"].ToString(),
                                Address2 = dt.Rows[0]["Address2"].ToString(),
                                DocCur = dt.Rows[0]["DocCur"].ToString(),
                                Iva = Convert.ToDouble(dt.Rows[0]["Iva"].ToString()),
                                Subtotal = Convert.ToDouble(dt.Rows[0]["Subtotal"].ToString()),                                
                                DocDueDate = Convert.ToDateTime(dt.Rows[0]["DocDueDate"].ToString()),
                                DocTotal = Convert.ToDouble(dt.Rows[0]["DocTotal"].ToString()),                                
                                Comments = dt.Rows[0]["Comments"].ToString(),
                                U_Name = dt.Rows[0]["U_Name"].ToString(),
                                PymntGroup = dt.Rows[0]["PymntGroup"].ToString(),
                                NitSucursal = dt.Rows[0]["NitSucursal"].ToString()
                            };

                            enc.Detalle = (from row in dt.AsEnumerable()
                                           select new DetalleOc
                                           {
                                               ItemCode = row["ItemCode"].ToString(),
                                               Dscription = row["Dscription"].ToString(),
                                               Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                                               Almacen = row["Almacen"].ToString(),
                                               PrecioU = Convert.ToDouble(row["PrecioU"].ToString()),
                                               UnidadMed = row["UnidadMed"].ToString(),
                                               LineTotal = Convert.ToDouble(row["LineTotal"].ToString())
                                           }).ToList();
                            datos = enc;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Aquí puedes loguear el error si lo deseas
                        Console.WriteLine("Error al ejecutar Filtro: " + ex.Message);
                    }
                }
            }

            return datos;
        }
        public static ListasGeneralesEntity getItemName(string ItemCode)
        {
            ListasGeneralesEntity Lista = new ListasGeneralesEntity();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("SELECT UPPER(ItemName) As ItemName FROM OITM WHERE ItemCode = '" + ItemCode + "' AND validFor = 'Y'", iConnection))
                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            if(dt.Rows.Count > 0) 
                            {
                                Lista.sCodigo = ItemCode;
                                Lista.Descripcion = dt.Rows[0]["ItemName"].ToString();
                            }                            
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Lista;
        }

        public static GetSingleOcEntity getOc(int DocNum)
        {
            GetSingleOcEntity datos = new GetSingleOcEntity();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_getSingleOc", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@DocNum", DocNum);

                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);
                            GetSingleOcEntity enc = new GetSingleOcEntity
                            {
                                DocEntry = Convert.ToInt32(dt.Rows[0]["DocEntry"].ToString()),
                                DocNum = Convert.ToInt32(dt.Rows[0]["DocNum"].ToString()),
                                CardCode = dt.Rows[0]["CardCode"].ToString(),
                                CardName = dt.Rows[0]["CardName"].ToString(),
                                DocDate = Convert.ToDateTime(dt.Rows[0]["DocDate"].ToString()),
                                DocDueDate = Convert.ToDateTime(dt.Rows[0]["DocDueDate"].ToString()),
                                DocTotal = Convert.ToDouble(dt.Rows[0]["DocTotal"].ToString()),
                                BPLId = Convert.ToInt32(dt.Rows[0]["BPLId"].ToString()),
                                Comments = dt.Rows[0]["Comments"].ToString()
                            };

                            enc.Detalle = (from row in dt.AsEnumerable()
                                     select new getSingleOcDetalleEntity
                                     {                                         
                                         ItemCode = row["ItemCode"].ToString(),
                                         Dscription = row["Dscription"].ToString(),
                                         TaxCode = row["TaxCode"].ToString(),
                                         Quantity = Convert.ToDouble(row["Quantity"]),
                                         PriceAfVat = Convert.ToDouble(row["PriceAfVat"]),
                                         GTotal = Convert.ToDouble(row["GTotal"]),
                                         OcrCode = Convert.ToString(row["OcrCode"])
                                     }).ToList();
                            datos = enc;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Aquí puedes loguear el error si lo deseas
                        Console.WriteLine("Error al ejecutar Filtro: " + ex.Message);
                    }
                }
            }

            return datos;
        }

        public static List<OrdenCompraFiltroEntity> Filtro(string CardCode, DateTime? FechaI, DateTime? FechaF, int? DocNum)
        {
            List<OrdenCompraFiltroEntity> Lista = new List<OrdenCompraFiltroEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_oc_filtros", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;

                    iCommand.Parameters.AddWithValue("@CardCode", string.IsNullOrEmpty(CardCode) ? (object)DBNull.Value : CardCode);
                    iCommand.Parameters.AddWithValue("@FechaI", FechaI.HasValue ? (object)FechaI.Value : DBNull.Value);
                    iCommand.Parameters.AddWithValue("@FechaF", FechaF.HasValue ? (object)FechaF.Value : DBNull.Value);
                    iCommand.Parameters.AddWithValue("@DocNum", DocNum.HasValue ? (object)DocNum.Value : DBNull.Value);

                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new OrdenCompraFiltroEntity
                                     {
                                         DocNum = Convert.ToInt32(row["DocNum"]),
                                         CardCode = row["CardCode"].ToString(),
                                         CardName = row["CardName"].ToString(),
                                         DocDate = Convert.ToDateTime(row["DocDate"]),
                                         DocTotal = Convert.ToDouble(row["DocTotal"]),
                                         Estado = row["Estado"].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Puedes loguear el error si deseas
                        // Console.WriteLine(ex.Message);
                    }
                }
            }

            return Lista;
        }

        public static List<ListasGeneralesEntity> ListasLineasProductos(int Tipo)
        {
            List<ListasGeneralesEntity> Lista = new List<ListasGeneralesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("SELECT UPPER(OcrCode) As OcrCode,UPPER(OcrName) As OcrName FROM OOCR", iConnection))
                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ListasGeneralesEntity
                                     {
                                         sCodigo = row[0].ToString(),
                                         Descripcion = row[1].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Lista;
        }

        public static List<ListasGeneralesEntity> ListadoTipoArticulo()
        {
            List<ListasGeneralesEntity> Lista = new List<ListasGeneralesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("SELECT FldValue As Codigo,Descr As Descripcion FROM UFD1 WHERE TableId = 'POR1' AND FieldID = 2", iConnection))
                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ListasGeneralesEntity
                                     {
                                         sCodigo = row[0].ToString(),
                                         Descripcion = row[1].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Lista;
        }

        public static List<ListasGeneralesEntity> getTipoDeCambio()
        {
            List<ListasGeneralesEntity> Lista = new List<ListasGeneralesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_oc_tipo_cambio", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ListasGeneralesEntity
                                     {
                                         sCodigo = row[0].ToString(),
                                         Descripcion = row[1].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Lista;
        }

        public static List<ListasGeneralesEntity> ListadoCentroDeCosto()
        {
            List<ListasGeneralesEntity> Lista = new List<ListasGeneralesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("SELECT UPPER(OcrCode) As OcrCode,UPPER(OcrName) As OcrName FROM OOCR", iConnection))
                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ListasGeneralesEntity
                                     {
                                         sCodigo = row[0].ToString(),
                                         Descripcion = row[1].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Lista;
        }

        public static List<ListasGeneralesEntity> ListadoProyectoEtapa(int Tipo, string Codigo)
        {
            List<ListasGeneralesEntity> Lista = new List<ListasGeneralesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_oc_proyecto_etapa " + Tipo + ", '" + Codigo + "'", iConnection))
                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ListasGeneralesEntity
                                     {
                                         Codigo = int.Parse(row[0].ToString()),
                                         Descripcion = row[1].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Lista;
        }

        public static List<ListasGeneralesEntity> ListadoSucursales()
        {
            List<ListasGeneralesEntity> Lista = new List<ListasGeneralesEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("SELECT a.BPLId,a.BPLName FROM OBPL a WHERE a.[Disabled] = 'N'", iConnection))
                {
                    iCommand.CommandType = CommandType.Text;
                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ListasGeneralesEntity
                                     {
                                         Codigo = int.Parse(row[0].ToString()),
                                         Descripcion = row[1].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            return Lista;
        }
        public static List<ProveedoresEntity> ListadoProveedores(string SocioDeNegocio)
        {
            List<ProveedoresEntity> Lista = new List<ProveedoresEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_oc_buscador_proveedores", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@SocioDeNegocio", string.IsNullOrEmpty(SocioDeNegocio) ? "" : SocioDeNegocio);                    

                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ProveedoresEntity
                                     {
                                         CardCode = row["CardCode"].ToString(),
                                         CardName = row["CardName"].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Podés loguear el error si querés
                        // Console.WriteLine(ex.Message);
                    }
                }
            }

            return Lista;
        }

        public static List<ProductosOcEntity> ListadoProducto(string Producto)
        {
            List<ProductosOcEntity> Lista = new List<ProductosOcEntity>();
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_oc_buscador_productos", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@Producto", string.IsNullOrEmpty(Producto) ? "" : Producto);

                    try
                    {
                        iConnection.Open();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            DataTable dt = new DataTable();
                            iDAResult.Fill(dt);

                            Lista = (from row in dt.AsEnumerable()
                                     select new ProductosOcEntity
                                     {
                                         ItemCode = row["ItemCode"].ToString(),
                                         ItemName = row["ItemName"].ToString()
                                     }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Podés loguear el error si querés
                        // Console.WriteLine(ex.Message);
                    }
                }
            }

            return Lista;
        }

    }
}
