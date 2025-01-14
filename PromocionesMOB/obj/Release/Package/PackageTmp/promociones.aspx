<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="promociones.aspx.cs" Inherits="PromocionesMOB.promociones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>My Own Baker</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/material-design-lite/1.3.0/material.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/material-design-lite/1.3.0/material.min.js"></script>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />

    <link href='https://fonts.googleapis.com/css?family=Libre Barcode 39' rel='stylesheet' />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        body {
            background-color: #ffffff;
            font-family: 'Roboto', sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
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

        /* Media query para pantallas pequeñas */
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

        .font {
            font-family: '3 of 9 Barcode','Libre Barcode 39';
            font-size: 32px;
            color: black;
            letter-spacing: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePageMethods="true" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="form-container">
                    <asp:MultiView runat="server" ActiveViewIndex="0" ID="mvw_opciones">
                        <asp:View runat="server" ID="vCorreo">
                            <h4>Para participar, ingresa tu correo</h4>
                            <asp:Label ID="lbl_message" runat="server" CssClass="error-message" Visible="false"></asp:Label>

                            <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                <asp:TextBox ID="txt_email" runat="server" CssClass="mdl-textfield__input" Placeholder="Correo Electrónico" />
                                <label class="mdl-textfield__label" for="txt_email">Correo Electrónico</label>
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
                        </asp:View>
                        <asp:View runat="server" ID="vCorreoValidacion">
                            <h4>Da clic para reclamar tu premio</h4>
                            <asp:Button runat="server" CssClass="mdl-button mdl-js-button mdl-button--raised mdl-button--colored" ID="btn_generar" Text="Reclamar" OnClick="btn_generar_Click" />
                        </asp:View>
                        <asp:View runat="server" ID="vPromocion">
                            <asp:Label runat="server" ID="lbl_promocion" />
                        </asp:View>
                        <asp:View runat="server" ID="vNoID">
                            NO ID
                        </asp:View>
                    </asp:MultiView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
