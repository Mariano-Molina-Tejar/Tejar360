using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entity
{
    public class SolicitudCompraEncabezadoEntity
    {
        public int? DocNum { get; set; }
        public int? DocEntry { get; set; }
        public int? IdSucursal { get; set; }
        public int? IdDepto { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string Email { get; set; }
        public double PreptoActual { get; set; }
        public string Observaciones { get; set; }
        public string UserCode { get; set; }
        public int IdTipo { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public List<SolicitudCompraDetalleEntity> DetalleTabla { get; set; }

        [JsonIgnore]
        public List<HttpPostedFileBase> Files { get; set; }
        public List<FileDto> FilesInfo
        {
            get
            {
                return Files?.Select(file => new FileDto
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    FileSize = file.ContentLength,
                    FileBase64 = DALEntity.GetFileBase64(file)
                }).ToList();
            }
        }
    }
    public class FileDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int FileSize { get; set; }
        public string FileBase64 { get; set; }
        public int MyProperty { get; set; }
    }
    public class SolicitudCompraDetalleEntity
    {
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public int? LineNum { get; set; }
        public string NombreCuenta { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public string WhsCode { get; set; }
        public string NotasLine { get; set; }
    }
}
