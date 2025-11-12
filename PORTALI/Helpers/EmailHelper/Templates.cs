using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}