using RTGLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GetWashReviseData : System.Web.UI.Page
{

    private bsSQL objbsSQL = new bsSQL();

    private DataTable HistoryDate = null;  //是否過帳的資料

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btcomplete.Visible = false;  //隱藏修改按鈕 
            if (objbsSQL.GetZhuanLiShi() == "1")
            {
                btGetWashRevise.Visible = false;
                Msg.Text = FormateMsg("正在過帳中..請稍後操作!!");
            }else {
                btGetWashRevise.Visible = true;
                Msg.Text = "";
            }
        }
    }

    /// <summary>
    /// 初始化改單的DataTable
    /// </summary>
    /// <returns></returns>
    private DataTable InitRTGDataTable()
    {
        DataTable dtRTG = new DataTable();
        dtRTG.Columns.Add("RTG_Club_id", typeof(string));
        dtRTG.Columns.Add("RTG_PlayerAccount", typeof(double));
        dtRTG.Columns.Add("RTG_Stake_Score", typeof(int));
        dtRTG.Columns.Add("RTG_Account_Score", typeof(double));
        dtRTG.Columns.Add("RTG_Rows", typeof(int));
        dtRTG.Columns.Add("RTG_MaxDateTime", typeof(string));
        dtRTG.Columns.Add("RTG_Game_id", typeof(string));
        dtRTG.Columns.Add("RTG_JackPot", typeof(double));
        dtRTG.Columns.Add("RTG_PKPoint", typeof(double));
        dtRTG.Columns.Add("RTG_SharePoint", typeof(double));
        dtRTG.Columns.Add("RTG_SessionId", typeof(string));
        /*-------------------------------------------------------*/
        dtRTG.Columns.Add("H1_Club_id", typeof(string));
        dtRTG.Columns.Add("H1_PlayerAccount", typeof(double));
        dtRTG.Columns.Add("H1_Stake_Score", typeof(int));
        dtRTG.Columns.Add("H1_Account_Score", typeof(double));
        dtRTG.Columns.Add("H1_Rows", typeof(int));
        dtRTG.Columns.Add("H1_MaxDateTime", typeof(string));
        dtRTG.Columns.Add("H1_Game_id", typeof(string));
        dtRTG.Columns.Add("H1_JackPot", typeof(double));
        dtRTG.Columns.Add("H1_PKPoint", typeof(double));
        dtRTG.Columns.Add("H1_SharePoint", typeof(double));
        dtRTG.Columns.Add("H1_SessionId", typeof(string));
        dtRTG.Columns.Add("H1_Id", typeof(string));
        dtRTG.Columns.Add("IsHistory", typeof(string));
        /*-------------------------------------------------------*/
        dtRTG.Columns.Add("H1_Stake_id", typeof(string));
        dtRTG.Columns.Add("H1_ZhuDan_Type", typeof(string));
        dtRTG.Columns.Add("H1_Active", typeof(string));
        dtRTG.Columns.Add("H1_Desk_id", typeof(string));
        dtRTG.Columns.Add("H1_No_Run", typeof(string));
        dtRTG.Columns.Add("H1_No_Active", typeof(string));
        dtRTG.Columns.Add("H1_JiTai_No", typeof(string));
        return dtRTG;
    }

    /// <summary>
    /// 檢查開分號轉日期在與現在的時間，是否有經歷過帳
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
    /// 刷新資料按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btGetWashRevise_Click(object sender, EventArgs e)
    {
        string PlayerId = string.Empty;
        double PlayerAccount = 0;
        int StakeScore = 0;
        double AccountScore = 0;
        int Rows = 0;
        string MaxDateTime = string.Empty;
        string GameId = string.Empty;
        double JackPot = 0;
        double PKPoint = 0;
        double SharePoint = 0;
        long SessionId = 0;
        int Id = 0;

        string Stake_id = string.Empty;
        string ZhuDan_Type = string.Empty;
        string Active = string.Empty;
        string Desk_id = string.Empty;
        string No_Run = string.Empty;
        string No_Active = string.Empty;
        string  JiTai_No = string.Empty;

        int NextMaxId;

        int WashReviseTotal = 0;

        RGTlib objRGTlib = new RGTlib();

        this.objbsSQL = new bsSQL();

        WashRevise objWashRevise = null;

        int MaxId = objbsSQL.GetMaxId();

        string sqlSessionId = string.Empty;

        string sqlClub_id = string.Empty;

        bool ishistory = false;

        DataTable dtH1 = null;

        HistoryDate = objbsSQL.ChkIsHistory();  //先撈出是否過帳的資料

        DataTable dtWashRevise = InitRTGDataTable();

        StringBuilder Sessionid = new StringBuilder("");

        StringBuilder Club_id = new StringBuilder("");

        objWashRevise = objRGTlib.GetWashRevise(MaxId+1); //調用RTG API取得改單資料

        //objWashRevise = CreateWashRevisedata();

        Msg.Text = "";

        if (objWashRevise.MsgID == 0 && objWashRevise.Message == "Success")
        {
            if (objWashRevise.Data.WashCount == 0)
            {
                Msg.Text = FormateMsg("查無任何資料!!");

                HistoryDate.Dispose();

                HistoryDate = null;

                return;
            }

            WashReviseList.Visible = true;

            NextMaxId = MaxId + objWashRevise.Data.WashCount;

            Session["NextMaxId"] = NextMaxId;

            WashReviseTotal = objWashRevise.Data.Content.Count;

            if (WashReviseTotal > 0)
            { 
                for (int j = 0; j < WashReviseTotal; j++)
                {
                    Sessionid.Append(string.Format("{0}", objWashRevise.Data.Content[j].SessionId));

                    Sessionid.Append(",");

                    Club_id.Append(string.Format("{0}", objWashRevise.Data.Content[j].PlayerId));

                    Club_id.Append(",");

                    /**********************************************************/
                    dtWashRevise.Rows.Add(objWashRevise.Data.Content[j].PlayerId,
                                   objWashRevise.Data.Content[j].PlayerAccount,
                                   objWashRevise.Data.Content[j].StakeScore,
                                   objWashRevise.Data.Content[j].AccountScore,
                                   objWashRevise.Data.Content[j].Rows,
                                   objWashRevise.Data.Content[j].MaxDateTime,
                                   objWashRevise.Data.Content[j].GameId,
                                   objWashRevise.Data.Content[j].JackPot,
                                   objWashRevise.Data.Content[j].PKPoint,
                                   objWashRevise.Data.Content[j].SharePoint,
                                   objWashRevise.Data.Content[j].SessionId,
                                     /*************************************************/
                                     PlayerId,
                                     PlayerAccount,
                                     StakeScore,
                                     AccountScore,
                                     Rows,
                                     MaxDateTime,
                                     GameId,
                                     JackPot,
                                     PKPoint,
                                     SharePoint,
                                     SessionId,
                                     Id,
                                     "N",
                                      Stake_id,
                                      ZhuDan_Type,
                                      Active,
                                      Desk_id,
                                      No_Run,
                                      No_Active,
                                      JiTai_No);
                }

                sqlSessionId = Sessionid.ToString().TrimEnd(',');

                sqlClub_id = Club_id.ToString().TrimEnd(',');

                if (sqlSessionId != "" && sqlClub_id != "")
                {
                    if (sqlSessionId.IndexOf(',') > -1 && sqlClub_id.IndexOf(',') > -1)
                    {
                        string[] sessionarray = sqlSessionId.Split(',');

                        string[] club_idarray = sqlClub_id.Split(',');

                        bool[] isHistory = new bool[sessionarray.Length-1]; 

                        DataTable[] tmpH1 = new DataTable[sessionarray.Length - 1];

                        bool dtH1Ishistory = false;

                        for (int j = 0; j < sessionarray.Length; j++)
                        {
                            if (HistoryDate != null)
                            {
                                try{

                                    ishistory = IsHistory(sessionarray[j], HistoryDate);  //檢查是否已經過完帳

                                 } catch (Exception ex){

                                    continue;
                                }
                             }
                            
                            if (j == 0)
                            {
                                if (ishistory)
                                {
                                    dtH1 = objbsSQL.GetStakeHistoryData(sessionarray[j], club_idarray[j]);

                                    dtH1Ishistory = true;
                                }
                                else
                                {
                                    dtH1 = objbsSQL.GetStakeCurrentData(sessionarray[j], club_idarray[j]);

                                    dtH1Ishistory = false;
                                }

                                if (dtH1Ishistory) 
                                {
                                    DataRow[] h1Row = dtH1.Select(string.Format("Club_id='{0}' and StartSeqNoFlag={1}", club_idarray[j], sessionarray[j]));

                                    if (h1Row.Length > 0)
                                    {
                                        h1Row[0]["IsHistory"] = "Y";
                                    }
                                }
                            }
                            else
                            {
                                if (ishistory)
                                {
                                    tmpH1[j - 1] = objbsSQL.GetStakeHistoryData(sessionarray[j], club_idarray[j]);

                                    isHistory[j - 1] = true;
                                }
                                else
                                {
                                    tmpH1[j - 1] = objbsSQL.GetStakeCurrentData(sessionarray[j], club_idarray[j]);

                                    isHistory[j - 1] = false;
                                }
                            }
                        }

                        DataRow[] workRow = new DataRow[tmpH1.Length]; 

                        for (int j = 0; j < tmpH1.Length; j++)  //為了後續方便，將多個DataTable的資料全部合到同一個
                        {
                            if (tmpH1[j].Rows.Count > 1 || tmpH1[j].Rows.Count == 0)
                            {
                                if (tmpH1[j].Rows.Count > 1)
                                {
                                    Msg.Text = FormateMsg("RTG傳來的開分號中，在著H1裡能對映到多筆開分號!!");
                                }
                                if (tmpH1[j].Rows.Count == 0)
                                {
                                    Msg.Text = FormateMsg("RTG傳來的開分號中，存在著H1不存在的開分號!!");
                                }
                               return;
                            }

                            foreach (DataRow row in tmpH1[j].Rows)
                            {
                                workRow[j] = dtH1.NewRow();
                                workRow[j]["Rows"] = row["Rows"].ToString();
                                workRow[j]["Club_id"] = row["Club_id"].ToString();
                                workRow[j]["Now_XinYong"] = row["Now_XinYong"].ToString();
                                workRow[j]["Account_Score"] = row["Account_Score"].ToString();
                                workRow[j]["Datetime"] = row["Datetime"].ToString();
                                workRow[j]["Game_id"] = row["Game_id"].ToString();
                                workRow[j]["Jackpot_Score"] = row["Jackpot_Score"].ToString();
                                workRow[j]["Id"] = row["Id"].ToString();
                                workRow[j]["Stake_Score"] = row["Stake_Score"].ToString();
                                workRow[j]["StartSeqNoFlag"] = row["StartSeqNoFlag"].ToString();
                                if (isHistory[j] == true)
                                {
                                    workRow[j]["IsHistory"] = "Y";
                                }else {
                                    workRow[j]["IsHistory"] = "N";
                                }
                                workRow[j]["Stake_id"] = row["Stake_id"].ToString();
                                workRow[j]["ZhuDan_Type"] = row["ZhuDan_Type"].ToString();
                                workRow[j]["Active"] = row["Active"].ToString();
                                workRow[j]["Desk_id"] = row["Desk_id"].ToString();
                                workRow[j]["No_Run"] = row["No_Run"].ToString();
                                workRow[j]["No_Active"] = row["No_Active"].ToString();
                                workRow[j]["JiTai_No"] = row["JiTai_No"].ToString(); 
                                workRow[j]["TableFee"] = row["TableFee"].ToString();     //公點
                                workRow[j]["Commission"] = row["Commission"].ToString(); //分潤
                                dtH1.Rows.Add(workRow[j]);
                            }
                        }

                    }else {

                        if (HistoryDate != null)
                        {
                            try
                            {
                                ishistory = IsHistory(sqlSessionId, HistoryDate);  //檢查是否已經過完帳

                                if (ishistory)
                                {
                                    dtH1 = objbsSQL.GetStakeHistoryData(sqlSessionId, sqlClub_id);

                                    DataRow[] h1Row = dtH1.Select(string.Format("Club_id='{0}' and StartSeqNoFlag={1}", sqlClub_id, sqlSessionId));

                                    if (h1Row.Length > 0)
                                    {
                                        h1Row[0]["IsHistory"] = "Y";
                                    }
                                }else {
                                
                                    dtH1 = objbsSQL.GetStakeCurrentData(sqlSessionId, sqlClub_id);
                                }

                                if (dtH1.Rows.Count > 1 || dtH1.Rows.Count == 0)
                                {
                                    if (dtH1.Rows.Count > 1)
                                    {
                                        Msg.Text = FormateMsg("RTG傳來的開分號中，在著H1對映至多筆開分號!!!!");
                                    }

                                    if (dtH1.Rows.Count == 0)
                                    {
                                        Msg.Text = FormateMsg("RTG傳來的開分號中，存在著H1不存在的開分號!!!!");
                                    }

                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                dtH1 = new DataTable();
                            }
                        }
                    }

                    if (WashReviseTotal != dtH1.Rows.Count)
                    {
                        if(WashReviseTotal > dtH1.Rows.Count)
                        {
                            Msg.Text = FormateMsg("H1有缺少未知的開分號!!");
                        } else {
                            Msg.Text = FormateMsg("H1有重覆未知的開分號，造成筆數大於RTG!!!");
                        }

                        return;
                    }

                    if (dtH1.Rows.Count > 0)
                    {
                        foreach (DataRow H1row in dtH1.Rows)
                        {
                            DataRow[] arrRows = dtWashRevise.Select(string.Format("RTG_SessionId='{0}'", H1row["StartSeqNoFlag"]));

                            for(int k=0;k< arrRows.Length;k++)
                            {
                                arrRows[k]["H1_Club_id"] = H1row["Club_id"];
                                arrRows[k]["H1_PlayerAccount"] = H1row["Now_XinYong"];
                                arrRows[k]["H1_Stake_Score"] = H1row["Stake_Score"];
                                arrRows[k]["H1_Account_Score"] = H1row["Account_Score"];
                                arrRows[k]["H1_Rows"] = H1row["Rows"];
                                arrRows[k]["H1_MaxDateTime"] = H1row["Datetime"];
                                arrRows[k]["H1_JackPot"] = H1row["Jackpot_Score"];
                                arrRows[k]["H1_PKPoint"] = H1row["TableFee"];          //公點
                                arrRows[k]["H1_SharePoint"] = H1row["Commission"];     //分潤
                                arrRows[k]["H1_SessionId"] = H1row["StartSeqNoFlag"];
                                arrRows[k]["H1_Id"] = H1row["Id"];
                                arrRows[k]["IsHistory"] = H1row["IsHistory"];
                                arrRows[k]["H1_Stake_id"] = H1row["Stake_id"].ToString();
                                arrRows[k]["H1_ZhuDan_Type"] = H1row["ZhuDan_Type"].ToString();
                                arrRows[k]["H1_Active"] = H1row["Active"].ToString();
                                arrRows[k]["H1_Desk_id"] = H1row["Desk_id"].ToString();
                                arrRows[k]["H1_No_Run"] = H1row["No_Run"].ToString();
                                arrRows[k]["H1_No_Active"] = H1row["No_Active"].ToString();
                                arrRows[k]["H1_JiTai_No"] = H1row["JiTai_No"].ToString();
                            }
                        }
 
                        //先將結果儲存在session中
                        Session["WashReviseData"] = dtWashRevise;

                        WashReviseList.DataSource = dtWashRevise;

                        WashReviseList.DataBind();

                        btcomplete.Visible = true;
                    }
                }
            }
        }else {

            WashReviseList.Visible = false;

            Msg.Text = objWashRevise.Message;
        } 
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
    /// 只用於產生假資料
    /// </summary>
    private WashRevise CreateWashRevisedata()
    {
        WashRevise objWashRevise = new WashRevise();

        WashReviseContent objcontent = new WashReviseContent();

        WashReviseContent objcontent1 = new WashReviseContent();

        WashReviseData objWashReviseData = new WashReviseData();

        objWashRevise.Message = "Success";

        objWashRevise.MsgID = 0;

        objWashRevise.Timestamp = 123456789;

        objWashReviseData.Content = new List<WashReviseContent>();

        objWashReviseData.WashCount = 1;

        objWashRevise.Data = objWashReviseData;

        objcontent.AccountScore = -48.00;

        objcontent.StakeScore = 70.00; // 73.00

        objcontent.Rows = 19; //19

        objcontent.RowId = 10;

        objcontent.PKPoint = 2.323;

        objcontent.SharePoint = 10.23;

        objcontent.PlayerId = "2003270264";

        objcontent.MaxDateTime = DateTime.Parse( "2020-09-09 14:06:21.000");

        objcontent.Location = "";

        objcontent.JackPot = 0;

        objcontent.GameId = 1001;

        objcontent.PlayerAccount = 49004.58;

        objcontent.SessionId = "20200910100124977";

        /*************************************************/
        objcontent1.AccountScore = 2.98;// 0.98

        objcontent1.StakeScore = 0.00;

        objcontent1.Rows = 4; //4

        objcontent1.RowId = 10;

        objcontent1.PKPoint = 2.323;

        objcontent1.SharePoint = 10.23;

        objcontent1.PlayerId = "2003270425";

        objcontent1.MaxDateTime = DateTime.Parse("2020-09-10 13:26:19.000");

        objcontent1.Location = "";

        objcontent1.JackPot = 0;

        objcontent1.GameId = 1001;

        objcontent1.PlayerAccount = 49999.01;

        objcontent1.SessionId = "20200910131444127";

        objWashRevise.Data.Content.Add(objcontent);

        objWashRevise.Data.Content.Add(objcontent1);

        return objWashRevise;
    }

    /// <summary>
    /// 依是否過帳及筆數下注，來判斷是否要處理
    /// </summary>
    /// <param name="_IsHistory"></param>
    /// <param name="_RTG_Rows"></param>
    /// <param name="_H1_Rows"></param>
    /// <param name="_RTG_Stake_Score"></param>
    /// <param name="_H1_Stake_Score"></param>
    /// <returns>No表示不處理，Yes表示處理</returns>
    public string ChkIsToDo(string _IsHistory,string _RTG_Rows,string _H1_Rows,string _RTG_Stake_Score,string _H1_Stake_Score) {
        string IsHistory = string.Empty;
        int RTG_Rows = 0;
        int H1_Rows = 0;
        double RTG_Stake_Score = 0;
        double H1_Stake_Score = 0;
        string rvalue = string.Empty; 
        IsHistory = _IsHistory;
        RTG_Rows = int.Parse(_RTG_Rows);
        H1_Rows = int.Parse(_H1_Rows);
        RTG_Stake_Score = double.Parse(_RTG_Stake_Score);
        H1_Stake_Score = double.Parse(_H1_Stake_Score);
        if (IsHistory.ToUpper() == "Y")
        {
            //2020-12-01 目前都要處理()
            rvalue = "Yes";

            /*2020-12-01 註解，由於曾經發生過補單補注金為負的，所以將此註解
            if (RTG_Rows < H1_Rows || RTG_Stake_Score < H1_Stake_Score)
            {
                rvalue = "No";
            }
            else
            {
                rvalue = "Yes";
            }*/

        }
        else{
            rvalue = "Yes";
        }

        return rvalue;
    }

    /// <summary>
    /// 修改按鈕事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btcomplete_Click(object sender, EventArgs e)
    {
        DataTable dtWashRevise = null;

        dbSQL objdbsql = new dbSQL();

        bool isok = false;

        if (Session["WashReviseData"] != null)
        {
            dtWashRevise = (DataTable)Session["WashReviseData"];

            dtWashRevise.TableName = "WashReviseData";

            Lib.WritLog("GetWashReviseData.aspx.cs", ConvertDatatableToXML(dtWashRevise));
        }

        if (Session["WashReviseData"] != null)
        {
            if (dtWashRevise.Rows.Count > 0)
            {
                isok = objdbsql.updateStakeCurrent(dtWashRevise, "GetWashReviseData.aspx.cs.btcomplete_Click");

                if (isok)
                {
                    if (Session["NextMaxId"] != null)
                    {
                        objbsSQL.SetMaxId(int.Parse(Session["NextMaxId"].ToString()));  //等全部處理完，再標示成處理完畢

                        Msg.Text = FormateMsg("修改成功!!!");
                    }
                    else
                    {
                        Msg.Text = FormateMsg("修改成功，但取得改單資料編號設定錯誤!!!");
                    }
                }
                else
                {
                    Msg.Text = FormateMsg("修改失敗!!!");
                }
            }
            else
            {
                Msg.Text = FormateMsg("資料不存在，修改失敗!!!");
            }
        }else {
            Msg.Text = FormateMsg("已經超出修改時限，請重新操作!!!");
        }
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
}