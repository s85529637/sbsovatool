using System;
using System.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["isLogin"] != null && Session["isLogin"].Equals(true))
            {
                plToolBar.Visible = true;
            }
            else
            {
                plToolBar.Visible = false;
                if (!Request.Path.Contains("Login.aspx"))
                {
                    Response.Redirect("Login.aspx");
                }                
            }
        }
    }
}

