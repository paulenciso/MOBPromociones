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
        string codigoUbicacion = "", token = "";
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


            cookie = Request.Cookies["_NoPromo"];
            if(cookie!= null)
            {
                tieneCookie = true;
                mvw_opciones.SetActiveView(vNoPromo);
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

        private void CrearCookieNoPromo()
        {
            HttpCookie oCookie = new HttpCookie("_MOB")
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
            string codigoBarras = promocion.ProductoGratis == true ? promocionGenerada.id_promocion_generada.ToString("D5") + "-" + promocion.Codigo
                                                                   : promocion.Codigo;


            
            string texto = $@"
                            <div class='mdl-card mdl-shadow--2dp' style='padding: 20px; max-width: 600px; margin: 20px auto;'>
                               <div style='display: flex; justify-content: center; align-items: center; gap: 20px; margin-bottom: 20px;'>
                                    <img src='Imagenes/LOGO-TAGLINE-NEGRO.jpg' alt='Imagen Decorativa' 
                                         style='width: 100%; max-width: 120px; height: auto; object-fit: cover;' />
                                    <img src='Imagenes/LogoNina.png' alt='Logo Nina Pastelería' 
                                         style='width: 100%; max-width: 120px; height: auto; object-fit: cover;' />
                                </div>
                                <h3 class='mdl-typography--headline' style='text-align: center; color: #ff4081; margin-bottom: 10px;'>🎉 FELICIDADES 🎉</h3>
                                <h4 class='mdl-typography--headline' style='text-align: center; color: #ff4081; margin-bottom: 10px;'>{(promocion.ProductoGratis == true ? "¡PRODUCTO GRATIS!" : "¡PROMOCION!")}</h3>
                                <p class='mdl-typography--body-1' style='font-size: 1.3em; text-align: justify; margin-bottom: 20px;'>{promocion.especificaciones}</p>
                                <p class='mdl-typography--body-1' style='font-size: 1.3em; text-align: justify; margin-bottom: 20px;'>Presenta este código en cualquiera de nuestras 15 sucursales de Nina Pastelería y disfruta de tu premio</p>
                                <div style = 'text-align: center; margin: 20px 0;'>
                                    <span class='mdl-typography--headline font'>*{codigoBarras}*</span>
                                    <span class= 'mdl-typography--body-1' style = 'display: block; letter-spacing: 4px; color: #555;' >{ codigoBarras}</span>
                                </div>
                            </div>";

            string mensaje = $@"
                        <div style='padding: 20px; max-width: 600px; margin: 20px auto; font-family: Arial, sans-serif; color: #444; text-align: center; border: 1px solid #ddd; border-radius: 8px; background-color: #f9f9f9;'>
                            <h2 style='color: #FF4081; font-size: 1.8em; margin-bottom: 10px;'>🎉 ¡FELICIDADES! 🎉</h2>
                            <h3 style='color: #FF4081; font-size: 1.5em; margin-bottom: 20px;'>
                                {(promocion.ProductoGratis == true ? "¡PRODUCTO GRATIS!" : "¡PROMOCIÓN!")}
                            </h3>
                            <p style='font-size: 1.2em; line-height: 1.6; margin-bottom: 20px;'>
                                {promocion.especificaciones}
                            </p>
                            <p style='font-size: 1.1em; margin-bottom: 20px;'>
                                Presenta este código en cualquiera de nuestras 15 sucursales de Nina Pastelería y disfruta de tu premio
                            </p>
                            <div style='text-align: center; margin: 20px 0;'>
                                <span style='display: block; font-size: 1.5em; color: #333; font-weight: bold; margin-bottom: 10px;'>*{codigoBarras}*</span>
                                <span style='display: block; letter-spacing: 4px; color: #555;'>{codigoBarras}</span>
                            </div>
                        </div>";

            Thread oHilo = new Thread(delegate ()
            {
                MOBClientes cliente = MOBClientesClass.ObtenerMOBCliente(promocionGenerada.id_cliente);
                MailClass.Enviar(cliente.correo, "My Own Baker", mensaje);
            });
            oHilo.IsBackground = true;
            oHilo.Start();


            return texto;
        }

        void ProcesarId(string _codigoUbicacion)
        {
            MOBUbicaciones ubicacion = MOBUbicacionesClass.ObtenerMOBUbicaciones()
                                       .FirstOrDefault(u => u.codigoURL.Equals(_codigoUbicacion));

            if (ubicacion == null)
            {
                mvw_opciones.SetActiveView(vNoID);
                return;
            }
            codigoUbicacion = ubicacion.codigoURL;
        }

        void ProcesarToken(string _token)
        {
            MOBClientes cliente = MOBClientesClass.ObtenerMOBClientes()
                                  .SingleOrDefault(c => c.tokenValidacion.Equals(_token));

            if (cliente == null)
            {
                mvw_opciones.SetActiveView(vNoID);
                return;
            }

            token = cliente.tokenValidacion;

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

                int idPromoAleatoria = 0;
                int.TryParse(MOBPromocionesClass.SeleccionarPromocionAleatoria()?.id_promocion.ToString(), out idPromoAleatoria);

                if (idPromoAleatoria > 0)
                {
                    MOBPromocionesGeneradas promocionGenerada = new MOBPromocionesGeneradas()
                    {
                        id_cliente = cliente.id_cliente,
                        id_promocion = idPromoAleatoria,
                        id_ubicacion = (int)cliente.id_ubicacion,
                        fecha_canje = null,
                        entregado = false,
                        id_ticket = 0,
                        fecha_creacion = DateTime.Now,
                        idSucursal = null
                    };

                    if (MOBPromocionesGeneradasClass.RegistrarMOBPromocionesGeneradas(promocionGenerada) > 0)
                    {
                        string texto = ObtenerTextoPromocion(promocionGenerada);
                        lbl_promocion.Text = texto;
                        CrearCookieCliente(cliente);

                        mvw_opciones.SetActiveView(vPromocion);
                    }
                }
                else
                {
                    CrearCookieNoPromo();
                    mvw_opciones.SetActiveView(vNoPromo);
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
                    MOBClientes cliente = MOBClientesClass.ObtenerMOBClientes().FirstOrDefault(c => c.correo.Equals(txt_email.Text.Trim()));
                    if (cliente == null) 
                    {
                        cliente = new MOBClientes()
                        {
                            correo = txt_email.Text,
                            fecha_validacion = null,
                            tokenValidacion = Guid.NewGuid().ToString(),
                            validado = false,
                            id_ubicacion = ubicacion.id_ubicacion
                        };

                        if (MOBClientesClass.RegistrarMOBClientes(cliente) > 0)
                        {
                            string mensaje = $@"
                                <div style='padding: 20px; max-width: 600px; margin: 20px auto; font-family: Arial, sans-serif; color: #444; text-align: center; border: 1px solid #ddd; border-radius: 8px; background-color: #f9f9f9;'>
                                    <h2 style='color: #4CAF50; font-size: 1.8em; margin-bottom: 20px;'>¡Estás a un paso de tu premio!</h2>
                                    <p style='font-size: 1.2em; line-height: 1.6; margin-bottom: 30px;'>
                                        Para participar en nuestra promoción y reclamar tu premio, haz clic en el botón a continuación.
                                    </p>
                                    <a href='{ConfigurationManager.AppSettings["url"]}Default.aspx?tkn={cliente.tokenValidacion}' 
                                       style='display: inline-block; padding: 12px 20px; background-color: #4CAF50; color: #fff; font-size: 1.1em; text-decoration: none; border-radius: 5px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                        Canjear Premio
                                    </a>
                                    <p style='font-size: 0.9em; color: #777; margin-top: 20px;'>
                                        Si el enlace no funciona, copia y pega esta URL en tu navegador:<br />
                                        <span style='color: #4CAF50;'>{ConfigurationManager.AppSettings["url"]}Default.aspx?tkn={cliente.tokenValidacion}</span>
                                    </p>
                                </div>";


                            Thread oHilo = new Thread(delegate ()
                            {
                                MailClass.Enviar(cliente.correo, "My Own Baker", mensaje);
                            });
                            oHilo.IsBackground = true;
                            oHilo.Start();

                            txt_email.Text = "";
                            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), $"setTimeout(\"mostrarNotificacion('success', 'Correo registrado correctamente.');\", 300);", true);
                            mvw_opciones.SetActiveView(vEnviado);
                        }
                    }
                    else
                    {
                        string mensaje = $@"
                                <div style='padding: 20px; max-width: 600px; margin: 20px auto; font-family: Arial, sans-serif; color: #444; text-align: center; border: 1px solid #ddd; border-radius: 8px; background-color: #f9f9f9;'>
                                    <h2 style='color: #4CAF50; font-size: 1.8em; margin-bottom: 20px;'>¡Estás a un paso de tu premio!</h2>
                                    <p style='font-size: 1.2em; line-height: 1.6; margin-bottom: 30px;'>
                                        Para participar en nuestra promoción y reclamar tu premio, haz clic en el botón a continuación.
                                    </p>
                                    <a href='{ConfigurationManager.AppSettings["url"]}Default.aspx?tkn={cliente.tokenValidacion}' 
                                       style='display: inline-block; padding: 12px 20px; background-color: #4CAF50; color: #fff; font-size: 1.1em; text-decoration: none; border-radius: 5px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                        Canjear Premio
                                    </a>
                                    <p style='font-size: 0.9em; color: #777; margin-top: 20px;'>
                                        Si el enlace no funciona, copia y pega esta URL en tu navegador:<br />
                                        <span style='color: #4CAF50;'>{ConfigurationManager.AppSettings["url"]}Default.aspx?tkn={cliente.tokenValidacion}</span>
                                    </p>
                                </div>";


                        Thread oHilo = new Thread(delegate ()
                        {
                            MailClass.Enviar(cliente.correo, "My Own Baker", mensaje);
                        });
                        oHilo.IsBackground = true;
                        oHilo.Start();

                        txt_email.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), $"setTimeout(\"mostrarNotificacion('error', 'El correo electrónico ingresado ya esta participando, revisa tu bandeja de entrada para continuar participando.');\", 300);", true);
                        return;
                    }
                }
                else
                {
                    txt_email.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), $"setTimeout(\"mostrarNotificacion('error', 'El correo electrónico es obligatorio.');\", 300);", true);
                    return;
                }
            }
            else
            {
                txt_email.Text = "";
                mvw_opciones.SetActiveView(vNoID);
            }
        }
    }
}