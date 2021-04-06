using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;

public partial class GetLockedPlayer : BasePage
{
    private void DataSearch()
    {
        bsSQL sql = new bsSQL();
        DataTable dt = sql.SovaGetPlayerInfo(txtAccount.Text.Trim());
        if (dt.Rows.Count >= 1)
        {
            dv.DataSource = dt;
            dv.DataBind();

            // �O���|���N�X
            hdnClub_id.Value = dt.Rows[0]["Club_id"].ToStr();
            hdnSession.Value = dt.Rows[0]["SessionNo"].ToStr();

            // TODO�G�|�����D�A�Ȥ��}��
            //// ��_�|�����C�������s�A�B�z�q�l�C�� Room Room �S���}�������D
            //if (dt.Rows[0]["Login_Game_Id"].ToStr() == "Room" && dt.Rows[0]["Login_Server_Id"].ToStr() == "Room" && dt.Rows[0]["IsEGame"].ToStr() == "�O" && dt.Rows[0]["LockStatus"].ToStr() == "�O")
            //{
            //    btnRecoveryLogin.Visible = true;
            //}
            //else
            //{
            //    btnRecoveryLogin.Visible = false;
            //}

            //// �j�������s
            //btnPowerReturnAccount.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.DataSearch();
    }

    /// <summary>
    /// ��_�|�����C�����A���ݯ��u
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRecoveryLogin_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�S���|�����');", true);
        }
        else
        {
            dbSQL sql = new dbSQL();
            SqlCommand cmd = new SqlCommand("dbo.C_RecoveryLoginForRoomRoom");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("CAC80563-9052-41C5-9616-0D16700AE919");
            cmd.Parameters.Add("@Club_id", SqlDbType.NChar, 20).Value = hdnClub_id.Value;
            var rows = sql.RunSQL(cmd);

            //
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�w�N�|����_���C����');", true);
            this.DataSearch();
        }
    }

    /// <summary>
    /// �j�����|���A���ݵL�b
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPowerReturnAccount_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�S���|�����');", true);
        }
        else if (hdnSession.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�S���}�����');", true);
        }
        else
        {
            dbSQL sql = new dbSQL();
            SqlCommand cmd = new SqlCommand("dbo.C_PowerReturnAccount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("F58B47EF-B7ED-486E-A06A-DE99EA3E91C5");
            cmd.Parameters.Add("@Club_id", SqlDbType.NChar, 20).Value = hdnClub_id.Value;
            cmd.Parameters.Add("@SessionNo", SqlDbType.NChar, 50).Value = hdnSession.Value;
            var rows = sql.RunSQL(cmd);

            // �j�������s
            btnPowerReturnAccount.Visible = false;

            //
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�w�j�����|��');", true);
            this.DataSearch();
        }
    }
}