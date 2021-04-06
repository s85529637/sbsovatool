using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class GameSwitch : NewBasePage
{
    public const string RTG = "RTG";

    public const string GCLUB = "GCLUB";

    public const string JDB = "JDB";

    public const string STAR = "Star";

    public const string JUMBO = "Jumbo";

    public const string SOVA = "Sova";

    public const string GOLDEN = "Golden";

    public const string ROYAL = "Royal";

    public const string MAESOT = "maesot";

    private static readonly string _SubSystem = ConfigurationManager.AppSettings["SubSystem"].ToString();
    //讀取SiteId只是為了判斷是不是美索
    public string _SiteId = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SiteId"].ToString()) ? "h1mini" : ConfigurationManager.AppSettings["SiteId"].ToString();
    public String ThirdPartyId = ConfigurationManager.AppSettings["ThirdPartyId"].ToString();
    String LoginUser = ConfigurationManager.AppSettings["LoginUser"].ToString();
    bsSQL ssql = new bsSQL();

    //
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DisplayPanel.Visible = ConfigurationManager.AppSettings["IsVisible"].ToStr().ToBoolean().GetValueOrDefault();
            GetData();
        }
    }

    /// <summary>
    /// Jumbo整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJumbo_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtJumbo.Text.Trim(), "SlotGame_StartFlag", rbJumbo.SelectedValue);
    }

    /// <summary>
    /// Sova整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSova_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtSova.Text.Trim(), "3DGame_StartFlag", rbSova.SelectedValue);
    }

    /// <summary>
    /// Golden整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGolden_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtGolden.Text.Trim(), "GoldenGame_StartFlag", rbGolden.SelectedValue);
    }

    /// <summary>
    ///  Royal整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRoyal_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtRoyal.Text.Trim(), "RoyalGame_StartFlag", rbRoyal.SelectedValue);
    }

    /// <summary>
    /// Star整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnStar_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtStar.Text.Trim(), "StarGame_StartFlag", rbStar.SelectedValue);
    }

    /// <summary>
    /// RTG整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRTG_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtRTG.Text.Trim(), "RTGGame_StartFlag", rbRTG.SelectedValue);
    }

    /// <summary>
    /// Gclub整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btGclub_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtGclub.Text.Trim(), "GClubGame_StartFlag", rbGclub.SelectedValue);
    }
    protected void btGclub_Click2(object sender, EventArgs e)
    {
        string _Password = ssql.GetSytemParameter(LoginUser);
        string Password = txtGclub2.Text.Trim();
        if (_Password.Equals(Password))
        {
            Lib.MsgBox(UpdatePanel1, "修改成功");
        }
        else
        {
            Lib.MsgBox(UpdatePanel1, "密碼錯誤");
        }
    }
    /// <summary>
    /// 官網整站開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFrontEnd_Click(object sender, EventArgs e)
    {
        UpdateFlag(txtFrontEnd.Text.Trim(), "FrontEnd_Maintain", rbFrontEnd.SelectedValue);
    }

    private void GetData()
    {
        rbJumbo.SelectedValue = ssql.GetSystemParameter("SlotGame_StartFlag");
        rbSova.SelectedValue = ssql.GetSystemParameter("3DGame_StartFlag");
        rbGolden.SelectedValue = ssql.GetSystemParameter("GoldenGame_StartFlag");
        rbRoyal.SelectedValue = ssql.GetSystemParameter("RoyalGame_StartFlag");
        rbJDB.SelectedValue = ssql.GetSystemParameter("JDBGame_StartFlag");
        rbStar.SelectedValue = ssql.GetSystemParameter("StarGame_StartFlag");
        rbFrontEnd.SelectedValue = ssql.GetSystemParameter("FrontEnd_Maintain");
        rbRTG.SelectedValue = ssql.GetSystemParameter("RTGGame_StartFlag");
        rbGclub.SelectedValue = ssql.GetSystemParameter("GClubGame_StartFlag");
        //
        using (SqlCommand cmd = new SqlCommand(@"SELECT t.Param_key, Param_name, t.Param_value FROM T_Sysparameter t WHERE t.Param_key LIKE 'SysSwitch[\_]%'"))
        using (System.Data.DataTable dtb = new dbSQL().SelectSQL(cmd))
        {
            this.gvResult.DataSource = dtb.DefaultView;
            this.gvResult.DataBind();
        }

        if (ThirdPartyId.IndexOf(JDB) > -1)
        {
            // 取得 JDB 單一遊戲開關
            using (SqlCommand cmd = new SqlCommand(@"EXEC A_Get_Game_Switch 'JDB'"))
            using (System.Data.DataTable dtb = new dbSQL().SelectSQL(cmd))
            {
                this.gvJDB.DataSource = dtb.DefaultView;
                this.gvJDB.DataBind();
            }
        }

        if (ThirdPartyId.IndexOf(RTG) > -1)
        {
            // 取得棋牌單一遊戲開關
            using (SqlCommand cmd = new SqlCommand(@"EXEC A_Get_Game_Switch 'RTG'"))
            using (System.Data.DataTable dtb = new dbSQL().SelectSQL(cmd))
            {
                foreach (DataRow row in dtb.Rows)
                {
                    if (row["Game_Id"].ToString() == "RTGRoom")
                    {
                        row.Delete();
                    }
                }
                this.gvRTG.DataSource = dtb.DefaultView;
                this.gvRTG.DataBind();
            }
        }

        //取得皇家電子二館開關狀態
        try
        {
            using (SlotGameWS2.SlotGameWSSoapClient ws2 = new SlotGameWS2.SlotGameWSSoapClient())
            {
                XmlNode xmlNode = ws2.GetMaintenanceStatus(_SubSystem);
                if (xmlNode != null)
                {
                    foreach (XmlNode chldNode in xmlNode.ChildNodes)
                    {
                        if (chldNode.Name == "Status")
                        {
                            rbRoyal2.SelectedValue = chldNode.FirstChild.Value;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lib.MsgBox(UpdatePanel1, "取得開關狀態失敗");
        }
    }

    /// <summary>
    /// 修改整館開關用(含官網)
    /// </summary>
    /// <param name="Password"></param>
    /// <param name="Param_key"></param>
    /// <param name="Param_value"></param>
    private void UpdateFlag(String Password, String Param_key, String Param_value)
    {
        String _Password = ssql.GetSytemParameter(LoginUser);

        if (_Password == null)
        {
            Lib.MsgBox(UpdatePanel1, "修改失敗");
        }
        else if (_Password.Equals(Password))
        {
            ssql.SetSystemParameter(Param_key, Param_value);
            Lib.MsgBox(UpdatePanel1, "修改成功");
        }
        else
        {
            Lib.MsgBox(UpdatePanel1, "密碼錯誤");
        }
    }

    protected void gvResult_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        RadioButtonList rdo = e.Row.FindControl("rdoButton") as RadioButtonList;
        if (rdo != null && e.Row.DataItem != null)
        {
            rdo.SelectedValue = ((System.Data.DataRowView)e.Row.DataItem)["Param_value"].ToStr();
        }
    }

    /// <summary>
    /// RTG單一遊戲開關列建立事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvRTG_RowCreated(object sender, GridViewRowEventArgs e)
    {
        RadioButtonList rdo = e.Row.FindControl("rdoButton") as RadioButtonList;
        if (rdo != null && e.Row.DataItem != null)
        {
            rdo.SelectedValue = ((System.Data.DataRowView)e.Row.DataItem)["Switch_Value"].ToStr();
        }
    }

    /// <summary>
    /// JDB單一遊戲開關列建立事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvJDB_RowCreated(object sender, GridViewRowEventArgs e)
    {
        RadioButtonList rdo = e.Row.FindControl("rdoButton") as RadioButtonList;
        if (rdo != null && e.Row.DataItem != null)
        {
            rdo.SelectedValue = ((System.Data.DataRowView)e.Row.DataItem)["Switch_Value"].ToStr();
        }
    }


    protected void batSave_Click(object sender, EventArgs e)
    {
        var sql = new dbSQL();
        SqlCommand cmd = new SqlCommand("UPDATE T_Sysparameter SET Param_value = @Param_value WHERE(Param_key = @Param_key)");
        foreach (GridViewRow row in this.gvResult.Rows)
        {
            var key = this.gvResult.DataKeys[row.RowIndex].Value;
            var rdo = row.FindControl("rdoButton") as RadioButtonList;
            var value = rdo == null ? "0" : rdo.SelectedValue;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Param_key", key);
            cmd.Parameters.AddWithValue("Param_value", value);
            sql.RunSQL(cmd);
        }

        ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('修改完畢');", true);
    }

    /// <summary>
    /// 棋牌單一遊戲開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void batSaveRTG_Click(object sender, EventArgs e)
    {
        var sql = new dbSQL();
        SqlCommand cmd = new SqlCommand("UPDATE T_Game_Switch SET Switch_Value = @Switch_Value, Modified_Time = GETDATE() WHERE(Category = @Category) AND (Id = @Id) ");
        string tmpvalue = "";
        foreach (GridViewRow row in this.gvRTG.Rows)
        {
            var category = this.gvRTG.DataKeys[row.RowIndex].Values["Category"];
            var id = this.gvRTG.DataKeys[row.RowIndex].Values["Id"];
            var rdo = row.FindControl("rdoButton") as RadioButtonList;
            var gameid = row.Cells[0].Text;
            var value = rdo == null ? "0" : rdo.SelectedValue;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Category", category);
            cmd.Parameters.AddWithValue("Id", id);
            cmd.Parameters.AddWithValue("Switch_Value", value);
            sql.RunSQL(cmd);
            if (string.IsNullOrEmpty(tmpvalue))
            {
                tmpvalue = string.Format("{0}|{1}", rdo.SelectedValue, gameid);
            }
            else
            {
                tmpvalue = string.Format("{0},{1}|{2}", tmpvalue, rdo.SelectedValue, gameid);
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('修改完畢，開始踼線');kickonegame('" + tmpvalue + "');", true);
    }

    /// <summary>
    /// JDB單一遊戲開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void batSaveJDB_Click(object sender, EventArgs e)
    {

        var sql = new dbSQL();
        SqlCommand cmd = new SqlCommand("UPDATE T_Game_Switch SET Switch_Value = @Switch_Value, Modified_Time = GETDATE() WHERE(Category = @Category) AND (Id = @Id) ");
        foreach (GridViewRow row in this.gvJDB.Rows)
        {
            var category = this.gvJDB.DataKeys[row.RowIndex].Values["Category"];
            var id = this.gvJDB.DataKeys[row.RowIndex].Values["Id"];
            var rdo = row.FindControl("rdoButton") as RadioButtonList;
            var value = rdo == null ? "0" : rdo.SelectedValue;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Category", category);
            cmd.Parameters.AddWithValue("Id", id);
            cmd.Parameters.AddWithValue("Switch_Value", value);
            sql.RunSQL(cmd);
        }

        ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('修改完畢');", true);
    }

    protected void rdoButtonAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoButtonAll.SelectedValue == "1")
        {
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                (row.FindControl("rdoButton") as RadioButtonList).SelectedValue = "1";
            }
        }
        else
        {
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                (row.FindControl("rdoButton") as RadioButtonList).SelectedValue = "0";
            }
        }
    }

    /// <summary>
    /// 棋牌單一遊戲下拉全開全關下拉事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoButtonAllRTG_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoButtonAllRTG.SelectedValue == "1")
        {
            foreach (GridViewRow row in this.gvRTG.Rows)
            {
                (row.FindControl("rdoButton") as RadioButtonList).SelectedValue = "1";
            }
        }
        else
        {
            foreach (GridViewRow row in this.gvRTG.Rows)
            {
                (row.FindControl("rdoButton") as RadioButtonList).SelectedValue = "0";
            }
        }
    }

    /// <summary>
    /// JDB單一遊戲下拉全開全關下拉事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rdoButtonAllJDB_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoButtonAllJDB.SelectedValue == "1")
        {
            foreach (GridViewRow row in this.gvJDB.Rows)
            {
                (row.FindControl("rdoButton") as RadioButtonList).SelectedValue = "1";
            }
        }
        else
        {
            foreach (GridViewRow row in this.gvJDB.Rows)
            {
                (row.FindControl("rdoButton") as RadioButtonList).SelectedValue = "0";
            }
        }
    }

    /// <summary>
    /// 皇家二館整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRoyal2_Click(object sender, EventArgs e)
    {
        String _Password = ssql.GetSytemParameter(LoginUser);

        if (_Password.Equals(txtRoyal2.Text.Trim()))
        {
            try
            {
                using (SlotGameWS2.SlotGameWSSoapClient ws2 = new SlotGameWS2.SlotGameWSSoapClient())
                {
                    ws2.SetMaintenanceStatus(_SubSystem, rbRoyal2.SelectedValue.ToInt32().GetValueOrDefault());
                }
                Lib.MsgBox(UpdatePanel1, "修改成功");
            }
            catch (Exception ex)
            {
                Lib.MsgBox(UpdatePanel1, "修改失敗");
            }
        }
        else
        {
            Lib.MsgBox(UpdatePanel1, "密碼錯誤");
        }
    }

    /// <summary>
    /// JDB整館開關事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnJDB_Click(object sender, EventArgs e)
    {
        String Password = txtJDB.Text.Trim();
        String _Password = ssql.GetSytemParameter(LoginUser);
        String Param_value = rbJDB.SelectedValue;
        String Param_key = "JDBGame_StartFlag";

        if (_Password == null || _Password == "")
        {
            Lib.MsgBox(UpdatePanel1, "修改失敗");
        }
        else if (_Password.Equals(Password))
        {
            ssql.SetSystemParameter(Param_key, Param_value);
            JDBlib JDBTool = new JDBlib("15");
            string Status = "";
            string Result = "";
            if (Param_value.Equals("0"))
            {
                Result = JDBTool.kickALLMember(ref Status);
            }

            Lib.MsgBox(UpdatePanel1, "修改成功");
        }
        else
        {
            Lib.MsgBox(UpdatePanel1, "密碼錯誤");
        }
    }


}