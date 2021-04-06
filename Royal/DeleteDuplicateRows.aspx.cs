using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeleteDuplicateRows : BasePage
{
    private string _LoginUser = ConfigurationManager.AppSettings["LoginUser"].ToString();
    private string _DayDbConnStr = ConfigurationManager.AppSettings["H1DayDB.ConnectionString"].ToStr();
    private string _DayCurConnStr = ConfigurationManager.AppSettings["H1DayCUR.ConnectionString"].ToStr();

    //
    private void DataSearch()
    {
        StringBuilder sql = new StringBuilder();
        sql.AppendLine("");
        sql.AppendLine("  SELECT s.Server_Name");
        sql.AppendLine("       , c.*");
        sql.AppendLine("    FROM (");
        sql.AppendLine("");
        sql.AppendLine("    SELECT t.Id FROM T_Club_Stake_Current_Duplicate_Log t WITH(nolock)");
        sql.AppendLine("      JOIN (");
        sql.AppendLine("               SELECT c.Club_id, c.DATETIME, c.MaHao");
        sql.AppendLine("                 FROM dbo.T_Club_Stake_Current_Duplicate_Log c WITH(nolock)");
        sql.AppendLine("                WHERE c.Del_Flag = 0");
        sql.AppendLine("             GROUP BY c.Club_id, c.DATETIME, c.MaHao");
        sql.AppendLine("               HAVING COUNT(*) > 1");
        sql.AppendLine("           ) c");
        sql.AppendLine("        ON t.Club_id = c.Club_id");
        sql.AppendLine("       AND t.DATETIME = c.DATETIME");
        sql.AppendLine("       AND t.MaHao = c.MaHao");
        sql.AppendLine("");
        sql.AppendLine("     UNION");
        sql.AppendLine("");
        sql.AppendLine("    SELECT t.Id FROM T_Club_Stake_Current_Duplicate_Log t WITH(nolock)");
        sql.AppendLine("      JOIN (");
        sql.AppendLine("               SELECT c.Club_id, c.ReportTime, c.MaHao");
        sql.AppendLine("                 FROM dbo.T_Club_Stake_Current_Duplicate_Log c WITH(nolock)");
        sql.AppendLine("                WHERE c.Del_Flag = 0");
        sql.AppendLine("             GROUP BY c.Club_id, c.ReportTime, c.MaHao");
        sql.AppendLine("               HAVING COUNT(*) > 1");
        sql.AppendLine("           ) c");
        sql.AppendLine("        ON t.Club_id = c.Club_id");
        sql.AppendLine("       AND t.ReportTime = c.ReportTime");
        sql.AppendLine("       AND t.MaHao = c.MaHao");
        sql.AppendLine("");
        sql.AppendLine("     UNION");
        sql.AppendLine("");
        sql.AppendLine("    SELECT t.Id FROM T_Club_Stake_Current_Duplicate_Log t WITH(nolock)");
        sql.AppendLine("      JOIN (");
        sql.AppendLine("               SELECT c.Club_id, c.StartSeqNoFlag");
        sql.AppendLine("                 FROM T_Club_Stake_Current_Duplicate_Log c WITH(nolock)");
        sql.AppendLine("                WHERE c.Del_Flag = 0");
        sql.AppendLine("                  AND c.StartSeqNoFlag <> 0");
        sql.AppendLine("             GROUP BY c.Club_id, c.StartSeqNoFlag");
        sql.AppendLine("               HAVING COUNT(*) > 1");
        sql.AppendLine("           ) c");
        sql.AppendLine("        ON t.Club_id = c.Club_id");
        sql.AppendLine("       AND t.StartSeqNoFlag = c.StartSeqNoFlag");
        sql.AppendLine("");
        sql.AppendLine(") t");
        sql.AppendLine("");
        sql.AppendLine("    JOIN T_Club_Stake_Current c WITH(nolock)");
        sql.AppendLine("      ON t.Id = c.Id");
        sql.AppendLine("    JOIN T_Server s WITH(nolock)");
        sql.AppendLine("      ON c.Server_id = s.Server_id");
        sql.AppendLine("ORDER BY c.Club_Ename, s.Server_Name, c.MaHao, c.Account_Score, t.Id");
        sql.AppendLine("");

        //
        var dayCurConn = bs.DbPF.CreateDbConnection("", _DayCurConnStr);
        DataTable dataTable = bs.DbPF.GetResultTable(dayCurConn, sql.ToStr());

        //
        this.gvResult.DataSource = dataTable;
        this.gvResult.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnBatDelete_Click(object sender, EventArgs e)
    {
        var dayDbConn = bs.DbPF.CreateDbConnection("", _DayDbConnStr);
        var dayCurConn = bs.DbPF.CreateDbConnection("", _DayCurConnStr);

        //
        StringBuilder sql = new StringBuilder();
        sql.AppendLine("");
        sql.AppendLine("  SELECT MAX(c.Id) MaxId");
        sql.AppendLine("    FROM dbo.T_Club_Stake_Current_Duplicate_Log c WITH(nolock)");
        sql.AppendLine("   WHERE c.Del_Flag = 0");
        sql.AppendLine("GROUP BY c.Club_id, c.DATETIME, c.MaHao");
        sql.AppendLine("  HAVING COUNT(*) > 1");
        sql.AppendLine("");
        sql.AppendLine("   UNION");
        sql.AppendLine("");
        sql.AppendLine("  SELECT MAX(c.Id) MaxId");
        sql.AppendLine("    FROM dbo.T_Club_Stake_Current_Duplicate_Log c WITH(nolock)");
        sql.AppendLine("   WHERE c.Del_Flag = 0");
        sql.AppendLine("GROUP BY c.Club_id, c.ReportTime, c.MaHao");
        sql.AppendLine("  HAVING COUNT(*) > 1");
        sql.AppendLine("");
        sql.AppendLine("   UNION");
        sql.AppendLine("");
        sql.AppendLine("  SELECT MAX(c.Id) MaxId");
        sql.AppendLine("    FROM dbo.T_Club_Stake_Current_Duplicate_Log c WITH(nolock)");
        sql.AppendLine("   WHERE c.Del_Flag = 0");
        sql.AppendLine("     AND c.StartSeqNoFlag <> 0");
        sql.AppendLine("GROUP BY c.Club_id, c.StartSeqNoFlag");
        sql.AppendLine("  HAVING COUNT(*) > 1");
        sql.AppendLine("");

        //
        DataTable dataTable = bs.DbPF.GetResultTable(dayCurConn, sql.ToStr());

        //
        foreach (System.Data.DataRow row in dataTable.Rows)
        {
            var id = (row["MaxId"] as long?).GetValueOrDefault();

            // 刪除日帳的記錄
            bs.DbPF.ExecNonQuery(dayDbConn, "DELETE T_Club_Stake_Current WITH(ROWLOCK) WHERE Id = @Id", id);

            // 變更日帳同步帳的刪除註記
            bs.DbPF.ExecNonQuery(dayCurConn, "UPDATE T_Club_Stake_Current_Duplicate_Log SET Del_Flag = 1 WHERE Id = @Id", id);
        }

        //
        this.DataSearch();
        //this.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('執行完畢');", true);
    }

    protected void gvResult_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var id = (this.gvResult.DataKeys[e.RowIndex]["Id"] as long?).GetValueOrDefault();

        // 刪除日帳的記錄
        var dayDbConn = bs.DbPF.CreateDbConnection("", _DayDbConnStr);
        bs.DbPF.ExecNonQuery(dayDbConn, "DELETE T_Club_Stake_Current WITH(ROWLOCK) WHERE Id = @Id", id);

        // 變更日帳同步帳的刪除註記
        var dayCurConn = bs.DbPF.CreateDbConnection("", _DayCurConnStr);
        bs.DbPF.ExecNonQuery(dayCurConn, "UPDATE T_Club_Stake_Current_Duplicate_Log SET Del_Flag = 1 WHERE Id = @Id", id);

        //
        this.DataSearch();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bsSQL ssql = new bsSQL();
        string pwd = ssql.GetSytemParameter(_LoginUser);

        //
        if (tbxPwd.Text == pwd)
        {
            // 避免重複一值按搜尋直接從帳務資料撈會很慢
            if (this.cbxTemp.Checked == true)
            {
                this.cbxTemp.Checked = false;
                var dayCurConn = bs.DbPF.CreateDbConnection("", _DayCurConnStr);
                bs.DbPF.ExecNonQuery(dayCurConn, "EXEC M_Stake_Current_Duplicate_Log");
            }

            //
            this.DataSearch();
        }
        else
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('密碼錯誤');", true);
        }
    }
}