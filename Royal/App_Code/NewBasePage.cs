using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// NewBasePage 的摘要描述
/// </summary>
public class NewBasePage : System.Web.UI.Page
{
    public NewBasePage(){}

    protected override void OnPreLoad(System.EventArgs e)
    {
        base.OnPreLoad(e);
        if (Session["isLogin"] == null)
        {
            Response.Redirect("Logout.aspx");
        }
    }

    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);
        if (Session["isLogin"]==null)
        {
            Response.Redirect("Logout.aspx");
        } 
    }

    protected override void OnPreRender(System.EventArgs e)
    {
        base.OnPreRender(e);
        if (Session["isLogin"] == null)
        {
            Response.Redirect("Logout.aspx");
        }
    }
}