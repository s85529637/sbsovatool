<%@ WebHandler Language="C#" Class="Gclub" %>

using System;
using System.Web;

public class Gclub : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        bool isok = false;
        string msg = string.Empty;
        string msg1 = string.Empty;
        bool islogin = false;
        string isopen = string.IsNullOrEmpty(context.Request["isopen"]) ? "N" : context.Request["isopen"];
        try
        {
            islogin = !string.IsNullOrEmpty(context.Session["isLogin"].ToString());
        }
        catch (Exception ex)
        {
            islogin = false;
        }
        if (islogin == true)
        {
            if (isopen == "Y")
            {
                isok = CtrlHall(context, 1, out msg);
                context.Response.Write("[開啟館別]" + msg);
            }
            else
            {
                isok = CloseHall(context, out msg); //先通知洗分

                msg1 = "[通知洗分]" + (string.IsNullOrEmpty(msg) ? "成功" : msg);

                if (isok)
                {
                    CtrlHall(context, 0, out msg); //關閉大廳

                    context.Response.Write(msg1 + "[關閉館別]" + msg);
                }
                else
                {
                    context.Response.Write("[通知洗分]" + msg);
                }
            }
        }

        context.Response.End();
    }

    /// <summary>
    /// 通知洗分
    /// </summary>
    /// <param name="context"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    private bool CloseHall(HttpContext context, out string msg)
    {
        CashoutMember objCashoutMember = Gclublib.CashoutMember();

        string rMsg = string.Empty;

        bool isok = false;

        if (objCashoutMember.status == 200)
        {
            if (objCashoutMember.msg != null)
            {
                if (objCashoutMember.msg == "")
                {
                    rMsg = ("API回覆成功!!");
                    isok = true;
                }
                else
                {
                    rMsg = (objCashoutMember.msg);
                }
            }
            else
            {
                rMsg = ("API回覆成功，但訊息為空!!");
            }
        }
        else
        {
            if (objCashoutMember.msg != null)
            {
                if (objCashoutMember.status == -999)
                {
                    rMsg = ("發生未知的例外︰" + objCashoutMember.msg);
                }
                else
                {
                    rMsg = ("API回覆失敗︰" + objCashoutMember.msg);
                }
            }
            else
            {
                rMsg = ("API回覆失敗，但訊息為空!!");
            }
        }
        msg = rMsg;
        return isok;
    }

    /// <summary>
    /// 控制大廳開關
    /// </summary>
    /// <param name="context"></param>
    /// <param name="istatus"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    private bool CtrlHall(HttpContext context, int istatus, out string msg)
    {
        ChangeWebHallStatus objChangeWebHallStatus = Gclublib.ChangeWebHallStatus(istatus);

        string rMsg = string.Empty;

        bool isok = false;

        if (objChangeWebHallStatus.status == 200)
        {
            if (istatus == 0)
            {
                rMsg = "Gclub關閉功成!!";
            }
            else
            {
                rMsg = "Gclub開放功成!!";
            }

            isok = true;
        }
        else
        {
            if (objChangeWebHallStatus.msg != null)
            {
                rMsg = ("API回覆Gclub開放失敗︰" + objChangeWebHallStatus.msg);
            }
            else
            {
                rMsg = ("API回覆Gclub開放失敗，但訊息為空!!");
            }
        }

        msg = rMsg;

        return isok;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}