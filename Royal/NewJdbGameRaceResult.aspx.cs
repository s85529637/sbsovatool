using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class ConfigRaceDataList
{
    public string JDBGameCode { get; set; }
    public string H1GameCode { get; set; }
    public string Name { get; set; }
}
public class ConfigRaceData
{
    public IList<ConfigRaceDataList> RaceData { get; set; }
}
/*****************************************/
public class CRaceResult
{
    /// <summary>
    /// H1的GameCode
    /// </summary>
    public string H1GameCode { get; set; }

    /// <summary>
    /// JDB的GameCode
    /// </summary>
    public string JDBGameCode { get; set; }

    /// <summary>
    /// 人們的稱呼
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsComplete { get; set; }

    /// <summary>
    /// 名次資料
    /// </summary>
    public DataTable RaceData { get; set; }

}

public partial class NewJdbGameRaceResult : System.Web.UI.Page
{
    private ResultData JResult = null;

    private  ConfigRaceData objConfigRaceData = null;

    private Dictionary<string, CRaceResult> _ResultData = new Dictionary<string, CRaceResult>();

    private readonly string JDBRaceResultConfigPath = string.IsNullOrEmpty( ConfigurationManager.AppSettings["JDBRaceResultConfigPath"].ToString()) ? "" : ConfigurationManager.AppSettings["JDBRaceResultConfigPath"].ToString();

    /// <summary>
    /// 讀取JSON文件
    /// </summary>
    /// <param name="path">JSON文件的完整路徑</param>
    /// <returns>JSON字串</returns>
    public string Readjson(string path)
    {
        string readText = string.Empty;

        try
        {
            if (File.Exists(path))
            {
                readText = File.ReadAllText(path);
            }
        }
        catch (Exception ex)
        {
            Lib.MsgBox(this, string.Format("讀取Json設定檔時，發生例外，路徑是︰{0}", path));
            Lib.WritLog("NewJdbGameRaceResult.aspx.cs.Readjson()",string.Format( "讀取Json設定檔時，發生例外，路徑是︰{0}，例外︰{1}", path, ex.ToString()));
        }

        return readText;
    }

    /// <summary>
    /// 取得設定檔的Json物件
    /// </summary>
    /// <returns></returns>
    public ConfigRaceData GetConfigData()
    {
        string JsonData = string.Empty;

        ConfigRaceData tmpConfigRaceData = null;

        try
        {
            if (!string.IsNullOrEmpty(JDBRaceResultConfigPath))
            {
                JsonData = Readjson(JDBRaceResultConfigPath);

                tmpConfigRaceData = JsonConvert.DeserializeObject<ConfigRaceData>(JsonData);

            }else {
                Lib.WritLog("NewJdbGameRaceResult.aspx.cs.GetConfigData()", "Json設定檔路徑未設定");
                Lib.MsgBox(this,"Json設定檔路徑未設定");
            }

        }catch(Exception ex)
        {
            Lib.WritLog("NewJdbGameRaceResult.aspx.cs.GetConfigData()", ex.ToString());
            Lib.MsgBox(this, string.Format( "在讀取設定檔時，發生未知的例外︰{0}",ex.Message));
        }

        return tmpConfigRaceData; 
    }

    public DataTable SetDataTable(AwardData Dragon,string TdName,string GameCode)
    {
        DataTable _Dragon = new DataTable(TdName);

        _Dragon.Columns.Add("名次", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("JDB-UID", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("帳號", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("獎金", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("GameCode", System.Type.GetType("System.String"));

        if (Dragon.rankData.Count > 0)
        {
            for (int j = 0; j < Dragon.rankData.Count; j++)
            {
                DataRow dr = _Dragon.NewRow();

                dr["名次"] = Dragon.rankData[j].rank;

                dr["JDB-UID"] = Dragon.rankData[j].uid;

                dr["帳號"] = Dragon.rankData[j].displayName;

                dr["獎金"] = Dragon.rankData[j].amount;

                dr["GameCode"] = GameCode;

                _Dragon.Rows.Add(dr);
            }
        }

        return _Dragon;
    }

   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {  
            seldate.Visible = false; 

            btDeliver.Visible = false; 
        }
    }

     
    protected void selCal_Click(object sender, EventArgs e)
    {
        seldate.Visible = true;
    }

    /// <summary>
    /// 日期挑選事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void seldate_SelectionChanged(object sender, EventArgs e)
    {
        string m = string.Empty;

        string d = string.Empty;

        if (seldate.SelectedDate.Month <= 9)
        {
            m = string.Format("0{0}", seldate.SelectedDate.Month);

        }
        else if (seldate.SelectedDate.Month <= 99 && seldate.SelectedDate.Month >= 10)
        {
            m = seldate.SelectedDate.Month.ToString();
        }

        if (seldate.SelectedDate.Day <= 9)
        {
            d = string.Format("0{0}", seldate.SelectedDate.Day);
        }
        else if (seldate.SelectedDate.Day <= 99 && seldate.SelectedDate.Day >= 10)
        {
            d = seldate.SelectedDate.Day.ToString();
        }

        seldateText.ReadOnly = false;

        seldateText.Text = string.Format("{0}-{1}-{2}", seldate.SelectedDate.Year.ToString(), m, d);

        seldateText.ReadOnly = true;

        Hidseldate.Value = string.Format("{0}{1}{2}", seldate.SelectedDate.Year.ToString(), m, d);

        seldate.Visible = false;
    }

    /// <summary>
    /// 向API索取資料，並繫結至Repeater上
    /// </summary>
    /// <returns></returns>
    private bool GetRaceData()
    {
        //定義在JDBib.cs
        JDBlib objJDBlib = new JDBlib("18");

        string Param = string.Empty;

        string Result = string.Empty;

        string AwardName = string.Empty;

        int isok = 0;

        bool OK = true;

        StringBuilder ErrorMsg = new StringBuilder("");

        CRaceResult tmpobj = null;

        Session["JdbRaceData"] = null;

        if (string.IsNullOrEmpty(Hidseldate.Value))
        {
            Lib.MsgBoxAndJump(this, "請選擇日期", "NewJdbGameRaceResult.aspx");
            OK = false;
            return OK;
        }

        JResult = objJDBlib.GetGameRaceResult(Hidseldate.Value.ToString(), out Param, out Result);

        //測試用缺少每日龍榜
        //string tmpdata = "{\"activityDate\":\"27-01-2021\",\"activityNo\":\"1611651261593\",\"activityName\":\"เทศกาลจับปลาJDB\",\"awardData\":[{\"awardId\":\"1\",\"awardName\":\"อันดับเสือรายวัน\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"0\",\"awardName\":\"ราชามังกรอัลติเมท\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270500\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"ราชาเสืออัลติเมท\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"0\",\"awardName\":\"อันดับความมั่งคั่ง\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270500\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"อันดับล่ามังกร\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"ราชานักจับปลา\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]}],\"status\":\"0\",\"description\":\"Success\"}";
        //測試用完整榜單
        //string tmpdata = "{\"activityDate\":\"27-01-2021\",\"activityNo\":\"1611651261593\",\"activityName\":\"เทศกาลจับปลาJDB\",\"awardData\":[{\"awardId\":\"0\",\"awardName\":\"อันดับมังกรรายวัน\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270500\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"อันดับเสือรายวัน\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"0\",\"awardName\":\"ราชามังกรอัลติเมท\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270500\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"ราชาเสืออัลติเมท\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"0\",\"awardName\":\"อันดับความมั่งคั่ง\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270500\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"อันดับล่ามังกร\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]},{\"awardId\":\"1\",\"awardName\":\"ราชานักจับปลา\",\"rankData\":[{\"rank\":\"1\",\"uid\":\"h1ob2003270222\",\"displayName\":\"\",\"amount\":\"4999\",\"parent\":\"gh3thbag\"},{\"rank\":\"2\",\"uid\":\"h1ob2003270009\",\"displayName\":\"\",\"amount\":\"2999\",\"parent\":\"gh3thbag\"},{\"rank\":\"3\",\"uid\":\"h1ob2003270128\",\"displayName\":\"\",\"amount\":\"1999\",\"parent\":\"gh3thbag\"},{\"rank\":\"4\",\"uid\":\"h1ob2003270225\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"},{\"rank\":\"5\",\"uid\":\"h1ob2003270010\",\"displayName\":\"\",\"amount\":\"599\",\"parent\":\"gh3thbag\"}]}],\"status\":\"0\",\"description\":\"Success\"}";
        //測試用
        //JResult = JsonConvert.DeserializeObject<ResultData>(tmpdata);

        Lib.WritLog("JdbGameRaceResult.aspx.cs.Page_Load()", string.Format("調用參數︰{0},結果︰{1}", Param, Result));

        if (JResult.status == "0" && JResult.description == "Success")
        {
            if (objConfigRaceData == null)
            {
                objConfigRaceData = GetConfigData();
            }

            if (objConfigRaceData != null)
            {
                if (objConfigRaceData.RaceData.Count > 0)
                {
                    _ResultData.Clear();

                    for (int i = 0; i < objConfigRaceData.RaceData.Count; i++)
                    {
                        tmpobj = new CRaceResult();

                        tmpobj.H1GameCode = objConfigRaceData.RaceData[i].H1GameCode.Trim();

                        tmpobj.JDBGameCode = objConfigRaceData.RaceData[i].JDBGameCode.Trim();

                        tmpobj.Name = objConfigRaceData.RaceData[i].Name;

                        tmpobj.RaceData = null;

                        tmpobj.IsComplete = false;

                        _ResultData.Add(tmpobj.JDBGameCode.Trim(), tmpobj);

                        tmpobj = null;
                    }
                }else {
                    Lib.MsgBox(this, "設定資料筆數為零，請檢查json設定檔");
                    _ResultData.Clear();
                    Session["JdbRaceData"] = null;
                    JResult = null;
                    OK = false;
                    return OK;
                }
            }else {
                //此處因為無設定檔資料或發生異常，故直接返回。
                Lib.MsgBox(this, "設定資料物件為NULL，請檢查json設定檔");
                _ResultData.Clear();
                Session["JdbRaceData"] = null;
                JResult = null;
                OK = false;
                return OK;
            }
          
            if (JResult.awardData != null)
            {
                if (JResult.awardData.Count > 0)
                {
                    ErrorMsg.Clear();

                    for (int j = 0; j < JResult.awardData.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(JResult.awardData[j].awardName))
                        {
                            AwardName = JResult.awardData[j].awardName.Trim();

                            if (_ResultData.Keys.Contains(AwardName))
                            {
                                if (_ResultData[AwardName].RaceData == null)
                                {
                                    _ResultData[AwardName].RaceData = SetDataTable(JResult.awardData[j], JResult.awardData[j].awardName.Trim(), _ResultData[AwardName].H1GameCode);
                                    isok++;
                                }

                            }else {
                                //這裡因為有些榜單只會在活動最後一天出現，而設定檔卻必須先設定，故不排除與設定不符的榜名
                                //ErrorMsg.Append(string.Format("出現未知的榜單資料︰{0}，已略過\r\n", _ResultData[AwardName].Name));
                            }
                        }else {
                            ErrorMsg.Append("API傳來的榜單名稱為NULL\r\n");
                        }
                    }

                    if (ErrorMsg.ToString()!="")
                    {
                        Lib.MsgBox(this, ErrorMsg.ToString());
                        Session["JdbRaceData"] = null;
                        JResult = null;
                        _ResultData.Clear();
                        Lib.WritLog("JdbGameRaceResult.aspx.cs.GetRaceData()", ErrorMsg.ToString());
                        OK = false;
                    }

                    if (isok > 0)
                    {
                        RaceRt.DataSource = CreateRaceTableName();
                        RaceRt.DataBind();
                        btDeliver.Visible = true;

                    }else {
                        Lib.MsgBox(this, "查無任何與設定檔符合的榜單資料!!");
                        Session["JdbRaceData"] = null;
                        JResult = null;
                        _ResultData.Clear();
                        OK = false; 
                    }

                    if (OK) //只有在都沒有問題時，再轉json字串存進Session
                    {
                        try
                        {
                            Session["JdbRaceData"] = JsonConvert.SerializeObject(_ResultData);
                        }
                        catch (Exception ex)
                        {
                            Lib.MsgBox(this, ErrorMsg.ToString());
                            Session["JdbRaceData"] = null;
                            JResult = null;
                            _ResultData.Clear();
                            Lib.WritLog("JdbGameRaceResult.aspx.cs.GetRaceData()", string.Format("將資料物件轉成Json字串時，發生例外︰{0}", ex.ToString()));
                            OK = false;
                        }
                    }

                }else {
                
                    Lib.MsgBox(this, "API回傳無活動榜資料!!");
                    Session["JdbRaceData"] = null;
                    JResult = null;
                    _ResultData.Clear();
                    Lib.WritLog("JdbGameRaceResult.aspx.cs.GetRaceData()", "API回傳無活動榜資料!!");
                    OK = false;
                }
            }
            else
            {
                Lib.MsgBox(this, "取得活動結果失敗，awardData為NULL");
                Session["JdbRaceData"] = null;
                JResult = null;
                _ResultData.Clear();
                Lib.WritLog("JdbGameRaceResult.aspx.cs.GetRaceData()", "取得活動結果失敗，awardData為NULL");
                OK = false;
            }
        }
        else
        {
            Lib.MsgBox(this, "API回傳取得活動結果失敗!!!");
            Lib.WritLog("JdbGameRaceResult.aspx.cs.GetRaceData()", string.Format("Josn轉換結果Status︰{0},Description︰{1}", JResult.status, JResult.description));
            Session["JdbRaceData"] = null;
            JResult = null;
            _ResultData.Clear();
            OK = false;
        }
        
        dbSQL objdbSQL = new dbSQL();

        if (!objdbSQL.IsSendAmount())
        {
            Lib.MsgBox(this, "今日已經派過彩了!");
            OK = false;
            return OK;
        }
        return OK;
    }

    /// <summary>
    /// 取得資料按鈕的點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void seldata_Click(object sender, EventArgs e)
    {
        if (GetRaceData())
        {
            btDeliver.Visible = true;
        }else {
            btDeliver.Visible = false;
        }
    }

    /// <summary>
    /// 派送的按鈕的點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btDeliver_Click(object sender, EventArgs e)
    {
        
        dbSQL objSql = new dbSQL();

        string JDBdress = ConfigurationManager.AppSettings["JDBdress"] ?? string.Empty;

        bool isok = true;

        string ErrorMsg = string.Empty;

        int rank = 0;

        decimal bonus = 0;

        StringBuilder _ErrorMsg = new StringBuilder("");

        DataTable Senddata = new DataTable();

        Senddata.Columns.Add("名次", System.Type.GetType("System.String"));

        Senddata.Columns.Add("JDB-UID", System.Type.GetType("System.String"));

        Senddata.Columns.Add("帳號", System.Type.GetType("System.String"));

        Senddata.Columns.Add("獎金", System.Type.GetType("System.String"));

        Senddata.Columns.Add("GameCode", System.Type.GetType("System.String"));

        if (Session["JdbRaceData"] != null)
        {
            _ResultData = null;
            try
            {
                _ResultData = JsonConvert.DeserializeObject<Dictionary<string, CRaceResult>>(Session["JdbRaceData"].ToString());
            }
            catch (Exception ex)
            {
                _ErrorMsg.Append(string.Format("轉Session中的json字串發生例外︰{0}", ex.ToString()));
                isok = false;
            }
        }

        if (_ResultData != null)
        {
            if (_ResultData.Count > 0)
            {
                foreach (string keys in _ResultData.Keys)
                {
                    if (_ResultData[keys].RaceData != null)
                    {
                        if (_ResultData[keys].RaceData.Rows.Count > 0)
                        {
                            foreach (DataRow row in _ResultData[keys].RaceData.Rows)
                            {
                                if (string.IsNullOrEmpty(row["名次"].ToString()) || string.IsNullOrEmpty(row["JDB-UID"].ToString()) || string.IsNullOrEmpty(row["GameCode"].ToString()) || string.IsNullOrEmpty(row["獎金"].ToString()))
                                {
                                    _ErrorMsg.Append(string.Format("榜名︰{0}，存在資料欄位為空\r\n", _ResultData[keys].Name));
                                    isok = false;
                                    break;
                                }

                                if (!decimal.TryParse(row["獎金"].ToString(), out bonus))
                                {
                                    _ErrorMsg.Append(string.Format("榜名︰{0},出現獎金資料型態錯誤!!\r\n", _ResultData[keys].Name));
                                    isok = false;
                                    break;
                                }

                                if (bonus <= 0)
                                {
                                    _ErrorMsg.Append(string.Format("榜名︰{0}，出現獎金小於或等於0!!\r\n", _ResultData[keys].Name));
                                    isok = false;
                                    break;
                                }

                                if (!int.TryParse(row["名次"].ToString(), out rank))
                                {
                                    _ErrorMsg.Append(string.Format("榜名︰{0}，出現名次資料型態錯誤!!\r\n", _ResultData[keys].Name));
                                    isok = false;
                                    break;
                                }

                                if (rank <= 0)
                                {
                                    _ErrorMsg.Append(string.Format("榜名︰{0}，出現名次小於或等於0!!\r\n", _ResultData[keys].Name));
                                    isok = false;
                                    break;
                                }

                                DataRow dr = Senddata.NewRow();

                                dr["名次"] = row["名次"];

                                dr["JDB-UID"] = row["JDB-UID"];

                                dr["帳號"] = row["帳號"];

                                dr["獎金"] = row["獎金"];

                                dr["GameCode"] = row["GameCode"];

                                Senddata.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            //這裡出現API回覆有榜單，但榜內資料卻為空的情況
                            _ErrorMsg.Append(string.Format("榜名︰{0} 出現資料筆數為0\r\n", _ResultData[keys].Name));
                            //isok = false;
                            //break;
                        }
                    }
                    else
                    {
                        //這裡因為有些榜單只會在活動最後一天出現，而設定檔卻必須先設定，故不排除與設定不符的榜名                        
                        /*_ErrorMsg.Append(string.Format("榜名︰{0} API尚未提供資料\r\n", _ResultData[keys].Name));
                        isok = false;
                        break;*/
                    }
                }
              
                if (isok)
                {
                    isok = objSql.NewSendAmount(Senddata, JDBdress, out ErrorMsg);
                }

                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    _ErrorMsg.Append(ErrorMsg);
                } 
            } 
        }

        if (isok)
        {
            Lib.MsgBoxAndJump(this, "發送成功!!", "NewJdbGameRaceResult.aspx");
        }
        else
        {
            Lib.MsgBoxAndJump(this, "發送失敗!!", "NewJdbGameRaceResult.aspx");
        }

        Lib.WritLog("NewJdbGameRaceResult.cs.btDeliver_Click", _ErrorMsg.ToString());
    }

    /// <summary>
    /// 給Repeater控件繫結用的Datatable
    /// </summary>
    /// <returns></returns>
    private DataTable CreateRaceTableName()
    {
        ConfigRaceData tmpConfigRaceData = GetConfigData();

        DataTable _Dragon = new DataTable();

        _Dragon.Columns.Add("Name", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("JDBGameCode", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("H1GameCode", System.Type.GetType("System.String"));

        if (tmpConfigRaceData.RaceData.Count > 0)
        {
            for (int j = 0; j < tmpConfigRaceData.RaceData.Count; j++)
            {
                DataRow dr = _Dragon.NewRow();

                dr["Name"] = tmpConfigRaceData.RaceData[j].Name;

                dr["JDBGameCode"] = tmpConfigRaceData.RaceData[j].JDBGameCode;

                dr["H1GameCode"] = tmpConfigRaceData.RaceData[j].H1GameCode;
 
                _Dragon.Rows.Add(dr);
            }
        }

        return _Dragon;
    }

    /// <summary>
    /// 給Repeater控件繫結事件
    /// </summary>
    /// <returns></returns>
    protected void RaceRt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string AwardName = string.Empty;

            GridView DataItem = (GridView)e.Item.FindControl("DataItem");

            Label title = (Label)e.Item.FindControl("title");

            Label Msg = (Label)e.Item.FindControl("Msg");

            HiddenField HasData = (HiddenField)e.Item.FindControl("HasData");

            for (int j = 0; j < JResult.awardData.Count; j++)
            {
                if (!string.IsNullOrEmpty(JResult.awardData[j].awardName))
                {
                    AwardName = JResult.awardData[j].awardName.Trim();

                    //Response.Write(_ResultData.Keys.Contains(AwardName).ToString() + "=" + AwardName + "= " + ((HiddenField)e.Item.FindControl("JdbGameCode")).Value+"|"+e.Item.ItemIndex +"<br>");

                    if (_ResultData.Keys.Contains(AwardName) && ((HiddenField)e.Item.FindControl("JdbGameCode")).Value == AwardName)
                    {
                        DataItem.DataSource = SetDataTable(JResult.awardData[j], JResult.awardData[j].awardName.Trim(), _ResultData[AwardName].H1GameCode);

                        DataItem.DataBind();

                        title.Text = _ResultData[AwardName].Name;

                        if (HasData.Value == "SET")
                        {
                            Msg.Visible = false;
                            Msg.Text = ""; 
                        }

                        HasData.Value = "Y";

                        if (DataItem.Rows.Count <= 0)
                        {
                            Msg.Visible = true;
                            Msg.Text = string.Format("<h3>API未提供{0}的資料!!</h3>", title.Text);
                        }
                    }else {
                        if (HasData.Value == "N")
                        {
                            HasData.Value = "SET";
                            Msg.Visible = true;
                            Msg.Text = string.Format("<h3>API未提供{0}的資料!!!</h3>", title.Text);
                        }
                    }
                }
                else
                {
                    title.Visible = false;
                    Msg.Visible = true;
                    Msg.Text = "<h3>API傳來的資料有誤，AwardName為NULL!!</h3>";
                }
            }
        }
    }
}