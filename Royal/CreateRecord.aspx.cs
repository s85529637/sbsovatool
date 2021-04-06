using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateRecord : System.Web.UI.Page
{
    private int totalpage =0;

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

    public int TotalPage {
        get {
            return totalrow;
        }
    } 

    public int PageIndex {
        get {
            return pageindex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!Page.IsPostBack)
        {
            if (Session["UptDateTime"] != null && Request["cmd"] != null && Request["ChkCode"] != null && Request["Id"] != null && Request["Belong"] != null && Request["Uid"] != null)
            {
                if (Session["UptDateTime"].ToString() == Request["ChkCode"].ToString() && Request["cmd"] == "UPT")
                {
                    lbId.Text = Request["Id"].ToString();

                    if (Request["IsTrace"] != null)
                    {
                        if (Request["IsTrace"].ToString().ToUpper() == "是")
                        {
                            dlIsTrace.SelectedIndex = dlIsTrace.Items.IndexOf(dlIsTrace.Items.FindByValue("1"));
                        }
                        else
                        {
                            dlIsTrace.SelectedIndex = dlIsTrace.Items.IndexOf(dlIsTrace.Items.FindByValue("0"));
                        }
                    }

                    lbUid.Text = Request["Uid"].ToString();

                    txteBelong.Text = Request["Belong"].ToString();

                    Session["UptDateTime"] = null;
                }
            }

            if (!string.IsNullOrEmpty(Request["listtrace"]))
            {
                if (Request["listtrace"].Trim().ToUpper() == "TRUE")
                {
                    cbIsTrace.Checked = true;
                }

                if (Request["listtrace"].Trim().ToUpper() == "FALSE")
                {
                    cbIsTrace.Checked = false;
                }
            }
        }

        ChageTraceOP();
    }

    protected void btSent1_Click(object sender, EventArgs e)
    {
        string Target = string.IsNullOrEmpty(txtTarget.Text) ? "" : txtTarget.Text;

        string Type = txtType.SelectedValue;//string.IsNullOrEmpty(txtType.Text) ? "" : txtType.Text;

        string Belong = string.IsNullOrEmpty(txtBelong.Text) ? "" : txtBelong.Text;

        string Note = string.Empty;

        int iAccountType = int.Parse(AccountType.SelectedValue.ToString());

        bsSQL objSQL = new bsSQL();

        if (Target == "" || Type == "")
        {
            lbCreateTrackMsg.Text = "缺少參數!!!";

            return;
        }

        if (Type == "Account")
        {
            if (iAccountType == 1 || iAccountType == 2)
            {
                if (iAccountType == 1)
                {
                    //代理端是1
                    Note = objSQL.GetMyAllUpFranchiser(1, Target);
                }

                if (iAccountType == 2)
                {
                    //會員端是0
                    Note = objSQL.GetMyAllUpFranchiser(0, Target);
                }

                if (!string.IsNullOrEmpty(Note))
                {
                    txtBelong.Text = string.Format("{0}[({1})]", txtBelong.Text, Note);
                }
                else
                {
                    Lib.MsgBoxAndJump(Page, "查無此帳號!!", "CreateRecord.aspx");
                    return;
                }
            }
        }

        lbCreateTrackMsg.Text = "";

        lbCreateTrackMsg.Text = Tracklib.CreateTrack(txtTarget.Text, txtType.Text, txtBelong.Text);

        Lib.MsgBoxAndJump(Page, lbCreateTrackMsg.Text, "CreateRecord.aspx");
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

        if (ITotalCount > 0 )
        {
            spage.Append("<table border='1'><tr><td>");
            spage.Append("總筆數︰");
            spage.Append(ITotalCount);
            spage.Append("筆</td><td>第");
            spage.Append("<Select onchange='location.href=this.value'>");
            for (int j = 1; j <= totalpage; j++)
            {
                spage.Append(string.Format("<option value='CreateRecord.aspx?page={0}&listtrace={3}' {2}>{1}</option>", j, j, ((PageIndex == j) ? "selected" : ""), cbIsTrace.Checked));
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
    /// GetTrack
    /// </summary>
    private void GetGetTrackData(int Page, int Rows, bool IsTrace)
    {
        RqGetTrack objResult = Tracklib.GetTrack(this, Page, Rows, IsTrace);

        DataTable dtRecord = CreateTrackDataTable();
        /*
        if (objResult == null)
        {
            Response.Write("YYY");
        }
        else {
            Response.Write(objResult.success.ToString());
        }*/

        if (objResult == null)
        {
            GetTrackDataMsg.Text = "Json物件為空";
            return; 
        }

          if (objResult.success)
          {
              if (objResult.data != null)
              {
                  totalrow = objResult.data.dataTotalCount;

                  page(totalrow);

                  if (objResult.data.dataList.Count > 0)
                  {
                      for (int j = 0; j < objResult.data.dataList.Count; j++)
                      {
                          dtRecord.Rows.Add(
                          objResult.data.dataList[j].id,
                          objResult.data.dataList[j].target,
                          objResult.data.dataList[j].type,
                          objResult.data.dataList[j].updateTime,
                          objResult.data.dataList[j].createTime,
                          objResult.data.dataList[j].isTrace.ToString().ToUpper() == "TRUE" ? "是" : "否",
                          objResult.data.dataList[j].belong);
                      }
                  }

                  GetTrackDataResult.DataSource = dtRecord;
                  GetTrackDataResult.DataBind();
              }
        }
        else
        {
            if (objResult.message != null)
            {
                GetTrackDataMsg.Text = "API回傳︰" + objResult.message;
            }
        }
    }

    private DataTable CreateTrackDataTable()
    {
        DataTable dtRecord = new DataTable();
        dtRecord.Columns.Add("ID", typeof(string));
        dtRecord.Columns.Add("追蹤目標", typeof(string));
        dtRecord.Columns.Add("類型", typeof(string));
        dtRecord.Columns.Add("更新時間", typeof(string));
        dtRecord.Columns.Add("建立時間", typeof(string));
        dtRecord.Columns.Add("是否追蹤", typeof(string));
        dtRecord.Columns.Add("備註", typeof(string));
        return dtRecord;
    }

    protected void GetTrackDataResult_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)e.CommandSource;
        int iIndex = int.Parse(e.CommandArgument.ToString());
        GridViewRow row = gv.Rows[iIndex];
        string Id = row.Cells[2].Text.Trim();
        string Uid = row.Cells[3].Text.Trim();
        string IsTrace = row.Cells[7].Text.Trim();
        string Belong = row.Cells[8].Text.Trim();
        string result = string.Empty;
        if ("Del".Equals(e.CommandName))
        {
            try
            {
                result = Tracklib.DelTrack(int.Parse(Id));
            }
            catch (Exception ex)
            {
                result = "發生未知例外";
                Lib.AlertWritLog("CreateRecord.aspx.cs.GetTrackDataResult_RowCommand", ex.ToString());
            }

            Lib.MsgBoxAndJump(Page, result, "CreateRecord.aspx");

        }
        else if ("Upt".Equals(e.CommandName))
        {

            Session["UptDateTime"] = DateTime.Now.ToString("yyyyyMMddHHmmssffff");

            Response.Redirect(string.Format("CreateRecord.aspx?cmd=UPT&Id={0}&IsTrace={1}&Belong={2}&ChkCode={3}&Uid={4}&page={5}&listtrace={6}", Id, IsTrace, Belong, Session["UptDateTime"], Uid,Request["page"], cbIsTrace.Checked));
        }
    }

    protected void bteditTrack_Click(object sender, EventArgs e)
    {
        string id = string.IsNullOrEmpty(lbId.Text) ? "" : lbId.Text;

        string IsTrcae = dlIsTrace.SelectedValue.ToString() == "1" ? "true" : "false";

        string Belong = string.IsNullOrEmpty(txteBelong.Text) ? "" : txteBelong.Text;

        string result = string.Empty;

        if (id == "")
        {
            return;
        }

        try
        {
            result = Tracklib.EditTrack(int.Parse(id), IsTrcae.ToString().ToUpper() == "TRUE" ? true : false, Belong);
        }
        catch (Exception ex)
        {
            Lib.AlertWritLog("CreateRecord.aspx.cs.bteditTrack_Click", ex.ToString());

            result = "發生未知例外";
        }

        lbId.Text = string.Empty;

        txteBelong.Text = string.Empty;

        Lib.MsgBoxAndJump(Page, result, string.Format("CreateRecord.aspx?page={0}&listtrace={1}", Request["page"], cbIsTrace.Checked));
    }


    protected void btsend_Click1(object sender, EventArgs e)
    {
        string Ip = string.IsNullOrEmpty(txtIp.Text) ? "" : txtIp.Text;

        string Uid = string.IsNullOrEmpty(txtUid.Text) ? "" : txtUid.Text;

        string Source = string.IsNullOrEmpty(txtSource.Text) ? "" : txtSource.Text;

        if (Ip == "" || Uid == "" || Source == "")
        {
            lbCreateRecordMsg.Text = "缺少參數!!!";

            return;
        }

        lbCreateRecordMsg.Text = "";

        lbCreateRecordMsg.Text = Tracklib.F_CreateRecord(Ip, Uid, Source);
    }

    /// <summary>
    /// 載入資料
    /// </summary>
    private void ChageTraceOP()
    {
        string sCurrPage = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString();

        int CurrPage = 0;

        if (!int.TryParse(sCurrPage, out CurrPage))
        {
            CurrPage = 1;
        }

        pageindex = CurrPage;

        GetGetTrackData(CurrPage, Tracklib.Row, cbIsTrace.Checked);
    }

    protected void cbIsTrace_CheckedChanged(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("CreateRecord.aspx?page={0}&listtrace={1}",Request["page"], cbIsTrace.Checked));
    }
}