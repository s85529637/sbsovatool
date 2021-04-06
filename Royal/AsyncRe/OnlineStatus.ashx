<%@ WebHandler Language="C#" Class="OnlineStatus" %>

using System;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Text;
using RTGLib;

public class OnlineStatus : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    private JDBlib objjdb = new JDBlib("5");

    private  string txtdevice = System.Configuration.ConfigurationManager.AppSettings["Device"].ToString();

    /// <summary>
    ///
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="islogin"></param>
    /// <param name="tid"></param>
    /// <param name="sid"></param>
    /// <returns></returns>
    private string CatchData(string uid,bool islogin,string tid,string sid,string device,string ZuBie)
    {
        string strKey = uid;

        string rValue = "";

        try
        {
            if (System.Web.HttpRuntime.Cache.Get(strKey) == null)
            {
                rValue =  getdata(uid,islogin,tid,sid,device,ZuBie);

                if (string.IsNullOrEmpty(rValue)) return "";
                /*  context.Cache.Add 說明
                    第一個參數是設定索引值。
                    第二個則是要快取的物件。
                    第三個則是設定與某個檔案的相依性，當該檔案有變更時快取的物件就會被移除，如果不設定相依性，直接設null就可以。
                    而第四、第五個參數是有關聯的，是設定快取的失效時間，
                    第四個參數可以設為NoAbsoluteExpiration或是設定要在哪一個時間過後失效，
                    而第五個則是設定在該快取的最後一次存取後，要過多久才失效，如果前一個參數有設定時間，那該參數就要設為NoSildingExpiration。
                    而最後兩個分別是設定移除快取的優先權及被移除時的call back function。
                    */
                System.Web.HttpRuntime.Cache.Add(
                                strKey,
                                rValue,
                                null,
                                DateTime.Now.AddSeconds(1),
                                System.Web.Caching.Cache.NoSlidingExpiration,
                                CacheItemPriority.High,
                                null
                                );
            }
            else
            {
                rValue = System.Web.HttpRuntime.Cache.Get(strKey).ToString();
            }
        }
        catch (Exception ex)
        {
            rValue = getdata(uid,islogin,tid,sid,device,ZuBie);
            Lib.WritLog("OnlineStatus.ashx.CatchData()", ex.ToString());
        }

        return rValue;

    }

    /// <summary>
    /// 調用jdb api 取得Oline資料
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="islogin"></param>
    /// <param name="tid"></param>
    /// <param name="sid"></param>
    /// <returns></returns>
    private string getdata(string uid,bool islogin,string tid,string sid,string device,string ZuBie)
    {
        string txtresult = string.Empty;

        if (uid != "" && islogin && tid!="" && device!="")
        {
            if ("JDB" == tid.ToUpper())
            {
                txtresult = JdbOnline(uid,islogin);

            }else  if ("ROYAL" == tid.ToUpper())
            {
                txtresult =  RoyalOnline(device,sid, uid);

            }else  if ("RTG" == tid.ToUpper())
            {
                txtresult =  RTGOnline(uid,ZuBie);
                if (txtresult == "ONLINE"){
                    txtresult = "1";
                } else if (txtresult == "OFFLINE"){
                    txtresult = "0";
                }else if (txtresult == "ERROR"){
                    txtresult = "-1";
                }
            }
        }

        return txtresult;
    }

    /// <summary>
    /// 皇家棋牌遊戲會員在線狀態
    /// </summary>
    /// <param name="device"></param>
    /// <param name="uid"></param>
    /// <param name="islogin"></param>
    /// <returns></returns>
    private string RTGOnline(string uid,string ZuBie)
    {
        string txtresult = "ERROR";

        RGTlib objRGTlib = null;

        MemberInfoResult result = null;

        try
        {
            if (uid != "" )
            {
                objRGTlib = new RGTlib();

                result = objRGTlib.Get_MemberInfo(uid,ZuBie);

                txtresult = result.MsgID.ToString();

                if (txtresult == "0")
                {
                    if (result.Data.UserStatus.ToString() == "2")
                    {
                        txtresult = "ONLINE";

                    }
                    else if (result.Data.UserStatus.ToString() == "1")
                    {

                        txtresult = "OFFLINE";
                    }
                }else {

                    txtresult = result.Message;
                }
            }
        }
        catch (Exception ex)
        {
            Lib.WritLog("OnlineStatus.ashx.RTGOnline()", ex.ToString());
        }

        return txtresult;

    }

    /// <summary>
    /// 調用JDB api 取得Oline資料
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="islogin"></param>
    /// <returns></returns>
    private string JdbOnline(string uid, bool islogin)
    {
        string txtresult = string.Empty;

        JDBlib.JDBOnline result = null;

        try
        {
            if (uid != "" && islogin)
            {
                result = objjdb.CheckMemberStatus(uid);

                if (result.status == "1")
                {
                    if (result.isonline)
                    {
                        txtresult = "1";  //在線
                    }
                    else
                    {
                        txtresult = "0";  //離線
                    }
                }
                else if (result.status == "-999")
                {
                    txtresult = "-1";    //錯誤
                }
                else
                {
                    txtresult = result.description;
                }
            }
        }
        catch (Exception ex)
        {
            txtresult = "-1";
            Lib.WritLog("OnlineStatus.ashx.JdbOnline()", ex.ToString());
        }

        return txtresult;

    }

    private string RoyalOnline(string Web_id,  string SessionNo,string Player_id)
    {
        string rvalue = "0";

        if (SessionNo == "" || Player_id == "") return "";

        try
        {
            using (SlotGameWS.SlotGameWSSoapClient ws = new SlotGameWS.SlotGameWSSoapClient())
            {
                //XmlNode root = ws.GetAPILogBySessionNo(Web_id, SessionNo, Player_id);
                XmlNode root = ws.GetPlayerInfo(Web_id, Player_id);

                XmlNode child = null;

                int inum = 0;

                if (root != null)
                {
                    child = root.SelectSingleNode("/DataInfo[IsOnline='True']");

                    if (child != null)
                    {
                        foreach (XmlNode item in child)
                        {
                            if (item != null)
                            {
                                rvalue = "1";
                                Lib.WritLog("RoyalOnline",string.Format("一共有{0}子項，目前是第{1}項，取到的值是{2}", child.ChildNodes.Count, inum, item.InnerText.ToUpper()));
                                inum++;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            rvalue =ex.ToString();
            Lib.WritLog("OnlineStatus.ashx.RoyalOnline()", ex.ToString());
        }

        return rvalue ;
    }

    public void ProcessRequest (HttpContext context) {

        string uid = string.IsNullOrEmpty(context.Request.QueryString["uid"]) ? "" : context.Request.QueryString["uid"];

        string tid = string.IsNullOrEmpty(context.Request.QueryString["tid"]) ? "" : context.Request.QueryString["tid"];  //第三方廠商ID

        string sid = string.IsNullOrEmpty(context.Request.QueryString["sid"]) ? "" : context.Request.QueryString["sid"];

        string ZuBie = string.IsNullOrEmpty(context.Request.QueryString["ZuBie"]) ? "" : context.Request.QueryString["ZuBie"];

        string _device = string.IsNullOrEmpty(txtdevice)?"mobile": txtdevice; //string.IsNullOrEmpty(context.Request.QueryString["device"]) ? "" : context.Request.QueryString["device"];

        bool islogin = false;

        try
        {
            islogin = !string.IsNullOrEmpty(context.Session["isLogin"].ToString());
        }
        catch (Exception ex)
        {
            islogin = false;
        }

        string txtresult = CatchData(uid,islogin,tid,sid ,_device,ZuBie);

        context.Response.Write(txtresult);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}