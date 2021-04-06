using SlotGameWS;
using System;
using System.Data;
using System.Text;
using System.Xml;

public partial class UnReturnAccount_Royal : NewBasePage
{
    String sFormat = "{0}:{1}";
    StringBuilder sb = new StringBuilder();
    String sMessage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LoadData();
        }
    }

    protected void LoadData()
    {
        DataTable dt = new DataTable();
        lblSeqNo.Text = "執行中";
        int SeqNo = CheckFailureData();
        if (SeqNo > 1)
        {
            SeqNo -= 1;
            SlotGameWSSoapClient ws = new SlotGameWSSoapClient();
            XmlNode root = ws.GetAPILogErrorList("mobile", SeqNo, 1);
            dt = DataTableSchema();
            Xml2DataTable(root, dt);

            PlayerReturnAccount(dt);
        }
    }

    private int CheckFailureData()
    {
        /*
        ReturnFlag 0:成功 1:失敗 2:錯誤 3:逾時
        GetType 0:開分 1:洗分
        */
        int iValue = -1;
        bsSQL db = new bsSQL();
        int SeqNo = db.GetRoyalGameReturnFailNo();
        lblSeqNo.Text = SeqNo.ToString();
        SlotGameWSSoapClient ws = new SlotGameWSSoapClient();
        XmlNode root = ws.GetAPILogErrorList("mobile", SeqNo, 100);

        DataTable dt = DataTableSchema();
        Xml2DataTable(root, dt);
        foreach (DataRow r in dt.Rows)
        {
			String id = r["id"].ToString();
			iValue = int.Parse(id);

            String Key = r["Key"].ToString();
            String Gameid = r["Gameid"].ToString();
            String SessionNo = r["SessionNo"].ToString();
            if (SessionNo.Trim().Equals(String.Empty))
            {
                continue;
            }
            else if (Key.Trim().Equals(String.Empty) || Gameid.Trim().Equals(String.Empty))
            {
                sMessage = String.Format(sFormat, id.ToString(), "資料有誤跳過");
                sb.AppendLine(sMessage);
                TextBox1.Text = sb.ToString();
                continue;
            }

            String GetType = r["GetType"].ToString();
            String ReturnFlag = r["ReturnFlag"].ToString();
            if ("1".Equals(GetType))
            {
                switch (ReturnFlag)
                {
                    case "1":
                    case "2":
                    case "3":
                        return int.Parse(id);
                    case "0":
                    default:
                        break;
                }
            }
        }
		if (iValue > -1)
			db.SetRoyalGameReturnFailNo(iValue);
        return -1;
    }

    private DataTable DataTableSchema()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(String));
        dt.Columns.Add("PlayerId", typeof(String));
        dt.Columns.Add("Key", typeof(String));
        dt.Columns.Add("WebId", typeof(String));
        dt.Columns.Add("SessionNo", typeof(String));
        dt.Columns.Add("StakeScore", typeof(Decimal));
        dt.Columns.Add("AccountScore", typeof(Decimal));
        dt.Columns.Add("Gameid", typeof(String));
        dt.Columns.Add("Location", typeof(String));
        dt.Columns.Add("StartTime", typeof(DateTime));
        dt.Columns.Add("EndTime", typeof(DateTime));
        dt.Columns.Add("Jackpot", typeof(Decimal));
        dt.Columns.Add("PlayerAccount", typeof(Decimal));
        dt.Columns.Add("Rows", typeof(String));
        dt.Columns.Add("GetType", typeof(String));
        dt.Columns.Add("ReturnFlag", typeof(String));
        return dt;
    }

    private void Xml2DataTable(XmlNode root, DataTable dt)
    {
        foreach (XmlNode c in root.ChildNodes)
        {
            DataRow r = dt.NewRow();
            foreach (XmlNode cc in c.ChildNodes)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (cc.Name == dc.ColumnName)
                    {
                        switch (dc.DataType.Name)
                        {
                            case "Decimal":
                            case "Float":
                            case "Double":
                                r[dc.ColumnName] = Decimal.Parse(cc.InnerText);
                                break;
                            default:
                                if (dc.ColumnName.Equals("Gameid"))
                                {
                                    int RoyalGameNo = 0;
                                    if (int.TryParse(cc.InnerText, out RoyalGameNo))
                                        r[dc.ColumnName] = Lib.ReturnRoyalGameId(cc.InnerText);
                                    else
                                        r[dc.ColumnName] = cc.InnerText;
                                }
                                else
                                    r[dc.ColumnName] = cc.InnerText;
                                break;
                        }
                    }
                }
            }
            dt.Rows.Add(r);
        }
    }

    private void PlayerReturnAccount(DataTable dtTemp)
    {
        if (dtTemp.Rows.Count == 0)
            return;

        bsSQL db = new bsSQL();
        DataRow row = dtTemp.Rows[0];

        int iID = int.Parse(row["id"].ToString());
        string PlayerId = row["PlayerId"].ToString();
        string Key = row["Key"].ToString();
        string WebId = row["WebId"].ToString();
        string SessionNo = row["SessionNo"].ToString();
        Decimal dStakeScore = Decimal.Parse(row["StakeScore"].ToString());
        Decimal dAccountScore = Decimal.Parse(row["AccountScore"].ToString());
        string RoyalGameid = row["Gameid"].ToString();
        string Location = row["Location"].ToString();
        string StartTime = row["StartTime"].ToString();
        string EndTime = row["EndTime"].ToString();
        Decimal dJackpot = Decimal.Parse(row["Jackpot"].ToString());
        Decimal dPlayerAccount = Decimal.Parse(row["PlayerAccount"].ToString());
        int Rows = int.Parse(row["Rows"].ToString());
        string GetType = row["GetType"].ToString();
        string ReturnFlag = row["ReturnFlag"].ToString();
        DateTime dEndTime = DateTime.Parse(EndTime);
        EndTime = dEndTime.ToString("yyyy/MM/dd HH:mm:ss");

        DataTable dt = db.ReturnAccount_Royal(PlayerId, WebId, Key, Location, dPlayerAccount, SessionNo, RoyalGameid, dStakeScore, dAccountScore, Rows, EndTime, dJackpot);
        if (dt.Rows.Count > 0 && dt.Columns.Count > 1)
        {
            db.SetRoyalGameReturnFailNo(iID);
            DataTable dtLogout = db.Logout_Royal(PlayerId, WebId, Key, "Royal");
            sMessage = String.Format(sFormat, iID.ToString(), "洗分成功");
            sb.AppendLine(sMessage);
            TextBox1.Text = sb.ToString();
            LoadData();
        }
        else if (dt.Rows.Count.Equals(1)
            && dt.Columns.Count.Equals(1)
            && dt.Rows[0][0].ToString().Trim().Equals("Repeat Return Account Error"))
        {
            db.SetRoyalGameReturnFailNo(iID);
            DataTable dtLogout = db.Logout_Royal(PlayerId, WebId, Key, "Royal");
            sMessage = String.Format(sFormat, iID.ToString(), "重覆洗分");
            sb.AppendLine(sMessage);
            TextBox1.Text = sb.ToString();
            LoadData();
        }
        else
        {
            sMessage = String.Format(sFormat, iID.ToString(), "洗分失敗");
            sb.AppendLine(sMessage);
            TextBox1.Text = sb.ToString();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void chkAuto_CheckedChanged(object sender, EventArgs e)
    {
        Timer1.Enabled = chkAuto.Checked;
    }
}