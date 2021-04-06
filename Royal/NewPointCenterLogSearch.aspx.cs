using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewPointCenterLogSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e){}

    protected void OpeaterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selAns = OpeaterDropDownList.SelectedValue.ToString();

        switch(selAns)
        {
            case "0":  //-----請選擇-----
                MemberAction.Visible = false;
                MemberQueue.Visible = false;
                JdbUrlConfig.Visible = false;
                SetJdbUrlConfig.Visible = false;
                RestartAllServer.Visible = false;
                break;
            case "1":  //取得會員活動LOG
                MemberAction.Visible = true;
                MemberQueue.Visible = false;
                JdbUrlConfig.Visible = false;
                SetJdbUrlConfig.Visible = false;
                RestartAllServer.Visible = false;
                break;
            case "2":  //取得會員下注LOG
                MemberAction.Visible = false;
                MemberQueue.Visible = true;
                JdbUrlConfig.Visible = false;
                SetJdbUrlConfig.Visible = false;
                RestartAllServer.Visible = false;
                break;
            case "3": //取得JDB網址設定
                MemberAction.Visible = false;
                MemberQueue.Visible = false;
                JdbUrlConfig.Visible = true;
                SetJdbUrlConfig.Visible = false;
                RestartAllServer.Visible = false;
                break;
            case "4": //設定JDB網址
                MemberAction.Visible = false;
                MemberQueue.Visible = false;
                JdbUrlConfig.Visible = false;
                SetJdbUrlConfig.Visible = true;
                RestartAllServer.Visible = false;
                break;
            case "5": //重啟Server
                MemberAction.Visible = false;
                MemberQueue.Visible = false;
                JdbUrlConfig.Visible = false;
                SetJdbUrlConfig.Visible = false;
                RestartAllServer.Visible = true;
                break;
        }       
    }


    /// <summary>
    /// 取得會員活動LOG查詢點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MemberAction_Sent_Click(object sender, EventArgs e)
    {
        string A_ename = string.IsNullOrEmpty(this.A_ename.Text) ? "" : this.A_ename.Text ;
        string p_MemberActionSTime = string.IsNullOrEmpty(this.p_MemberActionSTime.Value) ? "" : this.p_MemberActionSTime.Value; 
        string p_MemberActionETime = string.IsNullOrEmpty(this.p_MemberActionETime.Value) ? "" : this.p_MemberActionETime.Value; 
        string A_uid = string.IsNullOrEmpty(this.A_uid.Text) ? "" : this.A_uid.Text;
        string A_sid = string.IsNullOrEmpty(this.A_sid.Text) ? "" : this.A_sid.Text;
        string A_apin = string.IsNullOrEmpty(this.A_apin.Text) ? "" : this.A_apin.Text;
        JDBlib objJDBlib = new JDBlib();
        System.Data.DataTable dtMemberActionLog = objJDBlib.ConvertMemberActionLogToDataTable();
        MemberAction objMemberAction = objJDBlib.GetMemberActionLog(A_ename, p_MemberActionSTime, p_MemberActionETime, A_uid, A_sid, A_apin);
        MemberActionMsg.Text = "";
        if (objMemberAction != null)
        {
            if (objMemberAction.Status.ToString() == "0" && objMemberAction.Description.ToString() == "Success")
            {
                if (objMemberAction.Data != null)
                {
                    if (objMemberAction.Data.Count > 0)
                    {
                        for (int j = 0; j < objMemberAction.Data.Count; j++)
                        {
                            dtMemberActionLog.Rows.Add(
                                objMemberAction.Data[j].id,
                                objMemberAction.Data[j].名稱,
                                objMemberAction.Data[j].輸入,
                                objMemberAction.Data[j].輸出,
                                objMemberAction.Data[j].狀態碼,
                                objMemberAction.Data[j].訊息,
                                objMemberAction.Data[j].建立時間.ToString(),
                                objMemberAction.Data[j].JDB序號.ToString(),
                                objMemberAction.Data[j].執行內容.ToString());
                        }

                        MemberActionView.DataSource = dtMemberActionLog;
                        MemberActionView.DataBind();
                    }
                    else {
                        MemberActionMsg.Text = "<h3>查無資料!!</h3>";
                    }
                }
                else
                {
                    MemberActionMsg.Text = "<h3>查無資料!!</h3>";
                }
            }
            else
            {
                MemberActionMsg.Text = string.Format("<h3>{0}</h3>", objMemberAction.Description.ToString());
            }
        }
        else
        {
            MemberActionMsg.Text = "<h3>調用API發生未知的例外!!</h3>";
        }
    }

    /// <summary>
    /// 取得會員下注LOG查詢點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MemberQueue_Send_Click(object sender, EventArgs e)
    {
        string B_ename = string.IsNullOrEmpty(this.B_ename.Text) ? "" : this.B_ename.Text;
        string p_MemberQueueSTime = string.IsNullOrEmpty(this.p_MemberQueueSTime.Value) ? "" : this.p_MemberQueueSTime.Value;
        string p_MemberQueueETime = string.IsNullOrEmpty(this.p_MemberQueueETime.Value) ? "" : this.p_MemberQueueETime.Value;
        string B_uid = string.IsNullOrEmpty(this.B_uid.Text) ? "" : this.B_uid.Text;
        string B_sid = string.IsNullOrEmpty(this.B_sid.Text) ? "" : this.B_sid.Text;
        JDBlib objJDBlib = new JDBlib();
        System.Data.DataTable objtdMemberQueue = objJDBlib.ConvertMemberQueueLogToDataTable();
        MemberQueueLog objMemberQueueLog= objJDBlib.GetMemberQueueLog(B_ename, p_MemberQueueSTime, p_MemberQueueETime, B_uid, B_sid);
        MemberQueueMsg.Text = "";
        if (objMemberQueueLog != null)
        {
            if (objMemberQueueLog.Status.ToString() == "0" && objMemberQueueLog.Description.ToString() == "Success")
            {
                if (objMemberQueueLog.Data != null)
                {
                    if (objMemberQueueLog.Data.Count > 0)
                    {
                        for (int j = 0; j < objMemberQueueLog.Data.Count; j++)
                        {
                            objtdMemberQueue.Rows.Add(
                                objMemberQueueLog.Data[j].開分狀態,
                                objMemberQueueLog.Data[j].洗分狀態,
                                objMemberQueueLog.Data[j].H1序號,
                                objMemberQueueLog.Data[j].JDB序號,
                                objMemberQueueLog.Data[j].jqu_create_datetime,
                                objMemberQueueLog.Data[j].遊戲ID,
                                double.Parse(objMemberQueueLog.Data[j].剩下餘額.ToString()),
                                double.Parse(objMemberQueueLog.Data[j].下注金額.ToString()),
                                double.Parse(objMemberQueueLog.Data[j].彩金.ToString()),
                                double.Parse(objMemberQueueLog.Data[j].輸贏.ToString()),
                                objMemberQueueLog.Data[j].下注次數);
                        }
                        MemberQueueView.DataSource = objtdMemberQueue;
                        MemberQueueView.DataBind();
                    }
                    else
                    {
                        MemberQueueMsg.Text = "<h3>查無資料</h3>";
                    }
                }
                else
                {
                    MemberQueueMsg.Text = "<h3>查無資料</h3>";
                }
            }
            else
            {
                MemberQueueMsg.Text = string.Format("<h3>{0}</h3>", objMemberQueueLog.Description.ToString());
            }
        }else {
            MemberQueueMsg.Text = "<h3>調用API時，發生未知的例外!!</h3>";
        }
    }

    /// <summary>
    /// 取得JDB網址設定點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void JdbUrlConfigSend_Click(object sender, EventArgs e)
    {
        JDBlib objJDBlib = new JDBlib();
        JdbUrlConfig objJdbUrlConfig = objJDBlib.GetJdbUrlConfig();
        if (objJdbUrlConfig != null)
        {
            if (objJdbUrlConfig.Status.ToString() == "0" && objJdbUrlConfig.Description.ToString() == "Success")
            {
                lbJdbUrlConfig.Text = objJdbUrlConfig.Data[0].內容;
                lbJdbUrlConfigNoet.Text = objJdbUrlConfig.Data[0].註解;
            }
            else
            {
                lbJdbUrlConfig.Text = objJdbUrlConfig.Description.ToString();
            }
        }else {
            lbJdbUrlConfig.Text = "<h3>發生未知的例外!!!</h3>";
        }
    }

    /// <summary>
    /// 設定JDB網址點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btSetJdbUrlConfig_Click(object sender, EventArgs e)
    {
        string Url = string.IsNullOrEmpty(this.txtUrl.Text) ? "" : this.txtUrl.Text;
        JDBlib objJDBlib = new JDBlib();
        SetJdbUrlConfig objSetJdbUrlConfig = objJDBlib.SetJdbUrlConfig(Url);
        if (objSetJdbUrlConfig != null)
        {
            if (objSetJdbUrlConfig.Status.ToString() == "0" && objSetJdbUrlConfig.Description.ToString() == "Success")
            {
                SetJdbUrlConfigDescription.Text = objSetJdbUrlConfig.Description;
                SetJdbUrlConfigData.Text = objSetJdbUrlConfig.Data;
            }
            else {
                SetJdbUrlConfigDescription.Text = objSetJdbUrlConfig.Description.ToString();
            }
        }
        else
        {
            SetJdbUrlConfigDescription.Text = "<h3>發生未知的例外!!!</h3>";
        }
    }
    /// <summary>
    /// 重啟Server點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btRestartAllServer_Click(object sender, EventArgs e)
    {
        JDBlib objJDBlib = new JDBlib();
        RestartAllServer objRestartAllServer = objJDBlib.RestartAllServer();
        if (objRestartAllServer != null)
        {
            if (objRestartAllServer.Status.ToString() == "0" && objRestartAllServer.Description.ToString() == "Success")
            {
                lbRestartAllServer.Text = objRestartAllServer.Description;
                lbRestartAllServerNote.Text = objRestartAllServer.Data;
            }
            else
            {
                lbRestartAllServer.Text = objRestartAllServer.Description.ToString();
            }
        }
        else
        {
            lbRestartAllServer.Text = "<h3>發生未知的例外!!!</h3>";
        }
    }

    protected void MemberActionView_RowDataBound(object sender, GridViewRowEventArgs e)
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

                if (j == 8 || j == 2 || j == 3)
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
}