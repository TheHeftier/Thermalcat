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
            
            string id = cookie.Value;
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.idAcesso, tbAcesso.hierarquia, tbAfiliacao.nome FROM tbUsuario, tbAcesso, tbAfiliacao WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id and tbAfiliacao.id=tbUsuario.idAfiliacao",cn);
                    cmd.Parameters.AddWithValue("@id", id);
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
				List<Empresa> empresas = new List<Empresa>();

				// Cria e abre a conexão com o banco de dados
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())) {
					conn.Open();
					// Cria um comando para selecionar registros da tabela
					using (SqlCommand cmd = new SqlCommand("SELECT id, nome, logradouro, numero, bairro, cidade, estado, cep FROM tbAfiliacao ORDER BY tbAfiliacao.nome", conn)) {
						
						using (SqlDataReader reader = cmd.ExecuteReader()) {
							// Obtém os registros, um por vez
							while (reader.Read() == true) {
								Empresa p = new Empresa();
								p.Id = reader.GetInt32(0);
								p.Nome = reader.GetString(1);
								p.Rua = reader.GetString(2);
								p.Numero = reader.GetInt32(3);
								p.Bairro = reader.GetString(4);
								p.Cidade = reader.GetString(5);
								p.Estado = reader.GetString(6);
								p.Cep = reader.GetString(7);

								empresas.Add(p);
							}
						}
					}
				}

				listRepeater.DataSource = empresas;
				listRepeater.DataBind();
			}
        }   
    }
	public class Empresa {
		public int Id { get; set; }
		public string Nome { get; set; }
		public string Rua { get; set; }
		public int Numero { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string Estado { get; set; }
		public string Cep { get; set; }
	}
}