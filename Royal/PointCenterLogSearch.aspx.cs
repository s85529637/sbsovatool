using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PointCenterLogSearch : NewBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["isLogin"] == null) Response.Redirect("Logout.aspx");
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        GetWrongUidInfo();
    }

    protected void btMember_KaDan_Check_Click(object sender, EventArgs e)
    {
        GetMember_KaDan_Check();
    }

    /// <summary>
    /// 列繫結事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WrongUidInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string logText = string.Empty;
        string showText = string.Empty;
        string jsid = string.Empty;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                if (string.IsNullOrEmpty(e.Row.Cells[j].Text.Trim()) || ""== e.Row.Cells[j].Text.Trim()) continue;

                e.Row.Cells[j].Text = string.Format("{0}", e.Row.Cells[j].Text.Replace("\r\n", "<br>"));

                if (j == 10 || j == 2)
                {
                    jsid = Guid.NewGuid().ToString();
                    logText = e.Row.Cells[j].Text.Replace("\r\n", "<br>");
                    showText = string.IsNullOrEmpty(logText.Trim()) ? "" : logText.Length > 30 ? logText.Substring(0, 30) : logText;
                    if (showText != "")
                    {
                        e.Row.Cells[j].Text = string.Format("<div><div style='cursor: pointer;' onclick=openTextWindow(document.getElementById('{2}'));>{0}</div><div id='{2}' style='display:none;'>{1}</div></div>", showText, logText, jsid);
                    }
                }
            }
        }
    }

    protected void Member_KaDan_Check3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string logText = string.Empty;
        string showText = string.Empty;
        string jsid = string.Empty;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                if (string.IsNullOrEmpty(e.Row.Cells[j].Text.Trim()) || "" == e.Row.Cells[j].Text.Trim()) continue;

                e.Row.Cells[j].Text = string.Format("{0}", e.Row.Cells[j].Text.Replace("\r\n", "<br>"));

                if (j ==5)
                {
                    jsid = Guid.NewGuid().ToString();
                    logText = e.Row.Cells[j].Text.Replace("\r\n", "<br>");
                    showText = string.IsNullOrEmpty(logText.Trim()) ? "" : logText.Length > 30 ? logText.Substring(0, 30) : logText;
                    if (showText != "")
                    {
                        e.Row.Cells[j].Text = string.Format("<div><div style='cursor: pointer;' onclick=openTextWindow(document.getElementById('{2}'));>{0}</div><div id='{2}' style='display:none;'>{1}</div></div>", showText, logText, jsid);
                    }
                 }
             }
        }
    }



    /// <summary>
    /// 列繫結事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Member_KaDan_Check4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string logText = string.Empty;
        string showText = string.Empty;
        string jsid = string.Empty;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                if (string.IsNullOrEmpty(e.Row.Cells[j].Text.Trim()) || "" == e.Row.Cells[j].Text.Trim()) continue;

                e.Row.Cells[j].Text = string.Format("{0}", e.Row.Cells[j].Text.Replace("\r\n", "<br>"));

                if (j == 8 || j==2 )
                {
                    jsid = Guid.NewGuid().ToString();
                    logText = e.Row.Cells[j].Text.Replace("\r\n", "<br>");
                    showText = string.IsNullOrEmpty(logText.Trim()) ? "" : logText.Length >30 ? logText.Substring(0, 30) : logText;
                    if (showText != "")
                    {                    
                        e.Row.Cells[j].Text = string.Format("<div><div style='cursor: pointer;' onclick=openTextWindow(document.getElementById('{2}'));>{0}</div><div id='{2}' style='display:none;'>{1}</div></div>", showText, logText, jsid);
                    }
                }
            }
        }
    }

    private void GetMember_KaDan_Check()
    {
        string SResultMessage = string.Empty;
        int IResult = 0;
        string SMemberUid = string.IsNullOrEmpty(txt_MemberUid.Text) ? "" : txt_MemberUid.Text.Trim();
        StringBuilder Msg = new StringBuilder("");
        bsSQL bssql = new bsSQL();
        DataSet ds = bssql.NSP_Member_KaDan_Check(txtsubsystem.Text.Trim(), txtwebsite.Text.Trim(), txtvendor_id.Text.Trim(), txt_MemberUid.Text.Trim(), txtMemberAccount.Text.Trim(), ref  SResultMessage, ref IResult);
        if (ds.Tables.Count > 0)
        {
            for (int j = 0; j < ds.Tables.Count;j++)
            {
                if (j == 0) {
                    if (ds.Tables[j].Rows.Count > 0)
                    {
                        Member_KaDan_Check1.DataSource = ds.Tables[j];
                        Member_KaDan_Check1.DataBind();
                    }
                }

                if (j == 1)
                {
                    if (ds.Tables[j].Rows.Count > 0)
                    {
                        Member_KaDan_Check2.DataSource = ds.Tables[j];
                        Member_KaDan_Check2.DataBind();
                    }
                }

                if (j == 2)
                {
                    if (ds.Tables[j].Rows.Count > 0)
                    {
                        Member_KaDan_Check3.DataSource = ds.Tables[j];
                        Member_KaDan_Check3.DataBind();
                    }
                }

                if (j == 3)
                {
                    if (ds.Tables[j].Rows.Count > 0)
                    {
                        Member_KaDan_Check4.DataSource = ds.Tables[j];
                        Member_KaDan_Check4.DataBind();
                    }
                }
            }
        }

        if (IResult != 0)
        {
            Msg.Append("<h3>查詢發生錯誤!");
            Msg.Append(SResultMessage);
            Msg.Append("</h3>");
        }
        else
        {
            if (ds.Tables.Count <= 0)
            {
                Msg.Append("<h3>查無資料!!</h3>");
            }
        }
        objMsg1.Text = Msg.ToString();
    }

    private void GetWrongUidInfo()
    {
        string SResultMessage = string.Empty;
        int IResult = 0;
        string SMemberUid = string.IsNullOrEmpty(txtMemberUid.Text) ? "" : txtMemberUid.Text.Trim();
        StringBuilder Msg = new StringBuilder("");
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetWrongUidInfo(SMemberUid, ref SResultMessage, ref IResult);
        WrongUidInfo.DataSource = dt;
        WrongUidInfo.DataBind();
        if (IResult != 0)
        {
            Msg.Append("<h3>查詢發生錯誤!");
            Msg.Append(SResultMessage);
            Msg.Append("</h3>");
        }
        else
        {
            if (dt.Rows.Count <= 0)
            {
                Msg.Append("<h3>查無資料!!</h3>");
            }
        }
        objMsg.Text = Msg.ToString();
    }

 }