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

        public double Stake_Score { get; set; }

        public string MaHao { get; set; }
    }

    public void ProcessRequest(HttpContext context)
    {
        string money = context.Request["money"];
        string Club_Ename = context.Request["Club_Ename"];
        string Datetime = context.Request["selectdata"] + "%";
        List<clubserver> JsonsList = clubservercord(Club_Ename, Datetime, money);
        string jsonStr = JsonConvert.SerializeObject(JsonsList);
        context.Response.Write(jsonStr);
    }

    public List<clubserver> clubservercord(string Club_Ename, string Datetime, string money)
    {
        try
        {
            AppSettingsReader reader = new AppSettingsReader();
            string connString = reader.GetValue("Mon.ConnectionString", typeof(string)).ToString();
            List<clubserver> dt = null;
            string sqlCommand = string.Empty;
            if (money == "Million")
            {
                sqlCommand = @"select s.Server_id,s.Server_Name,c.No_Run,c.No_Active,Stake_Score/10000 Stake_Score,MaHao FROM  T_Club_Stake_History c join [T_Server] s on s.Server_id=c.Server_id
                                  where convert(varchar(40),Datetime,121) like @Datetime and Club_Ename =@Club_Ename and s.[Active] ='1' and s.Game_id in ('Bacc' , 'InsuBacc' ,'BCBacc') order by [Datetime] ";
            }
            else
            {
                sqlCommand = @"select s.Server_id,s.Server_Name,c.No_Run,c.No_Active,Stake_Score/1000 Stake_Score,MaHao FROM  T_Club_Stake_History c join [T_Server] s on s.Server_id=c.Server_id
                                  where convert(varchar(40),Datetime,121) like @Datetime and Club_Ename =@Club_Ename and s.[Active] ='1' and s.Game_id in ('Bacc' , 'InsuBacc' ,'BCBacc') order by [Datetime] ";
            }
            using (var conn = new SqlConnection(connString))
            {
                dt = conn.Query<clubserver>(sqlCommand, new { Club_Ename = Club_Ename, Datetime = Datetime }).ToList();
            }

            return dt;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}