using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Text;
using RTGLib;

public partial class UnReturnAccount : NewBasePage
{
    private static readonly string _SubSystem = ConfigurationManager.AppSettings["SubSystem"].ToString();

    public int ReloadInterval = string.IsNullOrEmpty( ConfigurationManager.AppSettings["ReloadInterval"].ToString()) ? 20000 : int.Parse(ConfigurationManager.AppSettings["ReloadInterval"].ToString());

    private JDBlib objjdb = new JDBlib("5");

    private int PageIndex = 1;

    private int PageSize = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PageSize"].ToString()) ? 50 : int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["isLogin"] == null) Response.Redirect("Logout.aspx");

        if (!IsPostBack)
        {
            if (Session["Curr_ThirdPartyId"] != null)
            {
                ddlThirdPartyId.SelectedValue = Session["Curr_ThirdPartyId"].ToString();
            }

            if (Session["page"] == null) Session["page"] = 1;
            Session["page"] = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString();
            if (!int.TryParse(Session["page"].ToString(), out PageIndex))
            {
                PageIndex = 1;
            }
            Timer1.Interval = ReloadInterval;
            initddLoin();
            GetOpenList(PageIndex, PageSize);
        }
    }

    /// <summary>
    /// 列繫結事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvList_RowDataBound(object sender,GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink txtLk = (HyperLink)e.Row.FindControl("mbrstats");

            Literal scriptarea = (Literal)e.Row.FindControl("txtscript");

            Button kick = (Button)e.Row.Cells[0].Controls[0];

            if (ddlThirdPartyId.SelectedValue.ToUpper() == "JDB")
            {
                txtLk.Text = "檢視";
                txtLk.NavigateUrl = "javascript:ShowJDBStatus('" + txtLk.ClientID + "','" + e.Row.Cells[2].Text + "','" + e.Row.Cells[4].Text + "','" + ddlThirdPartyId.SelectedValue + "','');";
                //scriptarea.Text = "<script >jdbchgvalue('" + txtLk.ClientID + "','" + e.Row.Cells[2].Text + "','" + e.Row.Cells[4].Text + "','" + ddlThirdPartyId.SelectedValue +  "');</script>";
            }
            else if (ddlThirdPartyId.SelectedValue.ToUpper() == "ROYAL")
            {
                txtLk.Text = "檢視";
                txtLk.NavigateUrl = "javascript:ShowRoyalStatus('" + txtLk.ClientID + "','" + e.Row.Cells[2].Text + "','" + e.Row.Cells[4].Text + "','" + ddlThirdPartyId.SelectedValue + "','');";
                //scriptarea.Text = "<script >royalchgvalue('" + txtLk.ClientID + "','" + e.Row.Cells[2].Text + "','" + e.Row.Cells[4].Text + "','" + ddlThirdPartyId.SelectedValue + "');</script>";
            }
            else if (ddlThirdPartyId.SelectedValue.ToUpper() == "RTG")
            {
                txtLk.Text = "檢視";
                txtLk.NavigateUrl = "javascript:ShowRTGStatus('" + txtLk.ClientID + "','" + e.Row.Cells[2].Text + "','" + e.Row.Cells[4].Text + "','" + ddlThirdPartyId.SelectedValue + "','','" + e.Row.Cells[10].Text + "');";
                //scriptarea.Text = "<script >royalchgvalue('" + txtLk.ClientID + "','" + e.Row.Cells[2].Text + "','" + e.Row.Cells[4].Text + "','" + ddlThirdPartyId.SelectedValue + "');</script>";
            }
            else if (ddlThirdPartyId.SelectedValue.ToUpper() == "GCLUB")
            {
                //txtLk.Text = "檢視";
                //txtLk.Enabled = false;
                kick.Visible = false;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ITotalCount">總筆數</param>
    /// <param name="SResultMessage">SP返回的訊息</param>
    /// <param name="IResult">結果0表成功，大於0表失敗</param>
    private void page(int ITotalCount, string SResultMessage, int IResult)
    {
        int totalpage =(int) Math.Ceiling(double.Parse(string.IsNullOrEmpty(ITotalCount.ToString())? "0.0" : ITotalCount.ToString()) / double.Parse(PageSize.ToString()));
        int start = (PageIndex - 1)<=1 ? 1 : PageIndex;

        if (totalpage <= 1)
        {
            totalpage = 1;
        }
        StringBuilder spage = new StringBuilder("");
        if (ITotalCount > 0 && IResult == 0)
        {
            spage.Append("<table border='1'><tr><td>");
            spage.Append("總筆數︰");
            spage.Append(ITotalCount);
            spage.Append("筆</td><td>第");
            spage.Append("<Select onchange='location.href=this.value'>");
            for (int j = 1; j <= totalpage; j++)
            {
                spage.Append(string.Format("<option value='UnReturnAccount.aspx?page={0}' {2}>{1}</option>", j, j,((PageIndex==j)?"selected": "")));
            }
            spage.Append("</Select>");
            spage.Append("頁</td></tr></table>");
            objpage.Text = spage.ToString();
        }else{
            if (ITotalCount <= 0)
            {
                spage.Append("<h1>查無資料!!</h1>");
                objpage.Text = spage.ToString();
            }else if (IResult > 0){
                spage.Append(string.Format("<h1>{0}</h1>", SResultMessage));
                objpage.Text = spage.ToString();
            }
        }
    }

    // 取得清單
    private void GetOpenList(int PageIndex, int PageSize)
    {
        int ITotalCount = 0;
        string SResultMessage = string.Empty;
        int IResult = 0;

        #region 排序判斷
        string se = ViewState["NowSE"] != null ? ViewState["NowSE"].ToString() : string.Empty;
        SortDirection sd = ViewState["NowSD"] != null ? (SortDirection)ViewState["NowSD"] : SortDirection.Ascending;

        string Sort = string.Empty;
        if (sd == SortDirection.Ascending)
        {
            Sort = se;
        }
        else
        {
            Sort = se + " DESC";
        }
        #endregion

        if (Session["Curr_ThirdPartyId"] == null)
        {
            Session["Curr_ThirdPartyId"] = ddlThirdPartyId.SelectedValue;
        }

        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetUnReturnAccount(Session["Curr_ThirdPartyId"].ToString(), txtClubEname.Text.Trim(), PageIndex, PageSize, ref ITotalCount, ref SResultMessage, ref IResult);
        //DataTable dt = bssql.GetUnReturnAccount(ddlThirdPartyId.SelectedValue, txtClubEname.Text.Trim());
        DataView dv = dt.DefaultView;
        dv.Sort = Sort;
        gvList.DataSource = dv;
        gvList.DataBind();
        page(ITotalCount, SResultMessage,IResult);
    }

    protected void chkAuto_CheckedChanged(object sender, EventArgs e)
    {
        Timer1.Enabled = chkAuto.Checked;
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        GetOpenList(PageIndex, PageSize);
    }
    protected void ddlThirdPartyId_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Curr_ThirdPartyId"] = ddlThirdPartyId.SelectedValue;
        initddLoin();
        GetOpenList(PageIndex, PageSize);
    }
    protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetOpenList(PageIndex, PageSize);
    }

    /// <summary>
    /// 依館別決定是否顯示在線狀態
    /// </summary>
    private void initddLoin()
    {
        switch (ddlThirdPartyId.SelectedValue.ToString())
        {
            case "All":
                gvList.Columns[1].Visible = false;
                break;
            case "Royal":
                gvList.Columns[1].Visible = true;
                break;
            case "Golden":
                gvList.Columns[1].Visible = false;
                break;
            case "JDB":
                gvList.Columns[1].Visible = true;
                break;
            case "RTG":
                gvList.Columns[1].Visible = true;
                break;
            case "GCLUB":
                gvList.Columns[1].Visible = false;
                break;
            default:
                gvList.Columns[1].Visible = false;
                break;
        }
    }
    protected void gvList_Sorting(object sender, GridViewSortEventArgs e)
    {
        string NowSE = ViewState["NowSE"] != null ? ViewState["NowSE"].ToString() : string.Empty;
        SortDirection NowSD = ViewState["NowSD"] != null ? (SortDirection)ViewState["NowSD"] : SortDirection.Ascending;

        if (string.IsNullOrEmpty(NowSE))
        {
            NowSE = e.SortExpression;
            NowSD = SortDirection.Ascending;
        }

        if (NowSE != e.SortExpression)
        {
            NowSE = e.SortExpression;
            NowSD = SortDirection.Ascending;
        }
        else
        {
            if (NowSD == SortDirection.Ascending)
            {
                NowSD = SortDirection.Descending;
            }
            else
            {
                NowSD = SortDirection.Ascending;
            }
        }
        ViewState["NowSD"] = NowSD;
        ViewState["NowSE"] = NowSE;

        GetOpenList(PageIndex, PageSize);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        GetOpenList(PageIndex, PageSize);
    }


    protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string ClubId = string.Empty;
        string ClubEname = string.Empty;
        string GameId = string.Empty;
        string ThirdPartyId = string.Empty;
        string ZuBie =string.Empty ;
        string Msg = string.Empty;
        if ("Kick".Equals(e.CommandName))
        {
            GridView gv = (GridView)e.CommandSource;
            int iIndex = int.Parse(e.CommandArgument.ToString());
            GridViewRow row = gv.Rows[iIndex];
            ClubId = row.Cells[2].Text.Trim();
            ClubEname = row.Cells[3].Text.Trim();
            GameId = row.Cells[5].Text.Trim();
            ThirdPartyId = row.Cells[8].Text.Trim();
            ZuBie =  row.Cells[10].Text.Trim();
            string Status = "", Message = "", Alert = "踢線", Success = "成功", Failure = "失敗";
            switch (ThirdPartyId)
            {
                case "Royal":
                    // 改由設定檔抓遊戲對應代碼
                    var key = ConfigurationManager.AppSettings.AllKeys.Where(x => x == "RoyalGame2Id_" + GameId).FirstOrDefault();
                   
                    if (key != null)
                    {
                        Message = Royal2Kick(ClubId, ref Status);
                    }
                    else
                    {
                        Message = RoyalKick(ClubId, ref Status);
                    }
                    break;
                case "Golden":
                    Message = GoldenKick(ClubEname, ref Status);
                    break;
                case "Sova":
                    Message = SovaKick(ClubId, ref Status);
                    break;
                case "JDB":
                    JDBlib JDBTool = new JDBlib("0");
                    Msg = JDBTool.kickMember(ClubId, ref Status);
                    if (Status == "1" && Msg == "Success")
                    {
                        Status = "1";
                    }else {
                        Status = "0";
                    }
                    break;
                case "RTG":
                    RGTlib objRGTlib = new RGTlib();
                    KickUserResult objKickUserResult = objRGTlib.KickUserMember(ClubId, ZuBie);
                    if (objKickUserResult.MsgID.ToString() == "0")
                    {
                        Status = "1";
                    }else {
                        Status = objKickUserResult.MsgID.ToString();
                        Failure = objKickUserResult.Message.ToString();
                    }
                    break;
                default:
                    Alert ="尚未開放";
                    break;
            }

            if (Status != "")
            {
                Alert = ((Status == "1") ? Success : Failure);
            }

            Lib.MsgBox(Page, Alert);
        }
    }

    protected void gvList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn = (Button)e.Row.Cells[0].Controls[0];
            btn.CommandArgument = e.Row.RowIndex.ToString();
            //在產生 GridView每一列的時候，偷偷加註每一列的索引號碼！放到按鈕裡面
        }
    }

    private string RoyalKick(String ClubId, ref string Status)
    {
        string sValue = "";
        using (SlotGameWS.SlotGameWSSoapClient ws = new SlotGameWS.SlotGameWSSoapClient())
        {
           
            XmlNode root = ws.KickUser("mobile", ClubId);
            foreach (XmlNode cn in root.ChildNodes)
            {
                switch (cn.Name)
                {
                    case "Status":
                        Status = cn.InnerText;
                        break;
                    case "Description":
                        sValue = cn.InnerText;
                        break;
                    default:
                        break;
                }
            }
        }
        return sValue;
    }

    private string Royal2Kick(String ClubId, ref string Status)
    {
        string sValue = "";
        using (SlotGameWS2.SlotGameWSSoapClient ws2 = new SlotGameWS2.SlotGameWSSoapClient())
        {
            XmlNode root = ws2.KickUser(_SubSystem, "mobile", ClubId);
            foreach (XmlNode cn in root.ChildNodes)
            {
                switch (cn.Name)
                {
                    case "Status":
                        Status = cn.InnerText;
                        break;
                    case "Description":
                        sValue = cn.InnerText;
                        break;
                    default:
                        break;
                }
            }
        }
        return sValue;
    }

    private string GoldenKick(String ClubEname, ref string Status)
    {
        using (ruby4ws.GameCommand ws = new ruby4ws.GameCommand())
        {
            ws.Url = ConfigurationManager.AppSettings["Golden.WebService"].ToString();
            ws.Discover();
            Status = ws.KickUser(ClubEname);
        }
        return "";
    }

    private string SovaKick(String ClubId, ref string Status)
    {
        string sValue = "";
        using (MicroSovaWS.MicroSovaWSSoapClient ws = new MicroSovaWS.MicroSovaWSSoapClient())
        {
            Status = ws.ServerKickPlayer(ClubId).ToString();
        }
        return sValue;
    }
}