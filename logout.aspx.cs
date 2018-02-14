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
			HttpCookie cookie = new HttpCookie("Login");
			cookie.Expires = DateTime.Now.AddYears(-1);
			Response.Cookies.Add(cookie);
			Response.Redirect("index.aspx");
		}
        
    }
    
} 