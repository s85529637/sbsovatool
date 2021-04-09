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

public partial class GameChart : System.Web.UI.Page
{
    public class server
    {
        public string Server_id { get; set; }
        public string Server_Name { get; set; }

        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AppSettingsReader reader = new AppSettingsReader();
            string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
            List<server> dt = null;
            string sqlCommand = @"select Server_id,Server_Name FROM [T_Server] where [Server_Name] like '%百家樂%' and [Active] ='1' and Game_id in ('Bacc' , 'InsuBacc' ,'BCBacc')";
            using (var conn = new SqlConnection(connString))
            {
                dt = conn.Query<server>(sqlCommand, new { }).ToList();
            }

            this.DropDownList1.DataSource = dt;
            DropDownList1.DataTextField = "Server_Name";
            DropDownList1.DataValueField = "Server_id";
            this.DropDownList1.DataBind();

        }
    }
}
