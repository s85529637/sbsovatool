<%@ WebHandler Language="C#" Class="ReturnAccount" %>

using System;
using System.Web;
using JDBLogWrapper;
using System.Xml;
using System.Text;
using System.IO;

public class ReturnAccount : IHttpHandler ,System.Web.SessionState.IRequiresSessionState {

    private JDBlib objjdb = new JDBlib("17");

    /// <summary>
    /// 調用H1 JDB Api 進行手動洗分
    /// </summary>
    /// <param name="playerid"></param>
    /// <param name="Password"></param>
    /// <param name="webid"></param>
    /// <param name="playeraccount"></param>
    /// <param name="sessionid"></param>
    /// <param name="stakescore"></param>
    /// <param name="accountscore"></param>
    /// <param name="rows"></param>
    /// <param name="maxdatetime"></param>
    /// <param name="Location"></param>
    /// <param name="gameid"></param>
    /// <param name="jackpot"></param>
    /// <param name="jdbsessionid"></param>
    /// <returns></returns>
    private string GetReturnAccountReuslt(string playerid, string Password, string webid, string playeraccount,
    string sessionid, string stakescore, string accountscore, string rows, string maxdatetime, string Location, string gameid, string jackpot, string jdbsessionid)
    {
        JDBLogWrapper.JDBLogWrapper objJdb = new JDBLogWrapper.JDBLogWrapper();
        string Player_Id  = string.Empty;
        string Web_Id = string.Empty;
        string status = string.Empty;
        string Description = string.Empty;
        XmlNode objxml = null;
        try
        {
            objxml = objJdb.PlayerReturnAccount(
                                        playerid,
                                        Password,
                                        objjdb.webid,
                                        playeraccount.ToString(),
                                        sessionid,
                                        stakescore.ToString(),
                                        accountscore.ToString(),
                                        rows.ToString(),
                                        maxdatetime,
                                        Location,
                                        gameid,
                                        jackpot.ToString(),
                                        jdbsessionid);


            Player_Id = objxml.SelectNodes("//PlayerInfo/PlayerId")[0].InnerText;
            Web_Id = objxml.SelectNodes("//PlayerInfo/WebId")[0].InnerText;
            status = objxml.SelectNodes("//PlayerInfo/Status")[0].InnerText;
            Description = objxml.SelectNodes("//PlayerInfo/Description")[0].InnerText;

            Lib.WritLog("ReturnAccount.ashx.GetReturnAccountReuslt[手動洗分]","IN︰"+
            string.Format(@"playerid={0},
                            Password={1},
                            objjdb.webid={2},
                            playeraccount.ToString()={3},
                            sessionid={4},
                            stakescore.ToString()={5},
                            accountscore.ToString()={6},
                            rows.ToString()={7},
                            maxdatetime={8},
                            Location={9},
                            gameid={10},
                            jackpot.ToString()={11},
                            jdbsessionid={12},",
                            playerid,
                            Password,
                            objjdb.webid,
                            playeraccount.ToString(),
                            sessionid,
                            stakescore.ToString(),
                            accountscore.ToString(),
                            rows.ToString(),
                            maxdatetime,
                            Location,
                            gameid,
                            jackpot.ToString(),
                            jdbsessionid)+";Out:"+ string.Format("{0},{1},{2},{3}", Player_Id, Web_Id, status, Description)
                            );
        }
        catch (Exception ex)
        {
            status = "-3";
            Description = ex.ToString();
            Lib.WritLog("ReturnAccount.ashx.GetReturnAccountReuslt()", ex.ToString());
        }

        return string.Format("{0},{1},{2},{3}", Player_Id, Web_Id, status, Description);
    }


    /// <summary>
    /// 調用jdb api 取得資料
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="islogin"></param>
    /// <returns></returns>
    private string getdata(string sid,string uid)
    {
        string txtresult = string.Empty;
        string playerid = string.Empty;
        string gameid = string.Empty;
        double playeraccount = 0;
        double stakescore = 0.0;
        double accountscore = 0.0;
        int rows = 0;
        string maxdatetime =  string.Empty;
        double jackpot = 0.0;
        string sessionid =  string.Empty;
        string jdbsessionid =  string.Empty;
        StringBuilder  accountResult=new StringBuilder("");
        if (sid != "" )
        {
            JDBlib.Accountlist result = objjdb.GetReCountAccount(sid,uid);

            if (result.status == "1" && result.data.Count > 0)
            {
                if (result.data.Count > 0)
                {
                    for (int j = 0; j < result.data.Count; j++)
                    {
                        try
                        {
                            playerid = result.data[j].playerid;
                            gameid = result.data[j].gameid;
                            playeraccount = double.Parse(result.data[j].playeraccount);
                            stakescore = -1 * double.Parse((result.data[j].stakescore));
                            accountscore = double.Parse(result.data[j].accountscore);
                            rows = int.Parse(result.data[j].rows);
                            maxdatetime = result.data[j].maxdatetime;
                            jackpot = double.Parse(result.data[j].jackpot);
                            sessionid = result.data[j].sessionid;
                            jdbsessionid = result.data[j].jdbsessionid;
                            accountResult.Append(
                                GetReturnAccountReuslt(
                                        playerid,
                                        "",          //Password
                                        objjdb.webid,
                                        playeraccount.ToString(),
                                        sessionid,
                                        stakescore.ToString(),
                                        accountscore.ToString(),
                                        rows.ToString(),
                                        maxdatetime,
                                        "",          //Location
                                        gameid,
                                        jackpot.ToString(),
                                        jdbsessionid)   //jdbsessionid
                                 );
                            accountResult.Append("|");
                        }
                        catch (Exception ex)
                        {
                            accountResult.Append(
                                string.Format("{0},{1},{2},{3}", playerid, gameid, "-2", ex.ToString()));
                            accountResult.Append("|");
                            Lib.WritLog("ReturnAccount.ashx.getdata()", accountResult.ToString());
                            break;
                        }
                    }
                }
            }
            else if (result.status == "-999")
            {
                accountResult.Append(
                   string.Format("{0},{1},{2},{3}", sid, gameid, "-999", "調用點數中心API發生問題"));
                accountResult.Append("|");
            }
            else if (result.status == "0" && result.data.Count <= 0)
            {
                accountResult.Append(
                     string.Format("{0},{1},{2},{3}",sid, gameid, "-6",result.description));
                accountResult.Append("|");

            }else{
                accountResult.Append(
                     string.Format("{0},{1},{2},{3}",sid, gameid, "-1",result.description));
                accountResult.Append("|");
            }

            txtresult = accountResult.ToString().TrimEnd('|');
        }

        return txtresult;
    }


    public void ProcessRequest (HttpContext context) {

        string sid = string.IsNullOrEmpty(context.Request.QueryString["sid"]) ? "" : context.Request.QueryString["sid"];

        string uid = string.IsNullOrEmpty(context.Request.QueryString["uid"]) ? "" : context.Request.QueryString["uid"];

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
            Result = getdata(sid,uid);
        }

        context.Response.Write(Result);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}