using MOBClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PromocionesMOB
{
    public partial class promociones : System.Web.UI.Page
    {
        #region Variables
        int idUbicacion;
        string token;
        #endregion
        #region Métodos
        private void ValidarParametros()
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                mvw_opciones.SetActiveView(vNoID);

            if (string.IsNullOrWhiteSpace(Request.QueryString["tkn"]))
                mvw_opciones.SetActiveView(vNoID);

            try { idUbicacion = 0; int.TryParse(Request.QueryString["id"], out idUbicacion); } catch { mvw_opciones.SetActiveView(vNoID); }
            try { token = Request.QueryString["tkn"]; } catch { mvw_opciones.SetActiveView(vCorreoValidacion); }
        }
        private void ValidarCookie()
        {
            var cookie = Request.Cookies["_CENC"];

            if (cookie != null)
            {
                byte[] textoCookie = HttpServerUtility.UrlTokenDecode(cookie.Value);
                byte[] desencriptado = MachineKey.Unprotect(textoCookie);

                //if (!IsPostBack) Login(Encoding.UTF8.GetString(desencriptado));
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidarParametros();
        }

        protected void btn_validar_Click(object sender, EventArgs e)
        {
            MOBUbicaciones ubicacion = MOBUbicacionesClass.ObtenerMOBUbicacion(idUbicacion);
            if (!txt_email.Text.Trim().Equals(""))
            {
                MOBClientes cliente = new MOBClientes()
                {
                    correo = txt_email.Text,
                    fecha_validacion = null,
                    tokenValidacion = Guid.NewGuid().ToString(),
                    validado = false
                };

                if(MOBClientesClass.RegistrarMOBClientes(cliente) > 0)
                {
                    string mensaje =
                            $@"Estas a un paso de participar en nuestra promoción, por favor ingresa al enlace para recibir tu premio.</strong>
                            <br/><br/>
                            <a href='{ConfigurationManager.AppSettings["url"]}promociones.aspx?tkn={cliente.tokenValidacion}'>Canjear premio</a>
                            <br/><br/>";
                }
            }
        }
    }
}