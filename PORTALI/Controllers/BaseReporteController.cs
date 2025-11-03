using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;

namespace PORTALI.Controllers
{
    public class BaseReporteController : Controller
    {
        protected FechaFiltroEntity ObtenerFiltroConFechas(string fechaInicio, string fechaFin)
        {
            var filtro = new FechaFiltroEntity
            {
                FechaInicio = string.IsNullOrEmpty(fechaInicio) ? DateTime.MinValue : Convert.ToDateTime(fechaInicio),
                FechaFin = string.IsNullOrEmpty(fechaFin) ? DateTime.MinValue : Convert.ToDateTime(fechaFin)
            };

            filtro.Validar(); // Aplica la lógica por defecto si las fechas son inválidas

            ViewBag.FechaInicio = filtro.FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = filtro.FechaFin.ToString("yyyy-MM-dd");

            return filtro;
        }
       
    }
}