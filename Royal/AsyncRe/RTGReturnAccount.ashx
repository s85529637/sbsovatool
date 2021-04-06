<%@ WebHandler Language="C#" Class="RTGReturnAccount" %>

using System;
using System.Web;
using System.Xml;
using System.Text;
using System.IO;
using RTGLib;

public class RTGReturnAccount : IHttpHandler ,System.Web.SessionState.IRequiresSessionState {


    private string ReturnAccountReuslt(string _AccountScore, string _GameId, string _MaxDateTime, string _PlayerAccount, string _PlayerId, string _Rows, string _SessionId,
        string _StakeScore, string _WebID, string _JackPot, string _PKPoint, string _SharePoint, string _Location)
    {
        string AccountScore = string.IsNullOrEmpty(_AccountScore) ? "" : _AccountScore ;
        string GameId =  string.IsNullOrEmpty(_GameId) ? "" : _GameId ;
        string MaxDateTime =   string.IsNullOrEmpty(_MaxDateTime) ? "" : _MaxDateTime ;
        string PlayerAccount =   string.IsNullOrEmpty(_PlayerAccount) ? "" : _PlayerAccount ;
        string PlayerId =   string.IsNullOrEmpty(_PlayerId) ? "" : _PlayerId ;
        string Rows =  string.IsNullOrEmpty(_Rows) ? "" : _Rows ;
        string SessionId =   string.IsNullOrEmpty(_SessionId) ? "" : _SessionId ;
        string StakeScore =   string.IsNullOrEmpty(_StakeScore) ? "" : _StakeScore ;
        string WebID =   string.IsNullOrEmpty(_WebID) ? "" : _WebID ;
        string JackPot =   string.IsNullOrEmpty(_JackPot) ? "" : _JackPot ;
        string PKPoint =   string.IsNullOrEmpty(_PKPoint) ? "" : _PKPoint ;
        string SharePoint = string.IsNullOrEmpty(_SharePoint) ? "" : _SharePoint ;
        string Location =   _Location ;
        string Status = string.Empty;
        string Description = string.Empty;
        XmlNode objxml = null;
        StringBuilder accountResult = new StringBuilder("");

        if (AccountScore == "" || GameId == "" || MaxDateTime == "" || PlayerAccount == "" || PlayerId == "" || Rows == "" || SessionId == "" || StakeScore == "" || WebID == "" || JackPot == "" || PKPoint == "" || SharePoint == "")
        {
            Status = "-4";
            Description = "Lost parameter!!";
            Lib.WritLog("RTGReturnAccount.ashx.ReturnAccountReuslt()", string.Format("{0},{1},{2},{3}", "NULL", "NULL", Status, Description));
            return string.Format("{0},{1},{2},{3}", "NULL", "NULL", Status, Description);
        }
        accountResult.Append("RTG.PlayerReturnAccount()參數︰");
        accountResult.Append(";AccountScore︰");
        accountResult.Append(AccountScore);
        accountResult.Append(";GameId︰");
        accountResult.Append(GameId);
        accountResult.Append(";MaxDateTime︰");
        accountResult.Append(MaxDateTime);
        accountResult.Append(";PlayerAccount︰");
        accountResult.Append(PlayerAccount);
        accountResult.Append(";PlayerId︰");
        accountResult.Append(PlayerId);
        accountResult.Append(";Rows︰");
        accountResult.Append(Rows);
        accountResult.Append(";SessionId︰");
        accountResult.Append(SessionId);
        accountResult.Append(";StakeScore︰");
        accountResult.Append(StakeScore);
        accountResult.Append(";WebID︰");
        accountResult.Append(WebID);
        accountResult.Append(";JackPot︰");
        accountResult.Append(JackPot);
        accountResult.Append(";PKPoint︰");
        accountResult.Append(PKPoint);
        accountResult.Append(";SharePoint︰");
        accountResult.Append(SharePoint);
        accountResult.Append(";Location︰");
        accountResult.Append(Location);

        try
        {
            RTG.RTG objRTG = new RTG.RTG();
            objxml =objRTG.PlayerReturnAccount(PlayerId, "HANDLY", WebID, SessionId, GameId, PlayerAccount, StakeScore, AccountScore, Rows, MaxDateTime, Location, JackPot, PKPoint, SharePoint);
            PlayerId = objxml.SelectNodes("//PlayerInfo/PlayerId")[0].InnerText;
            WebID = objxml.SelectNodes("//PlayerInfo/WebId")[0].InnerText;
            Status = objxml.SelectNodes("//PlayerInfo/Status")[0].InnerText;
            Description = objxml.SelectNodes("//PlayerInfo/Description")[0].InnerText;
            accountResult.Append("RTG.PlayerReturnAccount()結果︰");
            accountResult.Append(objxml.OuterXml.ToString());
            Lib.WritLog("ReturnAccount.ashx.ReturnAccountReuslt()", accountResult.ToString());
        }
        catch (Exception ex)
        {
            Status = "-3";
            Description = ex.ToString();
            Lib.WritLog("ReturnAccount.ashx.GetReturnAccountReuslt()", ex.ToString());
        }

        return string.Format("{0},{1},{2},{3}", PlayerId, WebID, Status, Description);
    }

    /// <summary>
    /// 調用RTG api 取得資料
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="islogin"></param>
    /// <returns></returns>
    private string getdata(string sid,string uid,string ZuBie)
    {
        string AccountScore = string.Empty;
        string GameId =  string.Empty;
        string MaxDateTime =  string.Empty;
        string PlayerAccount =  string.Empty;
        string PlayerId =  string.Empty;
        string Rows = string.Empty;
        string SessionId =  string.Empty;
        string StakeScore =  string.Empty;
        string WebID =  string.Empty;
        string JackPot =  string.Empty;
        string PKPoint =  string.Empty;
        string SharePoint = string.Empty;
        string Location =  string.Empty;
        string MsgID = string.Empty;
        string Message = string.Empty;
        string Timestamp = string.Empty;
        RGTlib objRGTlib = new RGTlib();
        string result = string.Empty;
        StringBuilder accountResult = new StringBuilder("");
        HandlyAccountResult objHandlyAccountResult =  objRGTlib.GetHandlyAccount(uid, sid, ZuBie);
        MsgID = objHandlyAccountResult.MsgID.ToString();
        Message = objHandlyAccountResult.Message;
        if (MsgID == "0" && Message == "Success")
        {
            AccountScore = objHandlyAccountResult.Data.Content.AccountScore.ToString();
            GameId = objHandlyAccountResult.Data.Content.GameId;
            MaxDateTime = objHandlyAccountResult.Data.Content.MaxDateTime;
            PlayerAccount = objHandlyAccountResult.Data.Content.PlayerAccount.ToString();
            PlayerId = objHandlyAccountResult.Data.Content.PlayerId;
            Rows = objHandlyAccountResult.Data.Content.Rows;
            SessionId = objHandlyAccountResult.Data.Content.SessionId;
            StakeScore = objHandlyAccountResult.Data.Content.StakeScore.ToString();
            WebID = objHandlyAccountResult.Data.Content.WebID;
            JackPot = objHandlyAccountResult.Data.Content.JackPot.ToString();
            PKPoint = objHandlyAccountResult.Data.Content.PKPoint.ToString();
            SharePoint = objHandlyAccountResult.Data.Content.SharePoint.ToString();
            Location = objHandlyAccountResult.Data.Content.Location.ToString();
            /***************************************************************/
            accountResult.Append("HandlyAccountResult.getdata()參數︰");
            accountResult.Append("uid︰");
            accountResult.Append(uid);
            accountResult.Append("sid︰");
            accountResult.Append(sid);
            accountResult.Append("HandlyAccountResult.getdata()結果︰");
            accountResult.Append(";MsgID︰");
            accountResult.Append(MsgID);
            accountResult.Append(";Message︰");
            accountResult.Append(Message);
            accountResult.Append(";AccountScore︰");
            accountResult.Append(AccountScore);
            accountResult.Append(";GameId︰");
            accountResult.Append(GameId);
            accountResult.Append(";MaxDateTime︰");
            accountResult.Append(MaxDateTime);
            accountResult.Append(";PlayerAccount︰");
            accountResult.Append(PlayerAccount);
            accountResult.Append(";PlayerId︰");
            accountResult.Append(PlayerId);
            accountResult.Append(";Rows︰");
            accountResult.Append(Rows);
            accountResult.Append(";SessionId︰");
            accountResult.Append(SessionId);
            accountResult.Append(";StakeScore︰");
            accountResult.Append(StakeScore);
            accountResult.Append(";WebID︰");
            accountResult.Append(WebID);
            accountResult.Append(";JackPot︰");
            accountResult.Append(JackPot);
            accountResult.Append(";PKPoint︰");
            accountResult.Append(PKPoint);
            accountResult.Append(";SharePoint︰");
            accountResult.Append(SharePoint);
            accountResult.Append(";Location︰");
            accountResult.Append(Location);
            accountResult.Append(";Timestamp︰");
            accountResult.Append(Timestamp);
            Lib.WritLog("ReturnAccount.ashx.getdata()", accountResult.ToString());
            result = ReturnAccountReuslt(AccountScore, GameId, MaxDateTime, PlayerAccount, PlayerId, Rows, SessionId,
            StakeScore, WebID, JackPot, PKPoint, SharePoint, Location);
        }else  {
            result = string.Format("{0},{1},{2},{3}", PlayerId, WebID, objHandlyAccountResult.MsgID.ToString(),objHandlyAccountResult.Message);
        }

        return result;
    }

    public void ProcessRequest (HttpContext context) {

        string sid = string.IsNullOrEmpty(context.Request.QueryString["sid"]) ? "" : context.Request.QueryString["sid"];

        string uid = string.IsNullOrEmpty(context.Request.QueryString["uid"]) ? "" : context.Request.QueryString["uid"]; 

        string ZuBie = string.IsNullOrEmpty(context.Request.QueryString["ZuBie"]) ? "" : context.Request.QueryString["ZuBie"];

        string Result = string.Empty;

        bool islogin = false;

        try
        {
            islogin = !string.IsNullOrEmpty(context.Session["isLogin"].ToString());
        }
        catch (Exception ex)
        {
            islogin = false;
        }

        if (sid != "" && uid!="" && islogin)
        {
            Result = getdata(sid , uid , ZuBie);
        }

        context.Response.Write(Result);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}