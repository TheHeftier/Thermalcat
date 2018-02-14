using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
			
			if (IsPostBack == false) {
				int id;
				if (int.TryParse(Request.QueryString["id"], out id) == false) {
					lblMsg.Text = "Id inválido!";
					return;
				}

				// Cria e abre a conexão com o banco de dados
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())) {

					// Cria um comando para selecionar registros da tabela
					using (SqlCommand cmd = new SqlCommand("SELECT nome, logradouro, numero, bairro, cidade, estado, cep FROM tbAfiliacao WHERE id = @id", conn)) {
						cmd.Parameters.AddWithValue("@id", id);
						conn.Open();
						using (SqlDataReader reader = cmd.ExecuteReader()) {
							// Tenta obter o registro
							if (reader.Read() == true) {
								CompanyName.Text = reader.GetString(0);
								Rua.Text = reader.GetString(1);
								Numero.Text = reader.GetInt32(2).ToString();
								Bairro.Text = reader.GetString(3);
								Cidade.Text = reader.GetString(4);
								Estados.SelectedValue = reader.GetString(5);
								Cep.Text = reader.GetString(6).ToString().Substring(0, 5);
								Digito.Text = reader.GetString(6).ToString().Substring(5, 3);
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
            
			// Cria e abre a conexão com o banco de dados
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())) {
				cn.Open();
				// Cria um comando para atualizar um registro da tabela
				SqlCommand cmd = new SqlCommand("UPDATE tbAfiliacao SET nome = @nome, logradouro = @rua, numero = @numero, bairro = @bairro, cidade = @cidade, estado = @estado, cep = @cep WHERE Id = @id",cn);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@rua", rua);
                cmd.Parameters.AddWithValue("@numero", numero);
                cmd.Parameters.AddWithValue("@bairro", bairro);
                cmd.Parameters.AddWithValue("@cidade", cidade);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@cep", cep);

				cmd.Parameters.AddWithValue("@id", id);

				cmd.ExecuteNonQuery();
			}
			Response.Redirect("list_company.aspx");
		}

		protected void btnVoltar_Click(object sender, EventArgs e) {
			Response.Redirect("list_company.aspx");
		}
	}
}
