using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OnlineMemberCountsList : NewBasePage
{

    private AppSettingsReader reader = new AppSettingsReader();

    private string EffectiveRange = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["isLogin"] == null) Response.Redirect("Logout.aspx");
    }

    private void GetData(string sdate,string edate)
    {
        EffectiveRange = string.IsNullOrEmpty(reader.GetValue("EffectiveRange", typeof(string)).ToString()) ? "3" : reader.GetValue("EffectiveRange", typeof(string)).ToString();

        bsSQL objsql = new bsSQL();

        DateTime tmpSDateTime;

        DateTime tmpEDateTime;

        bool S_isok = true;

        bool E_isok = true;

        string _Pavilion = Pavilion.SelectedValue;

        if (!DateTime.TryParse(sdate, out tmpSDateTime))
        {
            S_isok = false;
        }

        if (!DateTime.TryParse(edate, out tmpEDateTime))
        {
            E_isok = false;
        }

        if (S_isok && E_isok)
        {
            if (tmpEDateTime > tmpSDateTime)
            {
                TimeSpan ts = tmpEDateTime - tmpSDateTime;

                double s2 = ts.TotalDays;

                if (s2 < double.Parse(EffectiveRange))
                {
                    try
                    {
                        DataTable dt = objsql.GetOnlineMemberCounts_List(sdate, edate, _Pavilion);

                        if (dt == null)
                        {
                            dt = new DataTable();
                        }

                        if (dt.Rows.Count <= 0)
                        {
                            objMsg1.Text = "<h3>查無資料!!</h3>";
                        }

                        this.OnlineMemberCounts_List.DataSource = dt;

                        this.OnlineMemberCounts_List.DataBind();
                    }
                    catch (Exception ex)
                    {
                        objMsg1.Text = string.Format("<h3>發生未知例外!!{0}</h3>", ex.ToString());
                    }
                }
                else
                {
                    objMsg1.Text = string.Format("<h3>起訖日期不得超過{0}天!!</h3>", EffectiveRange);
                }
            }
            else {
                objMsg1.Text = "<h3>結束日期不能小於開始日期!!</h3>";
            }
        }else {
            objMsg1.Text = "<h3>請填寫正確的日期格式[yyyy-MM-dd HH:mm:ss]!!</h3>";
        }
    }

    protected void btsent_Click(object sender, EventArgs e)
    {
        string sdate = this.a_startdatetime.Value;
        string edate = this.a_enddatetime.Value;
        objMsg1.Text = string.Empty;
        GetData(sdate, edate);
    }
}