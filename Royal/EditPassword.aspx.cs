using System;
using System.Configuration;
using System.Web.UI;

public partial class EditPassword : NewBasePage
{
    String LoginUser = ConfigurationManager.AppSettings["LoginUser"].ToString();
    bsSQL ssql = new bsSQL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    protected void btnSova_Click(object sender, EventArgs e)
    {
        try
        {
            String _Password = ssql.GetSytemParameter(LoginUser);

            if (_Password == null)
            {
                Lib.MsgBox(UpdatePanel1, "取不到值");
                return;
            }
            else if (txtSovaPassOrg.Text.Trim() == "" || txtSovaPassNew1.Text.Trim() == "" || txtSovaPassNew2.Text.Trim() == "")
            {
                Lib.MsgBox(UpdatePanel1, "密碼不可空白！");
                return;
            }
            else if (!txtSovaPassNew1.Text.Trim().Equals(txtSovaPassNew2.Text.Trim()))
            {
                Lib.MsgBox(UpdatePanel1, "新密碼不一樣！");
                return;
            }
            else if (!_Password.Equals(txtSovaPassOrg.Text.Trim()))
            {
                Lib.MsgBox(UpdatePanel1, "密碼錯誤");
                return;
            }

            int iSuccessCount = ssql.SetSytemParameter(LoginUser, txtSovaPassNew1.Text.Trim());
            if (iSuccessCount > 0)
            {
                Lib.MsgBox(UpdatePanel1, "修改成功");
            }
            else
            {
                Lib.MsgBox(UpdatePanel1, "修改失敗");
            }
        }
        catch (Exception ex)
        {
            Lib.MsgBox(UpdatePanel1, ex.Message);
        }
    }

    protected void btnJumbo_Click(object sender, EventArgs e)
    {
        try
        {
            String _Password = ssql.GetJBGameStartPassword();

            if (_Password == null)
            {
                Lib.MsgBox(UpdatePanel1, "取不到值");
                return;
            }
            else if (txtJumboPassOrg.Text.Trim() == "" || txtJumboPassNew1.Text.Trim() == "" || txtJumboPassNew2.Text.Trim() == "")
            {
                Lib.MsgBox(UpdatePanel1, "密碼不可空白！");
                return;
            }
            else if (!txtJumboPassNew1.Text.Trim().Equals(txtJumboPassNew2.Text.Trim()))
            {
                Lib.MsgBox(UpdatePanel1, "新密碼不一樣！");
                return;
            }
            else if (!_Password.Equals(txtJumboPassOrg.Text.Trim()))
            {
                Lib.MsgBox(UpdatePanel1, "密碼錯誤");
                return;
            }


            int iSuccessCount = ssql.SetJBGameStartPassword(txtJumboPassNew1.Text.Trim());
            if (iSuccessCount > 0)
            {
                Lib.MsgBox(UpdatePanel1, "修改成功");
            }
            else
            {
                Lib.MsgBox(UpdatePanel1, "修改失敗");
            }
        }
        catch (Exception ex)
        {
            Lib.MsgBox(UpdatePanel1, ex.Message);
        }
    }
}

