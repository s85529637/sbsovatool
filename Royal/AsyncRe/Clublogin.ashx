<%@ WebHandler Language="C#" Class="Clublogin" %>

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

public class Clublogin : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string user_id = context.Request["Club_Ename"];
        string login_time = context.Request["selectdata"];
        login_time += "%";
        List<T_Login_Log> JsonsList = T_Login_Logcord(user_id,login_time);
        string jsonStr = JsonConvert.SerializeObject(JsonsList);
        context.Response.Write(jsonStr);

        ;
    }
    public class T_Login_Log
    {
        public string user_id { get; set; }
        public string login_local { get; set; }
        public string login_time { get; set; }
        public string login_flag { get; set; }
    }
    public List<T_Login_Log> T_Login_Logcord( string user_id,string login_time)
    {
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        List<T_Login_Log> dt = null;
        string sqlCommand = @"SELECT [user_id]
    　　　　　　　　　　　　　　　　　, [login_local]
    　　　　　　　　　　　　　　　　　, [login_time]
    　　　　　　　　　　　　　　　　　, case 
	　　　　　　　　　　　　　　　　　　when [login_flag] = '1' then '登入'
	　　　　　　　　　　　　　　　　　　else '登出'
	　　　　　　　　　　　　　　　　　　end as [login_flag]
　　　　　　　　　　　　　　　  FROM [HKNetGame_HJ].[dbo].[T_Login_Log]
                              WHERE user_id = @user_id
                                    AND convert(VARCHAR(40), [login_time], 121) LIKE @login_time";
        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<T_Login_Log>(sqlCommand, new {user_id=user_id,login_time=login_time }).ToList();
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