using System;
using System.Collections.Generic;
using System.Configuration;
using Dapper;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public class Openlist
{
    public int Win_Flag { get; set; }
}
public partial class GameChart : System.Web.UI.Page
{
    protected void btnInquire_Click(object sender, EventArgs e)
    {
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        List<Openlist> dt = null;
        string sqlCommand = @"select Win_Flag　from T_Baccarat_Openlist where Server_id='0604250001'　And No_Run='030406001'";
        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<Openlist>(sqlCommand, new { }).ToList();
        }
    }
}
