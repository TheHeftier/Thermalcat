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
 
namespace ThermalCat { 
    
    public partial class Default : System.Web.UI.Page { 
        
        protected void Page_Load(object sender, EventArgs e) {
            HttpCookie cookie = Request.Cookies["Login"];
            
            if (cookie == null) {
                Response.Redirect("index.aspx");
            	return;
            }
            
            string id = cookie.Value;
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                try {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tbUsuario, tbAcesso WHERE tbUsuario.id=@id and tbUsuario.idAcesso=tbAcesso.id",cn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader sdr = cmd.ExecuteReader();  
                    if(sdr.Read()){  
                        UserName.Text = sdr.GetValue(sdr.GetOrdinal("nome")).ToString() + sdr.GetValue(sdr.GetOrdinal("sobrenome")).ToString();
                        if(sdr.GetValue(0).ToString() != "4"){
                            UserAccess.Text = sdr.GetValue(sdr.GetOrdinal("hierarquia")).ToString();
                        }
                    } 
                } catch (Exception ex) {
                    Response.Write(ex.Message); 
                } finally {
                    cn.Close();
                };
            }
        }
        
    }
    
} 