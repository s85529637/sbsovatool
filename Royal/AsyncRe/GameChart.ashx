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
        string No_Run = context.Request["run"];
        List<Openlist> JsonsList = Openlistrecord(Server_id, No_Run);
        string jsonStr = JsonConvert.SerializeObject(JsonsList);
        context.Response.Write(jsonStr);
    }
    public class Openlist
    {
        public int Win_Flag { get; set; }

        public int No_Active { get; set; }


    }

    public List<Openlist> Openlistrecord(string Server_id, string No_Run)
    {

        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        List<Openlist> dt = null;
        string sqlCommand = @"declare @Server_Name nvarchar(20)
                               select @Server_Name=Server_Name from T_Server where Server_id=@Server_id
                               if(@Server_Name　LIKE '百家樂%')
                               (
                               select No_Active,Win_Flag from T_Baccarat_Openlist_History where Server_id=@Server_id　And No_Run=@No_Run  
                               )
                               IF(@Server_Name　LIKE '區塊鏈百家樂%')
                               (
                               select No_Active,Win_Flag from T_BCBacc_Openlist_History where Server_id=@Server_id　And No_Run=@No_Run  
                               )
                               IF(@Server_Name　LIKE '保險百家樂%')
                               (
                               select No_Active,Win_Flag from T_InsuBacc_Openlist_History where Server_id=@Server_id　And No_Run=@No_Run  
                               )";
        try
        {
            using (var conn = new SqlConnection(connString))
            {
                dt = conn.Query<Openlist>(sqlCommand, new { Server_id = Server_id, No_Run = No_Run }).OrderBy(x => x.No_Active).ToList();
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