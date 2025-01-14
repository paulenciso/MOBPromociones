using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MOBClass.Classes
{
    public static class MailClass
    {
        static string estructuraCorreo = @"<!DOCTYPE html>
            <html xmlns='http://www.w3.org/1999/xhtml'>
                <head runat='server'>
                    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>
                    <title>My Own Baker</title>
                </head>
                <body>
                    <div style='text-align: justify;'>
                        {0}
                        <br /><br /><br />
                        <img width='75px' height='49px' src='http://ninapasteleria.com/img/logo_nina.png' />
                        <br />
                        <a><strong><font color = 'DarkBlue' face='Verdana'>|</font></strong> <font color = 'LightSlateGray' face='Verdana' size='3'>NINA Server</font></a>
                        <br /><br />    
                        <a><font color = 'Green' face= 'Webdings' size= '4' >♻</font><font color = 'Green' face= 'Verdana' size= '2' > Por favor, piense en el medio ambiente antes de imprimir este correo.</font> </a>
                        <br />
                        <a><font color = 'Black' face= 'Verdana' size= '2' style= 'text-align: center;' > AVISO DE CONFIDENCIALIDAD</font></a>
                        <br />
                        <a><font color = 'Gray' face= 'Verdana' size= '1' > Esta comunicación contiene información que es confidencial y también puede contener información privilegiadas para uso exclusivo del destinatario.Si usted encuentra que no es el destinatario tenga en cuenta que cualquier distribución, copia o uso de esta comunicación o la información que contiene está estrictamente prohibida. Si usted ha recibido esta comunicación por error, por favor notifiquelo al remitente, o bien, vía correo electrónico a <a href = 'mailto:contacto@ninapasteleria.com' target= '_top' > contacto@ninapasteleria.com</a> o por télefono al +52(871)722-5409 y por favor proceda a eliminarlo de su cuenta.</font></a>
                    </div>
                </body>
            </html>";

        /// <summary>
        /// Método que envía un correo
        /// </summary>
        /// <param name="destinatarios">Correos destinarios separados con ;</param>
        /// <param name="asunto">Asunto del correo</param>
        /// <param name="mensaje">Mensaje del correo</param>
        public static void Enviar(string destinatarios, string asunto, string mensaje)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"], int.Parse(ConfigurationManager.AppSettings["smtpPort"]));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["supportMail"], "NinaServer. " + asunto, Encoding.Default);
                mail.Subject = asunto;
                mail.Body = String.Format(estructuraCorreo, mensaje);
                mail.IsBodyHtml = true;
                string[] arrDestinatarios = destinatarios.Split(';');
                foreach (string destinatario in arrDestinatarios)
                    mail.To.Add(destinatario);
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["supportMail"], ConfigurationManager.AppSettings["supportMailPass"]);
                SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSSL"]);
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Método que envía un correo
        /// </summary>
        /// <param name="destinatarios">Correos destinarios separados con ;</param>
        /// <param name="asunto">Asunto del correo</param>
        /// <param name="mensaje">Mensaje del correo</param>
        /// <param name="adjunto">Objeto de tipo Stream</param>
        public static void Enviar(string destinatarios, string asunto, string mensaje, Stream adjunto)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"], int.Parse(ConfigurationManager.AppSettings["smtpPort"]));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["supportMail"], "Nina pastelería. " + asunto, Encoding.Default);
                mail.Subject = asunto;
                mail.Body = string.Format(estructuraCorreo, mensaje);
                mail.IsBodyHtml = true;
                Attachment pdf = new Attachment(adjunto, "SolicitudNinaPasteleria.pdf", System.Net.Mime.MediaTypeNames.Application.Pdf);
                mail.Attachments.Add(pdf);
                string[] arrDestinatarios = destinatarios.Split(';');
                foreach (string destinatario in arrDestinatarios)
                    mail.To.Add(destinatario);
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["supportMail"], ConfigurationManager.AppSettings["supportMailPass"]);
                SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSSL"]);
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }


        /// <summary>
        /// Método que envía un correo
        /// </summary>
        /// <param name="destinatarios">Correos destinarios separados con ;</param>
        /// <param name="asunto">Asunto del correo</param>
        /// <param name="mensaje">Mensaje del correo</param>
        /// <param name="adjunto">Objeto de tipo Stream</param>
        public static void EnviarCompras(string destinatarios, string asunto, string mensaje, Stream adjunto)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"], int.Parse(ConfigurationManager.AppSettings["smtpPort"]));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["shoppingMail"], "Nina pastelería. " + asunto, Encoding.Default);
                mail.Subject = asunto;
                mail.Body = string.Format(estructuraCorreo, mensaje);
                mail.IsBodyHtml = true;
                Attachment pdf = new Attachment(adjunto, "SolicitudNinaPasteleria.pdf", System.Net.Mime.MediaTypeNames.Application.Pdf);
                mail.Attachments.Add(pdf);
                string[] arrDestinatarios = destinatarios.Split(';');
                foreach (string destinatario in arrDestinatarios)
                    mail.To.Add(destinatario);
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["shoppingMail"], ConfigurationManager.AppSettings["shoppingMailPass"]);
                SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSSL"]);
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
    }
}
