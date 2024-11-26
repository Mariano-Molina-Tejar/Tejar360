using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Connection;
using System.Data.OleDb;
using System.Data;

namespace DAL
{
    public class DALPortalMantProductos
    {
        public static List<PortalListadoGeneralEntity> ListaProductosVentaCruzada(string Id)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<PortalListadoGeneralEntity> listadoProductos = new List<PortalListadoGeneralEntity>();
            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                OleDbCommand iCommand = null;
                iCommand = new OleDbCommand("sp_mantenimientos_filtros_busqueda_productos", iConnection);
                iCommand.CommandType = CommandType.StoredProcedure;
                iCommand.Parameters.AddWithValue("@Id", Id);

                try
                {
                    OleDbDataAdapter iDAResult = null;
                    DataTable dt = new DataTable();
                    iDAResult = new OleDbDataAdapter();
                    iDAResult.SelectCommand = iCommand;
                    iDAResult.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listadoProductos = (from row in dt.AsEnumerable()
                                            select new PortalListadoGeneralEntity()
                                            {
                                                Id = row["Id"].ToString(),
                                                Dscription = row["Dscription"].ToString()
                                            }).ToList();
                        return listadoProductos;
                    }

                    return listadoProductos;
                }
                catch (Exception ex)
                {
                    return new List<PortalListadoGeneralEntity>();
                }
            }
        }
    }
}
