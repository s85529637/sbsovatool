<%@ WebHandler Language="C#" Class="GameChartclubmessage" %>

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

public class GameChartclubmessage : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string Server_id = context.Request["DropDownList"];
        string No_Run = context.Request["run"];
        string Club_Ename = context.Request["Club_Ename"];
        List<clubbet> JsonsList = gameChartclubmess(Server_id, No_Run, Club_Ename);
        string jsonStr = JsonConvert.SerializeObject(JsonsList);
        context.Response.Write(jsonStr);
    }
    public class clubbet
    {

        public string Stake_Score { get; set; }

        public string Account_Score { get; set; }

        public string No_Active { get; set; }

        public string MaHao { get; set; }

    }
    public List<clubbet> gameChartclubmess(string Server_id, string No_Run, string Club_Ename)
    {
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Mon.ConnectionString", typeof(string)).ToString();
        List<clubbet> dt = null;
        string sqlCommand = @"select Stake_Score,Account_Score,No_Active,
                                     case  when MaHao='Zhuang' then '莊'
                                     when MaHao='Xian' then '閒'
                                     when MaHao='He' then '和'
                                     when MaHao='ZDD' then '莊對'
                                     when MaHao='XDD' then '閒對'
                                     when MaHao='Big' then '大'
                                     when MaHao='Small' then '小'
                                     when MaHao='AnyPair' then '任意對子'
                                     else '完美對子'
                                     end MaHao 
                               FROM  T_Club_Stake_History  where Club_Ename =@Club_Ename and Server_id=@Server_id and No_Run=@No_Run order by No_Active  ";
        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<clubbet>(sqlCommand, new { Server_id = Server_id, No_Run = No_Run, Club_Ename = Club_Ename }).ToList();
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