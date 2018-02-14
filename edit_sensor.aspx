<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_sensor.aspx.cs" Inherits="ThermalCat.Default" %>
<!DOCTYPE html>
<html lang="pt-br">
	<head runat="server">
		<meta charset="utf-8">
		<title>Thermal Cat</title>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <link rel="icon" type="image/png" href="favicon-32x32.png" sizes="32x32" />
        <link rel="icon" type="image/png" href="favicon-16x16.png" sizes="16x16" />
        <link rel="stylesheet" href="_css/default.css">
		<style>
            #NavButton {
                position: absolute;
                z-index: 2;
            }
            
            section {
                margin-top: 11vh;
            }
            
            .fa-navicon {
                background: none; 
                border: none;
                font-size: 10vw; 
                color: #8e44ad;
                margin: 2vh;
            }
            
            .side-bar {
                height: 100vh;
                width: 0vw;
                position: fixed;
                z-index: 1;
                top: 0;
                left: 0;
                background-color: #bdc3c7;
                overflow: hidden;
                padding-top: 27vh;
                transition: 0.5s;
            }
            
            .side-bar .navbutton {
                text-decoration: none;
                text-align: center;
                border: none;
                background: none;
                display: block;
                transition: all 0.5s ease;
                padding: 2vh 0;
                color: #674172;
                font-size: 10vw;
                width: 25vw;
            }
            
            .side-bar .navbutton:hover {
                color: #BF55EC;
            }
            
            .navbutton.active {
                color: #9A12B3;
            }
            
            @media (min-width: 768px){
                
                body {
                    background-image: none;
                }
            
                section {
                    margin-top: 0;
                }
            
                .side-bar {
                    width: 8vw;
                    position: absolute;
                    z-index: auto;
                    border-right: 2px solid #9A12B3;
                    background-color: white;
                }
                
                .side-bar .navbutton {
                    font-size: 4vw;
                    width: 8vw;
                }
                
                .top-bar {
                    width: 100%;
                    overflow: auto;
                    border-right: 2px solid black;
                    border-left: 2px solid black;
                }
                
                .top-bar button {
                    border: none;
                    background: none;
                    float: left;
                    text-align: center;
                    width: 33.33%;
                    padding: 12px 0;
                    transition: all 0.3s ease;
                    color: #674172;
                    font-size: 4vw;
                }
                
                .top-bar button:hover {
                    color: #BF55EC;
                }
                
                section {
                    margin-left: 8vw;
                    height: 100%;
                    overflow-y: hidden; 
                }
                
                h5 {
                    text-align: center;
                    color: #8e44ad;
                }
                
                header {
                    margin-top: 3vh;
                    margin-left: 8vw;
                    overflow-y: hidden; 
                }
                
				#RegBtn {
					text-align: center;
					display: flex;
					flex-direction: column;
					padding: 14vh 0;
				}
                
				#RegBtn i {
					flex-grow: 1;
					padding: 4vmin; 
					margin: 2vmin 10vmin; 
					background-color: #2980b9; 
					color: white;
					font-size: 5vmin;
					border-radius: 25px; 
					cursor: pointer;
				}
            }
        </style>
    </head>
    
    <body>
        <aside class="side-bar">
            <button id="HomeButton" onclick="location.href = 'home.aspx';" class="navbutton fa fa-home" data-toggle="tooltip" data-placement="right" title="Home"></button>
            <button id="StatsButton" onclick="location.href = 'stats.aspx';" class="navbutton fa fa-bar-chart" data-toggle="tooltip" data-placement="right" title="Estatísticas"></button>
            <button id="FeedButton" class="navbutton fa fa-comments-o" data-toggle="modal" data-target="#feedbackModal" data-placement="right" title="Feedback"><i data-toggle=""></i></button>
            <button id="LogoutButton" onclick="location.href = 'logout.aspx';" class="navbutton fa fa-power-off" data-toggle="tooltip" data-placement="right" title="Logout"></button>
        </aside>
        
        <div class="modal fade" id="feedbackModal" tabindex="-1" role="dialog">
          <div class="modal-dialog" role="document">
            <div class="modal-content" style="background: rgba(255, 255, 255, .95); text-align: center;">
              <div class="modal-body">
                    <div class="form-group">
                        <img src="_images/icons/title_logo/icon-384x384.png" width="80vw" />
                    </div>
                    <script type="text/javascript" src="http://assets.freshdesk.com/widget/freshwidget.js"></script>
                    <style type="text/css" media="screen, projection">
                    	@import url(http://assets.freshdesk.com/widget/freshwidget.css); 
                    </style> 
                    <iframe title="Feedback Form" class="freshwidget-embedded-form" id="freshwidget-embedded-form" src="https://thermalcat.freshdesk.com/widgets/feedback_widget/new?&widgetType=embedded&screenshot=no&searchArea=no" scrolling="no" height="500px" width="100%" frameborder="0" >
                    </iframe>
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
                    <asp:Literal ID="ltScripts" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="col-md-2">
                <h1>Edição</h1>
            </div>
            <div class="col-md-3 col-md-offset-1">
                <h1><asp:Label id="Empresa" style="color: #674172;text-shadow: 3px 1px 1px #BF55EC;" Text="[Nome da Empresa]" runat="server"></asp:Label></h1>
            </div>
        </header>
        <div class="container visible-xs-block">
            <button id="NavButton" class="fa fa-navicon"></button>
        </div>
		<section>
			<div id="main" class="container-fluid">
				<div class="row" >
					<div id="RegBtn" class="col-md-4">
						<asp:Literal ID="RegMenus" runat="server"></asp:Literal>
					</div>
					<div class="col-md-7 hidden-xs">
						<div id="RegForm" class="well" style="overflow-y: scroll;">
							<form runat="server" style="height: 73vmin;">
                            	<div class="form-group">
                            		<label>Editar Sensor</label>
                            	</div>
                            	<div class="form-group">
                            		<asp:TextBox id="Sensor" CssClass="form-control" placeholder="Nome do Sensor" MaxLength="15" runat="Server" autofocus/>
                            	</div>
								<div class="form-group">
									<asp:DropDownList Id="Companies" CssClass="form-control" runat="Server"></asp:DropDownList>
								</div>
								<asp:Button Id="btnSalvar" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" runat="server" />
								<asp:Button Id="btnVoltar" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnVoltar_Click" runat="server" />
								<asp:Label Id="lblMsg" Text="" runat="server"/>
                            </form>
						</div>
					</div>
				</div>
			</div>
        </section>
        
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
		<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.6/umd/popper.min.js"></script>
		<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
		<script>
			$("i").click(function() {
			    if($(event.target).attr('id')=='UserForm'){
			        window.location.replace("list_user.aspx");
			    } else if($(event.target).attr('id')=='CompanyForm'){
			        window.location.replace("list_company.aspx");
			    } else if($(event.target).attr('id')=='SensorForm'){
			        window.location.replace("list_sensor.aspx");
			    }
			});
            
            var nav = false;
            $("button").click(function() {
                if($(event.target).attr('id')=='NavButton'){
                    if(nav){
                        $(".side-bar").css("width", "0vw");
                        nav = false;
                    } else {
                        $(".side-bar").css("width", "25vw");
                        nav = true;
                    }
                } else if($(event.target).attr('id')=='AddUser'){
			        window.location.replace("register_user.aspx");
                } else if($(event.target).attr('id')=='EditUser'){
			        window.location.replace("list_user.aspx");
                } else if($(event.target).attr('id')=='DeleteUser'){
			        window.location.replace("delete_user.aspx");
                }
            });
        </script>
	</body>
</html>