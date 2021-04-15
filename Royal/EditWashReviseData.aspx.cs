using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditWashReviseData : System.Web.UI.Page
{
    private bsSQL objbsSQL = new bsSQL();

    private DataTable HistoryDate = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (objbsSQL.GetZhuanLiShi() == "1")
            {
                btsend.Visible = false;
                Msg.Text = FormateMsg("正在過帳中..請稍後操作!!");
            }
            else
            {
                btsend.Visible = true;
                Msg.Text = "";
            }
        }
    }
    /// <summary>
    /// 檢查開分號，是否有經歷過帳
    /// </summary>
    /// <param name="strdate"></param>
    /// <returns></returns>
    public bool IsHistory(string sessionid, DataTable HistoryDate)
    {
        string strSessionid = string.IsNullOrEmpty(sessionid) ? "" : sessionid;

        string strdate = string.Empty;

        string sdate = string.Empty;

        string edate = string.Empty;

        string _QiShu_Name = string.Empty;

        bool isHistory = false;

        DateTime SessionDate;

        DateTime StartDt;

        DateTime EndDt;

        if (strSessionid != "")
        {
           
            IFormatProvider cultureStyle = new System.Globalization.CultureInfo("zh-TW", true);

            SessionDate = DateTime.ParseExact(strSessionid, "yyyyMMddHHmmssfff", cultureStyle);

            string strNowDay = string.Format("{0}{1}{2}", DateTime.Now.Year,
                         DateTime.Now.Month < 10 ? string.Format("0{0}", DateTime.Now.Month) : DateTime.Now.Month.ToString(), 
                         DateTime.Now.Day < 10 ? string.Format("0{0}", DateTime.Now.Day) : DateTime.Now.Day.ToString());

            string strSday = string.Format("{0}{1}{2}", SessionDate.Year,
                         SessionDate.Month < 10 ? string.Format("0{0}", SessionDate.Month) : SessionDate.Month.ToString(),
                         SessionDate.Day < 10 ? string.Format("0{0}", SessionDate.Day) : SessionDate.Day.ToString());

            if (HistoryDate.Rows.Count > 0)  //大於0，表已經過帳
            {
                foreach (DataRow row in HistoryDate.Rows)
                {
                    sdate = row["Start_Datetime"].ToString();

                    edate = row["End_Datetime"].ToString();

                    _QiShu_Name = row["QiShu_Name"].ToString();
                }

                if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                {
                    StartDt = DateTime.Parse(sdate);

                    EndDt = DateTime.Parse(edate);

                    isHistory = SessionDate >= StartDt && SessionDate <= EndDt;

                    if (!isHistory)
                    {
                        if ((long.Parse(strNowDay) - long.Parse(strSday)) >= 2)  //大於1，表示已經過帳
                        {
                            isHistory = true;
                        }

                    }
                }
            }
            else
            {  //尚未過帳
                
                if ((long.Parse(strNowDay) - long.Parse(strSday)) >= 2)  //大於1，表示已經過帳
                {
                    isHistory = true;
                }
            }
        }
        return isHistory;
    }

    /// <summary>
    /// 格式化警示訊息
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    private string FormateMsg(string msg)
    {
        return string.Format("<h4>{0}</h4>", msg);
    }

    /// <summary>
    /// 將DataTable匯出成XML字串 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public string ConvertDatatableToXML(DataTable dt)
    {
        MemoryStream str = new MemoryStream();
        dt.WriteXml(str, true);
        str.Seek(0, SeekOrigin.Begin);
        StreamReader sr = new StreamReader(str);
        string xmlstr;
        xmlstr = sr.ReadToEnd();
        return (xmlstr);
    }

    /// <summary>
    /// 依會員ID及開分號撈出帳務資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btsend_Click(object sender, EventArgs e)
    {
        string sessionid = string.Empty;

        string club_id = string.Empty;

        string _IsHistory = string.Empty;

        bool _isHistory = false;

        DataTable dtH1 = null;

        Msg.Text = "";

       club_id = string.IsNullOrEmpty(btClub_id.Text.ToString()) ? "" : btClub_id.Text;

        sessionid = string.IsNullOrEmpty(btSessioinid.Text.ToString()) ? "" : btSessioinid.Text;

        if (club_id != "" && sessionid != "" && sessionid.Length >= 17)
        {
            HistoryDate = objbsSQL.ChkIsHistory();  //先撈出是否過帳的資料

            try
            {
                _isHistory = IsHistory(sessionid, HistoryDate);
            }
            catch (Exception ex)
            {
                Msg.Text = FormateMsg("輸入的資料有誤，請重新操作!!");
                if (Session["dtH1Result"] != null)
                {
                    Session.Remove("dtH1Result");
                }
                return;
            }

            if (_isHistory)
            {
                dtH1 = objbsSQL.GetStakeHistoryData(sessionid, club_id);
                this.lbstatus.Text = "已過帳，將自動補單";
                _IsHistory = "Y";
            }
            else
            {
                dtH1 = objbsSQL.GetStakeCurrentData(sessionid, club_id);
                this.lbstatus.Text = "未過帳";
                _IsHistory = "N";
            }

            if (dtH1.Rows.Count > 0)
            {
                Session["dtH1Result"] = dtH1;

                foreach (DataRow row in dtH1.Rows)
                {
                    lbPlayerId.Text = row["Club_id"].ToString();
                    lbSessionId.Text = row["StartSeqNoFlag"].ToString();
                    btStakeScore.Text = row["Stake_Score"].ToString();
                    btAccount_Score.Text = row["Account_Score"].ToString();
                    btRows.Text = row["Rows"].ToString();
                    txtCommission.Text = row["Commission"].ToString();
                    txtTableFee.Text = row["TableFee"].ToString();
                    textJackpot.Text= row["Jackpot_Score"].ToString();
                    row["IsHistory"] = _IsHistory;
                }
            }
            else
            {
                Msg.Text = FormateMsg("查無資料!!");
                if (Session["dtH1Result"] != null)
                {
                    Session.Remove("dtH1Result");
                }
            }
        }else {
            Msg.Text = FormateMsg("輸入的資料有誤，請重新操作!!");
        }
    }

    /// <summary>
    /// 依會員ID及開分號修改或補增帳務資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btupdate_Click(object sender, EventArgs e)
    {
        /********************************/
        string Rows = string.Empty;
        string Club_id = string.Empty;
        string Now_XinYong = string.Empty;
        string Account_Score = string.Empty;
        string Datetime = string.Empty;
        string Game_id = string.Empty;
        string Jackpot_Score = string.Empty;
        string StartSeqNoFlag = string.Empty;
        string Id = string.Empty;
        string Stake_Score = string.Empty;
        string IsHistory = string.Empty;
        string Stake_id = string.Empty;
        string ZhuDan_Type = string.Empty;
        string Active = string.Empty;
        string Desk_id = string.Empty;
        string No_Run = string.Empty;
        string No_Active = string.Empty;
        string JiTai_No = string.Empty;
        string Commission = string.Empty;
        string TableFee = string.Empty;
        /*******************************/
        string _PlayerId = string.Empty;
        string _SessionId = string.Empty;
        string _StakeScore = string.Empty;
        string _Account_Score = string.Empty;
        string _Rows = string.Empty;
        string _Commission = string.Empty;
        string _TableFee = string.Empty ;
        string _Jackpot = string.Empty;
        
        /*******************************/
        dbSQL objdb = new dbSQL();

        DataTable H1Result = null;

        string sessionid = string.Empty;

        string club_id = string.Empty;

        bool Result = false;

        DataRow[] RowResult = null;

        Msg.Text = "";

        double d_StakeScore = 0;
        double d_Account_Score = 0;
        double d_Commission = 0;  //分潤
        double d__TableFee = 0;   //公點
        double d_Jackpot = 0;//彩金
        int d_Rows = 0;
        bool typeerror = true;
        StringBuilder objstrg = new StringBuilder("");

        club_id = string.IsNullOrEmpty(lbPlayerId.Text.ToString()) ? "" : lbPlayerId.Text;
        sessionid = string.IsNullOrEmpty(lbSessionId.Text.ToString()) ? "" : lbSessionId.Text;
        _StakeScore =string.IsNullOrEmpty( btStakeScore.Text.ToString()) ? "" : btStakeScore.Text;
        _Account_Score = string.IsNullOrEmpty(btAccount_Score.Text.ToString()) ? "" : btAccount_Score.Text;
        _Rows = string.IsNullOrEmpty(btRows.Text.ToString()) ? "" : btRows.Text;
        _Commission = string.IsNullOrEmpty(txtCommission.Text.ToString()) ? "" : txtCommission.Text;
        _TableFee = string.IsNullOrEmpty(txtTableFee.Text.ToString()) ? "" : txtTableFee.Text;
        _Jackpot = string.IsNullOrEmpty(textJackpot.Text.ToString()) ? "" : textJackpot.Text;

        if (_StakeScore != "" && _Account_Score != "" && _Rows != "" && sessionid != "" && club_id != "" && _Commission != "" && _TableFee != "")
        {
            objstrg.Append("預計修改資料︰");
            objstrg.Append(string.Format("_PlayerId︰{0}", club_id));
            objstrg.Append(string.Format("_SessionId︰{0}", sessionid));
            objstrg.Append(string.Format("_StakeScore︰{0}", _StakeScore));
            objstrg.Append(string.Format("_Account_Score︰{0}", _Account_Score));
            objstrg.Append(string.Format("_Rows︰{0}", _Rows));
            objstrg.Append(string.Format("_Commission︰{0}", _Commission));
            objstrg.Append(string.Format("_TableFee︰{0}", _TableFee));
            objstrg.Append(string.Format("_Jackpot︰{0}", _Jackpot));
            if (!double.TryParse(_StakeScore,out d_StakeScore))
            {
                typeerror = false;
            }

            if (!double.TryParse(_Account_Score, out d_Account_Score))
            {
                typeerror = false;
            }

            if (!int.TryParse(_Rows, out d_Rows))
            {
                typeerror = false;
            }

            if (!double.TryParse(_Commission, out d_Commission))
            {
                typeerror = false;
            }

            if (!double.TryParse(_TableFee, out d__TableFee))
            {
                typeerror = false;
            }
            if (!double.TryParse(_Jackpot, out d_Jackpot))
            {
                typeerror = false;
            }

            if (typeerror==false)
            {
                Msg.Text = FormateMsg("參數型態錯誤，請重新操作!!");
                objstrg.Append("參數型態錯誤，請重新操作!!");

                if (Session["dtH1Result"] != null)
                {
                    Session.Remove("dtH1Result");
                }
                return;
            }
            /*
            objstrg.Append(string.Format("Rows︰{0}", Rows));
            objstrg.Append(string.Format("Club_id︰{0}", Club_id));
            objstrg.Append(string.Format("Now_XinYong︰{0}", Now_XinYong));
            objstrg.Append(string.Format("Account_Score︰{0}", Account_Score));
            objstrg.Append(string.Format("Datetime︰{0}", Datetime));
            objstrg.Append(string.Format("Game_id︰{0}", Game_id));
            objstrg.Append(string.Format("Jackpot_Score︰{0}", Jackpot_Score));
            objstrg.Append(string.Format("StartSeqNoFlag︰{0}", StartSeqNoFlag));
            objstrg.Append(string.Format("Id︰{0}", Id));
            objstrg.Append(string.Format("Stake_Score︰{0}", Stake_Score));
            objstrg.Append(string.Format("IsHistory︰{0}", IsHistory));
            objstrg.Append(string.Format("Stake_id︰{0}", Stake_id));
            objstrg.Append(string.Format("ZhuDan_Type︰{0}", ZhuDan_Type));
            objstrg.Append(string.Format("Active︰{0}", Active));
            objstrg.Append(string.Format("Desk_id︰{0}", Desk_id));
            objstrg.Append(string.Format("No_Run︰{0}", No_Run));
            objstrg.Append(string.Format("No_Active︰{0}", No_Active));
            objstrg.Append(string.Format("JiTai_No︰{0}", JiTai_No));
            ******************************/
           
            objstrg.Append("修改前原始資料︰");

            if (Session["dtH1Result"] != null )
            {
                H1Result = (DataTable)Session["dtH1Result"];

                if (H1Result.Rows.Count > 0)
                {
                    objstrg.Append(ConvertDatatableToXML(H1Result));

                    foreach (DataRow row in H1Result.Rows)
                    {
                        Rows = row["Rows"].ToString();
                        Club_id = row["Club_id"].ToString();
                        Now_XinYong = row["Now_XinYong"].ToString();
                        Account_Score = row["Account_Score"].ToString();
                        Datetime = row["Datetime"].ToString();
                        Game_id = row["Game_id"].ToString();
                        Jackpot_Score = row["Jackpot_Score"].ToString();
                        StartSeqNoFlag = row["StartSeqNoFlag"].ToString();
                        Id = row["Id"].ToString();
                        Stake_Score = row["Stake_Score"].ToString();
                        IsHistory = row["IsHistory"].ToString();
                        Stake_id = row["Stake_id"].ToString();
                        ZhuDan_Type = row["ZhuDan_Type"].ToString();
                        Active = row["Active"].ToString();
                        Desk_id = row["Desk_id"].ToString();
                        No_Run = row["No_Run"].ToString();
                        No_Active = row["No_Active"].ToString();
                        JiTai_No = row["JiTai_No"].ToString();
                        Commission = row["Commission"].ToString();
                        TableFee = row["TableFee"].ToString();
                    }

                    Rows = _Rows;
                    Account_Score = _Account_Score;
                    Stake_Score = _StakeScore;
                    Commission = _Commission;
                    TableFee = _TableFee;
                    Jackpot_Score = _Jackpot;

                    if (IsHistory.ToUpper() == "Y")
                    {
                        objstrg.Append("，進行補單，");
                        Result = objdb.HandleEditStakeHistory(Rows, Club_id, Now_XinYong, Account_Score, Datetime, Game_id, Jackpot_Score, StartSeqNoFlag,
                                Id, Stake_Score, IsHistory, Stake_id, ZhuDan_Type, Active, Desk_id, No_Run, No_Active, JiTai_No, TableFee, Commission);
                    }
                    else
                    {
                        objstrg.Append("，修改原單，");
                        Result = objdb.HandleEditStakeCurrent(Id, Stake_Score, Account_Score, StartSeqNoFlag, Rows,TableFee, Commission, Jackpot_Score);
                    }

                    if (Result)
                    {
                        objstrg.Append("修改成功!!!");
                        Msg.Text = FormateMsg("成功!!");
                    }else {
                        objstrg.Append("修改失敗!!!");
                        Msg.Text = FormateMsg("失敗!!");
                    }

                    if (Session["dtH1Result"] != null)
                    {
                        Session.Remove("dtH1Result");
                    }

                }else {
                    Msg.Text = FormateMsg("來源資料有異，請重新操作!!");
                    objstrg.Append("來源資料有異，請重新操作!!");
                }
            }else
            {
                Msg.Text = FormateMsg("超出操作時限或來源資料有異，請重新操作!!");
                objstrg.Append("超出操作時限或來源資料有異，請重新操作!!");
            }
        }else {
            Msg.Text = FormateMsg("參數缺失，請重新操作!!");
            objstrg.Append("參數缺失，請重新操作!!");
        }

        Lib.WritLog("EditWashReviseData.cs.btupdate_Click", objstrg.ToString());
    }
}