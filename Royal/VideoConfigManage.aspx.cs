using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class VideoConfigManage : NewBasePage 
{
    private static readonly string _ConnString = ConfigurationManager.AppSettings["Main.ConnectionString"].ToStr();

    /// <summary>
    /// 查詢目前設定
    /// </summary>
    /// <returns></returns>
    DataTable GetNowResult()
    {
        var conn = bs.DbPF.CreateDbConnection("", _ConnString);

        // 目前設定
        string strVideoIPConfig = bs.DbPF.GetResultScalar(conn, "SELECT Param_value FROM T_Sysparameter WITH(nolock) WHERE Param_key = 'VideoIPConfig'").ToStr();
        lblSetupDisplay.Text = strVideoIPConfig;

        //
        bs.SqlBuilder sql = new bs.SqlBuilder();
        sql.AppendLine("    SELECT s.Server_Name");
        sql.AppendLine("         , s.Server_id");
        sql.AppendLine("         , s.Video_1");
        sql.AppendLine("         , s.Video_2");
        sql.AppendLine("      FROM T_Video_Profile v WITH(nolock)");
        sql.AppendLine("INNER JOIN T_Server s WITH(nolock)");
        sql.AppendLine("        ON v.Server_id = s.Server_id");
        sql.AppendLine("       AND v.Profile_Category = ?").AddParams(strVideoIPConfig);
        sql.AppendLine("");

        //
        DataTable dtbResult = bs.DbPF.GetResultTable(conn, sql);
        dtbResult.DefaultView.Sort = "Server_Name";

        // 目前設定
        this.gvT_Server.DataSource = dtbResult.DefaultView;
        this.gvT_Server.DataBind();

        //
        return dtbResult;
    }

    // 查詢設定範本
    DataTable GetNewResult(string category)
    {
        var conn = bs.DbPF.CreateDbConnection("", _ConnString);

        //
        bs.SqlBuilder sql = new bs.SqlBuilder();
        sql.AppendLine("    SELECT v.*");
        sql.AppendLine("         , s.Server_Name");
        sql.AppendLine("      FROM T_Video_Profile v WITH(nolock)");
        sql.AppendLine("INNER JOIN T_Server s WITH(nolock)");
        sql.AppendLine("        ON v.Server_id = s.Server_id");
        sql.AppendLine("       AND v.Profile_Category = ?").AddParams(category);
        sql.AppendLine("");

        //
        DataTable dtbResult = bs.DbPF.GetResultTable(conn, sql);
        dtbResult.DefaultView.Sort = "Server_Name";

        //
        this.gvT_Video_Profile.DataSource = dtbResult.DefaultView;
        this.gvT_Video_Profile.DataBind();

        //
        return dtbResult;
    }

    //
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            this.GetNowResult();

            //
            var conn = bs.DbPF.CreateDbConnection("", _ConnString);
            DataTable dtbCategory = bs.DbPF.GetResultTable(conn, "SELECT DISTINCT Profile_Category FROM T_Video_Profile WITH(nolock)");
            dtbCategory.DefaultView.Sort = "Profile_Category";
            rblCategory.DataSource = dtbCategory.DefaultView;
            rblCategory.DataValueField = "Profile_Category";
            rblCategory.DataBind();
            rblCategory.SelectedValue = lblSetupDisplay.Text;

            //
            this.GetNewResult(lblSetupDisplay.Text);
        }
    }

    //
    protected void rblCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.gvT_Video_Profile.DataSource = this.GetNewResult(this.rblCategory.SelectedValue).DefaultView;
        this.gvT_Video_Profile.DataBind();
    }

    protected void btnUpdateProfile_Click(object sender, EventArgs e)
    {
        // 更新資料邏輯
        // 1.更新 T_Video_Profile
    }

    protected void btnUpdateT_Server_Click(object sender, EventArgs e)
    {
        // 更新資料邏輯
        // 1.更新 T_Server
        // 2.更新 T_Sysparameter

        bs.SqlBuilder sql = new bs.SqlBuilder();
        sql.AppendLine("    UPDATE t");
        sql.AppendLine("       SET t.Video_1 = v.Video_1");
        sql.AppendLine("         , t.Video_2 = v.Video_2");
        sql.AppendLine("      FROM T_Server t WITH(rowlock)");
        sql.AppendLine("INNER JOIN (SELECT v.Server_id, v.Video_1, v.Video_2 FROM T_Video_Profile v WITH(nolock) WHERE v.Profile_Category = ?) v").AddParams(this.rblCategory.SelectedValue);
        sql.AppendLine("        ON t.Server_id = v.Server_id");
        sql.AppendLine("");

        //
        using (var conn = bs.DbPF.CreateDbConnection("", _ConnString))
        {
            conn.Open();
            var trans = conn.BeginTransaction();
            try
            {
                bs.DbPF.ExecNonQuery(trans, sql);
                bs.DbPF.ExecNonQuery(trans, "UPDATE T_Sysparameter WITH(rowlock) SET Param_value = ? WHERE Param_key = 'VideoIPConfig'", this.rblCategory.SelectedValue);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }

            //
            this.GetNowResult();
            this.GetNewResult(lblSetupDisplay.Text);
        }
    }
}