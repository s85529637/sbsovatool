using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Clublogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (txtdata.Text.Equals(String.Empty))
        {
            txtdata.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        }
    }
}