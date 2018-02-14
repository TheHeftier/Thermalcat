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
            }
            
            if (IsPostBack == false) {
				List<Pessoa> pessoas = new List<Pessoa>();

				// Cria e abre a conexão com o banco de dados
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())) {
					conn.Open();
					// Cria um comando para selecionar registros da tabela
                    SqlCommand cmd;
					if (hierarchy.Equals(1)){
                        cmd = new SqlCommand("SELECT tbUsuario.id, tbUsuario.nomeUsuario, tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.email, tbAfiliacao.nome FROM tbUsuario, tbAfiliacao WHERE tbAfiliacao.id = tbUsuario.idAfiliacao ORDER BY tbAfiliacao.nome, tbUsuario.nomeUsuario", conn);
                    } else {
                        cmd = new SqlCommand(@"SELECT tbUsuario.id, tbUsuario.nomeUsuario, tbUsuario.nome, tbUsuario.sobrenome, tbUsuario.email, tbAfiliacao.nome 
                                               FROM tbUsuario, tbAfiliacao WHERE tbUsuario.idAfiliacao = @afiliacao and tbAfiliacao.id = tbUsuario.idAfiliacao and tbUsuario.idAcesso>@acesso ORDER BY tbAfiliacao.nome, tbUsuario.nomeUsuario", conn);
                        cmd.Parameters.AddWithValue("@afiliacao", comp);
                        cmd.Parameters.AddWithValue("@acesso", hierarchy);
                    }
					using (SqlDataReader reader = cmd.ExecuteReader()) {
						// Obtém os registros, um por vez
						while (reader.Read() == true) {
							Pessoa p = new Pessoa();
							p.Id = reader.GetInt32(0);
							p.NomeUsuario = reader.GetString(1);
							p.Nome = reader.GetString(2);
							p.Sobrenome = reader.GetString(3);
							p.Email = reader.GetString(4);
							p.Empresa = reader.GetString(5);

							pessoas.Add(p);
						}
					}
				}

				listRepeater.DataSource = pessoas;
				listRepeater.DataBind();
			}
        }   
    }
	public class Pessoa {
		public int Id { get; set; }
		public string NomeUsuario { get; set; }
		public string Nome { get; set; }
		public string Sobrenome { get; set; }
		public string Email { get; set; }
		public string Empresa { get; set; }
	}
}