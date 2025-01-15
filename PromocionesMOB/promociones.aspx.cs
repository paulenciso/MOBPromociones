using MOBClass;
using MOBClass.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PromocionesMOB
{
    public partial class promociones : System.Web.UI.Page
    {
        #region Variables
        string codigoUbicacion, token;
        bool tieneCookie;
        #endregion
        #region Métodos
        private void ValidarParametros()
        {
            bool contieneParametro = !string.IsNullOrWhiteSpace(Request.QueryString["id"]) ||
                         !string.IsNullOrWhiteSpace(Request.QueryString["tkn"]);

            if (!contieneParametro)
            {
                mvw_opciones.SetActiveView(vNoID);
                return;
            }

            // Procesar id si está presente
            if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
            {
                ProcesarId(Request.QueryString["id"]);
                return;
            }

            // Procesar token si está presente
            if (!string.IsNullOrWhiteSpace(Request.QueryString["tkn"]))
            {
                ProcesarToken(Request.QueryString["tkn"]);
                return;
            }
        }
        private void ValidarCookie()
        {
            tieneCookie = false;
            var cookie = Request.Cookies["_MOB"];

            if (cookie != null)
            {
                byte[] textoCookie = HttpServerUtility.UrlTokenDecode(cookie.Value);
                byte[] desencriptado = MachineKey.Unprotect(textoCookie);
                string correo = Encoding.UTF8.GetString(desencriptado);

                MOBClientes cliente = MOBClientesClass.ObtenerMOBClientes().SingleOrDefault(c => c.correo.Equals(correo));
                if (cliente != null)
                {
                    MOBPromocionesGeneradas promocionGenerada = MOBPromocionesGeneradasClass.ObtenerMOBPromocionesGeneradas().FirstOrDefault(pg => pg.id_cliente == cliente.id_cliente);
                    if (promocionGenerada != null)
                    {
                        string texto = ObtenerTextoPromocion(promocionGenerada);
                        lbl_promocion.Text = texto;
                        mvw_opciones.SetActiveView(vPromocion);

                        tieneCookie = true;
                    }
                }
            }
        }

        private void CrearCookieCliente(MOBClientes cliente)
        {
            byte[] cookie = Encoding.UTF8.GetBytes(cliente.correo);
            byte[] encriptado = MachineKey.Protect(cookie);
            HttpCookie oCookie = new HttpCookie("_MOB", HttpServerUtility.UrlTokenEncode(encriptado))
            {
                Expires = DateTime.MaxValue
            };
            Response.SetCookie(oCookie);
        }

        private static string ObtenerTextoPromocion(MOBPromocionesGeneradas promocionGenerada)
        {
            string[] imagenes = new string[] { "Imagenes/CONEJOS_1.jpg", "Imagenes/CONEJOS_2.jpg", "Imagenes/CONEJOS_3.jpg", "Imagenes/CONEJOS_4.jpg", "Imagenes/CONEJOS_5.jpg", "Imagenes/CONEJOS_6.jpg", "Imagenes/CONEJOS_7.jpg" };
            Random random = new Random();
            string imagenAleatoria = imagenes[random.Next(imagenes.Length)];
        
            MOBPromociones promocion = MOBPromocionesClass.ObtenerMOBPromocion(promocionGenerada.id_promocion);
            string codigoBarras = promocionGenerada.id_ubicacion.ToString("D2") + promocionGenerada.id_promocion_generada.ToString("D3") + "-MOB" + promocionGenerada.id_promocion.ToString("D2");
            string texto = $@"
                            <div class='mdl-card mdl-shadow--2dp' style='padding: 20px; max-width: 600px; margin: 20px auto;'>
                                <div style='text-align: center; margin-bottom: 20px;'>
                                    <img src='{imagenAleatoria}' alt='Imagen Decorativa' style='transform: scale(0.5);width: 100%; max-width: 150px; height: auto;object-fit: cover;' />
                                </div>
                                <h3 class='mdl-typography--headline' style='text-align: center; color: #ff4081; margin-bottom: 10px;'>🎉 FELICIDADES 🎉</h3>
                                <p class='mdl-typography--body-1' style='font-size: 1.3em; text-align: justify; margin-bottom: 20px;'>{promocion.especificaciones}</p>
                                <p class='mdl-typography--body-1' style='font-size: 1.3em; text-align: justify; margin-bottom: 20px;'>Presenta el siguiente código en cualquiera de nuestras sucursales:</p>
                                <div style = 'text-align: center; margin: 20px 0;'>
                                    <span class='mdl-typography--headline font'>*{codigoBarras}*</span>
                                    <span class= 'mdl-typography--body-1' style = 'display: block; letter-spacing: 4px; color: #555;' >{ codigoBarras}</span>
                                </div>
                            </div>";
            return texto;
        }

        void ProcesarId(string codigoUbicacion)
        {
            MOBUbicaciones ubicacion = MOBUbicacionesClass.ObtenerMOBUbicaciones()
                                       .FirstOrDefault(u => u.codigoURL.Equals(codigoUbicacion));

            if (ubicacion == null)
            {
                mvw_opciones.SetActiveView(vNoID);
                return;
            }
        }

        void ProcesarToken(string token)
        {
            MOBClientes cliente = MOBClientesClass.ObtenerMOBClientes()
                                  .SingleOrDefault(c => c.tokenValidacion.Equals(token));

            if (cliente == null)
            {
                mvw_opciones.SetActiveView(vNoID);
                return;
            }

            if (cliente.validado == false)
            {
                mvw_opciones.SetActiveView(vCorreoValidacion);
                return;
            }

            MOBPromocionesGeneradas promocionGenerada = MOBPromocionesGeneradasClass.ObtenerMOBPromocionesGeneradas()
                                                        .FirstOrDefault(pg => pg.id_cliente == cliente.id_cliente);

            if (promocionGenerada != null)
            {
                CrearCookieCliente(cliente);
                lbl_promocion.Text = ObtenerTextoPromocion(promocionGenerada);
                mvw_opciones.SetActiveView(vPromocion);
            }
            else
            {
                mvw_opciones.SetActiveView(vNoID);
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidarCookie();
            if (!tieneCookie) ValidarParametros();
        }

        protected void btn_generar_Click(object sender, EventArgs e)
        {
            MOBClientes cliente = MOBClientesClass.ObtenerMOBClientes().SingleOrDefault(c => c.tokenValidacion.Equals(token));
            if (cliente != null)
            {
                cliente.validado = true;
                cliente.fecha_validacion = DateTime.Now;
                MOBClientesClass.ModificarMOBcliente(cliente);

                MOBPromocionesGeneradas promocionGenerada = new MOBPromocionesGeneradas()
                {
                    id_cliente = cliente.id_cliente,
                    id_promocion = MOBPromocionesClass.SeleccionarPromocionAleatoria().id_promocion,
                    id_ubicacion = (int)cliente.id_ubicacion,
                    fecha_canje = null,
                    entregado = false,
                    id_ticket = 0,
                    fecha_creacion = DateTime.Now
                };

                if (MOBPromocionesGeneradasClass.RegistrarMOBPromocionesGeneradas(promocionGenerada) > 0)
                {
                    string texto = ObtenerTextoPromocion(promocionGenerada);
                    lbl_promocion.Text = texto;
                    CrearCookieCliente(cliente);

                    mvw_opciones.SetActiveView(vPromocion);
                }
            }
        }

        protected void btn_validar_Click(object sender, EventArgs e)
        {
            MOBUbicaciones ubicacion = MOBUbicacionesClass.ObtenerMOBUbicaciones().FirstOrDefault(u => u.codigoURL.Equals(codigoUbicacion));

            if (ubicacion != null)
            {
                if (!txt_email.Text.Trim().Equals(""))
                {
                    if (!MOBClientesClass.ObtenerMOBClientes().Exists(c => c.correo.Equals(txt_email.Text.Trim())))
                    {
                        MOBClientes cliente = new MOBClientes()
                        {
                            correo = txt_email.Text,
                            fecha_validacion = null,
                            tokenValidacion = Guid.NewGuid().ToString(),
                            validado = false,
                            id_ubicacion = ubicacion.id_ubicacion
                        };

                        if (MOBClientesClass.RegistrarMOBClientes(cliente) > 0)
                        {
                            string mensaje =
                                    $@"Estas a un paso de participar en nuestra promoción, por favor ingresa al enlace para recibir tu premio.</strong>
                                   <br/><br/>
                                   <a href='{ConfigurationManager.AppSettings["url"]}promociones.aspx?tkn={cliente.tokenValidacion}'>Canjear premio</a>
                                   <br/><br/>";

                            Thread oHilo = new Thread(delegate ()
                            {
                                MailClass.Enviar(cliente.correo, "My Own Baker", mensaje);
                            });
                            oHilo.IsBackground = true;
                            oHilo.Start();

                            txt_email.Text = "";
                        }
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }
    }
}