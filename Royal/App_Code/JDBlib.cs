using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using Newtonsoft.Json;
/*******************************************/
public class MemberActionData
{
    public int id { get; set; }
    public string 名稱 { get; set; }
    public string 輸入 { get; set; }
    public string 輸出 { get; set; }
    public int 狀態碼 { get; set; }
    public string 訊息 { get; set; }
    public DateTime 建立時間 { get; set; }
    public string JDB序號 { get; set; }
    public string 執行內容 { get; set; }
}

public class MemberAction
{
    public int Status { get; set; }
    public string Description { get; set; }
    public IList<MemberActionData> Data { get; set; }

}
/*******************************************/

public class RestartAllServer
{
    public int Status { get; set; }
    public string Description { get; set; }
    public string Data { get; set; }
}

/*****************************************/
public class SetJdbUrlConfig
{
    public int Status { get; set; }
    public string Description { get; set; }
    public string Data { get; set; }

}
/*******************************************/
public class JdbUrlConfigData
{
    public string 內容 { get; set; }
    public string 註解 { get; set; }

}
public class JdbUrlConfig
{
    public int Status { get; set; }
    public string Description { get; set; }
    public IList<JdbUrlConfigData> Data { get; set; }
}

/******************************************/
public class MemberQueueLogData
{
    public int 開分狀態 { get; set; }
    public int 洗分狀態 { get; set; }
    public string H1序號 { get; set; }
    public string JDB序號 { get; set; }
    public DateTime jqu_create_datetime { get; set; }
    public string 遊戲ID { get; set; }
    public double 剩下餘額 { get; set; }
    public double 下注金額 { get; set; }
    public double 彩金 { get; set; }
    public double 輸贏 { get; set; }
    public string 下注次數 { get; set; }

}
public class MemberQueueLog
{
    public int Status { get; set; }
    public string Description { get; set; }
    public IList<MemberQueueLogData> Data { get; set; }

}


/********************************************/
public class RankData
{
    public string rank { get; set; }
    public string uid { get; set; }
    public string displayName { get; set; }
    public string amount { get; set; }
    public string parent { get; set; }

}
public class AwardData
{
    public string awardId { get; set; }
    public string awardName { get; set; }
    public IList<RankData> rankData { get; set; }

}
public class ResultData
{
    public string activityDate { get; set; }
    public string activityNo { get; set; }
    public string activityName { get; set; }
    public IList<AwardData> awardData { get; set; }
    public string status { get; set; }
    public string description { get; set; }

}

/********************************************************/


/// <summary>
/// JDBlib 的摘要描述
/// </summary>
public class JDBlib
{

    public JDBlib(string _action)
    {
        this.JDBURL = ConfigurationManager.AppSettings["JDBAPIURL"] ?? "http://210.64.214.189:12000/Index/LobbyAPI";
        this.action = _action;
        this.cp = ConfigurationManager.AppSettings["JDB.company"] ?? "Royal";
        this.system = ConfigurationManager.AppSettings["JDB.system"] ?? "H1";
        this.webid = ConfigurationManager.AppSettings["JDB.webid"] ?? "H1AG";
    }

    public JDBlib(){ }
    /// <summary>
    /// 調用方法
    /// </summary>
    public string JDBURL { get; set; }
    /// <summary>
    /// 調用方法
    /// </summary>
    public string action { get; set; }
    /// <summary>
    /// 公司代碼
    /// </summary>
    public string cp { get; set; }
    /// <summary>
    /// 系統代碼
    /// </summary>
    public string system { get; set; }
    /// <summary>
    /// 網站代碼
    /// </summary>
    public string webid { get; set; }


    public class JDBResponse
    {
        public string status { get; set; }
        public string description { get; set; }
    }

    /// <summary>
    /// 用於JDB回傳會員是否在線
    /// </summary>
    public class JDBOnline
    {
        public bool isonline { get; set; }
        public string status { get; set; }
        public string description { get; set; }
    }

    public class Accountitem
    {
        public string playerid { get; set; }
        public string gameid { get; set; }
        public string playeraccount { get; set; }
        public string stakescore { get; set; }
        public string accountscore { get; set; }
        public string rows { get; set; }
        public string maxdatetime { get; set; }
        public string jackpot { get; set; }
        public string sessionid { get; set; }
        public string jdbsessionid { get; set; }
    }

    public class Accountlist
    {
        public List<Accountitem> data { get; set; }
        public string status { get; set; }
        public string description { get; set; }
    }

   

    /// <summary>
    /// 方法踢單一會員
    /// </summary>
    /// <param name="jdbname"></param>
    /// <param name="Status"></param>
    /// <returns></returns>
    public string kickMember(string jdbname, ref string Status)
    {
        JDBResponse objmbr = null;

        string strjsoin = "{\"action\":\"" + this.action + "\",\"cp\":\"" + this.cp + "\",\"system\":\"" + this.system + "\",\"webid\":\"" + this.webid + "\",\"uid\":\"" + jdbname + "\"}";

        string description = string.Empty;

        try
        {
            objmbr = JsonConvert.DeserializeObject<JDBResponse>(Lib.PostJson(this.JDBURL, strjsoin));

            if (objmbr.status.Equals("1"))
            {
                Status = "1";
            }
            else
            {
                Status = "0";
            }

            description = objmbr.description;
        }
        catch (Exception ex)
        {
            Lib.WritLog("JDBlib.kickMember()",ex.ToString());
            //Logs.WriteLog("kickMember︰" + ex.ToString(), Session["UserName"].ToString());
        }

        return description;
    }

    /// <summary>
    /// 方法踢單一會員
    /// </summary>
    /// <param name="jdbname"></param>
    /// <param name="Status"></param>
    /// <returns></returns>
    public string kickALLMember(ref string Status)
    {
        JDBResponse objmbr = null;

        string description = string.Empty;

        string strjsoin = "{\"action\":\"" + this.action + "\",\"cp\":\"" + this.cp + "\",\"system\":\"" + this.system + "\",\"webid\":\"" + this.webid + "\"}";

        try
        {
            objmbr = JsonConvert.DeserializeObject<JDBResponse>(Lib.PostJson(this.JDBURL, strjsoin));

            if (objmbr.status.Equals("1"))
            {
                Status = "1";
            }
            else
            {
                Status = "0";
            }

            description = objmbr.description;
        }
        catch (Exception ex)
        {
            description = ex.ToString();
            Lib.WritLog("JDBlib.kickALLMember()", ex.ToString());
            //Logs.WriteLog("kickMember︰" + ex.ToString(), Session["UserName"].ToString());
        }

        return description;
    }

    /// <summary>
    /// 取得會員在JDB是否在線的狀態
    /// </summary>
    /// <param name="data">會員的id</param>
    /// <returns></returns>
    public JDBOnline CheckMemberStatus(string data)
    {
        string strjson = string.Format("{{\"action\":\"{4}\",\"cp\":\"{0}\",\"system\":\"{1}\",\"webid\":\"{2}\",\"uid\":\"{3}\"}}", this.cp, this.system, this.webid, data, this.action);

        string Result = Lib.PostJson(this.JDBURL, strjson);

        Lib.WritLog("JDBlib.CheckMemberStatus()", string.Format("in:{0},out:{1}", strjson, Result));

        JDBOnline objresult;

        try
        {
            objresult = JsonConvert.DeserializeObject<JDBOnline>(Result);

        } catch (Exception ex) {

            objresult = new JDBOnline();

            objresult.status = "-999";

            objresult.description = ex.ToString();

            Lib.WritLog("JDBlib.CheckMemberStatus()", ex.ToString());
        }

        return objresult;
    }

    /// <summary>
    /// 取得JDB重算帳務
    /// </summary>
    /// <param name="sessionid">開分號</param>
    /// <param name="uid">player id</param>
    /// <returns></returns>
    public Accountlist GetReCountAccount(string sessionid,string uid)
    {
        string strjson = string.Format("{{\"action\":\"{4}\",\"cp\":\"{0}\",\"system\":\"{1}\",\"webid\":\"{2}\",\"uid\":\"{5}\",\"sessionid\":\"{3}\"}}", this.cp, this.system, this.webid, sessionid, this.action,uid);

        string Result = Lib.PostJson(this.JDBURL, strjson);

        Lib.WritLog("JDBib.cs.GetReCountAccount()", string.Format("In:{0};Out{1}", strjson, Result));

        Accountlist objresult;

        try
        {
            objresult = JsonConvert.DeserializeObject<Accountlist>(Result);

        }
        catch (Exception ex)
        {

            objresult = new Accountlist();

            objresult.status = "-999";

            objresult.description = ex.ToString();

            Lib.WritLog("JDBib.cs.GetReCountAccount()", ex.ToString());
        }

        return objresult;
    }


    /// <summary>
    /// 取得比賽結果資料
    /// </summary>
    /// <param name="_param">回傳調用API的參數</param>
    /// <param name="_result">回傳調用API的Json結果</param>
    /// <returns></returns>
    public ResultData GetGameRaceResult(string strdate,out string _param,out string _result)
    {
        string day1 = strdate;

        string day2 = strdate;

        //string day1 = "20200729";

        //string day2 = "20200730";

        //範例︰{"action":"18","cp":"Royal","system":"H1","webid":"H1AG","starttime":"20200727","endtime":"20200728"}
        string strjson = string.Format("{{\"action\":\"{0}\",\"cp\":\"Royal\",\"system\":\"H1\",\"webid\":\"H1AG\",\"starttime\":\"{1}\",\"endtime\":\"{2}\"}}",this.action, day1, day2);

        _param = strjson;

        string Result = Lib.PostJson(this.JDBURL, strjson);

        _result = Result;

        ResultData JResult;

        try
        {
            JResult = JsonConvert.DeserializeObject<ResultData>(Result);
        }
        catch (Exception ex)
        {

            JResult = new ResultData();

            JResult.status = "-999";

            List<AwardData> objAwardData = new List<AwardData>();

            JResult.awardData = objAwardData;

            JResult.description = ex.ToString();

            Lib.WritLog("JDBib.cs.GetGameRaceResult()", ex.ToString());
        }

        return JResult;
    }

    string LogUrl = string.IsNullOrEmpty(ConfigurationManager.AppSettings["NewPointCenterApiLogs"]) ? "" : ConfigurationManager.AppSettings["NewPointCenterApiLogs"];

    public System.Data.DataTable ConvertMemberActionLogToDataTable()
    {
        System.Data.DataTable dtMemberQueue = new System.Data.DataTable();
        dtMemberQueue.Columns.Add("id", typeof(string));      
        dtMemberQueue.Columns.Add("名稱", typeof(string));    
        dtMemberQueue.Columns.Add("輸入", typeof(string));    
        dtMemberQueue.Columns.Add("輸出", typeof(string));     
        dtMemberQueue.Columns.Add("狀態碼", typeof(string));  
        dtMemberQueue.Columns.Add("訊息", typeof(string));                  
        dtMemberQueue.Columns.Add("建立時間", typeof(string));         
        dtMemberQueue.Columns.Add("JDB序號", typeof(string));           
        dtMemberQueue.Columns.Add("執行內容", typeof(string));               
        return dtMemberQueue;
    }

    /// <summary>
    /// 取得會員活動LOG
    /// </summary>
    /// <param name="_MemberAccount">會員帳號</param>
    /// <param name="_StartTime">開始時間</param>
    /// <param name="_EndTime">結束時間</param>
    /// <param name="_MemberUid">會員UID</param>
    /// <param name="_JdbSessionId">JdbSessionId</param>
    /// <param name="_API_Name">API名稱</param>
    public MemberAction GetMemberActionLog(string _MemberAccount,string _StartTime,string _EndTime,string _MemberUid,string _JdbSessionId,string _API_Name)
    {
        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        s.Start();//開始計時
        const string MethodURL = "log/GetMemberActionLog";
        string MemberAccount = string.IsNullOrEmpty(_MemberAccount) ? "" : _MemberAccount;
        string StartTime = string.IsNullOrEmpty(_StartTime) ? "" : _StartTime;
        string EndTime = string.IsNullOrEmpty(_EndTime) ? "" : _EndTime;
        string MemberUid = string.IsNullOrEmpty(_MemberUid) ? "" : _MemberUid;
        string JdbSessionId = string.IsNullOrEmpty(_JdbSessionId) ? "" : _JdbSessionId;
        string API_Name = string.IsNullOrEmpty(_API_Name) ? "" : _API_Name;
        string Content = string.Empty;
        string JsonResult = string.Empty;
        DateTime tmpStartTime;
        DateTime tmpEndTime;
        MemberAction objMemberAction = null;
        System.Text.StringBuilder logs = new System.Text.StringBuilder();
        logs.Append("_MemberAccount︰");
        logs.Append(MemberAccount);
        logs.Append("\r\n");
        logs.Append("_StartTime︰");
        logs.Append(StartTime);
        logs.Append("\r\n");
        logs.Append("_EndTime︰");
        logs.Append(EndTime);
        logs.Append("\r\n");
        logs.Append("_MemberUid︰");
        logs.Append(MemberUid);
        logs.Append("\r\n");
        logs.Append("_JdbSessionId︰");
        logs.Append(JdbSessionId);
        logs.Append("\r\n");
        logs.Append("_API_Name︰");
        logs.Append(API_Name);
        logs.Append("\r\n");
        if ((MemberAccount == "" &&  MemberUid=="") || StartTime == "" || EndTime == "")
        {
            logs.Append("缺少必要參數!!");
            logs.Append("共執行︰");
            s.Stop();
            logs.Append((s.ElapsedMilliseconds).ToString());
            logs.Append("毫秒");
        }else{
            if (!DateTime.TryParse(StartTime, out tmpStartTime) || !DateTime.TryParse(EndTime, out tmpEndTime))
            {
                logs.Append("參數型態錯誤!!");
                logs.Append("共執行︰");
                s.Stop();
                logs.Append((s.ElapsedMilliseconds).ToString());
                logs.Append("毫秒");
            }else {

                Content = string.Format("MemberAccount={0}&StartTime={1}&EndTime={2}&MemberUid={3}&JdbSessionId={4}&API_Name={5}", 
                                        MemberAccount, StartTime, EndTime,MemberUid,JdbSessionId, API_Name);
                JsonResult = Lib.GetDataToURL(
                    string.Format("{0}/{1}", LogUrl, MethodURL), Content);
                try
                {
                    objMemberAction = JsonConvert.DeserializeObject<MemberAction>(JsonResult);
                }
                catch (Exception ex)
                {
                    objMemberAction = new MemberAction();
                    objMemberAction.Status = -999;
                    objMemberAction.Description = "發生未知例外!!";
                    logs.Append("_Exception︰");
                    logs.Append(ex.ToString());
                    logs.Append("\r\n");
                }
                logs.Append("共執行︰");
                s.Stop();
                logs.Append((s.ElapsedMilliseconds).ToString());
                logs.Append("毫秒");
            }
        }

        Lib.WritLog("JDBib.cs.GetMemberActionLog()", logs.ToString());

        return objMemberAction;
    }

    /// <summary>
    /// 取得會員下注LOG
    /// </summary>
    /// <param name="_MemberAccount">會員帳號</param>
    /// <param name="_StartTime">開始時間</param>
    /// <param name="_EndTime">結束時間</param>
    /// <param name="_MemberUid">會員UID</param>
    /// <param name="_JdbSessionId">JdbSessionId</param>
    public MemberQueueLog GetMemberQueueLog(string _MemberAccount, string _StartTime, string _EndTime, string _MemberUid, string _JdbSessionId)
    {
        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        s.Start();//開始計時
        const string MethodURL = "log/GetMemberQueueLog";
        string MemberAccount = string.IsNullOrEmpty(_MemberAccount) ? "" : _MemberAccount;
        string StartTime = string.IsNullOrEmpty(_StartTime) ? "" : _StartTime;
        string EndTime = string.IsNullOrEmpty(_EndTime) ? "" : _EndTime;
        string MemberUid = string.IsNullOrEmpty(_MemberUid) ? "" : _MemberUid;
        string JdbSessionId = string.IsNullOrEmpty(_JdbSessionId) ? "" : _JdbSessionId;
        string Content = string.Empty;
        string JsonResult = string.Empty;
        MemberQueueLog objMemberQueueLog = null;
        DateTime tmpStartTime;
        DateTime tmpEndTime;
        System.Text.StringBuilder logs = new System.Text.StringBuilder();
        logs.Append("_MemberAccount︰");
        logs.Append(MemberAccount);
        logs.Append("\r\n");
        logs.Append("_StartTime︰");
        logs.Append(StartTime);
        logs.Append("\r\n");
        logs.Append("_EndTime︰");
        logs.Append(EndTime);
        logs.Append("\r\n");
        logs.Append("_MemberUid︰");
        logs.Append(MemberUid);
        logs.Append("\r\n");
        logs.Append("_JdbSessionId︰");
        logs.Append(JdbSessionId);
        logs.Append("\r\n");
        if (MemberAccount == "" || StartTime == "" || EndTime == "")
        {
            logs.Append("缺少必要參數!!");
            logs.Append("共執行︰");
            s.Stop();
            logs.Append((s.ElapsedMilliseconds).ToString());
            logs.Append("毫秒");
            objMemberQueueLog = new MemberQueueLog();
            objMemberQueueLog.Status = -999;
            objMemberQueueLog.Description = "缺少必要參數!!";
        }
        else
        {
            if (!DateTime.TryParse(StartTime, out tmpStartTime) || !DateTime.TryParse(EndTime, out tmpEndTime))
            {
                logs.Append("參數型態錯誤!!");
                logs.Append("共執行︰");
                s.Stop();
                logs.Append((s.ElapsedMilliseconds).ToString());
                logs.Append("毫秒");
                objMemberQueueLog = new MemberQueueLog();
                objMemberQueueLog.Status = -999;
                objMemberQueueLog.Description = "參數型態錯誤!!";
            }
            else
            {
                if (tmpStartTime > tmpEndTime)
                {
                    logs.Append("日期起訖錯誤!!");
                    logs.Append("共執行︰");
                    s.Stop();
                    logs.Append((s.ElapsedMilliseconds).ToString());
                    logs.Append("毫秒");
                    objMemberQueueLog = new MemberQueueLog();
                    objMemberQueueLog.Status = -999;
                    objMemberQueueLog.Description = "日期起訖錯誤!!";
                }
                else
                {

                    Content = string.Format("MemberAccount={0}&StartTime={1}&EndTime={2}&MemberUid={3}&JdbSessionId={4}",
                                            MemberAccount, StartTime, EndTime, MemberUid, JdbSessionId);
                    JsonResult = Lib.GetDataToURL(
                        string.Format("{0}/{1}", LogUrl, MethodURL), Content);
                    try
                    {
                        objMemberQueueLog = JsonConvert.DeserializeObject<MemberQueueLog>(JsonResult);
                    }
                    catch (Exception ex)
                    {
                        objMemberQueueLog = new MemberQueueLog();
                        objMemberQueueLog.Status = -999;
                        objMemberQueueLog.Description = "發生未知例外!!";
                        logs.Append("_Exception︰");
                        logs.Append(ex.ToString());
                        logs.Append("\r\n");
                    }
                    logs.Append("共執行︰");
                    s.Stop();
                    logs.Append((s.ElapsedMilliseconds).ToString());
                    logs.Append("毫秒");
                }
            }
        }

        Lib.WritLog("JDBib.cs.GetMemberQueueLog()", logs.ToString());

        return objMemberQueueLog;
    }

    public System.Data.DataTable ConvertMemberQueueLogToDataTable()
    {
        System.Data.DataTable dtMemberQueue = new System.Data.DataTable();
        /*
        dtMemberQueue.Columns.Add("OpenState", typeof(string));      //開分狀態
        dtMemberQueue.Columns.Add("ClearState", typeof(string));     //洗分狀態
        dtMemberQueue.Columns.Add("H1numbuerId", typeof(string));    //H1序號
        dtMemberQueue.Columns.Add("JdbnumbuerId", typeof(string));      //JDB序號
        dtMemberQueue.Columns.Add("jqu_create_datetime", typeof(string));   //jqu_create_datetime
        dtMemberQueue.Columns.Add("GameId", typeof(string));                   //遊戲ID
        dtMemberQueue.Columns.Add("PlayerAccount", typeof(double));         //剩下餘額
        dtMemberQueue.Columns.Add("Stake_Score", typeof(double));           //下注金額
        dtMemberQueue.Columns.Add("JackPot", typeof(double));               //彩金
        dtMemberQueue.Columns.Add("Account_Score", typeof(double));         //輸贏
        dtMemberQueue.Columns.Add("BetTimes", typeof(string));              //下注次數
        */
        dtMemberQueue.Columns.Add("開分狀態", typeof(string));      //開分狀態
        dtMemberQueue.Columns.Add("洗分狀態", typeof(string));     //洗分狀態
        dtMemberQueue.Columns.Add("H1序號", typeof(string));    //H1序號
        dtMemberQueue.Columns.Add("JDB序號", typeof(string));      //JDB序號
        dtMemberQueue.Columns.Add("jqu_create_datetime", typeof(string));   //jqu_create_datetime
        dtMemberQueue.Columns.Add("遊戲ID", typeof(string));                   //遊戲ID
        dtMemberQueue.Columns.Add("剩下餘額", typeof(double));         //剩下餘額
        dtMemberQueue.Columns.Add("下注金額", typeof(double));           //下注金額
        dtMemberQueue.Columns.Add("彩金", typeof(double));               //彩金
        dtMemberQueue.Columns.Add("輸贏", typeof(double));         //輸贏
        dtMemberQueue.Columns.Add("下注次數", typeof(string));              //下注次數
        return dtMemberQueue;
    }

    /// <summary>
    /// 取得JDB網址設定
    /// </summary>
    public JdbUrlConfig GetJdbUrlConfig()
    {
        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        s.Start();//開始計時
        const string MethodURL = "log/GetJdbUrlConfig";
        string Content = string.Empty;
        string JsonResult = string.Empty;
        JdbUrlConfig ojbJdbUrlConfig = null;
        System.Text.StringBuilder logs = new System.Text.StringBuilder();
        JsonResult = Lib.GetDataToURL(
                    string.Format("{0}/{1}", LogUrl, MethodURL), Content);

        try
        {
            ojbJdbUrlConfig = JsonConvert.DeserializeObject<JdbUrlConfig>(JsonResult);
        }
        catch (Exception ex)
        {
            ojbJdbUrlConfig = new JdbUrlConfig();
            ojbJdbUrlConfig.Status = -999;
            ojbJdbUrlConfig.Description = "發生未知例外!!";
            logs.Append("_Exception︰");
            logs.Append(ex.ToString());
            logs.Append("\r\n");
        }
        logs.Append("共執行︰");
        s.Stop();
        logs.Append((s.ElapsedMilliseconds).ToString());
        logs.Append("毫秒");
        Lib.WritLog("JDBib.cs.GetJdbUrlConfig()", logs.ToString());

        return ojbJdbUrlConfig;
    }

    /// <summary>
    /// 設定JDB網址
    /// </summary>
    /// <param name="_Url">網址</param>
    public SetJdbUrlConfig SetJdbUrlConfig(string _Url)
    {
        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        s.Start();//開始計時
        const string MethodURL = "log/SetJdbUrlConfig";
        string Url = string.IsNullOrEmpty(_Url) ? "" : _Url;
        string Content = string.Empty;
        string JsonResult = string.Empty;
        SetJdbUrlConfig objSetJdbUrlConfig = null;
        System.Text.StringBuilder logs = new System.Text.StringBuilder();
        logs.Append("_Url︰");
        logs.Append(Url);
        logs.Append("\r\n");

        if (Url == "")
        {
            logs.Append("缺少必要參數!!");
            logs.Append("共執行︰");
            s.Stop();
            logs.Append((s.ElapsedMilliseconds).ToString());
            logs.Append("毫秒");
        }
        else
        {
            Content = string.Format("Url={0}", Url);
            JsonResult = Lib.GetDataToURL(
                    string.Format("{0}/{1}", LogUrl, MethodURL), Content);
            try
            {
                JsonResult = JsonResult.Replace("\"\"", "\"");
                objSetJdbUrlConfig = JsonConvert.DeserializeObject<SetJdbUrlConfig>(JsonResult);
            }
            catch (Exception ex)
            {
                objSetJdbUrlConfig = new SetJdbUrlConfig();
                objSetJdbUrlConfig.Status = -999;
                objSetJdbUrlConfig.Description = "發生未知例外!!";
                logs.Append("_Exception︰");
                logs.Append(ex.ToString());
                logs.Append("\r\n");
            }
            logs.Append("共執行︰");
            s.Stop();
            logs.Append((s.ElapsedMilliseconds).ToString());
            logs.Append("毫秒"); 
        }

        Lib.WritLog("JDBib.cs.SetJdbUrlConfig()", logs.ToString());

        return objSetJdbUrlConfig;
    }

    /// <summary>
    /// 取得JDB網址設定
    /// </summary>
    public RestartAllServer RestartAllServer()
    {
        System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        s.Start();//開始計時
        const string MethodURL = "log/ReStartServer";
        string Content = string.Empty;
        string JsonResult = string.Empty;
        RestartAllServer ojbRestartAllServer = null;
        System.Text.StringBuilder logs = new System.Text.StringBuilder();
        JsonResult = Lib.GetDataToURL(
                    string.Format("{0}/{1}", LogUrl, MethodURL), Content);

        try
        {
            ojbRestartAllServer = JsonConvert.DeserializeObject<RestartAllServer>(JsonResult);
        }
        catch (Exception ex)
        {
            ojbRestartAllServer = new RestartAllServer();
            ojbRestartAllServer.Status = -999;
            ojbRestartAllServer.Description = "發生未知例外!!";
            logs.Append("_Exception︰");
            logs.Append(ex.ToString());
            logs.Append("\r\n");
        }
        logs.Append("共執行︰");
        s.Stop();
        logs.Append((s.ElapsedMilliseconds).ToString());
        logs.Append("毫秒");
        Lib.WritLog("JDBib.cs.RestartAllServer()", logs.ToString());

        return ojbRestartAllServer;
    }
}