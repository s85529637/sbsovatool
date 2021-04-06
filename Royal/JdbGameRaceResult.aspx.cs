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


public partial class JdbGameRaceResult : System.Web.UI.Page
{
    string activityDate = string.Empty;

    string activityNo = string.Empty;

    string activityName = string.Empty;

    ResultData JResult = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("已不使用");
        Response.End();
        if (!Page.IsPostBack)
        {
            Send.Visible = false;
            seldate.Visible = false;
            lbDragon.Visible = false;
            lbTiger.Visible = false;
            seldateText.ReadOnly = true; 
        }
    }

    public DataTable SetTigerDataTable(AwardData Tiger)
    {
        DataTable _Tiger = new DataTable("Tiger");

        _Tiger.Columns.Add("名稱", System.Type.GetType("System.String"));

        _Tiger.Columns.Add("JDB-UID", System.Type.GetType("System.String"));

        _Tiger.Columns.Add("帳號", System.Type.GetType("System.String"));

        _Tiger.Columns.Add("獎金", System.Type.GetType("System.String"));

        if (Tiger.rankData.Count > 0)
        {
            for (int j = 0; j < Tiger.rankData.Count; j++)
            {
                DataRow dr = _Tiger.NewRow();

                dr["名稱"] = Tiger.rankData[j].rank;

                dr["JDB-UID"] = Tiger.rankData[j].uid;

                dr["帳號"] = Tiger.rankData[j].displayName;

                dr["獎金"] = Tiger.rankData[j].amount;

                _Tiger.Rows.Add(dr);
            }
        }

        return _Tiger;
    }

    public DataTable SetDragonDataTable(AwardData Dragon)
    {
        DataTable _Dragon = new DataTable("Dragon");

        _Dragon.Columns.Add("名次", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("JDB-UID", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("帳號", System.Type.GetType("System.String"));

        _Dragon.Columns.Add("獎金", System.Type.GetType("System.String"));

        if (Dragon.rankData.Count > 0)
        {
            for (int j = 0; j < Dragon.rankData.Count; j++)
            {
                DataRow dr = _Dragon.NewRow();

                dr["名次"] = Dragon.rankData[j].rank;

                dr["JDB-UID"] = Dragon.rankData[j].uid;

                dr["帳號"] = Dragon.rankData[j].displayName;

                dr["獎金"] = Dragon.rankData[j].amount;

                _Dragon.Rows.Add(dr);
            }
        }

        return _Dragon;
    }

    protected void Send_Click(object sender, EventArgs e)
    {

        bool isok = false;

        dbSQL objdbSQL = new dbSQL();

        string JDBdress = ConfigurationManager.AppSettings["JDBdress"] ?? string.Empty;

        if (JResult == null)
        {
            //定義在JDBib.cs
            JDBlib objJDBlib = new JDBlib("18");

            string Param = string.Empty;

            string Result = string.Empty;

            JResult = objJDBlib.GetGameRaceResult(Hidseldate.Value.ToString(), out Param, out Result);

            Lib.WritLog("JdbGameRaceResult.aspx.cs.Page_Load()", string.Format("調用參數︰{0},結果︰{1}", Param, Result));
        }

        Msg.Text = string.Empty;

        if (JResult.status == "1" && JResult.description == "Success")
        {
            if (JResult.awardData != null)
            {
                if (JResult.awardData.Count > 0)
                {
                    isok = objdbSQL.SendAmount(JResult.awardData, JDBdress);

                }else {

                    txtscript.Text = "<h3>取得活動結果失敗!</h3>";
                }
            }
            else
            {
                txtscript.Text = "<h3>取得活動結果失敗!</h3>";
            }
        }
        else
        {
            txtscript.Text = "<h3>取得活動結果失敗!</h3>";
        }

        if (!isok)
        {
            txtscript.Text = "<h3>活動彩金派發失敗!</h3>";

        }else{
          
            txtscript.Text = "<h3>活動彩金派發成功!</h3>";

            Lib.WritLog("JdbGameRaceResult.aspx.cs.Send_Click()",string.Format("[{0}]活動彩金派發成功!activityNo︰{1}", JResult.activityDate, JResult.activityNo));
        }

        TigerList.Visible = false;
        DragonList.Visible = false;
        lbDragon.Visible = false;
        lbTiger.Visible = false;
        Send.Enabled = false;
        Send.Visible = false;
    }

    protected void seldata_Click(object sender, EventArgs e)
    {
        //定義在JDBib.cs
        JDBlib objJDBlib = new JDBlib("18");

        string Param = string.Empty;

        string Result = string.Empty;

        DataTable Dragon;

        DataTable Tiger;

        JResult = objJDBlib.GetGameRaceResult(Hidseldate.Value.ToString(), out Param, out Result);

        Lib.WritLog("JdbGameRaceResult.aspx.cs.Page_Load()", string.Format("調用參數︰{0},結果︰{1}", Param, Result));

        if (JResult.status == "1" && JResult.description == "Success")
        {
            if (JResult.awardData != null)
            {
                switch (JResult.awardData.Count)
                {
                    case 0:
                        lbDragon.Visible = false;
                        lbTiger.Visible = false;
                        break;
                    case 1:
                        if (JResult.awardData[0].awardName.ToUpper() == "DRAGON")
                        {
                            Dragon = SetDragonDataTable(JResult.awardData[0]);

                            DragonList.DataSource = Dragon;

                            DragonList.DataBind();

                            lbTiger.Visible = false;
                            TigerList.Visible = false;
                            DragonList.Visible = true;
                            lbDragon.Visible = true;
                        }
                        else if (JResult.awardData[0].awardName.ToUpper() == "TIGER")
                        {
                            Tiger = SetTigerDataTable(JResult.awardData[0]);

                            TigerList.DataSource = Tiger;

                            TigerList.DataBind();

                            lbDragon.Visible = false;
                            DragonList.Visible = false;
                            TigerList.Visible = true;
                            lbTiger.Visible = true;
                        } 
                        txtscript.Text = string.Empty;
                        Send.Visible = true;
                        Send.Enabled = true;
                        break;
                    case 2:
                        if (JResult.awardData[0].awardName.ToUpper() == "DRAGON")
                        {
                            Dragon = SetDragonDataTable(JResult.awardData[0]);

                            DragonList.DataSource = Dragon;

                            DragonList.DataBind();
                        }
                        else if (JResult.awardData[0].awardName.ToUpper() == "TIGER")
                        {
                            Tiger = SetTigerDataTable(JResult.awardData[0]);

                            TigerList.DataSource = Tiger;

                            TigerList.DataBind();
                        }

                        if (JResult.awardData[1].awardName.ToUpper() == "TIGER")
                        {
                            Tiger = SetTigerDataTable(JResult.awardData[1]);

                            TigerList.DataSource = Tiger;

                            TigerList.DataBind();

                        }
                        else if (JResult.awardData[1].awardName.ToUpper() == "DRAGON")
                        {
                            Dragon = SetDragonDataTable(JResult.awardData[1]);

                            DragonList.DataSource = Dragon;

                            DragonList.DataBind();
                        }
                        TigerList.Visible = true;
                        DragonList.Visible = true;
                        lbDragon.Visible = true;
                        lbTiger.Visible = true;
                        txtscript.Text = string.Empty;
                        Send.Visible = true;
                        Send.Enabled = true;
                        break;
                }
            }
            else
            {
                TigerList.Visible = false;
                DragonList.Visible = false;
                lbDragon.Visible = false;
                lbTiger.Visible = false;
                Send.Enabled = false;
                Send.Visible = false;
               txtscript.Text = "<h3>取得活動結果失敗!</h3>";
                Lib.WritLog("JdbGameRaceResult.aspx.cs.Page_Load()", "缺少龍虎榜資料!!");
            }
        }
        else
        {
            TigerList.Visible = false;
            DragonList.Visible = false;
            lbDragon.Visible = false;
            lbTiger.Visible = false;
            Send.Enabled = false;
            Send.Visible = false;
            txtscript.Text = "<h3>取得活動結果失敗!</h3>";
            Lib.WritLog("JdbGameRaceResult.aspx.cs.Page_Load()", string.Format("Josn轉換結果Status︰{0},Description︰{1}", JResult.status, JResult.description));
        }

        dbSQL objdbSQL = new dbSQL();

        if (!objdbSQL.IsSendAmount())
        {
            Send.Enabled = false;
            txtscript.Text = "<h3>今日已經派過彩了!</h3>";
            return;
        }
        else
        {
            Send.Enabled = true;
            txtscript.Text = string.Empty;
        }
    }

    protected void seldate_SelectionChanged(object sender, EventArgs e)
    {
        string m = string.Empty;

        string d = string.Empty;

        if (seldate.SelectedDate.Month <= 9)
        {
            m = string.Format("0{0}", seldate.SelectedDate.Month);

        }else if (seldate.SelectedDate.Month <= 99 && seldate.SelectedDate.Month >=10)
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

    protected void selCal_Click(object sender, EventArgs e)
    {
        seldate.Visible = true;
    }
}