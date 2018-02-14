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
            HttpCookie login = Request.Cookies["Login"];
            if(login != null && login.Value != ""){
                Response.Redirect("home.aspx");
            }
        }
        
        protected void ClickLogin(object sender, EventArgs e) {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbThermalCat"].ToString())){
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT * FROM tbUsuario 
                    WHERE nomeUsuario=@nomeUsuario and senha=@senha",cn)){
                    cmd.Parameters.AddWithValue("@nomeUsuario", User.Text);
                    cmd.Parameters.AddWithValue("@senha", Password.Text);
                    using (SqlDataReader sdr = cmd.ExecuteReader()){
                        if(sdr.Read()){
                            HttpCookie cookie = new HttpCookie("Login", sdr.GetValue(sdr.GetOrdinal("id")).ToString());
                            cookie.Expires = DateTime.Now.AddYears(1);
                            HttpCookie access = new HttpCookie("Access", sdr.GetValue(sdr.GetOrdinal("idAcesso")).ToString());
                            access.Expires = DateTime.Now.AddYears(1);
                            Response.Cookies.Add(cookie);
                            Response.Cookies.Add(access);
                            Response.Redirect("home.aspx");
                        } else {
                            WrongPanel.Style.Add("display", "block");
                        }
                    }
                } 
            }
        }
    } 
    
} 