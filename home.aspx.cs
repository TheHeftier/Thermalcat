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
        protected void Page_Load(object sender, EventArgs e) {
        
            HttpCookie cookie = Request.Cookies["Login"];
                
            if (cookie == null) {
                Response.Redirect("index.aspx");
            	return;
            }
            
            string id = cookie.Value;
            
            List<SensorTemperatura> sensores = new List<SensorTemperatura>();
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                cn.Open();
                using(SqlCommand cmd = new SqlCommand(@"
                    SELECT tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.idAcesso, tbAcesso.hierarquia, tbAfiliacao.nome 
                    FROM tbUsuario, tbAcesso, tbAfiliacao 
                    WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id and tbAfiliacao.id=tbUsuario.idAfiliacao",cn)){
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader sdr = cmd.ExecuteReader()){
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
                
                using(SqlCommand cmd = new SqlCommand(@"
                        SELECT tbArduino.identificador, tbTemperatura.temperatura
                        FROM tbArduino CROSS APPLY
                             (SELECT TOP(1) tbTemperatura.temperatura
                              FROM tbTemperatura
                              WHERE tbTemperatura.idArduino = tbArduino.id
                              ORDER BY tbTemperatura.dataHora DESC
                             ) tbTemperatura, tbUsuario, tbAfiliacao
                        WHERE tbUsuario.id=@id and tbAfiliacao.id=tbUsuario.idAfiliacao and tbArduino.idAfiliacao=tbAfiliacao.id
                    ",cn)){
                    
                    cmd.Parameters.AddWithValue("@id", id);
                    
    				using (SqlDataReader reader = cmd.ExecuteReader()) {
    					// Obtém os registros, um por vez
    					while (reader.Read() == true) {
    						SensorTemperatura s = new SensorTemperatura();
    						s.NomeSensor = reader.GetValue(0).ToString();
    						s.Temperatura = "0°";
    
    						sensores.Add(s);
    					}
    				}
                } 
            }

			listRepeater.DataSource = sensores;
			listRepeater.DataBind();
        }
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            
            HttpCookie cookie = Request.Cookies["Login"];

            if (cookie == null)
            {
                Response.Redirect("index.aspx");
                return;
            }

            string id = cookie.Value;
            
            List<SensorTemperatura> sensores = new List<SensorTemperatura>();

			// Cria e abre a conexão com o banco de dados
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())) {
				conn.Open();
				// Cria um comando para selecionar registros da tabela
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT tbArduino.identificador, tbTemperatura.temperatura
                    FROM tbArduino CROSS APPLY
                         (SELECT TOP(1) tbTemperatura.temperatura
                          FROM tbTemperatura
                          WHERE tbTemperatura.idArduino = tbArduino.id
                          ORDER BY tbTemperatura.dataHora DESC
                         ) tbTemperatura, tbUsuario, tbAfiliacao
                    WHERE tbUsuario.id=@id and tbAfiliacao.id=tbUsuario.idAfiliacao and tbArduino.idAfiliacao=tbAfiliacao.id
                ", conn)) {
                    cmd.Parameters.AddWithValue("@id", id);
    				using (SqlDataReader reader = cmd.ExecuteReader()) {
    					// Obtém os registros, um por vez
    					while (reader.Read() == true) {
    						SensorTemperatura s = new SensorTemperatura();
    						s.NomeSensor = reader.GetValue(0).ToString();
    						s.Temperatura = reader.GetValue(1).ToString()+"°";
    
    						sensores.Add(s);
    					}
    				}
                }
			}

			listRepeater.DataSource = sensores;
			listRepeater.DataBind();
        }
    }
	public class SensorTemperatura {
		public string NomeSensor { get; set; }
		public string Temperatura { get; set; }
	}
    
} 