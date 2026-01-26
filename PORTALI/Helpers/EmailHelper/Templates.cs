using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PORTALI.Helpers.EmailHelper
{
    public class Templates
    {
        public static string BodyMailSolicitud(
                                                string nombreEmpleado,
                                                string departamento,
                                                string motivo,
                                                string observaciones,
                                                string nombreSolicitante)
        {
            return $@"
                    <div style='max-width:600px; margin:auto; font-family:Arial, sans-serif; border-radius:10px; overflow:hidden; 
                                box-shadow:0 2px 8px rgba(0,0,0,0.1); border:1px solid #eee;'>

                        <div style='background-color:#C8102E; color:white; padding:12px 20px;'>
                            <h2 style='margin:0; font-size:18px;'>Solicitud de baja de personal</h2>
                        </div>

                        <div style='background-color:#FFF8DC; padding:20px;'>
                            <p>Estimado(a) equipo de <strong>Recursos Humanos</strong>,</p>

                            <p>Por este medio solicito la autorización para procesar la baja del siguiente colaborador:</p>

                            <ul style='list-style:none; padding-left:0;'>
                                <li><strong>👤 Empleado:</strong> {nombreEmpleado}</li>
                                <li><strong>🏢 Departamento:</strong> {departamento}</li>
                                <li><strong>📝 Motivo de la baja:</strong> {motivo}</li>
                                <li><strong>💬 Observaciones:</strong> {observaciones}</li>
                            </ul>

                            <p>Agradezco su apoyo para revisar y autorizar esta solicitud según corresponda.</p>

                            <p>Saludos cordiales,<br>
                            <strong>{nombreSolicitante}</strong></p>
                        </div>

                        <div style='background-color:#FFD700; text-align:center; padding:10px; font-weight:bold; color:#C8102E;'>
                            El Tejar 
                        </div>
                    </div>
                    ";
        }

        public static string BodyMailResponseGTH(
                                                    string nombreEmpleado,
                                                    string departamento,
                                                    string motivo,
                                                    string observaciones,
                                                    string nombreSolicitante,
                                                    int estado, // 1 = Autorizado, -1 = Rechazado
                                                    string comentariosGTH // Comentarios opcionales del área GTH
        )
        {
            // Definir valores según el estado numérico
            bool autorizado = estado == 1;
            string textoEstado = autorizado ? "Autorizado" : "Rechazado";
            string colorHeader = autorizado ? "#28A745" : "#DC3545"; // Verde o Rojo
            string emojiEstado = autorizado ? "✅" : "❌";

            return $@"
                <div style='max-width:600px; margin:auto; font-family:Arial, sans-serif; border-radius:10px; overflow:hidden; 
                            box-shadow:0 2px 8px rgba(0,0,0,0.1); border:1px solid #eee;'>

                    <div style='background-color:{colorHeader}; color:white; padding:12px 20px;'>
                        <h2 style='margin:0; font-size:18px;'>Respuesta de GTH - Solicitud de baja</h2>
                    </div>

                    <div style='background-color:#F8F9FA; padding:20px;'>
                        <p>Estimado(a) <strong>{nombreSolicitante}</strong>,</p>

                        <p>Se ha revisado la solicitud de baja del siguiente colaborador:</p>

                        <ul style='list-style:none; padding-left:0;'>
                            <li><strong>👤 Empleado:</strong> {nombreEmpleado}</li>
                            <li><strong>🏢 Departamento:</strong> {departamento}</li>
                            <li><strong>📝 Motivo:</strong> {motivo}</li>
                            <li><strong>💬 Observaciones:</strong> {observaciones}</li>
                        </ul>

                        <div style='background-color:white; border-left:5px solid {colorHeader}; padding:15px; margin:15px 0;'>
                            <p style='font-size:16px; margin:0;'>
                                <strong>Estado:</strong> {emojiEstado} <span style='color:{colorHeader}; font-weight:bold;'>{textoEstado}</span>
                            </p>
                            {(string.IsNullOrWhiteSpace(comentariosGTH) ? "" : $"<p style='margin-top:10px;'><strong>Comentarios GTH:</strong> {comentariosGTH}</p>")}
                        </div>

                        <p>Atentamente,<br>
                        <strong>Departamento de Gestión de Talento Humano</strong></p>
                    </div>

                    <div style='background-color:#FFD700; text-align:center; padding:10px; font-weight:bold; color:#C8102E;'>
                        El Tejar
                    </div>
                </div>
            ";
        }

        public static string BodyMailAspirante(
                                            string nombreAspirante,
                                            string usuario,
                                            string password,
                                            string fechaLimite,
                                            string linkAcceso)
        {
            return $@"
                <div style='max-width:600px; margin:auto; font-family:Arial, sans-serif; border-radius:10px; overflow:hidden;
                            box-shadow:0 2px 8px rgba(0,0,0,0.1); border:1px solid #eee;'>

                    <!-- Encabezado -->
                    <div style='background-color:#0066cc; color:white; padding:12px 20px;'>
                        <h2 style='margin:0; font-size:18px;'>Acceso a plataforma de reclutamiento</h2>
                    </div>

                    <!-- Contenido principal -->
                    <div style='background-color:#F4FAFF; padding:20px;'>
                        <p>Estimado(a) <strong>{nombreAspirante}</strong>,</p>

                        <p>
                            Le damos la bienvenida al proceso de reclutamiento de 
                            <strong>El Tejar</strong>. 
                            Para continuar, deberá ingresar a nuestra plataforma y completar su perfil.
                        </p>

                        <p>A continuación encontrará sus credenciales de acceso:</p>

                        <ul style='list-style:none; padding-left:0;'>
                            <li><strong>👤 Usuario:</strong> {usuario}</li>
                            <li><strong>🔐 Contraseña:</strong> {password}</li>
                            <li><strong>📅 Fecha límite para completar la información:</strong> {fechaLimite}</li>
                        </ul>

                        <p>Puede ingresar al sistema desde el siguiente enlace:</p>

                        <p style='text-align:center; margin:20px 0;'>
                            <a href='{linkAcceso}' 
                               style='background-color:#0066cc; color:white; padding:10px 18px; text-decoration:none; 
                                      border-radius:5px; font-weight:bold; display:inline-block;'>
                               Ingresar a la plataforma
                            </a>
                        </p>

                        <p>Dentro de la plataforma deberá completar la siguiente información:</p>

                        <ul>
                            <li>📌 Información personal</li>
                            <li>🎓 Información académica</li>
                            <li>💼 Experiencia laboral</li>
                        </ul>

                        <p>Además, deberá cargar los siguientes documentos:</p>

                        <ul>
                            <li>📄 Currículum Vitae (CV)</li>
                            <li>🪪 DPI (ambos lados)</li>
                            <li>📚 Constancias laborales</li>
                            <li>📄 Constancia de antecedentes penales</li>
                            <li>📄 Constancia de antecedentes policiales</li>
                        </ul>

                        <p>
                            Le recordamos que toda la información debe completarse antes de la fecha límite indicada. 
                            Esto permitirá continuar con su evaluación dentro del proceso de selección.
                        </p>

                        <p>Quedamos atentos ante cualquier consulta.</p>

                        <p>Saludos cordiales,<br>
                            <strong>Departamento de Recursos Humanos</strong>
                        </p>
                    </div>

                    <!-- Pie -->
                    <div style='background-color:#0066cc; text-align:center; padding:10px; font-weight:bold; color:white;'>
                        El Tejar
                    </div>
                </div>
                ";
        }

        public static string BodyMailSolicitudAutorizacion(
                string nombreSolicitante,
                string puesto,
                string fechaSolicitud,
                string observaciones,
                string linkAutorizar,
                string linkRechazar
            )
        {
            string _nombre = WebUtility.HtmlEncode(nombreSolicitante);
            string _puesto = WebUtility.HtmlEncode(puesto);
            string _fecha = WebUtility.HtmlEncode(fechaSolicitud);
            string _obs = WebUtility.HtmlEncode(observaciones);
            string _autorizar = linkAutorizar;
            string _rechazar = linkRechazar;
            string refId = DateTime.Now.ToString("yyyyMMddHHmmss");

            return $@"<!-- Ref: {refId} -->
<div style='max-width:600px; margin:auto; font-family:Arial, sans-serif; border-radius:10px; overflow:hidden;
            box-shadow:0 2px 8px rgba(0,0,0,0.1); border:1px solid #eee;'>

    <div style='background-color:#0066cc; color:white; padding:12px 20px;'>
        <h2 style='margin:0; font-size:18px;'>Solicitud de Autorización</h2>
    </div>

    <div style='background-color:#F4FAFF; padding:20px;'>
        <p>Estimado(a),</p>

        <p>
            Se ha generado una <strong>nueva solicitud de autorización</strong> dentro del sistema.
            A continuación, encontrará los detalles:
        </p>

        <ul style='list-style:none; padding-left:0;'>
            <li><strong>Solicitante:</strong> {_nombre}</li>
            <li><strong>Puesto:</strong> {_puesto}</li>
            <li><strong>Fecha de solicitud:</strong> {_fecha}</li>
            <li><strong>Observaciones:</strong> {_obs}</li>
        </ul>

        <p style='margin-top:20px;'>Seleccione una opción para continuar con el proceso:</p>

        <div style='text-align:center; margin:25px 0;'>

            <a href='{_autorizar}'
               style='background-color:#28a745; color:white; padding:10px 18px;
                      text-decoration:none; border-radius:5px; font-weight:bold;
                      display:inline-block; margin-right:10px;'>
                Autorizar
            </a>

            <a href='{_rechazar}'
               style='background-color:#dc3545; color:white; padding:10px 18px;
                      text-decoration:none; border-radius:5px; font-weight:bold;
                      display:inline-block;'>
                Rechazar
            </a>

        </div>

        <p>
            Al seleccionar alguna de las opciones, su decisión quedará registrada automáticamente en el sistema.
        </p>

        <p>Saludos cordiales,<br>
           <strong>Departamento de Recursos Humanos</strong>
        </p>
    </div>

    <div style='background-color:#0066cc; text-align:center; padding:10px; font-weight:bold; color:white;'>
        El Tejar
    </div>
</div>";

        }


    }
}