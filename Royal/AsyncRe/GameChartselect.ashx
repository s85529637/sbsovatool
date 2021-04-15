<%@ WebHandler Language="C#" Class="GameChartselect" %>

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

public class GameChartselect : IHttpHandler
{
    public class clubserver
    {
        public string Server_id { get; set; }

        public string Server_Name { get; set; }

        public string No_Run { get; set; }

        public string No_Active { get; set; }

        public int Stake_Score { get; set; }


    }

    public void ProcessRequest(HttpContext context)
    {
        string Club_Ename = context.Request["Club_Ename"];
        string Datetime = context.Request["selectdata"]+"%";
        List<clubserver> JsonsList = clubservercord(Club_Ename,Datetime);
        string jsonStr = JsonConvert.SerializeObject(JsonsList);
        context.Response.Write(jsonStr);
    }

    public List<clubserver> clubservercord(string Club_Ename,string Datetime)
    {
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        List<clubserver> dt = null;
        string sqlCommand = @"select s.Server_id,s.Server_Name,c.No_Run,c.No_Active,sum (Stake_Score) Stake_Score FROM  T_Club_Stake_History  c join [T_Server] s on s.Server_id=c.Server_id
                              where convert(varchar(40),Datetime,121) like @Datetime and Club_Ename =@Club_Ename and s.[Active] ='1' and s.Game_id in ('Bacc' , 'InsuBacc' ,'BCBacc') group by s.Server_id,s.Server_Name,c.No_Run,c.No_Active";
        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<clubserver>(sqlCommand, new { Club_Ename = Club_Ename ,Datetime=Datetime}).ToList();
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