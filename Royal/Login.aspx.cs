using System;
using System.Configuration;

public partial class Sova_Login : System.Web.UI.Page
{
    String LoginUser = ConfigurationManager.AppSettings["LoginUser"].ToString();
    String HomePage = ConfigurationManager.AppSettings["HomePage"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        // 設定 Session Timeout 時間(單位：分鐘)
        this.Session.Timeout = 60;

        //
        if (Session["isLogin"] != null && Session["isLogin"].Equals(true))
        {
            Response.Redirect(HomePage);
        }

        this.txtPassword.Focus();

        if (!string.IsNullOrEmpty(Request.QueryString["err"]))
        {
            if (Request.QueryString["err"].ToString() == "1")
            {
                Response.Write("<script>alert('請檢查驗證碼設定!!');location.href='Login.aspx';</script>");
                Response.End();
            }
            else if (Request.QueryString["err"].ToString() == "2")
            {
                Response.Write("<script>alert('驗證碼有誤!!');location.href='Login.aspx';</script>");
                Response.End();
            }
            if (Request.QueryString["err"].ToString() == "3")
            {
                Response.Write("<script>alert('密碼有誤!!');location.href='Login.aspx';</script>");
                Response.End();
            }
        }
    }

    private string CreatdCheckCode(string strAccount)
    {
        long duration = 30;

        string key = strAccount;

        GoogleAuthenticator authenticator = new GoogleAuthenticator(duration, key);
        
        var code = authenticator.GenerateCode();

        return code;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string CreateAccount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CheckAccount"]) ? "" : ConfigurationManager.AppSettings["CheckAccount"].ToString();

        String password = txtPassword.Text.Trim();

        String chkcode = string.Empty;

        String txtchkcode = string.Empty;
                if (CreateAccount == ""){

            Response.Redirect(string.Format("{0}?err=1", HomePage));

        }else {

            chkcode = CreatdCheckCode(CreateAccount);
        }

        if (!string.IsNullOrEmpty(txtCkeCode.Value))
        {
            if (txtCkeCode.Value.ToString() != "")
            {
                txtchkcode = txtCkeCode.Value.ToString();

                chkcode = CreatdCheckCode(CreateAccount);

                if (!txtchkcode.Equals(chkcode))
                {
                    Response.Redirect(string.Format("{0}?err=2","Login.aspx"));
                }

            }else{
                Response.Redirect(string.Format("{0}?err=2", "Login.aspx"));
            }

        }else {
            Response.Redirect(string.Format("{0}?err=2","Login.aspx"));
        }

        if (!password.Equals(String.Empty))
        {
            bsSQL ssql = new bsSQL();

            String _UserPass = ssql.GetSytemParameter(LoginUser);

            if (_UserPass.Equals(password))
            {
                Session["isLogin"] = true;
                Response.Redirect(HomePage);
            }else{
                Response.Redirect(string.Format("{0}?err=3","Login.aspx"));
            }
        }
    }
}