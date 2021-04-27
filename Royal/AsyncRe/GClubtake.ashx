<%@ WebHandler Language="C#" Class="GClubtake" %>

using System;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net.Http;
using System.Security.Policy;
using System.Security;
using System.Security.Cryptography;



public class GClubtake : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string RoyalAPI = System.Web.Configuration.WebConfigurationManager.AppSettings["GClub.API.ConnectionString"];
            string RoyalAPIkey = System.Web.Configuration.WebConfigurationManager.AppSettings["secret"];
            var id =  context.Request.QueryString.ToString();
            var Mytimestamp = GetTimestamp();
            var Mysignature = CreateMD5(id + Mytimestamp + RoyalAPIkey).ToLower();


            string QueryString = "channelId=RG&" + "username=" + id + "&" + "timestamp=" + Mytimestamp + "&" + "signature=" + Mysignature;



            string ResponseString = RoyalAPI + "/GClub/logoutgame.php?" + QueryString;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = client.GetAsync(ResponseString).Result;

                    response.EnsureSuccessStatusCode();
                    var responseBody =  response.Content.ReadAsStringAsync().Result;
                    context.Response.Write(responseBody);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", ex.Message);
                }
            }



        }
        catch (Exception ex)
        {
            //    LogModel log = new LogModel("WebSite", "api/LogOutGClubGame", "")
            //    {
            //        ErrorMessage = ex.ToString(),
            //        Requestdata = LogString
            //    };
            //    LogDAL.logger_exception(log);
            //    return Json(new { result = ex.ToString() });
            //}

            throw ex;
        }

    }
    /// <summary>
    /// MD5編譯
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string CreateMD5(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    /// < summary >
    /// 時間簽章
    /// </ summary >
    public static long GetTimestamp()
    {
        DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)((DateTime.UtcNow - Jan1st1970).TotalSeconds);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}