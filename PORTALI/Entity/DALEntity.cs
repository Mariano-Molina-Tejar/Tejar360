using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entity
{
    public class DALEntity
    {
        public static string GetFileBase64(HttpPostedFileBase file)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copia el contenido del InputStream al MemoryStream
                file.InputStream.CopyTo(memoryStream);

                // Convierte el contenido del MemoryStream a una cadena Base64
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}
