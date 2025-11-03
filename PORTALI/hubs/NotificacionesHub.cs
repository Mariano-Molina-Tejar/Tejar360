using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PORTALI.hubs
{
    public class NotificacionesHub: Hub
    {
        public override Task OnConnected()
        {
            string userCode = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userCode))
            {
                Groups.Add(Context.ConnectionId, userCode);
            }
            return base.OnConnected();
        }
    }
}