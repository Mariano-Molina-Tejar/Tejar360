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
    public class DALProspeccion
    {
        public static List<ListadosProspeccionEntity> getAllList(int Tipo)
        {
            ConnectionEntity pConnection = Connection.Conexion.ConexionDB();
            List<ListadosProspeccionEntity> listado = new List<ListadosProspeccionEntity>();

            using (OleDbConnection iConnection = new OleDbConnection("Provider=SQLOLEDB;Server=" + pConnection.ServerName + ";Database=" + pConnection.DataBase + ";Uid=" + pConnection.User + ";Pwd=" + pConnection.Password + ";"))
            {
                using (OleDbCommand iCommand = new OleDbCommand("sp_crm_prospeccion_visita", iConnection))
                {
                    iCommand.CommandType = CommandType.StoredProcedure;
                    iCommand.Parameters.AddWithValue("@Tipo", Tipo);

                    try
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter iDAResult = new OleDbDataAdapter(iCommand))
                        {
                            iDAResult.Fill(dt);
                        }

                        listado = (from row in dt.AsEnumerable()
                                   select new ListadosProspeccionEntity
                                   {
                                       Codigo = int.Parse(row["Code"].ToString()),
                                       Nombre = row["Name"].ToString()
                                   }).ToList();

                        return listado;
                    }
                    catch (Exception ex)
                    {
                        return new List<ListadosProspeccionEntity>();
                    }
                }
            }
        }
    }
}
