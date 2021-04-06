using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 用於警示頁的條件查詢
/// </summary>
public class SelAlertdata
{
    public string Type { get; set; }
    public string Source { get; set; }
    public string Condition { get; set; }
}

public partial class AutoAlert : System.Web.UI.Page
{
    public int a = 0;

    public string UptMsg = string.Empty;

    /***********************************/
    private int totalpage = 0;

    private int totalrow = 0;

    private int pageindex = 1;

    private bool istrcae = false;

    public bool IsTrace
    {
        get
        {
            return istrcae;
        }
    }

    public int TotalPage
    {
        get
        {
            return totalrow;
        }
    }

    public int PageIndex
    {
        get
        {
            if (!string.IsNullOrEmpty(Request["page"]))
            {
                if(!int.TryParse(Request["page"].ToString(),out pageindex))
                {
                    pageindex = 1;
                }
            }
            return pageindex;
        }
    }
    /********************************/
    /// <summary>
    /// 初始化登入裝置下拉選項
    /// </summary>
    private void InitdlSource()
    {
        string strdevice = string.Empty;
        string[] Device = null;
        if (Session["LoginDeviceList"] == null)
        {
            strdevice = Tracklib.GetLoginDevice();
            Session["LoginDeviceList"] = strdevice;
        }
        else
        {
            try
            {
                strdevice = Session["LoginDeviceList"].ToString();
            }
            catch (Exception ex)
            {
                strdevice = Tracklib.GetLoginDevice();
                Session["LoginDeviceList"] = strdevice;
            }
        }
        if (strdevice.IndexOf(',') > -1)
        {
            dlSource.Items.Clear();
            Device = strdevice.Split(',');
            for (int j = 0; j < Device.Length; j++)
            {
                ListItem objlititem = new ListItem();
                objlititem.Value = Device[j].Split('=')[0];
                objlititem.Text = Device[j].Split('=')[1];
                dlSource.Items.Add(objlititem);
            }
        }
        else
        {
            ListItem objlititem = new ListItem();
            objlititem.Value = strdevice.Split('=')[0];
            objlititem.Text = strdevice.Split('=')[1];
            dlSource.Items.Add(objlititem);
        }
    }

    /// <summary>
    /// 將IP轉換成查詢like如︰127.0%
    /// </summary>
    /// <param name="_Ip"></param>
    /// <returns></returns>
    private string WherelikeIp(string _Ip)
    {
        string Ip = _Ip;
        string[] Ipinfo = null;
        StringBuilder NewIp = new StringBuilder("");
        if (Ip.IndexOf('.') > -1)
        {
            Ipinfo = Ip.Split('.');
            for (int j = 0; j < Ipinfo.Length; j++)
            {
                if (Ipinfo[j] == "*")
                {
                    NewIp.Append("%");
                    break;
                }
                else
                {
                    NewIp.Append(Ipinfo[j]);
                    if ((Ipinfo.Length - 1) != j)
                    {
                        NewIp.Append(".");
                    }
                }
            }
        }else {
            NewIp.Append("%");
            NewIp.Append(_Ip);
            NewIp.Append("%");
        }

        return NewIp.ToString();
    }

    /// <summary>
    /// 建立SQL的Where字串
    /// </summary>
    /// <returns></returns>
    private string CreateWhere()
    {
        SelAlertdata[] objSel = null;

        StringBuilder SQL = new StringBuilder("");

        if (ConditionList.Items.Count > 0)
        {
            objSel = new SelAlertdata[ConditionList.Items.Count];
        }

        if (objSel != null)
        {
            for (int j = 0; j < ConditionList.Items.Count; j++)
            {
                objSel[j] = JsonConvert.DeserializeObject<SelAlertdata>(ConditionList.Items[j].Value);

                SQL.Append(" ");

                if ("IP" == objSel[j].Type.ToString().ToUpper())
                {
                    SQL.Append(objSel[j].Type);
                }
                else if ("ACCOUNT" == objSel[j].Type.ToString().ToUpper())
                {
                    SQL.Append("帳號");
                }
                else if ("SOURCE" == objSel[j].Type.ToString().ToUpper())
                {
                    SQL.Append("來源");
                }

                if ("SOURCE" == objSel[j].Type.ToString().ToUpper())
                {
                    SQL.Append("=");
                    SQL.Append(string.Format("'{0}'", objSel[j].Source, this)); //這裡需要轉換的原因是在轉成DataTable資料，也有做轉換，如果這裡不轉換會查不到資料
                }
                else
                {
                    if ("IP" == objSel[j].Type.ToString().ToUpper()) //如果是針對IP查詢，要特別過濾是否是要查網段的
                    {
                        SQL.Append(" like ");
                        SQL.Append(string.Format("'{0}'", WherelikeIp(objSel[j].Source)));
                    }
                    else
                    {
                        SQL.Append("=");
                        SQL.Append(string.Format("'{0}'", objSel[j].Source));
                    }
                }

                if (objSel[j].Condition.ToString().ToUpper() == "NONE")
                {
                    break;
                }
                else
                {
                    if ((ConditionList.Items.Count - 1) == j)  //避免最後加入的條件值不是選結束的
                    {
                        if (objSel[j].Condition.ToString().ToUpper() != "AND" && objSel[j].Condition.ToString().ToUpper() != "OR")
                        {
                            SQL.Append(" ");
                            SQL.Append(objSel[j].Condition);
                        }
                    }
                    else
                    {
                        SQL.Append(" ");
                        SQL.Append(objSel[j].Condition);
                    }
                }
            }
        }

        //Lib.AlertWritLog("AutoAlert.aspx.cs.CreateWhere", SQL.ToString());
        return SQL.ToString();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["listchkAuto"]))
            {
                if (Request["listchkAuto"].ToString().ToUpper() == "TRUE")
                {
                    chkAuto.AutoPostBack = false;
                    chkAuto.Checked = true;
                    chkAuto.AutoPostBack = true;
                }

                if (Request["listchkAuto"].ToString().ToUpper() == "FALSE")
                {
                    chkAuto.AutoPostBack = false;
                    chkAuto.Checked = false;
                    chkAuto.AutoPostBack = true;
                }
            }

            if (!string.IsNullOrEmpty(Request["IsCheck"]))
            {
                if (Request["IsCheck"].ToString().ToUpper() == "TRUE")
                {
                    IsCheck.AutoPostBack = false;
                    IsCheck.Checked = true;
                    IsCheck.AutoPostBack = true;
                }

                if (Request["IsCheck"].ToString().ToUpper() == "FALSE")
                {
                    IsCheck.AutoPostBack = false;
                    IsCheck.Checked = false;
                    IsCheck.AutoPostBack = true;
                }
            }
        }
        Timer1.Interval = initTimeInterval();
        GetRecordDate(PageIndex);
    }

    private int initTimeInterval()
    {
        int TimeInterval = 0;

        if (Session["TimeInterval"] == null)
        {
            if (!int.TryParse(Tracklib.TimeInterval, out TimeInterval))
            {
                TimeInterval = 10000;
            }

            Session["TimeInterval"] = TimeInterval;
        }
        else
        {
            if (Session["TimeInterval"] != null)
            {
                if (!int.TryParse(Session["TimeInterval"].ToString(), out TimeInterval))
                {
                    TimeInterval = 10000;
                }
            }else {
                TimeInterval = 10000;
            }  
        }

        return TimeInterval;
    }

    private DataTable CreateRecordDataTable()
    {
        DataTable dtRecord = new DataTable();
        dtRecord.Columns.Add("ID", typeof(string));
        dtRecord.Columns.Add("IP", typeof(string));
        dtRecord.Columns.Add("帳號", typeof(string));
        dtRecord.Columns.Add("來源", typeof(string));
        dtRecord.Columns.Add("登入時間", typeof(string));
        dtRecord.Columns.Add("追蹤目標", typeof(string)); 
        //dtRecord.Columns.Add("確認者", typeof(string));
        //dtRecord.Columns.Add("確認時間", typeof(string));
        dtRecord.Columns.Add("備註", typeof(string));
        return dtRecord;
    }

    private DataTable SelCondition(DataTable Source)
    {
        DataTable rTable = CreateRecordDataTable();
        string SqlWhere = CreateWhere();
        DataRow[] SelData = null;
        string ID = string.Empty;
        string IP = string.Empty;
        string Account = string.Empty;
        string sSource = string.Empty;
        string LoginTime = string.Empty;
        // string Confirm = string.Empty;
        // string ConfirmTime = string.Empty;
        string sNote = string.Empty;
        if (SqlWhere != "")
        {
            try
            {
                SelData = Source.Select(SqlWhere);
                for (int j = 0; j < SelData.Length; j++)
                {
                    ID = SelData[j]["ID"].ToString();
                    IP = SelData[j]["IP"].ToString();
                    Account = SelData[j]["帳號"].ToString();
                    sSource = SelData[j]["來源"].ToString();
                    LoginTime = SelData[j]["登入時間"].ToString();
                    //Confirm = SelData[j]["確認者"].ToString();
                    //ConfirmTime = SelData[j]["確認時間"].ToString();
                    sNote = SelData[j]["備註"].ToString();
                    rTable.Rows.Add(
                        ID,
                        IP,
                        Account,
                        sSource,
                        LoginTime,
                        // Confirm,
                        // ConfirmTime ,
                        sNote);
                }
            }
            catch (Exception ex)
            {
                FindRecordDateMsg.Text = ex.ToString();
            }
        }
        return rTable;
    }

    /// <summary>
    /// 產生及跳轉分頁
    /// </summary>
    /// <param name="ITotalCount">總筆數</param>
    /// <param name="IResult">結果0表成功，大於0表失敗</param>
    private void page(int ITotalCount)
    {
        int totalpage = (int)Math.Ceiling(double.Parse(string.IsNullOrEmpty(ITotalCount.ToString()) ? "0.0" : ITotalCount.ToString()) / double.Parse(Tracklib.Row.ToString()));

        int start = (pageindex - 1) <= 1 ? 1 : pageindex;

        if (totalpage <= 1)
        {
            totalpage = 1;
        }

        StringBuilder spage = new StringBuilder("");

        if (ITotalCount > 0)
        {
            spage.Append("<table border='1'><tr><td>");
            spage.Append("總筆數︰");
            spage.Append(ITotalCount);
            spage.Append("筆</td><td>第");
            spage.Append("<Select onchange='location.href=this.value'>");
            for (int j = 1; j <= totalpage; j++)
            {
                spage.Append(string.Format("<option value='AutoAlert.aspx?page={0}&listchkAuto={3}&IsCheck={4}' {2}>{1}</option>", j, j, ((PageIndex == j) ? "selected" : ""), chkAuto.Checked,IsCheck.Checked));
            }
            spage.Append("</Select>");
            spage.Append("頁</td></tr></table>");
            objpage.Text = spage.ToString();
        }
        else
        {
            if (ITotalCount <= 0)
            {
                //spage.Append("<h1>查無資料!!</h1>");
                spage.Clear();
                objpage.Text = spage.ToString();
            }
        }
    }

    /// <summary>
    /// 取得及繫結欲警示的資料
    /// </summary>
    private void GetRecordDate(int _Page = 1)
    {
        RqFindRecord objResult = Tracklib.FindRecord(_Page, Tracklib.Row, IsCheck.Checked);

        DataTable dtRecord = CreateRecordDataTable();

        DataTable SelResult = null;

        int count = 0;

        int cut = 0;

        if (objResult != null)
        {
            if (objResult.success)
            {
               
                if (objResult.data != null)
                {
                    if (objResult.data.dataList.Count > 0)
                    {
                        page(objResult.data.dataTotalCount);

                        for (int k = 0; k < objResult.data.dataList.Count; k++)
                        {
                            if (objResult.data.dataList[k].isCheck)
                            {
                                dtRecord.Rows.Add(objResult.data.dataList[k].id,
                                objResult.data.dataList[k].ip,
                                objResult.data.dataList[k].account,
                                objResult.data.dataList[k].source,
                                objResult.data.dataList[k].loginTime,
                                objResult.data.dataList[k].alarmMode,
                                //objResult.data[j].Inspector,
                                //objResult.data[j].CheckTime,
                                objResult.data.dataList[k].note);
                            }
                            else
                            {
                                cut++;
                            }
                        }
                       
                    }
                    count = objResult.data.dataList.Count;
                    count = count - cut;
                    if (ConditionList.Items.Count > 0)  //判斷是否存在過濾條件
                    {
                        Timer1.Enabled = false;
                        SelResult = SelCondition(dtRecord);
                        count = SelResult.Rows.Count;       //重新設定現有筆數，避免錯判，而造成無警示聲
                        if (count > 0)
                        {
                            FindRecordDateResult.DataSource = SelResult;
                            FindRecordDateResult.DataBind();
                            FindRecordDateMsg.Text = "";
                        }
                        else
                        {
                            SelResult = new DataTable();
                            FindRecordDateResult.DataSource = SelResult;
                            FindRecordDateResult.DataBind();
                            FindRecordDateMsg.Text = "查無資料!!";
                        }
                        Timer1.Enabled = true;
                        if (!chkAuto.Checked)
                        {
                            chkAuto.Checked = true;
                        }
                    }
                    else
                    {
                        FindRecordDateResult.DataSource = dtRecord;
                        FindRecordDateResult.DataBind();
                        FindRecordDateMsg.Text = "";
                        if (Timer1.Enabled == false)
                        {
                            Timer1.Enabled = true;
                        }
                    }
                    UptMsg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]";
                    Label2.Text = UptMsg;
                }
            }
            else
            {
                FindRecordDateMsg.Text = objResult.message;
            }

            a = count;
        }

    }

    /// <summary>
    /// 是否自動更新頁面事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkAuto_CheckedChanged(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("AutoAlert.aspx?page={0}&listchkAuto={1}&IsCheck={2}", Request["page"], chkAuto.Checked, IsCheck.Checked));
        /*
        Timer1.Enabled = chkAuto.Checked;

        if (!chkAuto.Checked)
        {
            UptMsg = "";
        }*/
    }

    /// <summary>
    /// Timer事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (chkAuto.Checked)
        {
            if (dlSource.Items.Count <= 0)
            {
                InitdlSource();
            }
            GetRecordDate(PageIndex);
        }
    }

    /// <summary>
    /// 按鈕命令事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FindRecordDateResult_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string result = string.Empty;
        string[] info = null;
        StringBuilder Msg = new StringBuilder("");
        Timer1.Enabled = false;
        if ("MARK".Equals(e.CommandName))
        {
            try
            {
                result = Tracklib.UptRecord(int.Parse(e.CommandArgument.ToString()), "****");
                ScriptManager.RegisterStartupScript(this, typeof(Button), Guid.NewGuid().ToString(), "alert('" + result.ToString() + "');location.href='AutoAlert.aspx';", true);
            }
            catch (Exception ex)
            {
                result = "[MARK]發生未知例外";
                Lib.AlertWritLog("AutoAlert.aspx.cs.FindRecordDateResult_RowCommand", ex.ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Button), Guid.NewGuid().ToString(), "alert('" + result + "');location.href='AutoAlert.aspx';", true);
            }
        }
        else if ("STOP".Equals(e.CommandName))
        {
            try
            {
                Msg.Clear();
                info = e.CommandArgument.ToString().Split('=');
                string blackid = FindTrackData(info[2]);
                if (!string.IsNullOrEmpty(blackid))
                {
                    result = Tracklib.EditTrack(int.Parse(blackid), false, info[1]);
                    //Msg.Append(string.Format("停止追蹤︰{0}", result));
                }else {
                    result = "查無資料，失敗!!";
                    //Msg.Append(string.Format("停止追蹤︰{0}", string.Format("帳號︰{0}無法反查ID!!", info[2])));
                } 
                result = Tracklib.UptRecord(int.Parse(info[0]), "****");
                Msg.Append(result);
                ScriptManager.RegisterStartupScript(this, typeof(Button), Guid.NewGuid().ToString(), "alert('" + Msg.ToString() + "');location.href='AutoAlert.aspx';", true);
            }
            catch (Exception ex)
            {
                result = "[STOP]發生未知例外";
                Lib.AlertWritLog("AutoAlert.aspx.cs.FindRecordDateResult_RowCommand", ex.ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Button), Guid.NewGuid().ToString(), "alert('" + result + "');location.href='AutoAlert.aspx';", true);
            }

        }
        else if ("FINDUID".Equals(e.CommandName))
        {
            string rvalue = string.Empty;
            bsSQL objSQl = new bsSQL();
            try
            {
                info = e.CommandArgument.ToString().Split('=');
                if (info[1].ToString().Trim().ToLower() == Tracklib.Manage.ToLower())
                {
                    rvalue = objSQl.GetMyAllUpFranchiser(1, info[0]); //如果是代理端回傳的值，就包含自己，所以不須再加工
                }
                else
                {
                    rvalue = objSQl.GetMyAllUpFranchiser(0, info[0]); //如果是會員端回傳的是通路端的值，所以要再加上會員帳號
                    if (!string.IsNullOrEmpty(rvalue))
                    {
                        rvalue = string.Format("{0}/{1}", rvalue, info[0]);
                    }
                }
                if (string.IsNullOrEmpty(rvalue))
                {
                    rvalue = "查無此帳號";
                }
                ScriptManager.RegisterStartupScript(this, typeof(Button), Guid.NewGuid().ToString(), "alert('族譜是︰" + rvalue + "');location.href='AutoAlert.aspx';", true);
            }
            catch (Exception ex)
            {
                result = "[FINDUID]發生未知例外";
                Lib.AlertWritLog("AutoAlert.aspx.cs.FindRecordDateResult_RowCommand", ex.ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Button), Guid.NewGuid().ToString(), "alert('" + result + "');location.href='AutoAlert.aspx';", true);
            }
        }
        Timer1.Enabled = true;
    }

    private string  FindTrackData(string Account)
    {
        RqGetTrack objRqGetTrack = null;
        string rvalue = string.Empty;
        if (Session["TrackData"] != null)
        {
            objRqGetTrack = (RqGetTrack)Session["TrackData"];
        }else { 
            objRqGetTrack = Tracklib.GetTrack(this,1,200,true);
        }
        if (objRqGetTrack != null)
        {
            if (objRqGetTrack.success)
            {
                if (objRqGetTrack.data != null)
                {
                    for (int j = 0; j < objRqGetTrack.data.dataList.Count; j++)
                    {
                        if (objRqGetTrack.data.dataList[j].target.ToString().Trim().ToLower() == Account.ToString().Trim().ToLower())
                        {
                            rvalue = objRqGetTrack.data.dataList[j].id.ToString();
                            break;
                        }
                    }
                }
            }
        }

        return rvalue;
    }

    protected void dlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtTarget.Visible = false;
        dlSource.Visible = false;
        if (dlType.SelectedValue == "Source")
        {
            txtTarget.Visible = false;
            dlSource.Visible = true;
        }
        else
        {
            txtTarget.Visible = true;
            dlSource.Visible = false;
        }
        InitdlSource();
    }

    /// <summary>
    /// 將條件值加入條件列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btAdd_Click(object sender, EventArgs e)
    {
        string Json = string.Empty;
        StringBuilder txtCondition = new StringBuilder();
        try
        {
            txtCondition.Append(dlType.Items[dlType.SelectedIndex].Text);
            txtCondition.Append("=");
            if (dlType.SelectedValue == "Source")
            {
                txtCondition.Append(dlSource.Items[dlSource.SelectedIndex].Text);
            }
            else
            {
                txtCondition.Append(txtTarget.Text);
            }
            if (dlCondition.SelectedValue == "NONE")
            {
                txtCondition.Append("結束");
            }
            else
            {
                txtCondition.Append(dlCondition.Items[dlCondition.SelectedIndex].Text);
            }

            if (dlType.SelectedValue == "Source")
            {
                Json = string.Format("\"Type\":\"{0}\",\"Source\":\"{1}\",\"Condition\":\"{2}\"",
                dlType.Items[dlType.SelectedIndex].Value.ToString(), dlSource.Items[dlSource.SelectedIndex].Value.ToString(), dlCondition.Items[dlCondition.SelectedIndex].Value.ToString());
                Json = "{" + Json + "}";
            }
            else
            {
                Json = string.Format("\"Type\":\"{0}\",\"Source\":\"{1}\",\"Condition\":\"{2}\"",
               dlType.Items[dlType.SelectedIndex].Value.ToString(), txtTarget.Text.ToString(), dlCondition.Items[dlCondition.SelectedIndex].Value.ToString());
                Json = "{" + Json + "}";
            }
            ListItem items = new ListItem();
            items.Value = Json;
            items.Text = txtCondition.ToString();
            ConditionList.Items.Add(items);
        }
        catch (Exception ex)
        {
            Lib.AlertWritLog("AutoAlert.aspx.cs.btAdd_Click", ex.ToString());
        }
    }

    protected void btCut_Click(object sender, EventArgs e)
    {
        if (ConditionList.SelectedIndex > -1)
        {
            ConditionList.Items.Remove(ConditionList.Items[ConditionList.SelectedIndex]);
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        GetRecordDate();
    }

    protected void FindRecordDateResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Text = Tracklib.ConvertDevice(e.Row.Cells[4].Text, this);
            /* 2020-12-03
             警示清單模式
            0 : 未定義
            1 : IP
            2 : 帳號
            3 : IP/帳號
             */
            if (e.Row.Cells[6].Text.ToString() == "0")
            {
                e.Row.Cells[6].Text = "未定義";
            }
            else if (e.Row.Cells[6].Text.ToString() == "1")
            {
                e.Row.Cells[6].Text = "IP";
            }
            if (e.Row.Cells[6].Text.ToString() == "2")
            {
                e.Row.Cells[6].Text = "帳號";
            }
            if (e.Row.Cells[6].Text.ToString() == "3")
            {
                e.Row.Cells[6].Text = @"IP/帳號";
            }
        }
    }

    protected void IsCheck_CheckedChanged(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("AutoAlert.aspx?page={0}&listchkAuto={1}&IsCheck={2}", Request["page"], chkAuto.Checked, IsCheck.Checked));
    }
}