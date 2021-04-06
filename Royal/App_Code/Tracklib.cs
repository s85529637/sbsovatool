using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

///api/Background/BackgroundRecord
public class BackgroundRecordData
{
    public string name { get; set; }
    public string ip { get; set; }
    public DateTime createTime { get; set; }
    public string note { get; set; }

}

public class BackgroundRecord
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public IList<BackgroundRecordData> data { get; set; }
}

/****************************************/
public class UptRecord
{
    public int id { get; set; }

    public string inspector { get; set; }
}

public class RqUptRecord
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public IList<RqUptRecordData> data { get; set; }
}

public class RqUptRecordData
{
    public int id { get; set; }
    public string target { get; set; }
    public string type { get; set; }
    public DateTime updateTime { get; set; }
    public DateTime createTime { get; set; }
    public bool isTrace { get; set; }
    public string belong { get; set; }
}

/******************************/

public class TrackDataList
{
    public int id { get; set; }
    public string target { get; set; }
    public string type { get; set; }
    public DateTime updateTime { get; set; }
    public DateTime createTime { get; set; }
    public bool isTrace { get; set; }
    public string belong { get; set; }
}

public class TrackData
{
    public IList<TrackDataList> dataList { get; set; }
    public int dataTotalCount { get; set; }
}

public class RqGetTrack
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public TrackData data { get; set; }
}
/*
public class RqGetTrack
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public IList<RqGetTrackData> data { get; set; }
}

public class RqGetTrackData
{
    public int id { get; set; }
    public string target { get; set; }
    public string type { get; set; }
    public DateTime updateTime { get; set; }
    public DateTime createTime { get; set; }
    public bool isTrace { get; set; }
    public string belong { get; set; }
}
*/
/****************************************/

public class RqFindRecord
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public RqFindRecordData data { get; set; }
}

public class RqFindRecordDataList
{
    public int id { get; set; }
    public string ip { get; set; }
    public string account { get; set; }
    public string source { get; set; }
    public int alarmMode { get; set; }
    public DateTime loginTime { get; set; }
    public bool isCheck { get; set; }
    public string inspector { get; set; }
    public DateTime? checkTime { get; set; }
    public string note { get; set; }

}
public class RqFindRecordData
{
    public IList<RqFindRecordDataList> dataList { get; set; }
    public int dataTotalCount { get; set; }

}
/**
public class RqFindRecordData
{
    public int ID { get; set; }
    public string IP { get; set; }
    public string Account { get; set; }
    public int alarmMode { get; set; }
    public bool isCheck { get; set; }
    public string Source { get; set; }
    public DateTime LoginTime { get; set; }
    public string Inspector { get; set; }
    public string CheckTime { get; set; }
    public string note { get; set; }
}
*/
/****************************************/
public class DelTrack
{
    public int id { get; set; }
}

public class RqDelTrack
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    //public RqDelTrackData data { get; set; }
    public object data { get; set; }
}

public class RqDelTrackData
{
    public string Description { get; set; }
}

/**********************************************/
public class EditTrack
{
    public int id { get; set; }
    public bool isTrace { get; set; }
    public string belong { get; set; }
}

public class RqEditTrack
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    //public RqEditTrackData data { get; set; }
    public object data { get; set; }
}

public class RqEditTrackData
{
    public string Description { get; set; }
}

/**************************************/
public class CreateTrack{
    public string target { get; set; }
    public string type { get; set; }
    public string belong { get; set; }
}
public class RqCreateTrack
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    //public RqCreateTrackData data { get; set; }
    public object data { get; set; }
}

public class RqCreateTrackData
{
    public string Description { get; set; }
}

/*****************************************/

public class Create_Record{
    public string ip { get; set; }
    public string account { get; set; }
    public string source { get; set; } 
}

public class RqCreateRecord
{
    public bool success { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    //public RqCreateRecordData data { get; set; }
    public object data { get; set; }
}

public class RqCreateRecordData
{
    public string Description { get; set; }
}

/***************************************************************/


/// <summary>
/// Tracklib 的摘要描述
/// </summary>
public class Tracklib
{
    //private static string Url = "http://172.16.10.17";
    private static string Url = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertSysApi"].ToString()) ? "http://172.16.10.17" : ConfigurationManager.AppSettings["AlertSysApi"].ToString();
 
    /// <summary>
    /// 登入裝置 格式︰key value = 顯示名稱
    /// </summary>
    private static string LoginDevice = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LoginDevice"].ToString()) ? "Web=電腦版,LoadBoard=載版,MBrowser=手機瀏覽器,iphoneapp=Iphone,ipadapp=Ipad,android=Android" : ConfigurationManager.AppSettings["LoginDevice"].ToString();

    private static string _Manage = string.IsNullOrEmpty(ConfigurationManager.AppSettings["ManageIs"].ToString()) ? "manage" : ConfigurationManager.AppSettings["ManageIs"].ToString();

    private static string _TimeInterval = string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeInterval"].ToString()) ? "10000" : ConfigurationManager.AppSettings["TimeInterval"].ToString();

    private static string _AlertRows = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AlertRows"].ToString()) ? "200" : ConfigurationManager.AppSettings["AlertRows"].ToString();

    public static string TimeInterval
    {
        get
        {
            return _TimeInterval;
        }
    }

    /// <summary>
    /// 與key值LoginDevice相關聯，用於指出表代代理端的值
    /// </summary>
    public static string Manage
    {
        get {
            return _Manage;
        }
    }

    private static int _Row = int.Parse(_AlertRows);

    /// <summary>
    /// 每頁筆數
    /// </summary>
    public static int Row
    {
        get {
            return _Row;
        }
    }

    private static int _Page = 1;

    public static int Page
    {
        get
        {
            return _Page;
        }
    }
    /// <summary>
    /// 登入裝置 格式︰key value = 顯示名稱
    /// </summary>
    /// <returns></returns>
    public static string GetLoginDevice()
    {
        return LoginDevice;
    }

  
    /// <summary>
    /// 轉換登入裝置名稱
    /// </summary>
    public static string ConvertDevice(string _value, System.Web.UI.Page objPage)
    {
        string strdevice = string.Empty;
        string[] Device = null;
        string rvalue = string.Empty;
        int isok = 0;
        string value = _value.Replace(",", "");
        if (objPage.Session["LoginDeviceList"] == null)
        {
            strdevice = GetLoginDevice();
            objPage.Session["LoginDeviceList"] = strdevice;
        }
        else
        {
            try
            {
                strdevice = objPage.Session["LoginDeviceList"].ToString();
            }
            catch (Exception ex)
            {
                strdevice = GetLoginDevice();
                objPage.Session["LoginDeviceList"] = strdevice;
            }
        }

        if (strdevice.IndexOf(',') > -1)
        {
            Device = strdevice.Split(',');
            for (int j = 0; j < Device.Length; j++)
            {
                if (value.ToString().ToUpper() == Device[j].Split('=')[0].ToString().ToUpper())
                {
                    rvalue = Device[j].Split('=')[1];
                    isok++;
                    break;
                }
            }
        }
        else
        {
            if (value.ToString().ToUpper() == strdevice.Split('=')[0].ToString().ToUpper())
            {
                rvalue = strdevice.Split('=')[1];
                isok++;
            }
        }
        /*
        if (isok == 0)
        {
            rvalue = "無名稱";
        }*/

        return rvalue;
    }

    /// <summary>
    /// [POST]新增登入紀錄
    /// </summary>
    /// <param name="_Ip"></param>
    /// <param name="_Account"></param>
    /// <param name="_Source"></param>
    /// <returns></returns>
    public static string F_CreateRecord(string _Ip,string _Account,string _Source)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        string Ip =string.IsNullOrEmpty(_Ip)? "" : _Ip ;
        string Account = string.IsNullOrEmpty(_Account)? "" : _Account;
        string Source = string.IsNullOrEmpty(_Source) ? "" : _Source;
        const string Mothed = "api/account/Record";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        StringBuilder msg = new StringBuilder();
        Create_Record objrecord = new Create_Record();
        RqCreateRecord objResult = null;
        if (Ip == "" || Account == "" || Source == "")
        {
            Rvalue = "參數為空";
        }
        objrecord.account = Account;
        objrecord.ip = Ip;
        objrecord.source = Source;
        msg.Append("_Ip︰");
        msg.Append(_Ip);
        msg.Append("\r\n");
        msg.Append("_Account︰");
        msg.Append(_Account);
        msg.Append("\r\n");
        msg.Append("_Source︰");
        msg.Append(_Source);
        msg.Append("\r\n");
        try
        {
            msg.Append("Result︰");           
            Request = JsonConvert.SerializeObject(objrecord);
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), Request,out StatusCode, "POST");
            msg.Append(Result);
            msg.Append("\r\n");

            if (StatusCode != 200)
            {
                objResult = new RqCreateRecord();
                objResult.success = false;
                objResult.message = string.Format("新增登入紀錄失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[POST]新增登入紀錄失敗(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }else{
                objResult = JsonConvert.DeserializeObject<RqCreateRecord>(Result);
            }

            if (objResult != null)
            {
                if (objResult.success)
                {
                    Rvalue = "Success";
                }
                else
                {
                    if (string.IsNullOrEmpty(objResult.message))
                    {
                        Rvalue = "API回傳的訊息為空";
                    }else{
                        Rvalue = objResult.message.ToString();
                    } 
                }
            }else {
                Rvalue = "Json物件為NULL";
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = string.Format( "發生未知的例外︰{0}", ex.Message);
        }
        sw.Stop();//碼錶停止
        //印出所花費的總豪秒數
        string result1 = sw.Elapsed.TotalMilliseconds.ToString();
        msg.Append("共執行︰");
        msg.Append(result1.ToString());
        msg.Append("\r\n");
        msg.Append("StatusCode︰");
        msg.Append(StatusCode);
        Lib.AlertWritLog("Tracklib.cs.F_CreateRecord", msg.ToString());
        return Rvalue;

    }

    /// <summary>
    /// [GET]查詢登入紀錄(回傳Json字串)
    /// </summary>
    /// <param name="_Startpage"></param>
    /// <param name="_Rows"></param>
    /// <returns></returns>
    public static string FindRecordByJson(int _Startpage, int _Rows)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        bool isError = false;
        int Startpage = _Startpage;
        int Rows = _Rows;
        const string Mothed = "api/account/Record";
        string Request = string.Empty;
        string Result = string.Empty;
        int StatusCode = 0;
        StringBuilder msg = new StringBuilder();
        string RequestStr = string.Format("Page={0}&Rows={1}", _Startpage, _Rows);
        msg.Append("RequestStr︰");
        msg.Append(RequestStr);
        msg.Append("\r\n");
        try
        {
            //Result = Lib.GetDataToURL(string.Format("{0}/{1}", Url, Mothed), RequestStr, "GET");
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), RequestStr, out StatusCode, "GET");
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            isError = true;
            Result = ex.ToString();
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
        Lib.AlertWritLog("Tracklib.cs.FindRecord", msg.ToString());
        return Result;
    }

    /// <summary>
    /// [GET]查詢登入紀錄
    /// </summary>
    /// <param name="_Startpage"></param>
    /// <param name="_Rows"></param>
    /// <returns></returns>
    public static RqFindRecord FindRecord(int _Startpage, int _Rows,bool _IsCheck = false)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        int Startpage = _Startpage;
        int Rows = _Rows;
        const string Mothed = "api/account/Record";
        string Request = string.Empty;
        string Result = string.Empty;
        int StatusCode = 0;
        StringBuilder msg = new StringBuilder();
        RqFindRecord objResult = null;
        string RequestStr = string.Format("Page={0}&Rows={1}&IsCheck={2}",  _Startpage, _Rows, _IsCheck.ToString().ToLower());
        msg.Append("RequestStr︰");
        msg.Append(RequestStr);
        msg.Append("\r\n");
        try
        {
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), RequestStr, out StatusCode, "GET");

            if (StatusCode != 200)
            {
                objResult = new RqFindRecord();
                objResult.success = false;
                objResult.message = string.Format("查詢登入紀錄失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[GET]查詢登入紀錄(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<RqFindRecord>(Result);
            }
           
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            objResult = new RqFindRecord();
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
        Lib.AlertWritLog("Tracklib.cs.FindRecord", msg.ToString());
        return objResult;
    }

    /****************************************************************************/
    /// <summary>
    /// [POST]新增追蹤
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_type"></param>
    /// <param name="_belong"></param>
    /// <returns></returns>
    public static string CreateTrack(string _target, string _type, string _belong)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        string Target = string.IsNullOrEmpty(_target) ? "" : _target;
        string Type = string.IsNullOrEmpty(_type) ? "" : _type;
        string Belong = string.IsNullOrEmpty(_belong) ? "" : _belong;
        const string Mothed = "api/account/Track";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        CreateTrack ojbCreateTrack = new CreateTrack();
        RqCreateRecord objResult = null;
        StringBuilder msg = new StringBuilder();
        if (Target == "" || Type == "" )
        {
            Rvalue = "參數為空";
        }
        ojbCreateTrack.target = Target;
        ojbCreateTrack.type = Type;
        ojbCreateTrack.belong = Belong;
        msg.Append("_target︰");
        msg.Append(_target);
        msg.Append("\r\n");
        msg.Append("_type︰");
        msg.Append(_type);
        msg.Append("\r\n");
        msg.Append("_belong︰");
        msg.Append(_belong);
        msg.Append("\r\n");
        try
        {
            msg.Append("Result︰");
            Request = JsonConvert.SerializeObject(ojbCreateTrack);
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "POST");
            //Result = Lib.PostJson(string.Format("{0}/{1}", Url, Mothed), Request,"POST");
            msg.Append(Result);
            msg.Append("\r\n");
            if (StatusCode != 200)
            {
                objResult = new RqCreateRecord();
                objResult.success = false;
                objResult.message = string.Format("新增追蹤失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[POST]新增追蹤(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<RqCreateRecord>(Result);
            }
            
            if (objResult != null)
            {
                if (objResult.success)
                {
                    Rvalue = "Success"; 
                }
                else
                {
                    if (string.IsNullOrEmpty(objResult.message))
                    {
                        Rvalue = "API回傳的訊息為空";
                    }
                    else
                    {
                        Rvalue = objResult.message.ToString();
                    }
                }
            }else {
                Rvalue = "Json物件為NULL";
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = string.Format("發生未知的例外︰{0}", ex.Message);
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
        Lib.AlertWritLog("Tracklib.cs.CreateTrack", msg.ToString());
        return Rvalue;
    }
    /// <summary>
    /// [GET]查詢追蹤清單
    /// </summary>
    /// <returns></returns>
    public static RqGetTrack GetTrack(Page objpage,int Page,int Rows,bool IsTrace)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        const string Mothed = "api/account/Track";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        string Apipara = string.Empty;
        CreateTrack ojbCreateTrack = new CreateTrack();
        RqGetTrack objResult = null;
        StringBuilder msg = new StringBuilder();
        try
        {
            Apipara = string.Format("Page={0}&Rows={1}&IsTrace={2}",Page,Rows,IsTrace);
            //Result = Lib.GetDataToURL(string.Format("{0}/{1}", Url, Mothed),"", "GET");
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed ), Apipara, out StatusCode, "GET");

            if (StatusCode != 200)
            {
                objResult = new RqGetTrack();
                objResult.success = false;
                objResult.message = string.Format("查詢追蹤清單失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[GET]查詢追蹤清單(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<RqGetTrack>(Result);
                objpage.Session["TrackData"] = objResult;
            }
           
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = "發生未知的例外";
            objResult = new RqGetTrack();
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
        Lib.AlertWritLog("Tracklib.cs.GetTrack", msg.ToString());
        return objResult;
    }

    /// <summary>
    /// [PUT]修改追蹤 
    /// </summary>
    /// <param name="_Id"></param>
    /// <param name="_IsTrue"></param>
    /// <param name="_Belong"></param>
    /// <returns></returns>
    public static string EditTrack(int _Id,bool _IsTrue,string _Belong)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        int Id = _Id;
        bool IsTrue =  _IsTrue;
        string Belong = string.IsNullOrEmpty(_Belong) ? "" : _Belong;
        const string Mothed = "api/account/Track";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        EditTrack ojbEditTrack = new EditTrack();
        RqEditTrack objResult = null;
        StringBuilder msg = new StringBuilder();
        if (Belong == "")
        {
            Rvalue = "參數為空";
        }
        ojbEditTrack.id = Id;
        ojbEditTrack.isTrace = _IsTrue;
        ojbEditTrack.belong = Belong;
        msg.Append("_Id︰");
        msg.Append(_Id);
        msg.Append("\r\n");
        msg.Append("_IsTrue︰");
        msg.Append(_IsTrue);
        msg.Append("\r\n");
        msg.Append("_Belong︰");
        msg.Append(_Belong);
        msg.Append("\r\n");
        try
        {
            msg.Append("Result︰");
            Request = JsonConvert.SerializeObject(ojbEditTrack);
            //Result = Lib.PostJson(string.Format("{0}/{1}", Url, Mothed), Request,"PUT");
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "PUT");
            msg.Append(Result);
            msg.Append("\r\n");
            if (StatusCode != 200)
            {
                objResult = new RqEditTrack();
                objResult.success = false;
                objResult.message = string.Format("修改追蹤失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[PUT]修改追蹤(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<RqEditTrack>(Result);
            }
           
            if (objResult != null)
            {
                if (objResult.success)
                {
                    Rvalue = "Success";
                }
                else
                {
                    if (string.IsNullOrEmpty(objResult.message))
                    {
                        Rvalue = "API回傳的訊息為空";
                    }
                    else
                    {
                        Rvalue = objResult.message.ToString();
                    }
                }
            }
            else
            {
                Rvalue = "Json物件為NULL";
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = string.Format("發生未知的例外︰{0}", ex.Message);
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
        Lib.AlertWritLog("Tracklib.cs.EditTrack", msg.ToString());
        return Rvalue;
    }
    /// <summary>
    /// [DELETE]刪除追蹤
    /// </summary>
    /// <param name="_Id"></param>
    /// <returns></returns>
    public static string DelTrack(int _Id)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        int Id = _Id;
        const string Mothed = "api/account/Track";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        DelTrack ojbDelTrack = new DelTrack();
        RqDelTrack objResult = null;
        StringBuilder msg = new StringBuilder();
        ojbDelTrack.id = Id;
        msg.Append("_Id︰");
        msg.Append(_Id);
        msg.Append("\r\n"); 
        try
        {
            msg.Append("Result︰");
            Request = JsonConvert.SerializeObject(ojbDelTrack);
            //Result = Lib.PostJson(string.Format("{0}/{1}", Url, Mothed), Request, "DELETE");
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "DELETE");
            msg.Append(Result);
            msg.Append("\r\n");

            if (StatusCode != 200)
            {
                objResult = new RqDelTrack();
                objResult.success = false;
                objResult.message = string.Format("刪除追蹤失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[DELETE]刪除追蹤(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<RqDelTrack>(Result);
            }
 
            if (objResult != null)
            {
                if (objResult.success)
                {
                    Rvalue = "Success";
                }
                else
                {
                    if (string.IsNullOrEmpty(objResult.message))
                    {
                        Rvalue = "API回傳的訊息為空";
                    }
                    else
                    {
                        Rvalue = objResult.message.ToString();
                    }
                }
            }
            else
            {
                Rvalue = "Json物件為NULL";
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = string.Format("發生未知的例外︰{0}", ex.Message);
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
        Lib.AlertWritLog("Tracklib.cs.DelTrack", msg.ToString());
        return Rvalue;
    }
    /// <summary>
    /// [PUT]取消警示
    /// </summary>
    /// <param name="_Id"></param>
    /// <param name="_Inspector"></param>
    /// <returns></returns>
    public static string UptRecord(int _Id, string _Inspector)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        int Id = _Id;
        string Inspector = _Inspector;
        const string Mothed = "api/account/Record";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        UptRecord ojbUptRecord = new UptRecord();
        RqUptRecord objResult = null;
        StringBuilder msg = new StringBuilder();
        
        ojbUptRecord.id = Id;
        ojbUptRecord.inspector = Inspector;

        msg.Append("_Id︰");
        msg.Append(_Id);
        msg.Append("_Inspector︰");
        msg.Append(_Inspector);
        msg.Append("\r\n");
        try
        {
            msg.Append("Result︰");
            Request = JsonConvert.SerializeObject(ojbUptRecord);
            //Result = Lib.PostJson(string.Format("{0}/{1}", Url, Mothed), Request, "PUT");
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), Request, out StatusCode, "PUT");
            msg.Append(Result);
            msg.Append("\r\n");
            if (StatusCode != 200)
            {
                objResult = new RqUptRecord();
                objResult.success = false;
                objResult.message = string.Format("取消警示失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[PUT]取消警示(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<RqUptRecord>(Result);
            }
            
            if (objResult != null)
            {
                if (objResult.success)
                {
                    Rvalue = "Success";
                }
                else
                {
                    if (string.IsNullOrEmpty(objResult.message))
                    {
                        Rvalue = "API回傳的訊息為空";
                    }
                    else
                    {
                        Rvalue = objResult.message.ToString();
                    }
                }
            }
            else
            {
                Rvalue = "Json物件為NULL";
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = string.Format("發生未知的例外︰{0}", ex.Message);
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
        Lib.AlertWritLog("Tracklib.cs.UptRecord", msg.ToString());
        return Rvalue;
    }

    /// <summary>
    /// [GET]查詢今日代理紀錄成功
    /// </summary>
    /// <returns></returns>
    public static BackgroundRecord GetBackgroundRecord()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
        sw.Reset();//碼表歸零
        sw.Start();//碼表開始計時
        const string Mothed = "api/Background/BackgroundRecord";
        string Request = string.Empty;
        string Result = string.Empty;
        string Rvalue = string.Empty;
        int StatusCode = 0;
        BackgroundRecord objResult = null;
        StringBuilder msg = new StringBuilder();
        try
        {
            Result = Lib.GetDataToURLRTG(string.Format("{0}/{1}", Url, Mothed), "", out StatusCode, "GET");

            if (StatusCode != 200)
            {
                objResult = new BackgroundRecord();
                objResult.success = false;
                objResult.message = string.Format("查詢今日代理紀錄失敗(API回應碼為︰{0})", StatusCode);
                objResult.code = "-999";
                msg.Append(string.Format("[GET]查詢今日代理紀錄(回應碼為︰{0})", StatusCode));
                msg.Append("\r\n");
            }
            else
            {
                objResult = JsonConvert.DeserializeObject<BackgroundRecord>(Result);
            }
        }
        catch (Exception ex)
        {
            msg.Append("Exception︰");
            msg.Append(ex.ToString());
            msg.Append("\r\n");
            Rvalue = "發生未知的例外";
            objResult = new BackgroundRecord();
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
        Lib.AlertWritLog("Tracklib.cs.GetBackgroundRecord", msg.ToString());
        return objResult;
    }
}