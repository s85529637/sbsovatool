<%@ WebHandler Language="C#" Class="WatchGs" %>

using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

public class WatchGs : IHttpHandler {

    /// <summary>
    /// 過濾掉防禦點及轉發的IP
    /// </summary>
    /// <param name="chkvalue">欲檢查的值</param>
    /// <returns></returns>
    private bool chkFORWARDEDIP(string chkvalue)
    {
        string str_FORWARDEDIP = System.Configuration.ConfigurationManager.AppSettings["FORWARDEDIP"];

        bool result = false;

        string[] temparyip = null;

        if (!string.IsNullOrEmpty(str_FORWARDEDIP))
        {
            if (str_FORWARDEDIP.IndexOf(",") > -1)
            {
                temparyip = str_FORWARDEDIP.Split(',');

                for (int i = 0; i < temparyip.Length; i++)
                {
                    if (chkvalue.Trim() == temparyip[i].Trim())
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                if (str_FORWARDEDIP == chkvalue)
                {
                    result = true;
                }
            }
        }

        return result;
    }

    //// <summary>
    /// 判断是否是IP地址格式 0.0.0.0
    /// </summary>
    /// <param name="str1">待判断的IP地址</param>
    /// <returns>true or false</returns>
    private bool IsIPAddress(string str1)
    {
        if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
        string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
        Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
        return regex.IsMatch(str1);
    }

    /// <summary>
    /// 將含有IP字串的全部分割成單一IP，並過濾內網IP及防禦點、轉發IP
    /// </summary>
    /// <param name="HeaderValue">欲檢查的字串</param>
    /// <returns>
    /// 回傳不為過濾內網IP或防禦點及轉發IP的IP
    /// </returns>
    private string Get_IP(string HeaderValue)
    {
        string User_HeaderValue = HeaderValue;

        string result = String.Empty;

        string[] temparyip = null;

        if (!string.IsNullOrEmpty(User_HeaderValue) && User_HeaderValue != String.Empty)
        {
            User_HeaderValue = User_HeaderValue.Replace(" ", "").Replace("'", "");

            if (User_HeaderValue.IndexOf(",;") > -1)
            {
                temparyip = User_HeaderValue.Split(",;".ToCharArray());
            }
            else if (User_HeaderValue.IndexOf(",") != -1)
            {
                temparyip = User_HeaderValue.Split(",".ToCharArray());
            }
            else if (User_HeaderValue.IndexOf(";") > -1)
            {
                temparyip = User_HeaderValue.Split(";".ToCharArray());
            }

            if (temparyip != null)
            {
                for (int i = 0; i < temparyip.Length; i++)
                {
                    if (IsIPAddress(temparyip[i])
                        && temparyip[i].Substring(0, 3) != "10."
                        && temparyip[i].Substring(0, 7) != "192.168"
                        && temparyip[i].Substring(0, 7) != "172.16."
                        && !chkFORWARDEDIP(temparyip[i].Trim()))
                    {
                        result = temparyip[i];
                        break;
                    }
                }
            }
            else
            {
                if (User_HeaderValue.IndexOf(".") > -1 && IsIPAddress(User_HeaderValue))
                {
                    result = !chkFORWARDEDIP(User_HeaderValue) ? User_HeaderValue : String.Empty;
                }
            }
        }

        return result;
    }

    private string GetIP()
    {
        try
        {
            string result = String.Empty;
            //string msg = string.Empty;
            //把所有可以抓到使用者真實IP的全部列出來
            string str_HTTP_CLIENT_IP = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]);
            string str_HTTP_X_FORWARDED_FOR = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            string str_HTTP_X_FORWARDED = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED"]);
            string str_HTTP_X_CLUSTER_CLIENT_IP = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"]);
            string str_HTTP_FORWARDED_FOR = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED_FOR"]);
            string str_HTTP_FORWARDED = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED"]);
            string str_HTTP_X_REAL_IP = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"]);
            string str_REMOTE_ADDR = Get_IP(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            string str_HTTP_VIA = Get_IP(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]);

            string[] temparyip = {
                                   str_HTTP_X_REAL_IP,
                                   str_HTTP_CLIENT_IP,
                                   str_HTTP_X_FORWARDED_FOR,
                                   str_HTTP_X_FORWARDED,
                                   str_HTTP_X_CLUSTER_CLIENT_IP,
                                   str_HTTP_FORWARDED_FOR,
                                   str_HTTP_FORWARDED,
                                   str_REMOTE_ADDR,
                                   str_HTTP_VIA };

            for (int i = 0; i < temparyip.Length; i++)
            {
                if (!string.IsNullOrEmpty(temparyip[i]))
                {
                    result = temparyip[i];
                    break;
                }
            }

            if (result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return result;

        }
        catch (Exception ex)
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
    }

    private bool chkIp(string ip)
    {
        string IPList = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["IPList"]) ? ",127.0.0.1," : System.Configuration.ConfigurationManager.AppSettings["IPList"];

        string myip = string.Format(",{0},", ip);

        bool isok = false;

        if (IPList.IndexOf(myip) > -1)
        {
            isok = true;
        }

        return isok;
    }

    public void ProcessRequest (HttpContext context) {

        string name = string.Empty;
        string msg = string.Empty;
        string ip = string.Empty;
        string strsql = string.Empty;
        int effect = 0;
        int rvalue = 0;
        name = string.IsNullOrEmpty(context.Request["Name"]) ? "" : context.Request["Name"];
        msg = string.IsNullOrEmpty(context.Request["Msg"]) ? "" : context.Request["Msg"];
        ip = GetIP();

        if (!chkIp(ip))
        {
            rvalue = -3;
            context.Response.Write(rvalue);
            return;
        }

        if (name != "" && msg != "")
        {
            try
            {
                strsql = "insert into GSMsg([Msg],[Ip],[Name],[CreateDate])values(@Msg,@Ip,@Name,@CreateDate)";
                SQLiteParameter[] cmdParms01;
                cmdParms01 = new SQLiteParameter[4];

                cmdParms01[0] = new SQLiteParameter("@Msg", DbType.String);
                cmdParms01[0].Value = msg;

                cmdParms01[1] = new SQLiteParameter("@Ip", DbType.String);
                cmdParms01[1].Value = ip;

                cmdParms01[2] = new SQLiteParameter("@Name", DbType.String);
                cmdParms01[2].Value = name;

                cmdParms01[3] = new SQLiteParameter("@CreateDate", DbType.String);
                cmdParms01[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                effect = DbHelperSQLite.ExecuteSql(strsql, cmdParms01);

                if (effect != 1)
                {
                    rvalue = 0;
                }else{
                    rvalue = 1;
                }
            }
            catch (Exception ex)
            {
                rvalue = -1;
            }
        }else {
            rvalue = -2;
        }

        context.Response.Write( rvalue.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}