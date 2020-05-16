using HipercoreASPNETCORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HipercoreASPNETCORE.Models
{
    public class ServicioEnvioDeEmails : IServicioEnvioEmail
    {
        public bool EnvioDeEmail(string Destino, string Asunto, string CuerpoHTML)
        {
            //aqui escribo el codigo q envia emails

            try
            {
                //------user secrets
                string _origen = "d3m0gon@gmail.com";
                string _pass = "1@Qwertyuiop0";
                //----------
                string _destino = Destino;
                string _cuerpoHTML = CuerpoHTML;
                string _asunto = Asunto;

                MailMessage EmailConfirmacion = new MailMessage(_origen, _destino, _asunto ,_cuerpoHTML);

                EmailConfirmacion.IsBodyHtml = true;
                SmtpClient clienteSMTP = new SmtpClient("smtp.gmail.com");
                clienteSMTP.EnableSsl = true;
                clienteSMTP.UseDefaultCredentials = false;
                clienteSMTP.Port = 587;
                clienteSMTP.Credentials = new System.Net.NetworkCredential(_origen, _pass);

                clienteSMTP.Send(EmailConfirmacion);
                clienteSMTP.Dispose();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
            

        }
    }
}
