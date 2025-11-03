using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entity
{
    public class FormularioCheckList1Entity
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string U_IdFormulario { get; set; }
        public DateTime U_FechaCreado { get; set; }
        public short U_HoraCreacion { get; set; }
        public int U_IdUsuario { get; set; }
        public DateTime U_FechaFinalizacion { get; set; }
        public short U_HoraFinalizacion { get; set; }
        public string U_DG_CodTienda { get; set; }
        public int U_DG_CodGerenteTienda { get; set; }
        public int U_DG_CodSupBodega { get; set; }
        public int U_DG_CodEncargadoBodega { get; set; }
        public int U_DG_SupVenta { get; set; }
        public int U_DG_CodArea { get; set; }
        public int U_Bodega { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_U_Alma_PA1T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PAIC { get; set; }
        public string U_Alma_ADPA1 { get; set; }
        public string U_Alma_IAPA1 { get; set; }
        public decimal U_Alma_Punteo1 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA2T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA2C { get; set; }
        public string U_Alma_ADPA2 { get; set; }
        public string U_Alma_IAPA2 { get; set; }
        public decimal U_Alma_Punteo2 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA3T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string U_Alma_PA3C { get; set; }
        public string U_Alma_ADPA3 { get; set; }
        public string U_Alma_IAPA3 { get; set; }
        public decimal U_Alma_Punteo3 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA4T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA4C { get; set; }
        public string U_Alma_ADPA4 { get; set; }
        public string U_Alma_IAPA4 { get; set; }
        public decimal U_Alma_Punteo4 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA5T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA5C { get; set; }
        public string U_Alma_ADPA5 { get; set; }
        public string U_Alma_IAPA5 { get; set; }
        public decimal U_Alma_Punteo5 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA6T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA6C { get; set; }
        public string U_Alma_ADPA6 { get; set; }
        public string U_Alma_IAPA6 { get; set; }
        public decimal U_Alma_Punteo6 { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA7T { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? U_Alma_PA7C { get; set; }
        public string U_Alma_ADPA7 { get; set; }
        public string U_Alma_IAPA7 { get; set; }
        public decimal U_Alma_Punteo7 { get; set; }

        // Recepción
        public string U_Recep_PR1 { get; set; }
        public string U_Recep_ADPR1 { get; set; }
        public string U_Recep_IAPR1 { get; set; }
        public decimal U_Recep_Punteo1 { get; set; }

        public string U_Recep_PR2 { get; set; }
        public string U_Recep_ADPR2 { get; set; }
        public string U_Recep_IAPR2 { get; set; }
        public decimal U_Recep_Punteo2 { get; set; }

        public string U_Recep_PR3 { get; set; }
        public string U_Recep_ADPR3 { get; set; }
        public string U_Recep_IAPR3 { get; set; }
        public decimal U_Recep_Punteo3 { get; set; }

        public string U_Recep_PR4 { get; set; }
        public string U_Recep_ADPR4 { get; set; }
        public string U_Recep_IAPR4 { get; set; }
        public decimal U_Recep_Punteo4 { get; set; }

        public string U_Recep_PR5 { get; set; }
        public string U_Recep_ADPR5 { get; set; }
        public string U_Recep_IAPR5 { get; set; }
        public decimal U_Recep_Punteo5 { get; set; }

        public string U_Recep_PR6 { get; set; }
        public string U_Recep_ADPR6 { get; set; }
        public string U_Recep_IAPR6 { get; set; }
        public decimal U_Recep_Punteo6 { get; set; }

        public string U_Recep_PR7 { get; set; }
        public string U_Recep_ADPR7 { get; set; }
        public string U_Recep_IAPR7 { get; set; }
        public decimal U_Recep_Punteo7 { get; set; }

        // Despacho
        public string U_Desp_PH1 { get; set; }
        public string U_Desp_ADPH1 { get; set; }
        public string U_Desp_IAHPH1 { get; set; }
        public decimal U_Desp_Punteo1 { get; set; }

        public string U_Desp_PH2 { get; set; }
        public string U_Desp_ADPH2 { get; set; }
        public string U_Desp_IAPH2 { get; set; }
        public decimal U_Desp_Punteo2 { get; set; }

        public string U_Desp_PH3 { get; set; }
        public string U_Desp_ADPH3 { get; set; }
        public string U_Desp_IAPH3 { get; set; }
        public decimal U_Desp_Punteo3 { get; set; }

        public string U_Desp_PH4 { get; set; }
        public string U_Desp_ADPH4 { get; set; }
        public string U_Desp_IAPH4 { get; set; }
        public decimal U_Desp_Punteo4 { get; set; }

        public string U_Desp_PH5 { get; set; }
        public string U_Desp_ADPH5 { get; set; }
        public string U_Desp_IAPH5 { get; set; }
        public decimal U_Desp_Punteo5 { get; set; }

        public string U_Desp_PH6 { get; set; }
        public string U_Desp_ADPH6 { get; set; }
        public string U_Desp_IAPH6 { get; set; }
        public decimal U_Desp_Punteo6 { get; set; }

        public string U_Desp_PH7 { get; set; }
        public string U_Desp_ADPH7 { get; set; }
        //public string U_Desp_IAPH7 { get; set; }
        public decimal U_Desp_Punteo7 { get; set; }

        // Daños
        public string U_Dan_PD1 { get; set; }
        public string U_Dan_ADPD1 { get; set; }
        public string U_Dan_IAPD1 { get; set; }
        public decimal U_Dan_Punteo1 { get; set; }

        public string U_Dan_PD2 { get; set; }
        public string U_Dan_ADPD2 { get; set; }
        public string U_Dan_IAPD2 { get; set; }
        public decimal U_Dan_Punteo2 { get; set; }

        public string U_Dan_PD3 { get; set; }
        public string U_Dan_ADPD3 { get; set; }
        public string U_Dan_IAPD3 { get; set; }
        public decimal U_Dan_Punteo3 { get; set; }

        public string U_Dan_PD4 { get; set; }
        public string U_Dan_ADPD4 { get; set; }
        public string U_Dan_IAPD4 { get; set; }
        public decimal U_Dan_Punteo4 { get; set; }

        public string U_Dan_PD5 { get; set; }
        public string U_Dan_ADPD5 { get; set; }
        public string U_Dan_IAPD5 { get; set; }
        public decimal U_Dan_Punteo5 { get; set; }

        public string U_Dan_PD6 { get; set; }
        public string U_Dan_ADPD6 { get; set; }
        public string U_Dan_IAPD6 { get; set; }
        public decimal U_Dan_Punteo6 { get; set; }

        public string U_Dan_PD7 { get; set; }
        public string U_Dan_ADPD7 { get; set; }
        public string U_Dan_IAPD7 { get; set; }
        public decimal U_Dan_Punteo7 { get; set; }

        // 5S
        public string U_5S_PS1 { get; set; }
        public string U_5S_ADPS1 { get; set; }
        public string U_5S_IAPS1 { get; set; }
        public decimal U_5S_Punteo1 { get; set; }

        public string U_5S_PS2 { get; set; }
        public string U_5S_ADPS2 { get; set; }
        public string U_5S_IAPS2 { get; set; }
        public decimal U_5S_Punteo2 { get; set; }

        public string U_5S_PS3 { get; set; }
        public string U_5S_ADPS3 { get; set; }
        public string U_5S_IAPS3 { get; set; }
        public decimal U_5S_Punteo3 { get; set; }

        public string U_5S_PS4 { get; set; }
        public string U_5S_ADPS4 { get; set; }
        public string U_5S_IAPS4 { get; set; }
        public decimal U_5S_Punteo4 { get; set; }

        public string U_5S_PS5 { get; set; }
        public string U_5S_ADPS5 { get; set; }
        public string U_5S_IAPS5 { get; set; }
        public decimal U_5S_Punteo5 { get; set; }

        public string U_5S_PS6 { get; set; }
        public string U_5S_ADPS6 { get; set; }
        public string U_5S_IAPS6 { get; set; }
        public decimal U_5S_Punteo6 { get; set; }

        public string U_5S_PS7 { get; set; }
        public string U_5S_ADPS7 { get; set; }
        public string U_5S_IAPS7 { get; set; }
        public decimal U_5S_Punteo7 { get; set; }

        // Totales
        public decimal U_PuntajeAlmacenaje { get; set; }
        public decimal U_PuntajeRecepcion { get; set; }
        public decimal U_PuntajeDespacho { get; set; }
        public decimal U_PuntajeDaniado { get; set; }
        public decimal U_Puntaje5S { get; set; }
        public decimal U_CalificacionTotal { get; set; }
        public string U_Desp_IAPH7 { get; set; }
        public int U_Estado { get; set; }
    }

    public class CheckListInfo
    {
        public string Correlativo { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Tienda { get; set; }
        public string Bodega { get; set; }
        public int Estado { get; set; }
        public double Puntaje { get; set; }
        public string Error { get; set; }
    }
    public class ReporteEvaluacion
    {
        public string Correlativo { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Tienda { get; set; }
        public string Bodega { get; set; }
        public string Estado { get; set; }

        // Almacenaje (decimales con 2 decimales)
        public decimal Alm1 { get; set; }
        public decimal Alm2 { get; set; }
        public decimal Alm3 { get; set; }
        public decimal Alm4 { get; set; }
        public decimal Alm5 { get; set; }
        public decimal Alm6 { get; set; }
        public decimal Alm7 { get; set; }

        // Recepción
        public decimal Rec1 { get; set; }
        public decimal Rec2 { get; set; }
        public decimal Rec3 { get; set; }
        public decimal Rec4 { get; set; }
        public decimal Rec5 { get; set; }
        public decimal Rec6 { get; set; }
        public decimal Rec7 { get; set; }

        // Despacho
        public decimal Des1 { get; set; }
        public decimal Des2 { get; set; }
        public decimal Des3 { get; set; }
        public decimal Des4 { get; set; }
        public decimal Des5 { get; set; }
        public decimal Des6 { get; set; }
        public decimal Des7 { get; set; }

        // Dañado
        public decimal Dan1 { get; set; }
        public decimal Dan2 { get; set; }
        public decimal Dan3 { get; set; }
        public decimal Dan4 { get; set; }
        public decimal Dan5 { get; set; }
        public decimal Dan6 { get; set; }
        public decimal Dan7 { get; set; }

        // 5S
        public decimal S1 { get; set; }
        public decimal S2 { get; set; }
        public decimal S3 { get; set; }
        public decimal S4 { get; set; }
        public decimal S5 { get; set; }
        public decimal S6 { get; set; }
        public decimal S7 { get; set; }

        // Puntajes
        public decimal U_PuntajeAlmacenaje { get; set; }
        public decimal U_PuntajeRecepcion { get; set; }
        public decimal U_PuntajeDespacho { get; set; }
        public decimal U_PuntajeDaniado { get; set; }
        public decimal U_Puntaje5S { get; set; }
        public decimal PuntajeGeneral { get; set; }

        public string Error { get; set; }

    }

    public class IdFormulario
    {
        public string Code { get; set; }
        public string Numero { get; set; }
    }
    public class ImagenRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IdFormulario { get; set; }
        public HttpPostedFileBase Imagen { get; set; }
    }

    public class TiendasRegionesCheckListEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public int Code { get; set; }
        public string Location { get; set; }
        public string Error { get; set; }
    }


}
