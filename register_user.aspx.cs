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
            
            if (cookie == null || access == null || access.Value.Equals("4")) {
                Response.Redirect("index.aspx");
            	return;
            }
            
            string id = cookie.Value;
			int hierarchy = 4;
			int comp = 0;
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.idAcesso, tbUsuario.idAfiliacao, tbAcesso.hierarquia, tbAfiliacao.nome FROM tbUsuario, tbAcesso, tbAfiliacao WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id and tbAfiliacao.id=tbUsuario.idAfiliacao",cn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader sdr = cmd.ExecuteReader();  
                    if(sdr.Read()){
                        string nome = sdr.GetValue(sdr.GetOrdinal("nome")).ToString() + " " + sdr.GetValue(sdr.GetOrdinal("sobrenome")).ToString();
                        UserName.Text = nome;
                        UserAccess.Text = sdr.GetValue(sdr.GetOrdinal("hierarquia")).ToString();
                        Empresa.Text = sdr.GetValue(5).ToString();
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
						hierarchy = sdr.GetInt32(sdr.GetOrdinal("idAcesso"));
						comp = sdr.GetInt32(sdr.GetOrdinal("idAfiliacao"));
                    }
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                
                try {
                    cn.Open();
                    SqlCommand cmd;
					if(hierarchy.Equals(1)){
                    	cmd = new SqlCommand("SELECT * FROM tbAfiliacao",cn);
					} else {
						cmd = new SqlCommand("SELECT * FROM tbAfiliacao WHERE tbAfiliacao.id = @afiliacao",cn);
						cmd.Parameters.AddWithValue("@afiliacao", comp);
					}
                    Companies.DataSource = cmd.ExecuteReader();
                    Companies.DataTextField = "nome";
                    Companies.DataValueField = "id";
                    Companies.DataBind();
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                try {
                    cn.Open();
                    SqlCommand cmd;
					if(!hierarchy.Equals(1)){
	                    cmd = new SqlCommand("SELECT * FROM tbAcesso where id>@hierarquia order by id desc",cn);
	                    cmd.Parameters.AddWithValue("@hierarquia", hierarchy);
					} else {
						cmd = new SqlCommand("SELECT * FROM tbAcesso order by id desc",cn);
					}
                    AccessLevel.DataSource = cmd.ExecuteReader();
                    AccessLevel.DataTextField = "hierarquia";
                    AccessLevel.DataValueField = "id";
                    AccessLevel.DataBind();
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
            }
            if(hierarchy.Equals(1)){
                Companies.Items.Insert(0, new ListItem("Selecione uma empresa", "0"));
            }
            if (AccessLevel.SelectedIndex == -1){   
                AccessLevel.SelectedIndex = 0;
            }
        }
        
        protected void Cadastrar(object sender, EventArgs e) {
            string nomeUsuario = Login.Text.Trim();
			if (nomeUsuario.Length == 0) {
				lblMsg.Text = "Nome de usuário inválido!";
				return;
			}

			string nome = FirstName.Text.Trim();
			if (nome.Length == 0) {
				lblMsg.Text = "Nome inválido!";
				return;
			}

			string sobrenome = LastName.Text.Trim();
			if (sobrenome.Length == 0) {
				lblMsg.Text = "Sobrenome inválido!";
				return;
			}
            
            string senha = Pass.Text;
            if (senha.Length == 0) {
				lblMsg.Text = "Senha inválida!";
				return;
			}
            string cSenha = ConfirmPass.Text;
            if (cSenha.Length == 0) {
				lblMsg.Text = "Confirmação de Senha inválida!";
				return;
			}
            
            if (!senha.Equals(cSenha)) {
				lblMsg.Text = "Senha e Confirmação diferentes!";
				return;
			}

			string email = Email.Text.Trim();
			int arroba, arroba2, ponto;

			arroba = email.IndexOf('@');
			arroba2 = email.LastIndexOf('@');
			ponto = email.LastIndexOf('.');

			if (arroba <= 0 || ponto <= (arroba + 1) || ponto == (email.Length - 1) || arroba2 != arroba) {
				lblMsg.Text = "E-mail inválido!";
				return;
			}

			//Vamos forçar o uso das regras de data utilizadas no Brasil
			string company = Request.Form[Companies.UniqueID];
			if (company.Equals("0")) {
				lblMsg.Text = "Selecione uma empresa";
				return;
			}

			string level = Request.Form[AccessLevel.UniqueID];
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT nomeUsuario FROM tbUsuario, tbAfiliacao WHERE tbUsuario.nomeUsuario = @nomeUsuario and tbAfiliacao.id = @nome",cn);
                    cmd.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                    cmd.Parameters.AddWithValue("@nome", company);
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbUsuario(nomeUsuario, nome, sobrenome, senha, email, idAfiliacao, idAcesso) values (@nomeUsuario, @nome, @sobrenome, @senha, @email, @idAfiliacao, @idAcesso)",cn);
                    cmd.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@sobrenome", sobrenome);
                    cmd.Parameters.AddWithValue("@senha", senha);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@idAfiliacao", company);
                    cmd.Parameters.AddWithValue("@idAcesso", level);
            		cmd.ExecuteNonQuery();
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                Login.Text = "";
    			FirstName.Text = "";
    			LastName.Text = "";
    			Pass.Text = "";
    			ConfirmPass.Text = "";
    			Email.Text = "";
    			Pass.Text = "";
    			ConfirmPass.Text = "";
                AccessLevel.SelectedIndex = 0; 
    			lblMsg.Text = "Pessoa cadastrada com sucesso!";
            }
        }
    }
    
} 