using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CheckGsRoad : NewBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetDataList();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        GetDataList();
    }

    private void GetDataList() {
        string strsql = "select [Name] as 遊戲 ,[Msg] as 訊息, [CreateDate] as 建立日期  from GSMsg order by [CreateDate] desc  limit 100 ";
        DataSet ds = DbHelperSQLite.Query(strsql);
        DataTable dt = ds.Tables[0];
        GwCheckGsRoad.DataSource = dt;
        GwCheckGsRoad.DataBind();
    }
}