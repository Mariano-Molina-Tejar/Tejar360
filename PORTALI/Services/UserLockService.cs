using DAL;
using Entity;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
namespace PORTALI.Services
{
    public class UserLockService
    {
        private readonly LockServiceDAL _dal = new LockServiceDAL();
        public async Task<Reply> LockUser(int empId)
        {
            Reply reply = new Reply();

            var usuarios = await _dal.ObtenerUsuarios(empId);

            //if(usuarios != null)

            return reply;
        }

        public Reply LockUserSAP(int userId) 
        {
            Reply reply = new Reply();
            var url = "GestionDePersonal/BloquearUsuarioSAP";
            var response = DAL_API.enviarDatosSL(url, new {UserId = userId});
            
            return reply;
        }

        public int guardarProcesoDeBaja(int userId, int IdSolicitud, int IdEstado, string Observaciones)
        {
            string url = "GestionDePersonal/GuardarProcesoGestion";
            try
            {
                var ObjectSend = new
                {
                    U_CodeBaja = IdSolicitud,
                    U_FechaProceso = DateTime.Now,
                    U_IdProceso = IdEstado,
                    U_Comentarios = Observaciones,
                    U_Usuario = userId,
                    U_Hora = DateTime.Now.ToString("HH:mm")
                };

                string response = DAL_API.enviarDatosSL(url, ObjectSend);

                var reply = JsonConvert.DeserializeObject<Reply>(response);

                if (reply.result == 1)
                    return 1;

                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
