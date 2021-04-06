using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class MoveStakeAccount : BasePage
{
    public int a = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetOpenList();
        }
    }

    // 取得清單
    private void GetOpenList()
    {
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetUnTongJiStakeAccount();

        OpenList.DataSource = dt;

        a = dt.Rows.Count;

        OpenList.DataBind();
    }

    protected void OpenList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "")
        {
            bsSQL bssql = new bsSQL();
            string sReturn = bssql.MoveUnTongJiStakeAccount(e.CommandArgument.ToString());
            if ("0".Equals(sReturn))
            {
                Lib.MsgBox(UpdatePanel1, "搬移成功");
                this.AddLogItem(e.CommandArgument.ToString(), "搬移成功");
            }
            else
            {
                Lib.MsgBox(UpdatePanel1, "搬移失敗");
                this.AddLogItem(e.CommandArgument.ToString(), "搬移失敗");
            }
        }
        GetOpenList();
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        GetOpenList();
    }

    protected void chkAuto_CheckedChanged(object sender, EventArgs e)
    {
        Timer1.Enabled = chkAuto.Checked;
    }
}