<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="ThermalCat.Default" %>
<!DOCTYPE html>
<html lang="pt-br">
	<head runat="server">
		<meta charset="utf-8">
		<title>Thermal Cat</title>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons">
        <link rel="stylesheet" href="_css/default.css">
		<style>
            
        </style>
    </head>
    
    <body>
        <aside class="side-bar">
            <button id="HomeButton" class="navbutton fa fa-home active"></button>
            <button id="StatsButton" class="navbutton fa fa-bar-chart"></button>
            <button id="FeedButton" class="navbutton fa fa-comments-o" data-toggle="modal" data-target="#feedbackModal"></button>
            <button id="SupportButton" class="navbutton fa fa-desktop"></button>
            <button id="ConfigButton" class="navbutton fa fa-cog"></button>
            <button id="LogoutButton" onclick="location.href = 'logout.aspx';" class="navbutton fa fa-power-off"></button>
        </aside>
        
        <div class="modal fade" id="feedbackModal" tabindex="-1" role="dialog">
          <div class="modal-dialog" role="document">
            <div class="modal-content" style="background: rgba(255, 255, 255, .8); text-align: center;">
              <div class="modal-body">
                <form>
                    <div class="form-group">
                        <img src="_images/icons/title_logo/icon-384x384.png" width="80vw" />
                    </div>
                    <div class="form-group">
                        <label>Feedback</label>
                    </div>
                    <div class="form-group">
                        <input type="text" class="form-control" id="recipient-name" placeholder="Assunto...">
                    </div>
                    <div class="form-group">
                        <textarea class="form-control" id="message-text"rows="10" placeholder="Mensagem..."></textarea>
                    </div>
                </form>
              </div>
            </div>
          </div>
        </div>
        
        <header class="hidden-xs">
            <div class="col-md-3">
                <h3 class="text-center">
                    <asp:Label id="UserName" runat="server">[Nome de Usuário]</asp:Label><br/>
                    <small><asp:Label id="UserAccess" runat="server">[Nível de Acesso]</asp:Label></small>
                </h3>
            </div>
            <div class="col-md-3">
                <div class="top-bar">
                    <button id="AddUser" class="fa fa-plus-circle"></button>
                    <button id="EditUser" class="fa fa-pencil"></button>
                    <button id="DeleteUser" class="fa fa-trash"></button> 
                </div>
            </div>
        </header>
        <div class="container visible-xs-block">
            <button id="NavButton" class="fa fa-navicon"></button>
        </div>
        
        <section></section>
        
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
		<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.6/umd/popper.min.js"></script>
		<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
		<script src="_scripts/main.js"></script>
        <script>
            var page = "home.aspx";
            
            var nav = false;
            $("button").click(function() {
                $("button").removeClass('active');
                if($(event.target).attr('id')=='HomeButton'){
                    page = "home.aspx";
                    $("#HomeButton").addClass('active');
                } else if($(event.target).attr('id')=='StatsButton'){
                    page = "stats.aspx";
                    $("#StatsButton").addClass('active');
                } else if($(event.target).attr('id')=='SupportButton'){
                    page = "support.aspx";
                    $("#SupportButton").addClass('active');
                } else if($(event.target).attr('id')=='ConfigButton'){
                    page = "config.aspx";
                    $("#ConfigButton").addClass('active');
                } else if($(event.target).attr('id')=='AddUser'){
			        window.location.replace("register_user.aspx");
                } else if($(event.target).attr('id')=='EditUser'){
			        window.location.replace("edit_user.aspx");
                } else if($(event.target).attr('id')=='DeleteUser'){
			        window.location.replace("delete_user.aspx");
                } else if($(event.target).attr('id')=='NavButton'){
                    if(nav){
                        $(".side-bar").css("width", "0vw");
                        nav = false;
                    } else {
                        $(".side-bar").css("width", "25vw");
                        nav = true;
                    }
                }
            });
            
            $(document).load(function() {
                $("section").load(page);
            });
        </script>
	</body>
</html>