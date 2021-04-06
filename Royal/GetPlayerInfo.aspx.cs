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

            // 記錄會員代碼
            hdnClub_id.Value = dt.Rows[0]["Club_id"].ToStr();
            hdnSession.Value = dt.Rows[0]["SessionNo"].ToStr();

            // TODO：尚有問題，暫不開放
            //// 恢復會員為遊戲中按鈕，處理電子遊戲 Room Room 又有開分的問題
            //if (dt.Rows[0]["Login_Game_Id"].ToStr() == "Room" && dt.Rows[0]["Login_Server_Id"].ToStr() == "Room" && dt.Rows[0]["IsEGame"].ToStr() == "是" && dt.Rows[0]["LockStatus"].ToStr() == "是")
            //{
            //    btnRecoveryLogin.Visible = true;
            //}
            //else
            //{
            //    btnRecoveryLogin.Visible = false;
            //}

            //// 強制解鎖按鈕
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
    /// 恢復會員為遊戲中，對方需能踢線
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRecoveryLogin_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('沒有會員資料');", true);
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
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('已將會員恢復為遊戲中');", true);
            this.DataSearch();
        }
    }

    /// <summary>
    /// 強制解鎖會員，對方需無帳
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPowerReturnAccount_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('沒有會員資料');", true);
        }
        else if (hdnSession.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('沒有開分資料');", true);
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

            // 強制解鎖按鈕
            btnPowerReturnAccount.Visible = false;

            //
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('已強制解鎖會員');", true);
            this.DataSearch();
        }
    }
}