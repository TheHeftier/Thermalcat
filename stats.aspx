<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stats.aspx.cs" Inherits="ThermalCat.Default" %>
<!DOCTYPE html>
<html lang="pt-br">
	<head runat="server">
		<meta charset="utf-8">
		<title>Thermal Cat</title>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
        <link rel="stylesheet" href="_css/default.css">
		<style>
            #NavButton {
                position: absolute;
                z-index: 2;
            }
            
            section {
                margin-top: 11vh;
            }
            
            #Temp {
                font-size: 6vmin;
            }
            
            #TitleLogo {
                width: 20vw;
                position: absolute; 
                right: 2vw;
            }
            
            #ColdCat {
                display: block; 
                margin: auto;
            }
            .fa-share {
                text-decoration: none; 
                transform: rotate(90deg); 
                background-color: #9B34B7; 
                color: white; 
                padding: 3vmin; 
                border-radius: 50%; 
                margin: 3vw; 
                font-size: 12vmin;
            }
            
            .fa-navicon {
                background: none; 
                border: none;
                font-size: 10vw; 
                color: #8e44ad;
                margin: 2vh;
            }
                
            .temp-panel {
                background-color: #8e44ad; 
                color: white; 
                text-align: center; 
                padding: 1vh 2vw; 
                margin: 6vh 15vw 1vh 15vw;
                border-radius: 10px;
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
                
                #HomeRow {
                    display: flex; 
                    height: 80vh; 
                    align-items: stretch;
                }
                
                .general {
                    display: flex; 
                    flex-grow: 1; 
                    flex-direction: column; 
                }
                
                #TitleLogo {
                    width: 6vw;
                }
                
                #ColdCat {
                    margin-top: 20vh;
                }
                
                .panel {
                    box-shadow: 10px 10px 20px black;
                    text-align: center;
                }
                
                .menus {
                    padding: 10%;
                    flex-grow: 1;
                    cursor: pointer;
                }
                
                .menus i {
                    font-size: 8vw; 
                    color: #732372;
                }
                
                .line {
                    border: 2px solid #dedede;
                    background-color: #f1f1f1;
                    border-radius: 5px;
                    margin: 5px;
                }
                
                .line::after {
                    content: "";
                    clear: both;
                    display: table;
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
                
                .temp-panel {
                    background-color: #8e44ad; 
                    color: white; 
                    text-align: center; 
                    padding: 1vh 2vw; 
                    margin: 1vh 10vw;
                    border-radius: 15px;
                }
                
                section {
                    margin-left: 8vw;
                    height: 100%;
                    overflow-y: hidden; 
                }
                
                #MainPanel {
                    flex-grow: 1;
                    margin-bottom: 10vh;
                    margin-top: 2vh;
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
                <h1>Estatísticas</h1>
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
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <form class="form-inline" runat="server">
                                    <div class="form-group">
                                        <asp:DropDownList Id="Arduinos" CssClass="form-control" OnClick="Cadastrar" runat="Server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="DateMenu" TextMode="Date" runat="server"/>
                                    </div>
                                    <asp:Button Id="Report" Text="Gerar Gráfico" OnClick="Click_Button" CssClass="btn btn-default" runat="server" />
                                </form>
                            </div>
                            <div class="panel-body" style="display: flex; flex-direction: column; height: 70vmin;">
                                <div id = "Diario" style="flex-grow: 1;"></div>
                                <asp:Literal ID="Day" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        
		<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
		<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.6/umd/popper.min.js"></script>
		<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
        <script>
            $( document ).ready(function(event) {
                var nav = false;
                $("button").click(function(event) {
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
            });
        </script>
	</body>
</html>