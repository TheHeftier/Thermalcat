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
            
            string login = cookie.Value;
			int hierarchy = 4;
			int comp = 0;
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.idAcesso, tbUsuario.idAfiliacao, tbAcesso.hierarquia, tbAfiliacao.nome FROM tbUsuario, tbAcesso, tbAfiliacao WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id and tbAfiliacao.id=tbUsuario.idAfiliacao",cn);
                    cmd.Parameters.AddWithValue("@id", login);
                    SqlDataReader sdr = cmd.ExecuteReader();  
                    if(sdr.Read()){
                        string nome = sdr.GetValue(sdr.GetOrdinal("nome")).ToString() + " " + sdr.GetValue(sdr.GetOrdinal("sobrenome")).ToString();
                        UserName.Text = nome;
                        UserAccess.Text = sdr.GetValue(sdr.GetOrdinal("hierarquia")).ToString();
                        Empresa.Text = sdr.GetValue(5).ToString();
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
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
            }

			if (IsPostBack == false) {
				int id;
				if (int.TryParse(Request.QueryString["id"], out id) == false) {
					divMsg.Attributes.Add("class", "alert alert-danger");
					lblMsg.Text = "Id inválido!";
					return;
				}
				string tabela = Request.QueryString["tabela"];
				if (tabela == null) {
					divMsg.Attributes.Add("class", "alert alert-danger");
					lblMsg.Text = "Tabela inválida!";
					return;
				}

				// Cria e abre a conexão com o banco de dados
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
					conn.Open();
					// Cria um comando para selecionar registros da tabela
					using (SqlCommand cmd = new SqlCommand("SELECT id FROM "+tabela+" WHERE id = @id", conn)) {
						cmd.Parameters.AddWithValue("@id", id);

						using (SqlDataReader reader = cmd.ExecuteReader()) {
							// Tenta obter o registro
							if (reader.Read() == false) {
								divMsg.Attributes.Add("class", "alert alert-danger");
								lblMsg.Text = "Id não encontrado!";
								return;
							}
						}
					}
				}
	
				// Cria e abre a conexão com o banco de dados
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
					conn.Open();
					// Cria um comando para excluir o registro
					using (SqlCommand cmd = new SqlCommand("DELETE FROM " + tabela + " WHERE Id = @id", conn)) {
						try {
							cmd.Parameters.AddWithValue("@id", id);
							if(tabela.Equals("tbArduino")){
								using (SqlCommand cm = new SqlCommand("DELETE FROM tbTemperatura WHERE idArduino = @id", conn)) {
									cm.Parameters.AddWithValue("@id", id);
									cm.ExecuteNonQuery();
								}
							}
							cmd.ExecuteNonQuery();
							
							divMsg.Attributes.Add("class", "alert alert-success");
							switch(tabela){
								case "tbAfiliacao": lblMsg.Text = "Empresa excluída com sucesso!"; break;
								case "tbUsuario": lblMsg.Text = "Usuário excluído com sucesso!"; break;
								case "tbArduino": lblMsg.Text = "Sensor excluído com sucesso!"; break;
							}
						} catch (Exception ex) {
							divMsg.Attributes.Add("class", "alert alert-danger");
		                    switch(tabela){
								case "tbAfiliacao": lblMsg.Text = "Empresa não pode ser excluída pois tem usuários atrelados a ela!"; break;
								case "tbUsuario": lblMsg.Text = "Usuário excluído com sucesso!"; break;
								case "tbArduino": lblMsg.Text = "Sensor não pode ser excluído pois tem empresas atreladas a ele!"; break;
							}
		                } finally {
		                    conn.Close();
		                }
					}
				}
			}
		}
		
		protected void btnNao_Click(object sender, EventArgs e) {
			string tabela = Request.QueryString["tabela"];
			if (tabela == null) {
				lblMsg.Text = "Tabela inválida!";
				return;
			}
			string volta = "delete_user.aspx";
			switch(tabela){
				case "tbAfiliacao": volta = "delete_company.aspx"; break;
				case "tbUsuario": volta = "delete_user.aspx"; break;
				case "tbArduino": volta = "delete_sensor.aspx"; break;
			}
			
			Response.Redirect(volta);
		}
	}
}
