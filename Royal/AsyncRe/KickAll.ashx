<%@ WebHandler Language="C#" Class="KickAll" %>

using System;
using System.Web;
using RTGLib;

public class KickAll : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    private string KickMember(string _KickType, string _GameId)
    {
        string KickType = _KickType;

        string GameId = _GameId;

        int iKickType = 0;

        string rvalue = string.Empty;

        RGTlib objRGTlib = new RGTlib();

        KickAllResult objKickAllResult = null;

        if (!int.TryParse(KickType, out iKickType))
        {
            iKickType = 0;
        }

        objKickAllResult = objRGTlib.KickAll(iKickType, GameId);

        rvalue = string.Format("{0}|{1}", objKickAllResult.MsgID, objKickAllResult.Message);

        return rvalue;

    }

    public void ProcessRequest (HttpContext context) {

        string KickType = string.IsNullOrEmpty(context.Request.QueryString["KickType"]) ? "" : context.Request.QueryString["KickType"];

        string GameId = string.IsNullOrEmpty(context.Request.QueryString["GameId"]) ? "" : context.Request.QueryString["GameId"];

        bool islogin = false;

        string txtresult = string.Empty;

        try
        {
            islogin = !string.IsNullOrEmpty(context.Session["isLogin"].ToString());
        }
        catch (Exception ex)
        {
            islogin = false;
        }

        if (islogin)
        {
            txtresult = KickMember(KickType, GameId);
        }

        context.Response.Write(txtresult);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}