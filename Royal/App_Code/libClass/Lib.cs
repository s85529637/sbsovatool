using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using PointCenterApi;

public static class Lib
{
    #region 初始化
    static Dictionary<string, int> GoldenGames = new Dictionary<string, int>() {
        {"12Ball", 2},
        {"Ninja", 3},
        {"MonkeyKing", 4},
        {"5PK", 7},
        {"SWarLord", 12},
        {"HorseRace", 13},
        {"Jungle", 16},
        {"CMonkey", 21},
        {"Circus", 24},
        {"MoneyCat", 58},
        {"GoldenBar", 65},
        {"FPlanet", 75},
        {"LDiamond", 105},
        {"Vikings", 106},
        {"FruitBar", 108},
        {"AParty", 109},
        {"FRoulette", 110},
        {"CrazyStone", 113},
        {"Sicbo", 115},
        {"FPCrab", 116},
        {"HUSA", 117},
        {"Super8A", 122},
        {"Super8B", 123},
        {"Party88", 126}
    };

    /// <summary>
    /// 將皇家電子遊戲代碼轉換為 H1 的遊戲代碼，設定在 Web.config
    /// </summary>
    /// <param name="GameId">皇家電子的 GameId</param>
    /// <returns></returns>
    public static String ReturnRoyalGameId(String GameId)
    {
        // 改由設定檔抓遊戲對應代碼
        var key = ConfigurationManager.AppSettings.AllKeys.Where(x => x == "RoyalGameId_" + GameId).FirstOrDefault();
        if (key != null)
        {
            GameId = ConfigurationManager.AppSettings[key];
        }

        //switch (GameId)
        //{
        //    case "1": GameId = "FortuneTai"; break; //FortuneThai(泰有錢)
        //    case "2": GameId = "MagicGem"; break; //MagicGem(魔法石)
        //    case "3": GameId = "Royal777"; break; //Royal777(皇家777)
        //    case "4": GameId = "LoveCity"; break; //LoveCity(慾望城市)
        //    case "5": GameId = "GChicken"; break; //GoldChicken(金雞報喜)
        //    case "6": GameId = "Pharoh"; break; //PHARAOH(法老王)
        //    case "7": GameId = "Alibaba"; break; //Alibaba(阿里巴巴)
        //    case "8": GameId = "LuckyFruit"; break; //Lucky Fruits(幸運水果)
        //    case "10": GameId = "RJungle"; break; //Jungle(動物叢林)
        //    case "11": GameId = "CHook"; break; //CaptainHook(虎克船長)
        //    case "12": GameId = "HUCA"; break; //HUCA(HUCA)
        //    case "14": GameId = "SweetCandy"; break; //SweetCandy(甜蜜糖果)
        //    case "16": GameId = "POPEYE"; break; //POPEYE(大力水手)
        //    case "17": GameId = "CDoctor"; break; //CrazyDoctor(瘋狂博士)
        //    case "18": GameId = "Nonstop"; break; // 永不停止
        //    case "19": GameId = "5Dragons"; break; //5Dragons(五龍爭霸)
        //    case "20": GameId = "GoldFish3"; break; //GoldFish3(金魚3)
        //    case "21": GameId = "72Changes"; break; //72Changes(七十二變)
        //    case "22": GameId = "CRabbit"; break; //CrazyRabbit(瘋狂免子)
        //    case "23": GameId = "Mermaid"; break; //Mermaid(美人魚)
        //    case "24": GameId = "Buffalo"; break; //Buffalo(荒野水牛)
        //    case "25": GameId = "WildPanda"; break; //WildPanda(竹林熊貓)
        //    case "26": GameId = "LuckyThai"; break; //(泰好運)
        //    case "27": GameId = "GodWealth"; break; //(財神到)
        //    case "28": GameId = "LDragon"; break; //(行運一條龍)
        //    case "29": GameId = "HUSA"; break; //(野蠻世界)
        //    default: break;
        //}

        //
        return GameId;
    }
    #endregion

    #region 前端處理
    public static void MsgBoxAndJump(Page page, string msg,string JumpTo)
    {
        if (msg == null) return;
        msg = string.Format("alert('{0}');location.href='{1}';", msg.Replace("'", @"\'"), JumpTo);
        OutputScript(page, msg);
    }
    public static void MsgBox(Page page, string msg)
    {
        if (msg == null) return;
        msg = string.Format("alert('{0}');", msg.Replace("'", @"\'"));
        OutputScript(page, msg);
    }
    public static void OutputScript(Page page, string sScript)
    {
        string format = @"<script type='text/javascript'>{0}</script>";
        format = string.Format(format, sScript);
        page.ClientScript.RegisterStartupScript(page.GetType(), "", format);
    }

    public static void MsgBox(UpdatePanel up, string msg)
    {
        if (msg == null) return;
        msg = string.Format("alert('{0}');", msg.Replace("'", @"\'"));
        OutputScript(up, msg);
    }
    public static void OutputScript(UpdatePanel up, string sScript)
    {
        //string format = @"<script type='text/javascript'>{0}</script>";
        //format = string.Format(format, sScript);
        ScriptManager.RegisterStartupScript(up, up.GetType(), "", sScript, true);
    }
    #endregion

    #region 資料取得
    public static string GetConfig(string sKey)
    {
        AppSettingsReader reader = new AppSettingsReader();
        return reader.GetValue(sKey, typeof(string)).ToString();
    }

    public static object GetConfig(string sKey, Type type)
    {
        AppSettingsReader reader = new AppSettingsReader();
        return reader.GetValue(sKey, type);
    }

    public static XmlNode GetXmlNode(string xFile, string xPath)
    {
        XmlDocument document = new XmlDocument();
        XmlTextReader reader = new XmlTextReader(xFile);
        document.Load(reader);
        return document.SelectSingleNode(xPath);
    }

    public static XmlNodeList GetXmlNodes(string xFile, string xPath)
    {
        XmlDocument document = new XmlDocument();
        XmlTextReader reader = new XmlTextReader(xFile);
        document.Load(reader);
        return document.SelectNodes(xPath);
    }

    /// <summary>
    /// 取得網址回應
    /// </summary>
    /// <param name="Url">網址</param>
    /// <param name="PostData">要傳送的資料</param>
    /// <returns></returns>
    public static string CallApi(string Url, string PostData)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
        string st = "";
        HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(Url);
        objWebRequest.Method = WebRequestMethods.Http.Post;
        objWebRequest.ContentType = "application/x-www-form-urlencoded";
        objWebRequest.ContentLength = byteArray.Length;
        objWebRequest.Timeout = 10000;
        try
        {
            using (Stream newStream = objWebRequest.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                newStream.Close();
            }
            using (HttpWebResponse res = (HttpWebResponse)objWebRequest.GetResponse())
            {
                using (StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    st = sr.ReadToEnd();
                    sr.Close();
                    res.Close();
                }
            }
        }
        catch
        {
            //objlog.writetoLog("Get_Web_Data", "Ccommunication.cs", temp_value, "通信發生例外", "訊息︰" + err.Message + "_來源︰" + err.StackTrace);
            return "{\"ErrorCode\":\"1\",\"ErrorInfo\":\"99\",\"ResultData\":\"\"}";
        }
        return st;
    }
    #endregion

    #region 資料轉換
    /// <summary>
    /// 轉換成黃金遊戲代號
    /// </summary>
    /// <param name="GameId">H1遊戲代碼</param>
    /// <returns></returns>
    public static int GetGoldenGameNo(string GameId)
    {
        int GoldenGameNo = 0;
        if (GoldenGames.ContainsKey(GameId))
        {
            GoldenGameNo = GoldenGames[GameId];
        }
        else
        {
            GoldenGameNo = -99;
        }

        return GoldenGameNo;
    }

    /// <summary>
    /// 轉換成H1遊戲代碼
    /// </summary>
    /// <param name="GameNo">黃金遊戲代號</param>
    /// <returns></returns>
    public static string GetGoldenGameId(int GameNo)
    {
        foreach (var item in GoldenGames)
        {
            if (item.Value.Equals(GameNo))
            {
                return item.Key;
            }
        }
        return "";
    }
    #endregion

    public static string GetDataGclub(string url, string content, out int _StatusCode, string Mothed = "POST")
    {
        string result = "";
        int StatusCode = 0;
        HttpWebResponse resp = null;
        StringBuilder msg = new StringBuilder("");
        msg.Append("Lib.cs.GetDataGclub()︰參數︰");
        msg.Append("方法︰");
        msg.Append(Mothed);
        msg.Append(" url︰");
        msg.Append(url);
        msg.Append(" content︰");
        msg.Append(content);
        HttpWebRequest req = null;
        try
        {
            if (Mothed.ToUpper() == "GET")
            {
                url = string.Format("{0}?{1}", url, content);
                req = (HttpWebRequest)WebRequest.Create(url);
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = Mothed;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                byte[] data = Encoding.UTF8.GetBytes(content);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
            }
            resp = (HttpWebResponse)req.GetResponse();
            StatusCode = (int)resp.StatusCode;
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            resp.Close();
            msg.Append(" Lib.cs.GetDataGclub()︰結果︰");
            msg.Append(" result︰");
            msg.Append(result);
        }
        catch (WebException we)
        {
            if (we.Response != null)
            {
                StatusCode = (int)((HttpWebResponse)we.Response).StatusCode;
            }
            msg.Append(" Lib.cs.GetDataGclub()︰WebException例外︰");
            msg.Append(we.ToString());
        }
        catch (Exception ex)
        {
            msg.Append(" Lib.cs.GetDataGclub()︰Exception例外︰");
            msg.Append(ex.ToString());
        }
        _StatusCode = StatusCode;
        msg.Append(" StatusCode︰");
        msg.Append(_StatusCode);
        Lib.WritLog(" Lib.cs.GetDataGclub()", msg.ToString());
        return result;
    }

    public static string GetDataCDN(string url, string content, out int _StatusCode, string Mothed = "POST")
    {
        string result = "";
        int StatusCode = 0;
        HttpWebResponse resp = null;
        StringBuilder msg = new StringBuilder("");
        msg.Append("Lib.cs.GetDataCDN()︰參數︰");
        msg.Append("方法︰");
        msg.Append(Mothed);
        msg.Append("url︰");
        msg.Append(url);
        msg.Append("content︰");
        msg.Append(content);
        HttpWebRequest req = null;
        try
        {
            if (Mothed.ToUpper() == "GET")
            {
                url = string.Format("{0}/{1}", url, content);
                req = (HttpWebRequest)WebRequest.Create(url);
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = Mothed;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                byte[] data = Encoding.UTF8.GetBytes(content);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
            }
            resp = (HttpWebResponse)req.GetResponse();
            StatusCode = (int)resp.StatusCode;
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            resp.Close();
            msg.Append("Lib.cs.GetDataCDN()︰結果︰");
            msg.Append("result︰");
            msg.Append(result);
        }
        catch (WebException we)
        {
            if (we.Response != null)
            {
                StatusCode = (int)((HttpWebResponse)we.Response).StatusCode;
            }
            msg.Append("Lib.cs.GetDataCDN()︰WebException例外︰");
            msg.Append(we.ToString());
        }
        catch (Exception ex)
        {
            msg.Append("Lib.cs.GetDataCDN()︰Exception例外︰");
            msg.Append(ex.ToString());
        }
        _StatusCode = StatusCode;
        msg.Append("StatusCode︰");
        msg.Append(_StatusCode);
        Lib.WritLog("Lib.cs.GetDataCDN()", msg.ToString());
        return result;
    }


    public static string GetDataToURLRTG(string url, string content,out int _StatusCode, string Mothed = "POST")
    {
        string result = "";
        int StatusCode = 0;
        HttpWebResponse resp = null;
        StringBuilder msg = new StringBuilder("");
        msg.Append("Lib.cs.GetDataToURL()︰參數︰");
        msg.Append("方法︰");
        msg.Append(Mothed);
        msg.Append("url︰");
        msg.Append(url);
        msg.Append("content︰");
        msg.Append(content);
        HttpWebRequest req = null;
        try
        {
            if (Mothed.ToUpper() == "GET")
            {
                url = string.Format("{0}?{1}", url, content);
                req = (HttpWebRequest)WebRequest.Create(url);
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = Mothed;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                byte[] data = Encoding.UTF8.GetBytes(content);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
            }
            resp = (HttpWebResponse)req.GetResponse();
            StatusCode =(int)resp.StatusCode;
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            resp.Close();
            msg.Append("Lib.cs.GetDataToURL()︰結果︰");
            msg.Append("result︰");
            msg.Append(result);
        }catch (WebException we) {
            if (we.Response != null)
            {
                StatusCode = (int)((HttpWebResponse)we.Response).StatusCode;
            }
            msg.Append("Lib.cs.GetDataToURL()︰WebException例外︰");
            msg.Append(we.ToString());
        }catch (Exception ex){ 
            msg.Append("Lib.cs.GetDataToURL()︰Exception例外︰");
            msg.Append(ex.ToString());
        }
        _StatusCode = StatusCode;
        msg.Append("StatusCode︰");
        msg.Append(_StatusCode);
        Lib.AlertWritLog("Lib.cs.GetDataToURLRTG()", msg.ToString());
        return result;
    }

    /// <summary>  
    /// 指定GET地址使用GET方式获取全部字符串  
    /// </summary>  
    /// <param name="url">请求后台地址</param>  
    /// <param name="content">GET提交数据内容(utf-8编码的)</param>  
    /// <returns></returns>  
    public static string GetDataToURL(string url, string content,string Mothed="POST")
    {
        string result = "";
        HttpWebResponse resp = null;
        StringBuilder msg = new StringBuilder("");
        msg.Append("Lib.cs.GetDataToURL()︰參數︰");
        msg.Append("方法︰");
        msg.Append(Mothed);
        msg.Append("url︰");
        msg.Append(url);
        msg.Append("content︰");
        msg.Append(content);
        HttpWebRequest req = null;
        try
        {
            if (Mothed.ToUpper() == "GET")
            {
                url = string.Format("{0}?{1}", url, content);
                req = (HttpWebRequest)WebRequest.Create(url);
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = Mothed;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "*/*";
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                byte[] data = Encoding.UTF8.GetBytes(content);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
            }
            resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            resp.Close();
            msg.Append("Lib.cs.GetDataToURL()︰結果︰");
            msg.Append("result︰");
            msg.Append(result);
        }
        catch (Exception ex)
        {
            //Lib.WritLog("Lib.cs.PostJson()",ex.ToString());
            msg.Append("Lib.cs.GetDataToURL()︰例外︰");
            msg.Append(ex.ToString());
        }

        Lib.WritLog("Lib.cs.GetDataToURL()", msg.ToString());

        return result;
    }

    /// <summary>  
    /// 指定Post地址使用POST 方式获取全部字符串  
    /// </summary>  
    /// <param name="url">请求后台地址</param>  
    /// <param name="content">Post提交数据内容(utf-8编码的)</param>
    /// <param name="Mothed"></param>
    /// <returns></returns>  
    public static string PostJson(string url, string content,string Mothed)
    {
        string result = "";
        HttpWebResponse resp = null;
        StringBuilder msg = new StringBuilder("");
        msg.Append("Lib.cs.PostJson()︰參數︰");
        msg.Append("方法︰");
        msg.Append(Mothed);
        msg.Append("url︰");
        msg.Append(url);
        msg.Append("content︰");
        msg.Append(content);
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = Mothed;
            req.ContentType = "application/json; charset=UTF-8";
            req.Accept = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            resp.Close();
            msg.Append("Lib.cs.PostJson()︰結果︰");
            msg.Append("result︰");
            msg.Append(result);
        }
        catch (Exception ex)
        {
            //Lib.WritLog("Lib.cs.PostJson()",ex.ToString());
            msg.Append("Lib.cs.PostJson()︰例外︰");
            msg.Append(ex.ToString());
        }

        Lib.WritLog("Lib.cs.PostJson()", msg.ToString());

        return result;
    }

    /// <summary>  
    /// 指定Post地址使用POST 方式获取全部字符串  
    /// </summary>  
    /// <param name="url">请求后台地址</param>  
    /// <param name="content">Post提交数据内容(utf-8编码的)</param>  
    /// <returns></returns>  
    public static string PostJson(string url, string content)
    {
        string result = "";
        HttpWebResponse resp = null;
        StringBuilder msg = new StringBuilder("");
        msg.Append("Lib.cs.PostJson()︰參數︰");
        msg.Append("url︰");
        msg.Append(url);
        msg.Append("content︰");
        msg.Append(content);
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json; charset=UTF-8";
            req.Accept = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            resp.Close();
            msg.Append("Lib.cs.PostJson()︰結果︰");
            msg.Append("result︰");
            msg.Append(result);
        }
        catch (Exception ex)
        {
            //Lib.WritLog("Lib.cs.PostJson()",ex.ToString());
            msg.Append("Lib.cs.PostJson()︰例外︰");
            msg.Append(ex.ToString());
        }

        Lib.WritLog("Lib.cs.PostJson()", msg.ToString());

        return result;
    }


    public static DataTable TxtConvertToDataTable(string File, string TableName, string delimiter)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        StreamReader s = new StreamReader(File, System.Text.Encoding.Default);
        //string ss = s.ReadLine();//skip the first line
        string[] columns = s.ReadLine().Split(delimiter.ToCharArray());
        ds.Tables.Add(TableName);
        foreach (string col in columns)
        {
            bool added = false;
            string next = "";
            int i = 0;
            while (!added)
            {
                string columnname = col + next;
                columnname = columnname.Replace("#", "");
                columnname = columnname.Replace("'", "");
                columnname = columnname.Replace("&", "");

                if (!ds.Tables[TableName].Columns.Contains(columnname))
                {
                    ds.Tables[TableName].Columns.Add(columnname.ToUpper());
                    added = true;
                }
                else
                {
                    i++;
                    next = "_" + i.ToString();
                }
            }
        }

        string AllData = s.ReadToEnd();
        string[] rows = AllData.Split("\n".ToCharArray());

        foreach (string r in rows)
        {
            string[] items = r.Split(delimiter.ToCharArray());
            ds.Tables[TableName].Rows.Add(items);
        }

        s.Close();

        dt = ds.Tables[0];

        return dt;
    }

    /// <summary>
    /// 記錄Log
    /// </summary>
    /// <param name="logtype">記錄標題</param>
    /// <param name="logString">記錄內容</param>
    public static void WritLog(string logtype, string logString)
    {
        string message = string.Format("{0}: {1}: {2}", logtype, logString, DateTime.Now.ToString("MM/dd HH:mm:ss"));

        string logpath =  ConfigurationManager.AppSettings["Logpath"] ;

        if (string.IsNullOrEmpty(logpath))
        {
            return;
        }
        else if (logpath=="")
        {
            return;
        }

        try
        {
            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            } 　

            using (StreamWriter writer = File.AppendText(logpath + DateTime.Now.ToString("yyyyMMddHH") + ".txt"))
            {
                writer.WriteLine(message);
            }
        }
        catch (Exception ex)
        { }
    }
    /// <summary>
    /// 警示系統專用的寫log
    /// </summary>
    /// <param name="logtype"></param>
    /// <param name="logString"></param>
    public static void AlertWritLog(string logtype, string logString)
    {
        string message = string.Format("{0}: {1}: {2}", logtype, logString, DateTime.Now.ToString("MM/dd HH:mm:ss"));

        string logpath = ConfigurationManager.AppSettings["AlertLogpath"];

        if (string.IsNullOrEmpty(logpath))
        {
            return;
        }
        else if (logpath == "")
        {
            return;
        }

        try
        {
            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }

            using (StreamWriter writer = File.AppendText(logpath + DateTime.Now.ToString("yyyyMMddHH") + ".txt"))
            {
                writer.WriteLine(message);
            }
        }
        catch (Exception ex)
        { }
    }
}