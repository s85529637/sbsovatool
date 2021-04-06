using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;



public class ChangeWebHallStatus
{
    public int status { get; set; }
    public int amount { get; set; }
    public string apiVersion { get; set; }
    public string msg { get; set; }
}
/******************************/
public class CashoutMember
{
    public int status { get; set; }
    public int amount { get; set; }
    public string apiVersion { get; set; }
    public string msg { get; set; }
}

/******************************/
/// <summary>
/// Gclubelib 的摘要描述
/// </summary>
public class Gclublib
{
    private static string Secret = string.IsNullOrEmpty(ConfigurationManager.AppSettings["GCLUBSecretKey"].ToString()) ? "B7Jupv4PDHmI" : ConfigurationManager.AppSettings["GCLUBSecretKey"].ToString();

    private static string TargetUrl = string.IsNullOrEmpty(ConfigurationManager.AppSettings["GCLUBAPI"].ToString()) ? "http://139.162.61.141" : ConfigurationManager.AppSettings["GCLUBAPI"].ToString();

    public Gclublib(){}
    /// <summary>
    /// 開啟或關閉場館API, status 只能帶 0（關閉） 或 1（開啟）
    /// </summary>
    /// <param name="istatus"> 0（關閉） 或 1（開啟）</param>
    /// <returns></returns>
    public static ChangeWebHallStatus ChangeWebHallStatus(int istatus)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        StringBuilder msg = new StringBuilder();
        const string Mothed = "GClub/changewebhallstatus.php";
        string strTimestamp = GetTimestamp().ToString();
        string signature = EncryptMD5(strTimestamp + Secret);
        string strParam = string.Format("channelId=Web&status={0}&timestamp={1}&signature={2}",istatus,strTimestamp, signature.ToLower());
        string Result = string.Empty;
        int StatusCode = 0;
        bool isok = true;
        ChangeWebHallStatus objChangeWebHallStatus = null;
        msg.Append("ChangeWebHallStatus()");
        msg.Append("&Mothed=");
        msg.Append(Mothed);
        msg.Append("&status=");
        msg.Append(istatus);
        msg.Append("&Secret=");
        msg.Append(Secret);
        msg.Append("&Timestamp=");
        msg.Append(strTimestamp);
        msg.Append("&signature=");
        msg.Append(signature);
        msg.Append("&strParam=");
        msg.Append(strParam);
        try
        {
            Result = Lib.GetDataGclub(string.Format("{0}/{1}", TargetUrl, Mothed), strParam, out StatusCode, "GET");
            msg.Append("&Result=");
            msg.Append(Result);
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            objChangeWebHallStatus = new ChangeWebHallStatus();
            objChangeWebHallStatus.status = -999;
            objChangeWebHallStatus.msg = ex.ToString();
            sw.Stop();//碼錶停止
                      //印出所花費的總豪秒數
            string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            msg.Append("&共執行=");
            msg.Append(result1.ToString());
            msg.Append("&StatusCode=");
            msg.Append(StatusCode);
            msg.Append("\r\n");
            Lib.WritLog("Gclublib.cs.ChangeWebHallStatus", msg.ToString());
            isok = false;
        }

        if (StatusCode != 200)
        {
            objChangeWebHallStatus = new ChangeWebHallStatus();
            objChangeWebHallStatus.status = -999;
            objChangeWebHallStatus.msg = string.Format("API發生未知例外，HTTP狀態碼傳回︰{0}", StatusCode);
            sw.Stop();//碼錶停止
            //印出所花費的總豪秒數
            string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            msg.Append("&共執行=");
            msg.Append(result1.ToString());
            msg.Append("&StatusCode=");
            msg.Append(StatusCode);
            msg.Append("\r\n");
            Lib.WritLog("Gclublib.cs.ChangeWebHallStatus", msg.ToString());
            isok = false;
        }

        if (objChangeWebHallStatus == null)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                try
                {
                    objChangeWebHallStatus = JsonConvert.DeserializeObject<ChangeWebHallStatus>(Result);
                }
                catch (Exception ex)
                {
                    msg.Append("Exception︰");
                    msg.Append(ex.ToString());
                    msg.Append("\r\n");
                    objChangeWebHallStatus = new ChangeWebHallStatus();
                    objChangeWebHallStatus.status = -999;
                    objChangeWebHallStatus.msg = ex.ToString();
                    sw.Stop();//碼錶停止
                              //印出所花費的總豪秒數
                    string result1 = sw.Elapsed.TotalMilliseconds.ToString();
                    msg.Append("&共執行=");
                    msg.Append(result1.ToString());
                    msg.Append("&StatusCode=");
                    msg.Append(StatusCode);
                    msg.Append("\r\n");
                    Lib.WritLog("Gclublib.cs.ChangeWebHallStatus", msg.ToString());
                    isok = false;
                }
            }
            else
            {
                objChangeWebHallStatus = new ChangeWebHallStatus();
                objChangeWebHallStatus.status = -999;
                objChangeWebHallStatus.msg = "API回傳結果為NULL";
                sw.Stop();//碼錶停止
                          //印出所花費的總豪秒數
                string result1 = sw.Elapsed.TotalMilliseconds.ToString();
                msg.Append("&共執行=");
                msg.Append(result1.ToString());
                msg.Append("&StatusCode=");
                msg.Append(StatusCode);
                msg.Append("\r\n");
                Lib.WritLog("Gclublib.cs.ChangeWebHallStatus", msg.ToString());
                isok = false;
            }
        }

        if (isok == true)
        {
            sw.Stop();//碼錶停止
            //印出所花費的總豪秒數
            string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            msg.Append("&共執行=");
            msg.Append(result1.ToString());
            msg.Append("&StatusCode=");
            msg.Append(StatusCode);
            msg.Append("\r\n");
            Lib.WritLog("Gclublib.cs.ChangeWebHallStatus", msg.ToString());
        }

        return objChangeWebHallStatus;
    }

    public static CashoutMember CashoutMember()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        StringBuilder msg = new StringBuilder();
        const string Mothed = "GClub/cashoutmember.php";
        string strTimestamp = GetTimestamp().ToString();
        //string signature = EncryptMD5("2003270401"+strTimestamp + Secret);
        // string strParam =string.Format("channelId=Web&timestamp={0}&signature={1}&username={2}", strTimestamp, signature, "2003270401");
        string signature = EncryptMD5(strTimestamp + Secret);
        string strParam =string.Format("channelId=Web&timestamp={0}&signature={1}", strTimestamp, signature.ToLower());
        string Result = string.Empty;
        int StatusCode = 0;
        bool isok = true;
        CashoutMember objCashoutMember = null;
        msg.Append("CashoutMember()");
        msg.Append("&Mothed=");
        msg.Append(Mothed);
        msg.Append("&Secret=");
        msg.Append(Secret);
        msg.Append("&Timestamp="); 
        msg.Append(strTimestamp);
        msg.Append("&signature=");
        msg.Append(signature);
        msg.Append("&strParam=");
        msg.Append(strParam);
        try
        {
            Result = Lib.GetDataGclub(string.Format("{0}/{1}", TargetUrl, Mothed), strParam, out StatusCode, "GET");
            msg.Append("&Result=");
            msg.Append(Result);
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            objCashoutMember = new CashoutMember();
            objCashoutMember.status = -999;
            objCashoutMember.msg = ex.ToString();
            sw.Stop();//碼錶停止
                      //印出所花費的總豪秒數
            string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            msg.Append("&共執行=");
            msg.Append(result1.ToString());
            msg.Append("&StatusCode=");
            msg.Append(StatusCode);
            msg.Append("\r\n");
            Lib.WritLog("Gclublib.cs.CashoutMember", msg.ToString());
            isok = false;
        }

        if (StatusCode != 200)
        {
            objCashoutMember = new CashoutMember();
            objCashoutMember.status = -999;
            objCashoutMember.msg =string.Format("API發生未知例外，HTTP狀態碼傳回︰{0}", StatusCode);
            sw.Stop();//碼錶停止
            //印出所花費的總豪秒數
            string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            msg.Append("&共執行=");
            msg.Append(result1.ToString());
            msg.Append("&StatusCode=");
            msg.Append(StatusCode);
            msg.Append("\r\n");
            Lib.WritLog("Gclublib.cs.CashoutMember", msg.ToString());
            isok = false;
        }

        if (objCashoutMember == null)
        {
            if (!string.IsNullOrEmpty(Result))
            {
                try
                {
                    objCashoutMember = JsonConvert.DeserializeObject<CashoutMember>(Result);
                }
                catch (Exception ex)
                {
                    msg.Append("Exception︰");
                    msg.Append(ex.ToString());
                    msg.Append("\r\n");
                    objCashoutMember = new CashoutMember();
                    objCashoutMember.status = -999;
                    objCashoutMember.msg = ex.ToString();
                    sw.Stop();//碼錶停止
                              //印出所花費的總豪秒數
                    string result1 = sw.Elapsed.TotalMilliseconds.ToString();
                    msg.Append("&共執行=");
                    msg.Append(result1.ToString());
                    msg.Append("&StatusCode=");
                    msg.Append(StatusCode);
                    msg.Append("\r\n");
                    Lib.WritLog("Gclublib.cs.CashoutMember", msg.ToString());
                    isok = false;
                }
            }else {
                objCashoutMember = new CashoutMember();
                objCashoutMember.status = -999;
                objCashoutMember.msg ="API回傳結果為NULL";
                sw.Stop();//碼錶停止
                          //印出所花費的總豪秒數
                string result1 = sw.Elapsed.TotalMilliseconds.ToString();
                msg.Append("&共執行=");
                msg.Append(result1.ToString());
                msg.Append("&StatusCode=");
                msg.Append(StatusCode);
                msg.Append("\r\n");
                Lib.WritLog("Gclublib.cs.CashoutMember", msg.ToString());
                isok = false;
            }
        }

        if (isok == true)
        {
            sw.Stop();//碼錶停止
            //印出所花費的總豪秒數
            string result1 = sw.Elapsed.TotalMilliseconds.ToString();
            msg.Append("&共執行=");
            msg.Append(result1.ToString());
            msg.Append("&StatusCode=");
            msg.Append(StatusCode);
            msg.Append("\r\n");
            Lib.WritLog("Gclublib.cs.CashoutMember", msg.ToString());
        }

        return objCashoutMember;
    }

    private static long GetTimestamp()
    {
        DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)((DateTime.UtcNow - Jan1st1970).TotalSeconds);
    }

   
    /// <summary>
    /// MD5加密(輸出base64的資料)
    /// </summary>
    /// <param name="plainText">資料</param>
    /// <returns></returns>
    public static string EncryptMD5(string plainText)
    {
        using (var cryptoMD5 = System.Security.Cryptography.MD5.Create())
        {
            //將字串編碼成 UTF8 位元組陣列
            var bytes = Encoding.UTF8.GetBytes(plainText);
            //取得雜湊值位元組陣列
            var hash = cryptoMD5.ComputeHash(bytes);
            //取得 MD5
            var md5 = BitConverter.ToString(hash)
                .Replace("-", String.Empty);
            return md5;
        }
    }
    
}