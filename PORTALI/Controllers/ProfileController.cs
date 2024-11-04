using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using DAL;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace PORTALI.Controllers
{
    public class ProfileController : Controller
    {   
        // GET: Profile
        public ActionResult Perfil()
        {
            if (Session["PropertiesEntity"] == null)
            {
                return View();
            }
            PortalPerfilEntity _perfil = new PortalPerfilEntity();

            var sessions = (Entity.SessionLoginEntity)Session["PropertiesEntity"];            
            _perfil = DALPortalLogin.Profile(sessions.UserCode, Server.MapPath("~/ImgUp/"));
            Session["NombreImagen"] = _perfil.NombrePic;
            return View(_perfil);
        }    

        [HttpPost]
        public ActionResult Perfil(HttpPostedFileBase file, PortalPerfilEntity portalPerfilEntity)
        {
            try 
            {
                string imgName = "", imgExt = "", imgPath = "";
                if (file != null && file.ContentLength > 0)
                {
                    imgName = Path.GetFileName(file.FileName);
                    imgExt = Path.GetExtension(imgName);
                    imgPath = Path.Combine(Server.MapPath("~/ImgUp/"), imgName);
                    file.SaveAs(imgPath);
                    Session["NombreImagen"] = imgName;                    
                }
                portalPerfilEntity.NombrePic = Session["NombreImagen"].ToString();
                if (!ModelState.IsValid)
                {
                    return View(portalPerfilEntity);
                }

                //if (DALPortalLogin.ValidarPasswordOld(portalPerfilEntity.UserCode, portalPerfilEntity.OldPassword))
                //{
                //    ViewBag.ShowDialog = true;
                //    return View(portalPerfilEntity);
                //}

                if (DALPortalLogin.GuardarDatosPerfil(portalPerfilEntity))
                {
                    Session["PropertiesEntity"] = null;
                    Session["NombreImagen"] = null;
                    return RedirectToAction("Login", "Account");
                }
                return View(portalPerfilEntity);
            }
            catch (Exception ex) 
            {
                return View(portalPerfilEntity);
            }
        }
    }
}