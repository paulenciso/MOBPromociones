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
            bool contieneParametro = Request.QueryString["id"] != null || Request.QueryString["tkn"] != null;

            if (contieneParametro)
            {
                if (Request.QueryString["id"] != null)
                {
                    if (string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                        mvw_opciones.SetActiveView(vNoID);

                    codigoUbicacion = Request.QueryString["id"];
                    MOBUbicaciones ubicacion = MOBUbicacionesClass.ObtenerMOBUbicaciones().FirstOrDefault(u=>u.codigoURL.Equals(codigoUbicacion));
                    if (ubicacion == null) mvw_opciones.SetActiveView(vNoID);
                }

                if (Request.QueryString["tkn"] != null)
                {
                    if (string.IsNullOrWhiteSpace(Request.QueryString["tkn"]))
                        mvw_opciones.SetActiveView(vNoID);
                    try { token = Request.QueryString["tkn"]; } catch { mvw_opciones.SetActiveView(vNoID); }

                    mvw_opciones.SetActiveView(vCorreoValidacion);
                }
            }
            else
            {
                mvw_opciones.SetActiveView(vNoID);
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
                        MOBPromociones promocion = MOBPromocionesClass.ObtenerMOBPromocion(promocionGenerada.id_promocion);
                        string codigoBarras = promocionGenerada.id_ubicacion.ToString("D2") + promocionGenerada.id_promocion_generada.ToString("D3") + "-MOB" + promocionGenerada.id_promocion.ToString("D2");
                        string texto = $@"
                        <h3>FELICIDADES</h3>
                        <br />
                        <p style = 'font-size: 1.3em; text-align: justify;' >
                            {promocion.especificaciones}
                        </p>
                        <p style = 'font-size: 1.3em; text-align: justify;'> Presenta el siguiente código en cualquiera de nuestras sucursales:</p>
                        <p style='text-align: center'>
                            <span class='font'>{codigoBarras}</span>
                            <br />
                            <span style='letter-spacing:4px;'>{codigoBarras}</span>
                        </p>";


                        lbl_promocion.Text = texto;
                        mvw_opciones.SetActiveView(vPromocion);

                        tieneCookie = true;
                    }
                }
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
                    MOBPromociones promocion = MOBPromocionesClass.ObtenerMOBPromocion(promocionGenerada.id_promocion);
                    string codigoBarras = promocionGenerada.id_ubicacion.ToString("D2") + promocionGenerada.id_promocion_generada.ToString("D3") + "-MOB" + promocionGenerada.id_promocion.ToString("D2"); 
                    string texto =$@"
                        <h3>FELICIDADES</h3>
                        <br />
                        <p style = 'font-size: 1.3em; text-align: justify;' >
                            {promocion.especificaciones}
                        </p>
                        <p style = 'font-size: 1.3em; text-align: justify;'> Presenta el siguiente código en cualquiera de nuestras sucursales:</p>
                        <p style='text-align: center'>
                            <span class='font'>{codigoBarras}</span>
                            <br />
                            <span style='letter-spacing:4px;'>{codigoBarras}</span>
                        </p>";


                    lbl_promocion.Text = texto;


                    byte[] cookie = Encoding.UTF8.GetBytes(cliente.correo);
                    byte[] encriptado = MachineKey.Protect(cookie);
                    HttpCookie oCookie = new HttpCookie("_MOB", HttpServerUtility.UrlTokenEncode(encriptado))
                    {
                        Expires = DateTime.MaxValue
                    };
                    Response.SetCookie(oCookie);

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