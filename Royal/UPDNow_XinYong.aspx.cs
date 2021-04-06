using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Serilog;
using Serilog.Core;
using System.Net.Mail;
using System.IO;

public partial class UPDNow_XinYong : System.Web.UI.Page
{

    String HomePage = "UPDNow_XinYong.aspx";
    private Dictionary<string, string> _MailList;
    protected void Page_Load(object sender, EventArgs e)
    {
        // 設定 Session Timeout 時間(單位：分鐘)
        this.Session.Timeout = 60;

        if (!string.IsNullOrEmpty(Request.QueryString["err"]))
        {
            if (Request.QueryString["err"].ToString() == "1")
            {
                Response.Write("<script>alert('請檢查驗證碼設定!!');location.href='UPDNow_XinYong.aspx';</script>");
                Response.End();
            }
            else if (Request.QueryString["err"].ToString() == "2")
            {
                Response.Write("<script>alert('驗證碼有誤!!');location.href='UPDNow_XinYong.aspx';</script>");
                Response.End();
            }
        }
    }
    //寄信
    private void SendMailByGmail(string MailBody)
    {
        this._MailList = new Dictionary<string, string>();
        foreach (var data in ConfigurationManager.AppSettings)
        {
            string key = data.ToString();
            string value = ConfigurationManager.AppSettings[key].ToString();
            if (key.IndexOf("Mail_") >= 0)
            {
                this._MailList.Add(key, value);
            }
        }
        System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        foreach (var receiver in _MailList)
        {
            msg.To.Add(receiver.Value);
        }
        msg.From = new MailAddress("service@yamaplay.com", "修改額度", System.Text.Encoding.UTF8);
        /* 上面3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼*/
        msg.Subject = "修改會員信用額度";//郵件標題
        msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼
        msg.Body = "<h1>" + MailBody + "</h1>"; //郵件內容
        msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼  
        msg.IsBodyHtml = true;//是否是HTML郵件 
                              //msg.Priority = MailPriority.High;//郵件優先級 
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new System.Net.NetworkCredential("service@yamaplay.com", "yama9988ss"); //這裡要填正確的帳號跟密碼
        client.EnableSsl = true; //gmail預設開啟驗證
        client.Send(msg); //寄出信件
    }
    //按鈕查詢
    protected void btnInquire_Click(object sender, EventArgs e)
    {
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        string Club_Ename = txtAccount.Text.Trim();
        List<Card> dt = null;
        string sqlCommand = @"IF EXISTS (select 1 from T_Club where Club_Ename =@Club_Ename)
                                  BEGIN
                                      SELECT Club_Id,Club_Ename,Now_XinYong,
                                      CASE LOGIN
                                      WHEN 0 THEN '否'
                                  　　ELSE '是' END　as LOGIN 
                                      FROM T_CLUB  with(nolock) 
                                      WHERE Club_Ename=@Club_Ename
                                  END
                                  ELSE   
                                  BEGIN
                                  SELECT 0 as Club_Id,0 as Club_Ename,0 as Now_XinYong,0 as LOGIN
                                  END";

        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<Card>(sqlCommand, new { @Club_Ename = Club_Ename }).ToList();
            dv.DataSource = dt;
            dv.DataBind();
        }
        if (dt.Any(x => x.Club_Ename == "0"))
        {
            ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('會員帳號錯誤');", true);
        }
    }
    //按鈕修改額度
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string CreateAccount = string.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdQuota"]) ? "" : ConfigurationManager.AppSettings["UpdQuota"].ToString();

        String chkcode = string.Empty;

        String txtchkcode = string.Empty;
        string Now_XinYong = TextBox1.Text.Trim();
        if (Now_XinYong != "")
        {
            if (CreateAccount == "")
            {
                Response.Redirect(string.Format("{0}?err=1", HomePage));
            }
            else
            {
                chkcode = CreatdCheckCode(CreateAccount);
            }
            if (!string.IsNullOrEmpty(TextBox2.Value))
            {
                if (TextBox2.Value.ToString() != "")
                {
                    txtchkcode = TextBox2.Value.ToString();

                    chkcode = CreatdCheckCode(CreateAccount);

                    if (!txtchkcode.Equals(chkcode))
                    {
                        Response.Redirect(string.Format("{0}?err=2", "UPDNow_XinYong.aspx"));
                    }

                }
                else
                {
                    Response.Redirect(string.Format("{0}?err=2", "UPDNow_XinYong.aspx"));
                }

            }
            else
            {
                Response.Redirect(string.Format("{0}?err=2", "UPDNow_XinYong.aspx"));
            }
        }
        var logger = BuildSerilog();
        try
        {
            this.DataSearch();

        }
        catch (Exception ex)
        {
            logger.Error("錯誤內容:{System},{Application},{DB環境},{Error}", "Sovatool", "錯誤資訊", "舊公測", ex);
        }
    }
    //抓SEQ位置
    private static Logger BuildSerilog()
    {
        var logger = new LoggerConfiguration()
                       .ReadFrom.AppSettings()
                       .CreateLogger();
        try
        {
            Log.Logger = logger;
        }
        catch (Exception ex)
        {
            
            
        }
        return logger;
    }
    //抓取GOOGLE的金鑰
    private string CreatdCheckCode(string strAccount)
    {
        long duration = 30;

        string key = strAccount;

        GoogleAuthenticator authenticator = new GoogleAuthenticator(duration, key);

        var code = authenticator.GenerateCode();

        return code;
    }
    //存DB資料的物件
    public class Card
    {
        public string Club_Id { get; set; }
        public string Club_Ename { get; set; }
        public double Now_XinYong { get; set; }
        public string Login { get; set; }
        public double XinYong { get; set; }

        public DateTime UpdDate { get; set; }

    }
    //抓DB資料及修改
    private void DataSearch()
    {
        AppSettingsReader reader = new AppSettingsReader();
        var logger = BuildSerilog();
        string connString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        string Club_Ename = txtAccount.Text.Trim();
        List<Card> dt = null;
        double Now_XinYong = Double.Parse(TextBox1.Text.Trim());
        string sqlCommand = @"EXEC UPD_XinYong @Club_Ename,@Now_XinYong";
        using (var conn = new SqlConnection(connString))
        {
            dt = conn.Query<Card>(sqlCommand, new { @Now_XinYong = Now_XinYong, @Club_Ename = Club_Ename }).ToList();
            dv.DataSource = dt;
            dv.DataBind();
        }
        if (dt.Where(c => c.Club_Ename.ToLower() == Club_Ename.ToLower()).SingleOrDefault().Login == "是")
        {
            ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('會員還在線上');", true);
        }
        else
        {   
            ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('修改成功');", true);
            string mailBody = "會員帳號:" + dt[0].Club_Ename + "<br>修改後信用:" + dt[0].Now_XinYong + "<br>修改前信用:" + dt[0].XinYong + "<br>修改時間:" + dt[0].UpdDate;
            SendMailByGmail(mailBody);
            logger.Information("修改額度資訊:{System},{Application},{DB環境},{會員帳號},{修改前額度},{修改後額度}", "Sovatool", "修改信任額度","舊公測", dt[0].Club_Ename,dt[0].Now_XinYong,dt[0].XinYong);
        }
       
    }
}