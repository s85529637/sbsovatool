<%@ WebHandler Language="C#" Class="GameChart" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using Dapper;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Newtonsoft.Json;

public class GameChart : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string Server_id = context.Request["DropDownList"];
        string No_Run = context.Request["Run"];
        List<Openlist> JsonsList = Openlistrecord(Server_id,No_Run);
        string jsonStr = JsonConvert.SerializeObject(JsonsList);
        context.Response.Write(jsonStr);
    }
    public class Openlist
    {
        public int Win_Flag { get; set; }

        public int No_Active { get; set; }

    }

    public List<Openlist> Openlistrecord(string Server_id,string _No_Run)
    {
        string No_Run = "030407" + _No_Run;
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        List<Openlist> dt = null;
        string sqlCommand = @"select No_Active,Win_Flag from T_Baccarat_Openlist_History where Server_id=@Server_id　And No_Run=@No_Run";
        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<Openlist>(sqlCommand, new { Server_id = Server_id,No_Run=No_Run }).ToList();
        }
        return dt;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}