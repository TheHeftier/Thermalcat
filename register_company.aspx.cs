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
            }
        }
        
        protected void Cadastrar(object sender, EventArgs e) {
            string nome = CompanyName.Text.Trim();
			if (nome.Length == 0) {
				lblMsg.Text = "Nome inválido!";
				return;
			}

			string cep = Cep.Text.Trim()+Digito.Text.Trim();
			if (cep.Length == 0 || cep.Length < 8) {
				lblMsg.Text = "CEP inválido!";
				return;
			}

			string rua = Rua.Text.Trim();
			if (rua.Length == 0) {
				lblMsg.Text = "Rua inválida!";
				return;
			}
            
            string numero = Numero.Text;
            if (numero.Length == 0) {
				lblMsg.Text = "Numero inválido!";
				return;
			}
            
            string bairro = Bairro.Text;
            if (bairro.Length == 0) {
				lblMsg.Text = "Bairro inválido!";
				return;
			}
            
            string cidade = Cidade.Text;
            if (cidade.Length == 0) {
				lblMsg.Text = "Cidade inválida!";
				return;
			}
			
			string estado = Estados.SelectedValue;
			if (estado.Equals("estado")) {
				lblMsg.Text = "Selecione uma empresa";
				return;
			}
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT nome FROM tbAfiliacao WHERE nome = @nome",cn);
                    cmd.Parameters.AddWithValue("@nome", nome);
            		SqlDataReader sdr = cmd.ExecuteReader();  
                    if(sdr.Read()){  
                        lblMsg.Text = "Empresa já Cadastrada!";
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbAfiliacao(nome, logradouro, numero, bairro, cidade, estado, cep) values (@nome, @rua, @numero, @bairro, @cidade, @estado, @cep)",cn);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@rua", rua);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.Parameters.AddWithValue("@bairro", bairro);
                    cmd.Parameters.AddWithValue("@cidade", cidade);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@cep", cep);
            		cmd.ExecuteNonQuery();
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
                CompanyName.Text = "";
    			Rua.Text = "";
    			Cep.Text = "";
    			Digito.Text = "";
    			Numero.Text = "";
    			Cidade.Text = "";
    			Bairro.Text = "";
    
    			lblMsg.Text = "Empresa cadastrada com sucesso!";
            }
        }
    }
    
} 