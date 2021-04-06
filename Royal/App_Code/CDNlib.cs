using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/********************************************/
public class GetGameDomainUrlListData
{
    public string domain { get; set; }
    public bool isLive { get; set; }
    public string webSite { get; set; }
    public DateTime createTime { get; set; }
    public string modifyUser { get; set; }
    public DateTime modifyTime { get; set; }
    public string note { get; set; }

}

public class GetGameDomainUrlList
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public IList<GetGameDomainUrlListData> data { get; set; }

}
/************************************/
//[PUT]修改遊戲域名
public class CUpdateGameUrlResult
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public object data { get; set; }
}

public class CUpdateGameUrl
{
    public string domain { get; set; }
    public string webSite { get; set; }
    public bool isLive { get; set; }
    public string modifyUser { get; set; }
    public string note { get; set; }
}
/***************************************/
//[DELETE]刪除遊戲域名
public class CDelGameUrlResult
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public object data { get; set; }
}

public class CDelGameUrl
{
    public string domain { get; set; }
    public string webSite { get; set; }

}
/*************************************/
//[POST]新增遊戲域名
public class CAddGameUrlResult
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public object data { get; set; }
}

public class CAddGameUrl
{
    public string domain { get; set; }
    public string webSite { get; set; }
    public string modifyUser { get; set; }
    public string note { get; set; }

}
/********************************************/
/// <summary>
/// CDNlib 的摘要描述
/// </summary>
public class CDNlib
{
    private static string Url = string.IsNullOrEmpty(ConfigurationManager.AppSettings["CDNAPIURL"].ToString()) ? "http://172.16.10.17:8800" : ConfigurationManager.AppSettings["CDNAPIURL"].ToString();

    public CDNlib(){}

    /// <summary>
    /// [POST]新增遊戲域名 / 官網備用域名
    /// </summary>
    /// <param name="_Domain"></param>
    /// <param name="_WebSite"></param>
    /// <param name="_ModifyUser"></param>
    /// <param name="_Note"></param>
    /// <returns></returns>
    public static CAddGameUrlResult AddGameUrl(string _Domain,string _WebSite,string _ModifyUser,string _Note)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        const string Mothed = "api/Domain";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        string Domain = string.IsNullOrEmpty(_Domain) ? "" : _Domain;
        string WebSite = string.IsNullOrEmpty(_WebSite) ? "" : _WebSite;
        string ModifyUser = string.IsNullOrEmpty(_ModifyUser) ? "" : _ModifyUser;
        string Note = string.IsNullOrEmpty(_Note) ? "" : _Note;
        int StatusCode = 0;
        CAddGameUrl objAddGame = null;
        CAddGameUrlResult objResult = null;
        StringBuilder msg = new StringBuilder();

        if (Domain == "" || WebSite == "" || ModifyUser == "")
        {
            objResult = new CAddGameUrlResult();
            objResult.success = false;
            objResult.message = "[POST]新增遊戲域名失敗(參數為空)";
            objResult.code = "-999";
            msg.Append("[POST]新增遊戲域名失敗(參數為空)");
            msg.Append("\r\n");
            return objResult;
        }else {
            objAddGame = new CAddGameUrl();
            objAddGame.domain = Domain;
            objAddGame.modifyUser = ModifyUser;
            objAddGame.note = Note;
            objAddGame.webSite = WebSite;
            Request = JsonConvert.SerializeObject(objAddGame);
        }

        try
        {
            Result = Lib.GetDataCDN(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "POST");

            if (StatusCode != 200)
            {
                objResult = new CAddGameUrlResult();
                objResult.success = false;
                objResult.message = string.Format("[POST]新增遊戲域名失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[POST]新增遊戲域名失敗(API回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<CAddGameUrlResult>(Result);
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = "發生未知的例外";
            objResult = new CAddGameUrlResult();
            objResult.success = false;
            objResult.message = ex.ToString();
        }
        sw.Stop();//碼錶停止
        //印出所花費的總豪秒數
        string result1 = sw.Elapsed.TotalMilliseconds.ToString();
        msg.Append("共執行︰");
        msg.Append(result1.ToString());
        msg.Append("\r\n");
        msg.Append("StatusCode︰");
        msg.Append(StatusCode);
        msg.Append("\r\n");
        Lib.AlertWritLog("CDNlib.cs.CAddGameUrlResult", msg.ToString());
        return objResult;
    }

    /// <summary>
    /// [DELETE]刪除新增遊戲域名
    /// </summary>
    /// <param name="_Domain"></param>
    /// <param name="_WebSite"></param>
    public static CDelGameUrlResult DelGameUrl(string _Domain, string _WebSite)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        const string Mothed = "api/Domain";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        string Domain = string.IsNullOrEmpty(_Domain) ? "" : _Domain;
        string WebSite = string.IsNullOrEmpty(_WebSite) ? "" : _WebSite;
        int StatusCode = 0;
        CDelGameUrl objDelGameUrl = null;
        CDelGameUrlResult objResult = null;
        StringBuilder msg = new StringBuilder();

        if (Domain == "" || WebSite == "" )
        {
            objResult = new CDelGameUrlResult();
            objResult.success = false;
            objResult.message = "[DELETE]刪除新增遊戲域名失敗(參數為空)";
            objResult.code = "-999";
            msg.Append("[DELETE]刪除新增遊戲域名失敗(參數為空)");
            msg.Append("\r\n");
            return objResult;
        }
        else
        {
            objDelGameUrl = new CDelGameUrl();
            objDelGameUrl.domain = Domain;
            objDelGameUrl.webSite = WebSite;
            Request = JsonConvert.SerializeObject(objDelGameUrl);
        }

        try
        {
            Result = Lib.GetDataCDN(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "DELETE");

            if (StatusCode != 200)
            {
                objResult = new CDelGameUrlResult();
                objResult.success = false;
                objResult.message = string.Format("[DELETE]刪除新增遊戲域名失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[DELETE]刪除新增遊戲域名失敗(API回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<CDelGameUrlResult>(Result);
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = "發生未知的例外";
            objResult = new CDelGameUrlResult();
            objResult.success = false;
            objResult.message = ex.ToString();
        }
        sw.Stop();//碼錶停止
        //印出所花費的總豪秒數
        string result1 = sw.Elapsed.TotalMilliseconds.ToString();
        msg.Append("共執行︰");
        msg.Append(result1.ToString());
        msg.Append("\r\n");
        msg.Append("StatusCode︰");
        msg.Append(StatusCode);
        msg.Append("\r\n");
        Lib.AlertWritLog("CDNlib.cs.DelGameUrl", msg.ToString());
        return objResult;
    }

    /// <summary>
    /// [PUT]修改遊戲域名
    /// </summary>
    /// <param name="_Domain"></param>
    /// <param name="_WebSite"></param>
    /// <param name="_ModifyUser"></param>
    /// <param name="_Note"></param>
    /// <returns></returns>
    public static CUpdateGameUrlResult UpdateGameUrl(string _Domain, string _WebSite,string _IsLive, string _ModifyUser, string _Note)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        const string Mothed = "api/Domain";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        string Domain = string.IsNullOrEmpty(_Domain) ? "" : _Domain;
        string WebSite = string.IsNullOrEmpty(_WebSite) ? "" : _WebSite;
        string IsLive = string.IsNullOrEmpty(_IsLive) ? "" : _IsLive;
        string ModifyUser = string.IsNullOrEmpty(_ModifyUser) ? "" : _ModifyUser;
        string Note = string.IsNullOrEmpty(_Note) ? "" : _Note;
        int StatusCode = 0;
        CUpdateGameUrl objUpdateGameUrl = null;
        CUpdateGameUrlResult objResult = null;
        StringBuilder msg = new StringBuilder();

        if (Domain == "" || WebSite == "" || ModifyUser == "" )
        {
            objResult = new CUpdateGameUrlResult();
            objResult.success = false;
            objResult.message = "[PUT]修改遊戲域名(參數為空)";
            objResult.code = "-999";
            msg.Append("[PUT]修改遊戲域名(參數為空)");
            msg.Append("\r\n");
            return objResult;
        }
        else
        {
            objUpdateGameUrl = new CUpdateGameUrl();
            objUpdateGameUrl.domain = Domain;
            objUpdateGameUrl.webSite = WebSite;
            objUpdateGameUrl.isLive = IsLive.ToString().ToUpper() =="TRUE" ? true : false ;
            objUpdateGameUrl.modifyUser = "******";
            objUpdateGameUrl.note = Note;
            Request = JsonConvert.SerializeObject(objUpdateGameUrl);
        }

        try
        {
            Result = Lib.GetDataCDN(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "PUT");

            if (StatusCode != 200)
            {
                objResult = new CUpdateGameUrlResult();
                objResult.success = false;
                objResult.message = string.Format("[PUT]修改遊戲域名失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[PUT]修改遊戲域名失敗(API回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<CUpdateGameUrlResult>(Result);
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = "發生未知的例外";
            objResult = new CUpdateGameUrlResult();
            objResult.success = false;
            objResult.message = ex.ToString();
        }
        sw.Stop();//碼錶停止
        //印出所花費的總豪秒數
        string result1 = sw.Elapsed.TotalMilliseconds.ToString();
        msg.Append("共執行︰");
        msg.Append(result1.ToString());
        msg.Append("\r\n");
        msg.Append("StatusCode︰");
        msg.Append(StatusCode);
        msg.Append("\r\n");
        Lib.AlertWritLog("CDNlib.cs.UpdateGameUrl", msg.ToString());
        return objResult;
    }

    /// <summary>
    /// [GET]查詢域名
    /// </summary>
    /// <param name="_WebSite"></param>
    /// <param name="_System"></param>
    /// <param name="_IsLive"></param>
    public static GetGameDomainUrlList GetGameDomainUrlList(string _WebSite,string _System,string _IsLive)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        const string Mothed = "api/Domain";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        string WebSite = string.IsNullOrEmpty(_WebSite) ? "" : _WebSite;
        string System = string.IsNullOrEmpty(_System) ? "" : _System;
        string IsLive = string.IsNullOrEmpty(_IsLive) ? "" : _IsLive;
        int StatusCode = 0;
        string Apipara = string.Empty; 
        GetGameDomainUrlList objResult = null;
        StringBuilder msg = new StringBuilder();
        if (System == "" || WebSite == "" || IsLive == "")
        {
            objResult = new GetGameDomainUrlList();
            objResult.success = false;
            objResult.message = "[GET]查詢域名(參數為空)";
            objResult.code = "-999";
            return objResult;
        }else {
            if (IsLive.ToLower() != "all" && IsLive.ToLower() != "true" && IsLive.ToLower()!="false")
            {
                objResult = new GetGameDomainUrlList();
                objResult.success = false;
                objResult.message = "[GET]查詢域名(IsLive出現非法參數)";
                objResult.code = "-999";
                return objResult;
            }

            if (WebSite != "Direct" && WebSite != "Official")
            {
                objResult = new GetGameDomainUrlList();
                objResult.success = false;
                objResult.message = "[GET]查詢域名(WebSite出現非法參數)";
                objResult.code = "-999";
                return objResult;
            }

            if (System != "H1" && System != "Mini")
            {
                objResult = new GetGameDomainUrlList();
                objResult.success = false;
                objResult.message = "[GET]查詢域名(System出現非法參數)";
                objResult.code = "-999";
                return objResult;
            }
        }

        try
        {
            //string tmpstr = "{\"success\":true,\"code\":\"100\",\"message\":\"成功\",\"data\":[{\"id\":1016,\"domain\":\"https://www.google.com/D\",\"isLive\":true,\"webSite\":\"Direct-H1\",\"createTime\":\"2020-12-09T16:55:09.44\",\"modifyUser\":\"vincent2\",\"modifyTime\":\"2020-12-09T16:55:07.883\",\"note\":\"Note\"},{\"id\":1017,\"domain\":\"https://www.google.com/E\",\"isLive\":true,\"webSite\":\"Direct-H1\",\"createTime\":\"2020-12-09T16:55:12.99\",\"modifyUser\":\"vincent2\",\"modifyTime\":\"2020-12-09T16:55:11.42\",\"note\":\"Note\"},{\"id\":1031,\"domain\":\"https://www.google.com/Y\",\"isLive\":true,\"webSite\":\"Direct-H1\",\"createTime\":\"2020-12-11T10:33:21.11\",\"modifyUser\":\"vincent2\",\"modifyTime\":\"2020-12-11T10:33:16.783\",\"note\":\"Note\"}]}";
            //objResult = JsonConvert.DeserializeObject<GetGameDomainUrlList>(tmpstr);
            
            Apipara = string.Format("{0}/{1}/{2}", WebSite, System, IsLive.ToLower());
            Result = Lib.GetDataCDN(string.Format("{0}/{1}", Url, Mothed), Apipara, out StatusCode, "GET");
            if (StatusCode != 200)
            {
                objResult = new GetGameDomainUrlList();
                objResult.success = false;
                objResult.message = string.Format("[GET]查詢域名失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[GET]查詢域名失敗(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<GetGameDomainUrlList>(Result);
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = "發生未知的例外";
            objResult = new GetGameDomainUrlList();
            objResult.success = false;
            objResult.message = ex.ToString();
        }
        sw.Stop();//碼錶停止
        //印出所花費的總豪秒數
        string result1 = sw.Elapsed.TotalMilliseconds.ToString();
        msg.Append("共執行︰");
        msg.Append(result1.ToString());
        msg.Append("\r\n");
        msg.Append("StatusCode︰");
        msg.Append(StatusCode);
        msg.Append("\r\n");
        Lib.AlertWritLog("CDNlib.cs.GetGameDomainUrlList", msg.ToString());
        return objResult;
    }

    /// <summary>
    /// 檢查網址有效性
    /// </summary>
    /// <param name="strUrl"></param>
    /// <returns></returns>
    public static bool UrlCheck(string strUrl)
    {
        if (!strUrl.Contains("http://") && !strUrl.Contains("https://"))
        {
            strUrl = "http://" + strUrl;
        }

        try
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            myRequest.Method = "HEAD";
            myRequest.Timeout = 10000;  //超時時間10秒
            HttpWebResponse res = (HttpWebResponse)myRequest.GetResponse();
            return (res.StatusCode == HttpStatusCode.OK);
        }
        catch
        {
            return false;
        }
    }
}