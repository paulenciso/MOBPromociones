<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PromocionesMOB.promociones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>My Own Baker</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/material-design-lite/1.3.0/material.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/material-design-lite/1.3.0/material.min.js"></script>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />

    <link href='https://fonts.googleapis.com/css?family=Libre Barcode 39' rel='stylesheet' />


    <!-- SweetAlert2 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />

    <!-- SweetAlert2 JS -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        body {
            display: flex;
            justify-content: center;
            align-items: center;
            font-family: Athelas-Regular, ITCAvantGardePro-Md;
            text-align: center;
            margin: 0;
            padding: 0;
            height: 100vh;
            position: relative;
        }

            body::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                z-index: -1;
                background: url('Imagenes/conejo_1.svg') 0 0, url('Imagenes/mob_logo.svg') 0 150px;
                background-repeat: repeat;
                background-size: 250px 250px;
                opacity: 0.3;
                pointer-events: none;
            }


        .form-container {
            width: 100%;
            max-width: 400px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
            border-radius: 8px;
            background-color: #ffffff;
            text-align: center;
        }

            .form-container h4 {
                margin-bottom: 20px;
                color: #333333;
            }

            .form-container .mdl-textfield {
                width: 100%;
            }

            .form-container .mdl-button {
                width: 100%;
                margin-top: 20px;
            }

        .error-message {
            color: #d32f2f;
            font-size: 12px;
        }

        @media (max-width: 600px) {
            .form-container {
                padding: 15px;
            }

                .form-container h4 {
                    font-size: 18px;
                }
        }


        @font-face {
            font-family: '3 of 9 Barcode';
            src: local('3 of 9 Barcode'), url('~/fonts/3OF9_NEW.TTF') format('truetype');
        }

        @font-face {
            font-family: 'ITCAvantGardePro-Md';
            font-style: normal;
            font-weight: normal;
            src: local('ITCAvantGardePro-Md'), url('~/fonts/ITCAvantGardePro-Md.TTF') format('truetype');
        }

        @font-face {
            font-family: 'ITCAvantGardePro-Md';
            font-style: normal;
            font-weight: normal;
            src: local('Athelas-Regular'), url('~/fonts/Athelas-Regular-Md.TTF') format('truetype');
        }

        .font {
            font-family: '3 of 9 Barcode','Libre Barcode 39';
            font-size: 32px;
            color: black;
            letter-spacing: 3px;
        }

        .mdl-card {
            margin: 50px auto;
            max-width: 800px;
            padding: 20px;
        }

        .mdl-card__title {
            background: linear-gradient(45deg, #ff4081, #ff80ab);
            color: white;
            padding: 0;
            text-align: center;
        }

            .mdl-card__title img {
                width: 100%;
                height: auto;
                max-height: 300px;
                object-fit: cover;
            }

        .mdl-card__supporting-text {
            font-size: 1.8em;
            line-height: 1.8;
            color: black;
        }

        .highlight {
            color: #ff4081;
            font-weight: bold;
        }

        button.mdl-button {
            pointer-events: auto;
        }

        .mdl-snackbar {
            background-color: #f44336; /* Rojo para errores */
            color: #fff;
            font-weight: bold;
        }

        .mdl-snackbar__action {
            color: #ffccbc; /* Color de acción para contraste */
        }
    </style>

    <style>
        @keyframes fadeIn {
            from {
                opacity: 0;
            }

            to {
                opacity: 1;
            }
        }

        @keyframes bounce {
            0%, 100% {
                transform: translateY(0);
            }

            50% {
                transform: translateY(-10px);
            }
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePageMethods="true" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:MultiView runat="server" ActiveViewIndex="0" ID="mvw_opciones">
                    <asp:View runat="server" ID="vCorreo">
                        <div class="form-container">
                            <div>
                                <img src="Imagenes/LogoNina.png" alt="Imagen decorativa" style="width: 100%; max-width: 150px; height: auto; margin-bottom: 20px;" />
                            </div>
                            <%--<h4>¡Participa ahora y gana! Ingresa tu correo y forma parte de nuestras promociones exclusivas. Puedes canjear tu premio en cualquiera de las sucursales de Nina Pastelería. ¡No te lo pierdas!</h4>--%>


                            <!-- Título -->
                            <h4 class="mdl-typography--headline" style="color: #ff4081; margin-bottom: 10px;">¡Participa ahora y gana!</h4>

                            <!-- Mensaje descriptivo -->
                            <p class="mdl-typography--body-1" style="font-size: 1.2em; text-align: justify; margin-bottom: 20px;">
                               Ingresa tu correo y forma parte de nuestras promociones exclusivas. Puedes canjear tu premio en cualquiera de las 15 sucursales de Nina Pastelería. ¡No te lo pierdas!
                            </p>

                            <asp:Label ID="lbl_message" runat="server" CssClass="error-message" Visible="false"></asp:Label>

                            <div class="mdl-textfield mdl-js-textfield ">
                                <asp:TextBox ID="txt_email" runat="server" CssClass="mdl-textfield__input" Placeholder="Correo Electrónico" />
                                <asp:RegularExpressionValidator
                                    ID="regexEmailValidator"
                                    runat="server"
                                    ControlToValidate="txt_email"
                                    ErrorMessage="Introduce un correo válido."
                                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                                    CssClass="error-message"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator
                                    ID="requiredEmailValidator"
                                    runat="server"
                                    ControlToValidate="txt_email"
                                    ErrorMessage="El correo es obligatorio."
                                    CssClass="error-message"></asp:RequiredFieldValidator>
                            </div>

                            <asp:Button runat="server" CssClass="mdl-button mdl-js-button mdl-button--raised mdl-button--colored" ID="btn_validar" Text="Participar" OnClick="btn_validar_Click" />
                        </div>
                    </asp:View>
                    <asp:View runat="server" ID="vCorreoValidacion">
                        <div class="form-container">
                            <h4>Da clic para reclamar tu premio</h4>
                            <asp:Button runat="server" CssClass="mdl-button mdl-js-button mdl-button--raised mdl-button--colored" Style="background-color: #af91c1 !important;" ID="btn_generar" Text="Reclamar" OnClick="btn_generar_Click" />
                        </div>
                    </asp:View>
                    <asp:View runat="server" ID="vPromocion">
                        <asp:Label runat="server" ID="lbl_promocion" />
                    </asp:View>
                    <asp:View runat="server" ID="vNoID">
                        <div class="mdl-card mdl-shadow--4dp">
                            <div class="mdl-card__title">
                                <img src="Imagenes/welcome.jpg" alt="Welcome to Paradise">
                            </div>
                            <div class="mdl-card__supporting-text">
                                <p>Estamos de fiesta celebrando nuestro <span class="highlight">6to aniversario</span>, y tú eres parte de esta historia. 🐰🌟</p>
                                <p><span class="highlight">15 traviesos conejos</span> se han escapado en <b>Torreón, Gómez y Lerdo</b>, y necesitamos tu ayuda para encontrarlos. 🕵️‍♀️🔍</p>
                                <p>🎁 <b>¡Recompensa garantizada!</b> Cada conejo que encuentres te acercará a los increíbles premios de <span class="highlight">My Own Bake</span> que tenemos para ti en <span class="highlight">Nina Pastelería</span>. ¡No dejes que se te escapen!</p>
                            </div>
                            <div class="mdl-card__actions mdl-card--border">
                                <span class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect">¡Únete a la aventura!
                                    </span>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View runat="server" ID="vEnviado">
                        <div class="mdl-card mdl-shadow--2dp" style="padding: 20px; max-width: 600px; margin: 40px auto; text-align: center;">
                            <div style="font-size: 3em; color: #4CAF50; margin-bottom: 20px;">📬</div>
                            <h4 class="mdl-typography--headline" style="color: #3F51B5; margin-bottom: 20px;">¡Revisa tu correo electrónico!</h4>
                            <p class="mdl-typography--body-1" style="font-size: 1.2em; color: #555; line-height: 1.6;">
                                Hemos enviado un mensaje a tu bandeja de entrada con las instrucciones para continuar. Si no lo encuentras, revisa tu carpeta de <b>spam</b> o <b>correo no deseado</b>.
                           
                            </p>
                        </div>
                    </asp:View>
                    <asp:View runat="server" ID="vNoPromo">
                        <div class="mdl-card mdl-shadow--2dp" style="padding: 20px; max-width: 600px; margin: 20px auto; text-align: center; animation: fadeIn 1s ease-in-out;">
                            <div style="margin-bottom: 20px;">
                                <i class="material-icons" style="font-size: 4em; color: #FF5722; animation: bounce 1.5s infinite;">info_outline</i>
                            </div>
                            <h2 class="mdl-typography--headline" style="color: #FF5722; margin-bottom: 10px;">¡Gracias por Participar!</h2>
                            <p class="mdl-typography--body-1" style="font-size: 1.2em; line-height: 1.6; color: #555; margin-bottom: 20px;">
                                Lamentamos informarte que ya no hay promociones disponibles. Agradecemos mucho tu interés y entusiasmo.
                           
                            </p>
                            <p class="mdl-typography--body-1" style="font-size: 1.1em; color: #777;">
                                Te invitamos a estar atento a nuestras futuras promociones, donde tendremos más sorpresas para ti.
                           
                            </p>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <!-- JS de MDL -->
    <script defer src="https://code.getmdl.io/1.3.0/material.min.js"></script>
    <!-- SweetAlert2 Scripts -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script>
        // Función para mostrar la notificación
        function mostrarNotificacion(tipo, mensaje) {
            Swal.fire({
                icon: tipo, // 'success', 'error', 'warning', 'info', 'question'
                title: mensaje,
                showConfirmButton: false,
                timer: 3000,
            });
        }
    </script>

</body>
</html>
