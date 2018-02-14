<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="ThermalCat.Default" %> 
<!DOCTYPE html>
<html lang="pt-br">
    <head runat="server">
        <meta charset="utf-8" />
        <title>Thermal Cat</title>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <link rel="icon" type="image/png" href="favicon-32x32.png" sizes="32x32" />
        <link rel="icon" type="image/png" href="favicon-16x16.png" sizes="16x16" />
        <link rel="stylesheet" href="_css/default.css">
        <link rel="stylesheet" href="_css/index.css">
    </head>
    <body>
        <section>
            <div class="container-fluid">
                <form runat="server">
                    <div class="form-group inner-addon right-addon">
                        <i class="glyphicon glyphicon-user"></i>
                        <asp:TextBox id="User" Type="Text" CssClass="form-control" PlaceHolder="Usuário" tabindex="1" runat="server" autofocus></asp:TextBox>
                    </div>
                    <div class="form-group inner-addon right-addon">
                        <i class="glyphicon glyphicon-lock"></i>
                        <asp:TextBox id="Password" type="Password" CssClass="form-control" PlaceHolder="Senha" tabindex="2" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group pull-right">
                        <asp:CheckBox id="RememberMe" Text="Lembrar-me" runat="server"></asp:CheckBox>
                    </div>
                    <div class="form-group">
                        <asp:Button id="Login" OnClick="ClickLogin" CssClass="btn btn-block" runat="server" Text="Entrar"></asp:Button>
                    </div>
                    <div class="form-group">
                        <asp:Panel id="WrongPanel" CssClass="alert alert-danger" runat="server" role="alert" style="display: none;">
                            <asp:Label id="WrongCrendential" Text="Usuário e/ou senha incorretas!" role="alert" runat="server"></asp:Label>
                        </asp:Panel>
                    </div>
                </form>
            </div>
        </section>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    </body>
</html>
