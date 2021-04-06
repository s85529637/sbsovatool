using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class GetPlayerInfoAdminBatch : System.Web.UI.Page
{
    private int PageIndex = 1;

    private int PageSize = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PageSize"].ToString()) ? 50 : int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

    /// <summary>
    /// 將皇家電子API的版本，依GameId為Key塞入 Dictionary內
    /// </summary>
    /// <param name="GameId"></param>
    /// <returns></returns>
    private void InitRoyalApiVersionByGameId()
    {
        Dictionary<string, string> Royal1GameId = new Dictionary<string, string>();

        Dictionary<string, string> Royal2GameId = new Dictionary<string, string>();

        if (Session["Royal1GameId"] != null && Session["Royal2GameId"] != null)
        {
            return;
        }

        foreach (string keys in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
        {
            if (Session["Royal1GameId"] == null)
            {
                if (keys.StartsWith("RoyalGameId"))
                {
                    Royal1GameId.Add(System.Configuration.ConfigurationManager.AppSettings[keys].Trim(), "1");
                }
            }

            if (Session["Royal2GameId"] == null)
            {
                if (keys.StartsWith("RoyalGame2Id"))
                {
                    Royal2GameId.Add(keys.Split('_')[1], "2");
                }
            }
        }

        if (Session["Royal1GameId"] == null)
        {
            Session["Royal1GameId"] = Royal1GameId;
        }

        if (Session["Royal2GameId"] == null)
        {
            Session["Royal2GameId"] = Royal2GameId;
        }
    }

    /// <summary>
    /// 依GameId取得皇電API版本號
    /// </summary>
    /// <param name="GameId"></param>
    private string GetRoyalApiVersionByGameId(string GameId)
    {
        string rvalue = string.Empty;

        Dictionary<string, string> Royal1GameId = null;

        Dictionary<string, string> Royal2GameId = null;

        if (Session["Royal1GameId"] != null && Session["Royal2GameId"] != null)
        {
            Royal1GameId = (Dictionary<string, string>)Session["Royal1GameId"];
            Royal2GameId = (Dictionary<string, string>)Session["Royal2GameId"];
        }else {
            InitRoyalApiVersionByGameId();
            Royal1GameId = (Dictionary<string, string>)Session["Royal1GameId"];
            Royal2GameId = (Dictionary<string, string>)Session["Royal2GameId"];
        }

        if (Royal1GameId.Keys.Contains(GameId))
        {
            rvalue = "1";

        }else if (Royal2GameId.Keys.Contains(GameId))
        {
            rvalue = "2";
        }

        return rvalue;
    }

    /// <summary>
    /// 遞迴的方式列將XmlNode轉成字串
    /// </summary>
    /// <param name="objxml"></param>
    /// <param name="Msg"></param>
    /// <returns></returns>
    private string ConverXmlNodeToString(XmlNode objxml, StringBuilder Msg)
    {
       
        if (objxml.ChildNodes.Count > 0)
        {
            for (int j = 0; j < objxml.ChildNodes.Count; j++)
            {
                Msg.Append("[");
                Msg.Append(objxml.ChildNodes[j].Name);
                Msg.Append(":");
                Msg.Append(objxml.ChildNodes[j].Value);
                Msg.Append("(");
                Msg.Append(objxml.ChildNodes[j].InnerText);
                Msg.Append(")");
                Msg.Append("]");
                if (objxml.ChildNodes[j].ChildNodes.Count > 0)
                {
                    Msg.Append(ConverXmlNodeToString(objxml.ChildNodes[j], Msg));
                }
            }
        }

        return Msg.ToString();
    }

    /// <summary>
    /// 調用皇電API(一代或二代)，檢查是否存在帳務
    /// </summary>
    /// <param name="PlayerId"></param>
    /// <param name="session_id"></param>
    /// <returns>True 表有帳務，False表無帳務</returns>
    private bool GetRoyalHasAccount(string Version, string SubSystem, string WebId, string PlayerId, string Session_id)
    {
        SlotGameWS.SlotGameWSSoapClient objRoyal1 = new SlotGameWS.SlotGameWSSoapClient();   //一代
        SlotGameWS2.SlotGameWSSoapClient objRoyal2 = new SlotGameWS2.SlotGameWSSoapClient(); //二代
        XmlNode objxml = null;
        XmlNode objflashxml = null;
        string SessionId = string.Empty;
        int DataCount = -999;
        int Royal2Status = 0;
        bool rvalue = false;
        bool isError = false;
        StringBuilder LogMsg = new StringBuilder("");
        string param = string.Format("傳入參數=Version:{0},SubSystem:{1},WebId:{2},PlayerId:{3},Session_id:{4}", Version, SubSystem, WebId, PlayerId, Session_id);
        LogMsg.Append(param);
        try
        {
            /*******************************************************/
            //測試區段
            /*string xmlContent = @"<root RowCount='1'> 
                                       <DataInfo>
                                           <RowId>1</RowId>
                                           <SequenNumber>1131085</SequenNumber>
                                           <SessionNo>20210113135854640</SessionNo>
                                           <PlayerId>2005293653</PlayerId>
                                           <WebId>Mobile</WebId>
                                           <GameID>48</GameID>
                                           <AccDenom>1.0000</AccDenom>
                                           <PlayDenom>0.0100</PlayDenom>
                                           <BetAmt>5.0000</BetAmt>
                                           <WinAmt>-5.0000</WinAmt>
                                           <JackpotAccumulateAmt>0.0000</JackpotAccumulateAmt>
                                           <GambleWinAmt>0.0000</GambleWinAmt>
                                           <JackpotWinAmt>0.0000</JackpotWinAmt>
                                           <PrepayAmt>0.0000</PrepayAmt>
                                           <BeforeCredit>49452.9400</BeforeCredit>
                                           <AfterCredit>49447.9400</AfterCredit>
                                           <CurrencyRate>1.1110</CurrencyRate>
                                           <PlayTime>2021-01-13 13:59:10</PlayTime>
                                           </DataInfo>
                                           <DataInfo>
                                           <RowId>2</RowId>
                                           <SequenNumber>1131086</SequenNumber>
                                           <SessionNo>20210113135854640</SessionNo>
                                           <PlayerId>2005293653</PlayerId>
                                           <WebId>Mobile</WebId>
                                           <GameID>48</GameID>
                                           <AccDenom>1.0000</AccDenom>
                                           <PlayDenom>0.0100</PlayDenom>
                                           <BetAmt>5.0000</BetAmt>
                                           <WinAmt>-5.0000</WinAmt>
                                           <JackpotAccumulateAmt>0.0000</JackpotAccumulateAmt>
                                           <GambleWinAmt>0.0000</GambleWinAmt>
                                           <JackpotWinAmt>0.0000</JackpotWinAmt>
                                           <PrepayAmt>0.0000</PrepayAmt>
                                           <BeforeCredit>49447.9400</BeforeCredit>
                                           <AfterCredit>49442.9400</AfterCredit>
                                           <CurrencyRate>1.1110</CurrencyRate>
                                           <PlayTime>2021-01-13 13:59:13</PlayTime>
                                        </DataInfo>
                                 </root> ";
           //xmlContent = @"<root RowCount='0'/> ";
           XmlDocument doc = new XmlDocument();
           doc.LoadXml(xmlContent);
           objxml = doc.DocumentElement;
           StringBuilder txtMsg = new StringBuilder("");
           Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount111",
                           objxml.InnerXml.ToString());
            return true;*/
            /*****************************************/

            if (!string.IsNullOrEmpty(Version))
            {
                if (Version == "1")
                {
                    try
                    {
                        objxml = objRoyal1.GetPlayerGameHistory(PlayerId, WebId, Session_id, 1, 10);

                        if (objxml != null)
                        {
                            if (objxml.ChildNodes.Count > 0)
                            {
                                LogMsg.Append("調用一代API結果︰");
                                LogMsg.Append(objxml.InnerXml.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("調用一代API發生例外︰{0}", ex.ToString()));
                    }
                }
                else if (Version == "2")
                {
                    try
                    {
                        objxml = objRoyal2.GetPlayerGameHistory(PlayerId, SubSystem, WebId, Session_id, 1, 10);

                        if (objxml != null)
                        {
                            if (objxml.ChildNodes.Count > 0)
                            {
                                LogMsg.Append("調用二代API結果︰");
                                LogMsg.Append(objxml.InnerXml.ToString());
                                Royal2Status = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("調用二代API發生例外︰{0}", ex.ToString()));
                        Royal2Status = 0;
                    }

                    try
                    {
                        if (Royal2Status == 0)
                        {
                            objflashxml = objRoyal2.GetPlayerFishGameMinuteSummery(PlayerId, SubSystem, WebId, Session_id, 1, 10);

                            if (objflashxml != null)
                            {
                                if (objflashxml.ChildNodes.Count > 0)
                                {
                                    LogMsg.Append("調用魚機API結果︰");
                                    LogMsg.Append(objflashxml.InnerXml.ToString());
                                    Royal2Status = 1;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("調用魚機API發生例外︰{0}", ex.ToString()));
                        Royal2Status = 0;
                    }
                }
                else
                {
                    Lib.MsgBox(this, "皇家API版本判別程序無法判別版本!!");
                    return true;
                }
            }
            else
            {
                Lib.MsgBox(this, "版本號為空!!");
                return true;
            }

            if (Version == "1")
            {
                if (objxml != null)
                {
                    if (objxml.ChildNodes.Count > 0)
                    {
                        SessionId = objxml.SelectNodes("//DataInfo/SessionNo")[0].InnerText;

                        if (SessionId != null)
                        {
                            rvalue = true;
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回傳開分號為NULL，請聯絡RD查看");
                            isError = true;
                        }
                    }
                    else
                    {
                        rvalue = false;
                    }
                }
                else
                {
                    Lib.MsgBox(this, "API回傳NULL，請聯絡RD查看");
                    isError = true;
                }
            }
            else if (Version == "2")
            {
                rvalue = false;

                if (objxml != null)
                {
                    if (objxml.ChildNodes.Count > 0)
                    {
                        SessionId = objxml.SelectNodes("//DataInfo/SessionNo")[0].InnerText;

                        if (SessionId != null)
                        {
                            rvalue = true;
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回傳開分號為NULL，請聯絡RD查看");
                            isError = true;
                        }
                    }
                }

                if (objflashxml != null)
                {
                    if (objflashxml.ChildNodes.Count > 0)
                    {
                        SessionId = objflashxml.SelectNodes("//DataInfo/DataCount")[0].InnerText;

                        if (SessionId != null)
                        {
                            if (!int.TryParse(SessionId, out DataCount))
                            {
                                Lib.MsgBox(this, "API回傳魚機筆數為非數字，請聯絡RD查看");

                                isError = true;
                            }

                            if (DataCount > 0)
                            {
                                rvalue = true;
                            }
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回傳魚機筆數為NULL，請聯絡RD查看");
                            isError = true;
                        }
                    }
                }

                if (objxml == null && objflashxml == null)
                {
                    Lib.MsgBox(this, "API回傳NULL，請聯絡RD查看");
                    isError = true;
                }
            }

        }
        catch (Exception ex)
        {
            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount", ex.ToString());
            Lib.MsgBox(this, "發生未知例外，請聯絡RD查看[" + ex.Message + "]");
            isError = true;
        }

        if (isError)  //如果有錯誤，則回傳True，迫使程式無法強制回復帳號狀態
        {
            rvalue = true;
        }

        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount", LogMsg.ToString());

        return rvalue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ITotalCount">總筆數</param>
    /// <param name="SResultMessage">SP返回的訊息</param>
    /// <param name="IResult">結果0表成功，大於0表失敗</param>
    private void page(int ITotalCount, string SResultMessage, int IResult)
    {
        int totalpage = (int)Math.Ceiling(double.Parse(string.IsNullOrEmpty(ITotalCount.ToString()) ? "0.0" : ITotalCount.ToString()) / double.Parse(PageSize.ToString()));

        int start = (PageIndex - 1) <= 1 ? 1 : PageIndex;

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
                spage.Append(string.Format("<option value='GetPlayerInfoAdminBatch.aspx?page={0}' {2}>{1}</option>", j, j, ((PageIndex == j) ? "selected" : "")));
            }
            spage.Append("</Select>");
            spage.Append("頁</td></tr></table>");
            objpage.Text = spage.ToString();
        }
        else
        {
            if (ITotalCount <= 0)
            {
                spage.Append("<h1>查無資料!!</h1>");
                objpage.Text = spage.ToString();
            }
            else if (IResult > 0)
            {
                spage.Append(string.Format("<h1>{0}</h1>", SResultMessage));
                objpage.Text = spage.ToString();
            }
        }
    }

    /// <summary>
    /// 復原會員狀態
    /// </summary>
    /// <param name="_Club_id"></param>
    /// <param name="_Session"></param>
    /// <returns></returns>
    private int ResetMemberStatus(string _Club_id,string _Session)
    {
        dbSQL sql = new dbSQL();
        SqlCommand cmd = new SqlCommand("dbo.C_PowerReturnAccount");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("F58B47EF-B7ED-486E-A06A-DE99EA3E91C5");
        cmd.Parameters.Add("@Club_id", SqlDbType.NChar, 20).Value = _Club_id;
        cmd.Parameters.Add("@SessionNo", SqlDbType.NChar, 50).Value = _Session;
        var rows = sql.RunSQL(cmd);
        return rows;
    }

    /// <summary>
    /// 取得未洗分清單
    /// </summary>
    /// <param name="PageIndex"></param>
    /// <param name="PageSize"></param>
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

        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetUnReturnAccount("Royal", "", PageIndex, PageSize, ref ITotalCount, ref SResultMessage, ref IResult);
        DataView dv = dt.DefaultView;
        dv.Sort = Sort;
        gvList.DataSource = dv;
        gvList.DataBind();
        page(ITotalCount, SResultMessage, IResult);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitRoyalApiVersionByGameId();

            if (!string.IsNullOrEmpty(Request["page"]))
            {
                if (Session["page"] == null) Session["page"] = 1;
                Session["page"] = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString();
                if (!int.TryParse(Session["page"].ToString(), out PageIndex))
                {
                    PageIndex = 1;
                }
                GetOpenList(PageIndex, PageSize);
                if (gvList.Rows.Count > 0)
                {
                    btCheckedAll.Visible = true;
                    btprocess.Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// 排序事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


    /// <summary>
    /// 取得未洗分資料的點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btGetData_Click(object sender, EventArgs e)
    {
        if (Session["page"] == null) Session["page"] = 1;
        Session["page"] = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString();
        if (!int.TryParse(Session["page"].ToString(), out PageIndex))
        {
            PageIndex = 1;
        }
        GetOpenList(PageIndex, PageSize);
        if (gvList.Rows.Count > 0)
        {
            btCheckedAll.Visible = true;
            btprocess.Visible = true;
        }
    }

    /// <summary>
    /// 全選及全取消的點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btCheckedAll_Click(object sender, EventArgs e)
    {
        if (btCheckedAll.Text == "全選")
        {
            for (int j = 0; j < gvList.Rows.Count; j++)
            {
                CheckBox objCheckbox = (CheckBox)gvList.Rows[j].Cells[0].FindControl("iReSet");

                objCheckbox.Checked = true;
            }

            btCheckedAll.Text = "全部取消";
        }
        else if (btCheckedAll.Text == "全部取消")
        {
            for (int j = 0; j < gvList.Rows.Count; j++)
            {
                CheckBox objCheckbox = (CheckBox)gvList.Rows[j].Cells[0].FindControl("iReSet");

                objCheckbox.Checked = false;
            }

            btCheckedAll.Text = "全選";
        }
    }

    /// <summary>
    /// 處理復原會員狀態的點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btprocess_Click(object sender, EventArgs e)
    {
        string tmpMemberInfo = string.Empty;

        string[] MemberInfo = null;

        string Club_Ename = string.Empty;

        string Club_Id = string.Empty;

        string Session_Id = string.Empty;

        string GameId = string.Empty;

        string Version = string.Empty;

        int isOk = 0;

        int isError = 0;

        int hasAccount = 0;

        int total = 0;

        StringBuilder Msg = new StringBuilder("");

        for (int j = 0; j < gvList.Rows.Count; j++)
        {
            CheckBox objCheckbox = (CheckBox)gvList.Rows[j].Cells[0].FindControl("iReSet");

            if (objCheckbox.Checked)
            {
                total++;

                if (!string.IsNullOrEmpty(objCheckbox.ToolTip))
                {
                    tmpMemberInfo = objCheckbox.ToolTip;

                    MemberInfo = tmpMemberInfo.Split('_');

                    Club_Ename = MemberInfo[0];

                    Club_Id = MemberInfo[2];

                    Session_Id = MemberInfo[1];

                    GameId = MemberInfo[3];

                    Version = GetRoyalApiVersionByGameId(GameId);

                    if (!GetRoyalHasAccount(Version, "h1", "mobile", Club_Id, Session_Id))
                    {
                        if (1 != ResetMemberStatus(Club_Id, Session_Id))
                        {
                            Msg.Append("\r\n");
                            Msg.Append("Version:");
                            Msg.Append(Version);
                            Msg.Append(";");
                            Msg.Append("Club_Id:");
                            Msg.Append(Club_Id);
                            Msg.Append(";");
                            Msg.Append("Session_Id:");
                            Msg.Append(Session_Id);
                            Msg.Append(";");
                            Msg.Append("Club_Ename:");
                            Msg.Append(Club_Ename);
                            Msg.Append(";");
                            Msg.Append("GameId:");
                            Msg.Append(GameId);
                            Msg.Append(";Result:復原失敗");
                            Msg.Append("\r\n");
                            isError++;
                        }else {
                            isOk++;
                        }
                    }else {
                        Msg.Append("\r\n");
                        Msg.Append("Version:");
                        Msg.Append(Version);
                        Msg.Append(";");
                        Msg.Append("Club_Id:");
                        Msg.Append(Club_Id);
                        Msg.Append(";");
                        Msg.Append("Session_Id:");
                        Msg.Append(Session_Id);
                        Msg.Append(";");
                        Msg.Append("Club_Ename:");
                        Msg.Append(Club_Ename);
                        Msg.Append(";");
                        Msg.Append("GameId:");
                        Msg.Append(GameId);
                        Msg.Append(";Result:存在帳務或是存在異常情況");
                        Msg.Append("\r\n");
                        hasAccount++;
                    }
                }
            } 
        }

        Lib.WritLog("GetPlayerInfoAdminBatch.aspx.cs.btprocess_Click", Msg.ToString());

        Lib.MsgBoxAndJump(this,string.Format( "已經全部處理完畢，仍存在帳務有:{0}筆，復原失敗有︰{1}筆，成功有︰{2}筆，全部勾選筆數有︰{3}筆", hasAccount, isError, isOk, total), "GetPlayerInfoAdminBatch.aspx");

    }
}