using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PORTALI.Services
{
    public class FirmaDigitalService
    {
        /// <summary>
        /// Genera un hash SHA256 a partir de un string.
        /// </summary>
        public string CalcularSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Genera el hash final combinando el contenido de la solicitud + identificador del usuario.
        /// </summary>
        public string GenerarHashFirma(string contenido, int usuarioId)
        {
            string hashContenido = CalcularSHA256(contenido);
            return CalcularSHA256($"{hashContenido}|{usuarioId}");
        }

        /// <summary>
        /// Verifica si el hash actual coincide con el hash almacenado.
        /// </summary>
        public bool VerificarFirma(string contenido, int usuarioId, string hashGuardado)
        {
            string hashActual = GenerarHashFirma(contenido, usuarioId);
            return string.Equals(hashActual, hashGuardado, StringComparison.InvariantCulture);
        }
    }
}