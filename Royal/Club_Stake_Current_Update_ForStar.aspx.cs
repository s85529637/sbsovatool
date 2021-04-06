using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Club_Stake_Current_Update_ForStar : NewBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            this.tabModify.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        long? id = tbx_Id.Text.ToInt64();
        string club_id = tbxClub_Id.Text.Trim();
        decimal? startSeqNoFlag = tbxStartSeqNoFlag.Text.ToDecimal();
        DataTable dtb = new dbSQL().A_ms_UpdateStakeData_ForStar(id, club_id, startSeqNoFlag).Tables[0];

        //
        if (dtb.Rows.Count > 0)
        {
            this.tbx_Id.Text = dtb.Rows[0]["Id"].ToStr();
            this.tbxClub_Id.Text = dtb.Rows[0]["會員ID"].ToStr();
            this.tbxStartSeqNoFlag.Text = dtb.Rows[0]["開分號"].ToStr();
        }

        //
        this.gvResultBefore.DataSource = dtb.DefaultView;
        this.gvResultBefore.DataBind();
        this.gvResultAfter.Visible = false;
        this.tabModify.Visible = false;
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        this.id.Text = this.tbx_Id.Text;
        this.Club_id.Text = this.tbxClub_Id.Text;
        this.StartSeqNoFlag.Text = this.tbxStartSeqNoFlag.Text;
        this.tabModify.Visible = true;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
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
        long? id = this.id.Text.Trim().ToInt64();
        string club_id = this.Club_id.Text.Trim();
        decimal? startSeqNoFlag = this.StartSeqNoFlag.Text.ToDecimal();
        decimal? stake_Score = this.Stake_Score.Text.ToDecimal();
        decimal? youXiaoYaFen = this.YouXiaoYaFen.Text.ToDecimal();
        decimal? account_Score = this.Account_Score.Text.ToDecimal();
        decimal? shengDian = this.ShengDian.Text.ToDecimal();
        int? rows = this.Rows.Text.ToInt32();

        //
        var ds = new dbSQL().A_ms_UpdateStakeData_ForStar(id, club_id, startSeqNoFlag, stake_Score, youXiaoYaFen, account_Score, shengDian, rows);

        //
        this.gvResultBefore.DataSource = ds.Tables[0];
        this.gvResultBefore.DataBind();

        //
        if (ds.Tables.Count > 1)
        {
            this.gvResultAfter.DataSource = ds.Tables[1];
            this.gvResultAfter.DataBind();
        }

        //
        this.tabModify.Visible = false;
        this.gvResultAfter.Visible = true;

        // 記錄 Log
        Dictionary<string, object> log = new Dictionary<string, object>();
        log.Add("PageName", this.GetType().FullName);
        log.Add("DateTime", System.DateTime.UtcNow.AddHours(8).ToDateTimeMillisecondString());
        log.Add("IPAddress", Request.UserHostAddress);
        log.Add("OPAccount", opAccount);
        log.Add("id", this.id.Text);
        log.Add("Club_id", Club_id.Text);
        log.Add("StartSeqNoFlag", StartSeqNoFlag.Text);
        log.Add("Stake_Score", Stake_Score.Text);
        log.Add("YouXiaoYaFen", YouXiaoYaFen.Text);
        log.Add("Account_Score", Account_Score.Text);
        log.Add("ShengDian", ShengDian.Text);
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
        this.Stake_Score.Text = "";
        this.YouXiaoYaFen.Text = "";
        this.Account_Score.Text = "";
        this.ShengDian.Text = "";
        this.Rows.Text = "";

        //
        ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('修改完成');", true);
    }
}