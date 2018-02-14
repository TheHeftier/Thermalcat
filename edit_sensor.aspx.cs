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
            HttpCookie access = Request.Cookies["Access"];
            
            if (cookie == null || access == null || !access.Value.Equals("1")) {
                Response.Redirect("index.aspx");
            	return;
            }
            
            string login = cookie.Value;
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.idAcesso, tbAcesso.hierarquia, tbAfiliacao.nome FROM tbUsuario, tbAcesso, tbAfiliacao WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id and tbAfiliacao.id=tbUsuario.idAfiliacao",cn);
                    cmd.Parameters.AddWithValue("@id", login);
                    SqlDataReader sdr = cmd.ExecuteReader();  
                    if(sdr.Read()){
                        string nome = sdr.GetValue(sdr.GetOrdinal("nome")).ToString() + " " + sdr.GetValue(sdr.GetOrdinal("sobrenome")).ToString();
                        UserName.Text = nome;
                        UserAccess.Text = sdr.GetValue(sdr.GetOrdinal("hierarquia")).ToString();
                        Empresa.Text = sdr.GetValue(4).ToString();
                        StringBuilder reg = new StringBuilder(); 
                        reg.Append(@"
                            <i id='UserForm' class='fa fa-user'> | Usuário</i>
                        ");
						if(!sdr.GetValue(sdr.GetOrdinal("idAcesso")).ToString().Equals("4")){  
                            StringBuilder strScript = new StringBuilder(); 
                            strScript.Append(@"
                                <button id='AddUser' class='fa fa-plus-circle' data-toggle='tooltip' data-placement='bottom' title='Cadastrar'></button>
                                <button id='EditUser' class='fa fa-pencil' data-toggle='tooltip' data-placement='bottom' title='Editar'></button>
                                <button id='DeleteUser' class='fa fa-trash' data-toggle='tooltip' data-placement='bottom' title='Deletar'></button>
                            ");
                            ltScripts.Text = strScript.ToString();
                        }
                        if(sdr.GetValue(sdr.GetOrdinal("idAcesso")).ToString().Equals("1")){
                            reg.Append(@"
        						<i id='CompanyForm' class='fa fa-building'> | Empresa</i>
        						<i id='SensorForm' class='fa fa-thermometer'> | Sensor </i>
                            ");
                        }
                        RegMenus.Text = reg.ToString();
                    }
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                
                try {
                    cn.Open();
                    SqlCommand cmd;
                	cmd = new SqlCommand("SELECT * FROM tbAfiliacao",cn);
                    Companies.DataSource = cmd.ExecuteReader();
                    Companies.DataTextField = "nome";
                    Companies.DataValueField = "id";
                    Companies.DataBind();
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                Companies.Items.Insert(0, new ListItem("Selecione uma empresa", "0"));
            }
            
            if (IsPostBack == false) {
				int id;
				if (int.TryParse(Request.QueryString["id"], out id) == false) {
					lblMsg.Text = "Id inválido!";
					return;
				}

				// Cria e abre a conexão com o banco de dados
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())) {

					// Cria um comando para selecionar registros da tabela
					using (SqlCommand cmd = new SqlCommand("SELECT identificador, idAfiliacao FROM tbArduino WHERE id = @id", conn)) {
						cmd.Parameters.AddWithValue("@id", id);
						conn.Open();
						using (SqlDataReader reader = cmd.ExecuteReader()) {
							// Tenta obter o registro
							if (reader.Read() == true) {
								Sensor.Text = reader.GetString(0);
								Companies.SelectedValue = reader.GetInt32(1).ToString();
							} else {
								lblMsg.Text = "Id não encontrado!";
							}
						}
					}
				}
			}
        }
        
        protected void btnSalvar_Click(object sender, EventArgs e) {
			int id;
			if (int.TryParse(Request.QueryString["id"], out id) == false) {
				lblMsg.Text = "Id inválido!";
				return;
			}
            string nome = Sensor.Text.Trim();
			if (nome.Length == 0) {
				lblMsg.Text = "Nome inválido!";
				return;
			}
            
			string company = Request.Form[Companies.UniqueID];
			if (company.Equals("0")) {
				lblMsg.Text = "Selecione uma empresa";
				return;
			}
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT identificador FROM tbArduino WHERE identificador = @identificador",cn);
                    cmd.Parameters.AddWithValue("@identificador", nome);
            		SqlDataReader sdr = cmd.ExecuteReader();  
                    if(sdr.Read()){  
                        lblMsg.Text = "Nome já Cadastrado!";
				        return;
                    }
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
            }
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("Update tbArduino set identificador = @identificador, idAfiliacao = @idAfiliacao WHERE id = @id",cn);
                    cmd.Parameters.AddWithValue("@identificador", nome);
                    cmd.Parameters.AddWithValue("@idAfiliacao", company);
                    cmd.Parameters.AddWithValue("@id", id);
            		cmd.ExecuteNonQuery();
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                
    			Response.Redirect("list_sensor.aspx");
            }
        }

		protected void btnVoltar_Click(object sender, EventArgs e) {
			Response.Redirect("list_sensor.aspx");
		}
    }
    
} 