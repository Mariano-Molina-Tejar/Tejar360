using PORTALI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PORTALI.Filter
{
    public class VerifySession: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var oUser = (Entity.SessionLoginEntity)HttpContext.Current.Session["PropertiesEntity"];
            if(oUser == null)
            {
                if(filterContext.Controller is AccountController == false)
                {
                    filterContext.HttpContext.Response.Redirect("~/Account/Login");
                }
            }
            else
            {
                if (filterContext.Controller is AccountController == true)
                {
                    filterContext.HttpContext.Response.Redirect("~/Dashboard/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}