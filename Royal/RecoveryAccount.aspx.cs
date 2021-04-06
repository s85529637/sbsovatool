using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class UnReturnAccount : NewBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetList();
        }
    }

    // 取得清單
    private void GetList()
    {
        bsSQL bssql = new bsSQL();
        DataTable dt0 = bssql.GetUserStat(txtClubEname.Text.Trim());
        Table0.DataSource = dt0;
        Table0.DataBind();
        if (dt0.Rows.Count > 0)
        {
            string club_id = dt0.Rows[0]["Club_id"].ToString();
            lblClubId.Text = club_id;
            //Sova/Golden未洗分紀錄
            bssql = new bsSQL();
            DataTable dt1 = bssql.GetUserSovaGoldenSession(club_id);
            Table1.DataSource = dt1;
            Table1.DataBind();
            Label1.Visible = (dt1.Rows.Count != 0);
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            if ("H1".Equals( ConfigurationManager.AppSettings["SiteId"].ToString()))
            {
                //Jumbo最新開分紀錄
                bssql = new bsSQL();
                dt2 = bssql.GetUserJumboSessionIn(club_id);
                Table2.DataSource = dt2;
                Table2.DataBind();
                Label2.Visible = (dt2.Rows.Count != 0);
                //Jumbo最新洗分紀錄
                bssql = new bsSQL();
                dt3 = bssql.GetUserJumboSession(club_id);
                Table3.DataSource = dt3;
                Table3.DataBind();
                Label3.Visible = (dt3.Rows.Count != 0);
            }

            Button1.Visible = false;
            if (dt1.Rows.Count > 0)
            {
                lblStatus.Text = "此帳號有Ruby4/Sova未洗分紀錄，請依遊戲使用Ruby4/Sova後台手動洗分功能或Ruby4未洗分會員";
            }
            else if (
               (dt2.Rows.Count > 0 && dt3.Rows.Count == 0) ||
               (dt2.Rows.Count == 1 && dt3.Rows.Count == 1 && dt2.Rows[0]["SessionNo"].ToString() != dt3.Rows[0]["SessionNo"].ToString())
               )
            {
                lblStatus.Text = "此帳號有Jumbo未洗分紀錄，請使用Jumbo未洗分會員";
            }
            else
            {
                DataRow r = dt0.Rows[0];
                if ("0" == r["Login"].ToString() &&
                    ("room" != r["Login_Game_Id"].ToString().ToLower() || "room" != r["Login_Server_Id"].ToString().ToLower() || true == (Boolean)r["Login_EGame"] || true == (Boolean)r["Login_Room"]))
                {
                    lblStatus.Text = "此帳號無未洗分紀錄，且狀態異常，可執行帳號狀態恢復";
                    Button1.Visible = true;
                }
                else
                {
                    string sStatus = ("0" == r["Login"].ToString()) ? "仍在線上" : "帳號狀態正常";
                    lblStatus.Text = "此帳號" + sStatus + "，無需修改！";
                    Button1.Visible = false;
                }
            }
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        GetList();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Club_Ename = txtClubEname.Text.Trim();
        if ("" != Club_Ename)
        {
            try
            {
                bsSQL bssql = new bsSQL();
                int i = bssql.RecoveryClubStat(lblClubId.Text);
                if (1 == i)
                {                    
                    Lib.MsgBox(this.UpdatePanel1, "更新成功！");
                }
                else
                {
                    Lib.MsgBox(this.UpdatePanel1, "更新失敗！");
                }
            }
            catch { Lib.MsgBox(this.UpdatePanel1, "更新失敗！"); }
            GetList();
        }
    }
}