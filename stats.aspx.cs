using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;  

namespace ThermalCat { 
    
    public partial class Default : System.Web.UI.Page { 
        
        string id;
        
        protected void Page_Load(object sender, EventArgs e) {
            HttpCookie cookie = Request.Cookies["Login"];
            
            if (cookie == null) {
                Response.Redirect("index.aspx");
            	return;
            }
            
            id = cookie.Value;
            
            if (!IsPostBack){
                DateMenu.Text = DateTime.Now.ToString("yyyy-MM-dd");
                StringBuilder build = new StringBuilder();
                build.Append(@"
                    <script type = 'text/javascript' src = 'https://www.gstatic.com/charts/loader.js'></script>
                    <script type = 'text/javascript'>google.charts.load('current', {packages: ['corechart','line']});</script>
                    <script language = 'JavaScript'>
                         function drawChart() {
                            // Define the chart to be drawn.
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Hora');
                            data.addColumn('number', '°C');
                            data.addRows([
                ");
                for(int i = 0;i < 24;i++){
                    build.Append(" ['" + i + "',  null],");
                }
                build.Remove(build.Length - 1, 1);  
                build.Append("]);");  
                build.Append(@"
                            // Set chart options
                            var options = {
                                'title' : 'Temperatura Média',
                                curveType: 'function',
                                hAxis: {
                                    title: 'Hora'
                                },
                                vAxis: {
                                    title: 'Temperatura'
                                }  
                            };
                
                            // Instantiate and draw the chart.
                            var chart = new google.visualization.LineChart(document.getElementById('Diario'));
                            chart.draw(data, options);
                         }
                         google.charts.setOnLoadCallback(drawChart);
                      </script>
                ");
                Day.Text = build.ToString();
            }
            using(SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                cn.Open();
                using(SqlCommand cmd = new SqlCommand(@"
                    SELECT tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.idAcesso, tbAcesso.hierarquia, tbAfiliacao.nome 
                    FROM tbUsuario, tbAcesso, tbAfiliacao 
                    WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id and tbAfiliacao.id=tbUsuario.idAfiliacao",cn)){
                    cmd.Parameters.AddWithValue("@id", id);
                    using(SqlDataReader sdr = cmd.ExecuteReader()){  
                        if(sdr.Read()){
                            string nome = sdr.GetValue(sdr.GetOrdinal("nome")).ToString() + " " + sdr.GetValue(sdr.GetOrdinal("sobrenome")).ToString();
                            UserName.Text = nome;
                            UserAccess.Text = sdr.GetValue(sdr.GetOrdinal("hierarquia")).ToString();
                            Empresa.Text = sdr.GetValue(4).ToString();
                            if(!sdr.GetValue(sdr.GetOrdinal("idAcesso")).ToString().Equals("4")){  
                                StringBuilder strScript = new StringBuilder(); 
                                strScript.Append(@"
                                    <button id='AddUser' class='fa fa-plus-circle' data-toggle='tooltip' data-placement='bottom' title='Cadastrar'></button>
                                    <button id='EditUser' class='fa fa-pencil' data-toggle='tooltip' data-placement='bottom' title='Editar'></button>
                                    <button id='DeleteUser' class='fa fa-trash' data-toggle='tooltip' data-placement='bottom' title='Deletar'></button>
                                ");
                                ltScripts.Text = strScript.ToString();
                            }
                        }
                    }
                } 
                
                if (!IsPostBack){
                    using(SqlCommand cmd = new SqlCommand(@"
                            SELECT * FROM tbArduino, tbAfiliacao, tbUsuario 
                            WHERE tbUsuario.id=@id and tbAfiliacao.id=tbUsuario.idAfiliacao and tbAfiliacao.id=tbArduino.idAfiliacao",cn)){
                        cmd.Parameters.AddWithValue("@id", id);
                        Arduinos.DataSource = cmd.ExecuteReader();
                        Arduinos.DataTextField = "identificador";
                        Arduinos.DataValueField = "id";
                        Arduinos.DataBind();
                    } 
                    Arduinos.Items.Insert(0, new ListItem("Selecione um sensor", "0"));
                }
            }
        }
        
        protected void Click_Button(object sender, EventArgs e){
            
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                conn.Open();
                
                StringBuilder build = new StringBuilder();
                build.Append(@"
                    <script type = 'text/javascript' src = 'https://www.gstatic.com/charts/loader.js'></script>
                    <script type = 'text/javascript'>google.charts.load('current', {packages: ['corechart','line']});</script>
                    <script language = 'JavaScript'>
                         function drawChart() {
                            // Define the chart to be drawn.
                            var data = new google.visualization.DataTable();
                            data.addColumn('string', 'Hora');
                            data.addColumn('number', '°C');
                            data.addRows([
                ");
                for(int i = 0;i < 24;i++){
                    using (SqlCommand cmd = new SqlCommand(@"
                        SELECT TOP 10 avg(tbTemperatura.temperatura) FROM tbArduino, tbTemperatura 
                        WHERE tbArduino.id=@id and tbArduino.id=tbTemperatura.idArduino and
                        dataHora > CAST(@d1 AS DATE) AND dataHora <= DATEADD(day,1, CAST(@d2 AS DATE))
                        AND DATEPART(hh,dataHora) >= @h1 AND DATEPART(hh,dataHora) <= @h2",conn)){
                        cmd.Parameters.AddWithValue("@id", Arduinos.Text);
                        cmd.Parameters.AddWithValue("@d1", DateMenu.Text);
                        cmd.Parameters.AddWithValue("@d2", DateMenu.Text);
                        cmd.Parameters.AddWithValue("@h1", i);
                        cmd.Parameters.AddWithValue("@h2", (i+1));
                        using (SqlDataReader sdr = cmd.ExecuteReader()){ 
                            int c = 0;
                            while(sdr.Read()){
                                if(!(sdr.GetValue(0) is DBNull)){
                                    build.Append(" ['" + i + "',  (" + Convert.ToInt32(sdr.GetValue(0)) + ")],");
                                } else {
                                    build.Append(" ['" + i + "',  null],");
                                }
                            }
                        }
                    }
                }
                build.Remove(build.Length - 1, 1);  
                build.Append("]);");  
                build.Append(@"
                            // Set chart options
                            var options = {
                                'title' : 'Temperatura Média',
                                curveType: 'function',
                                hAxis: {
                                    title: 'Hora'
                                },
                                vAxis: {
                                    title: 'Temperatura'
                                }  
                            };
                
                            // Instantiate and draw the chart.
                            var chart = new google.visualization.LineChart(document.getElementById('Diario'));
                            chart.draw(data, options);
                         }
                         google.charts.setOnLoadCallback(drawChart);
                      </script>
                ");
                Day.Text = build.ToString();
            }
        }
    }
} 