using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Club_Stake_Current_Insert_ForStar : NewBasePage
{
    private string sReturnStateDescription = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            this.tabAdd.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        long? id = tbx_Id.Text.ToInt64();
        string club_id = tbxClub_Id.Text.Trim();
        decimal? startSeqNoFlag = tbxStartSeqNoFlag.Text.ToDecimal();
        string sReturnStateDescription = "";
        var ds = new dbSQL().A_ms_InsertStakeData_ForStar(id, club_id, startSeqNoFlag, out sReturnStateDescription);
        this.gvResultBefore.DataSource = ds.Tables[0];
        this.gvResultBefore.DataBind();
        if (ds.Tables.Count > 1)
        {
            this.gvResultAfter.DataSource = ds.Tables[1];
            this.gvResultAfter.DataBind();

            //
            if (ds.Tables[1].Rows.Count > 0)
            {
                this.tbx_Id.Text = ds.Tables[1].Rows[0]["Id"].ToStr();
                this.tbxClub_Id.Text = ds.Tables[1].Rows[0]["會員代碼"].ToStr();
                this.tbxStartSeqNoFlag.Text = ds.Tables[1].Rows[0]["開分號"].ToStr();
            }
        }
        this.tabAdd.Visible = false;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.id.Text = this.tbx_Id.Text;
        this.Club_id.Text = this.tbxClub_Id.Text;
        this.StartSeqNoFlag.Text = this.tbxStartSeqNoFlag.Text;
        this.tabAdd.Visible = true;
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        // 判斷帳號密碼，帳號密碼設定在 Web.config
        string opAccount = "";
        string opPassword = "";
        var keys = ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("Star_OPAccount_"));
        foreach (var key in keys)
        {
            if (ConfigurationManager.AppSettings[key] == this.tbxPwd.Text)
            {
                opAccount = key;
                opPassword = ConfigurationManager.AppSettings[key];
            }
        }
        if (opAccount == string.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('密碼錯誤，請重新輸入');", true);
            return;
        }

        //
        //long? id = this.id.Text.ToInt64();
        string club_id = this.Club_id.Text.Trim();
        decimal? startSeqNoFlag = this.StartSeqNoFlag.Text.ToDecimal();
        string game_id = this.Game_id.Text.Trim();
        string desk = this.Desk.Text.Trim();
        decimal? stake_Score = this.Stake_Score.Text.ToDecimal();
        decimal? youXiaoYaFen = this.YouXiaoYaFen.Text.ToDecimal();
        decimal? account_Score = this.Account_Score.Text.ToDecimal();
        int? rows = this.Rows.Text.ToInt32();

        //// 輸入資料檢核
        //if (id.HasValue == false)
        //{
        //    this.ChangeTextColorToBlack();
        //    this.Label9.ForeColor = Color.Red;
        //    return;
        //}
        if (club_id.IsBlank())
        {
            this.ChangeTextColorToBlack();
            this.Label1.ForeColor = Color.Red;
            return;
        }
        if (startSeqNoFlag.HasValue == false)
        {
            this.ChangeTextColorToBlack();
            this.Label2.ForeColor = Color.Red;
            return;
        }
        if (game_id.IsBlank())
        {
            this.ChangeTextColorToBlack();
            this.Label3.ForeColor = Color.Red;
            return;
        }
        if (desk.IsBlank())
        {
            this.ChangeTextColorToBlack();
            this.Label4.ForeColor = Color.Red;
            return;
        }
        if (stake_Score.HasValue == false)
        {
            this.ChangeTextColorToBlack();
            this.Label5.ForeColor = Color.Red;
            return;
        }
        if (youXiaoYaFen.HasValue == false)
        {
            this.ChangeTextColorToBlack();
            this.Label6.ForeColor = Color.Red;
            return;
        }
        if (account_Score.HasValue == false)
        {
            this.ChangeTextColorToBlack();
            this.Label7.ForeColor = Color.Red;
            return;
        }
        if (rows.HasValue == false)
        {
            this.ChangeTextColorToBlack();
            this.Label8.ForeColor = Color.Red;
            return;
        }

        //
        string sReturnStateDescription = "";
        var ds = new dbSQL().A_ms_InsertStakeData_ForStar(club_id, startSeqNoFlag, game_id, desk, stake_Score, youXiaoYaFen, account_Score, rows, out sReturnStateDescription);
        if (sReturnStateDescription.IsNotBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + sReturnStateDescription + "');", true);
            return;
        }

        //
        this.gvResultBefore.DataSource = ds.Tables[0];
        this.gvResultBefore.DataBind();
        if (ds.Tables.Count > 1)
        {
            this.gvResultAfter.DataSource = ds.Tables[1];
            this.gvResultAfter.DataBind();
        }

        //
        this.tabAdd.Visible = false;
        this.ChangeTextColorToBlack();

        // 記錄 Log
        Dictionary<string, object> log = new Dictionary<string, object>();
        log.Add("PageName", this.GetType().FullName);
        log.Add("DateTime", System.DateTime.UtcNow.AddHours(8).ToDateTimeMillisecondString());
        log.Add("IPAddress", Request.UserHostAddress);
        log.Add("OPAccount", opAccount);
        log.Add("id", this.id.Text);
        log.Add("Club_id", Club_id.Text);
        log.Add("StartSeqNoFlag", StartSeqNoFlag.Text);
        log.Add("Game_id", Game_id.Text);
        log.Add("Desk", Desk.Text);
        log.Add("Stake_Score", Stake_Score.Text);
        log.Add("YouXiaoYaFen", YouXiaoYaFen.Text);
        log.Add("Account_Score", Account_Score.Text);
        log.Add("Rows", Rows.Text);
        string json = JsonConvert.SerializeObject(log);
        if (System.IO.Directory.Exists(Server.MapPath("~/LogFiles")) == false)
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~/LogFiles"));
        }
        System.IO.File.AppendAllText(Server.MapPath("~/LogFiles/StarChangeLog_" + System.DateTime.UtcNow.AddHours(8).ToDateString() + ".txt"), json + System.Environment.NewLine, System.Text.Encoding.Default);

        //
        this.id.Text = "";
        this.Club_id.Text = "";
        this.StartSeqNoFlag.Text = "";
        this.Game_id.Text = "";
        this.Desk.Text = "";
        this.Stake_Score.Text = "";
        this.YouXiaoYaFen.Text = "";
        this.Account_Score.Text = "";
        this.Rows.Text = "";

        //
        ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('新增完成');", true);
    }

    private void ChangeTextColorToBlack()
    {
        this.Label1.ForeColor = Color.Black;
        this.Label2.ForeColor = Color.Black;
        this.Label3.ForeColor = Color.Black;
        this.Label4.ForeColor = Color.Black;
        this.Label5.ForeColor = Color.Black;
        this.Label6.ForeColor = Color.Black;
        this.Label7.ForeColor = Color.Black;
        this.Label8.ForeColor = Color.Black;
        this.Label9.ForeColor = Color.Black;
    }
}