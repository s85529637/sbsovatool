<%@ WebHandler Language="C#" Class="GClubtake" %>

using System;
using System.Web;
using System.Configuration;


public class GClubtake : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {


        string LogString = string.Empty;
        string LogResponseString = string.Empty;
        try
        {
            string id = context.Request.QueryString.ToString();
            var Mytimestamp = " ";/*GetTimestamp();*/
            var Mysignature = (id + Mytimestamp + "B7Jupv4PDHmI").ToLower();
            var RequestData = new
            {
                channelId = "RG",
                username = id,
                timestamp = Mytimestamp,
                signature = Mysignature
            };
            string QueryString = "channelId=RG&" + "username=" + id + "&" + "timestamp=" + Mytimestamp + "&" + "signature=" + Mysignature;
            //AppSettingsReader reader = new AppSettingsReader();
            //string RoyalAPI = reader.GetValue("GClub.API.ConnectionString", typeof(string)).ToString();
            string RoyalAPI = System.Web.Configuration.WebConfigurationManager.AppSettings["GClub.API.ConnectionString"];
            //    ResponseModel<Common_UserLoginModel> ResponseData = new ResponseModel<Common_UserLoginModel>();
            //    LogString = "/GClub/logoutgame.php?" + QueryString + RoyalAPI;
            //    //傳送資料的Log
            //    LogModel Requestlog = new LogModel("WebSite", "api/LogOutGClubGame", "")
            //    {
            //        Requestdata = RoyalAPI + "/GClub/logoutgame.php?" + QueryString
            //    };
            //    LogDAL.logger_Debug(Requestlog);
            //    string ResponseString = CommonTool.GetServer(RequestData, "/GClub/logoutgame.php?" + QueryString, RoyalAPI, this._ihc).Result;
            //    ResponseData = Common_Serializable_JSON.ObjectDeSerializable<ResponseModel<Common_UserLoginModel>>(ResponseString);
            //    //接收資料的Log
            //    LogModel Responselog = new LogModel("WebSite", "api/LogOutGClubGame", "")
            //    {
            //        ErrorMessage = ResponseString,
            //        Requestdata = RoyalAPI + "/GClub/logoutgame.php?" + QueryString
            //    };
            //    LogDAL.logger_Debug(Responselog);
            //    return Json(new { result = ResponseString });
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
        }
        /// <summary>
        /// 時間簽章
        /// </summary>
        //public static long GetTimestamp()
        //{
        //    DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    return (long)((DateTime.UtcNow - Jan1st1970).TotalSeconds);
        //}


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}