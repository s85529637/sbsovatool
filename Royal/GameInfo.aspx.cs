using LiveGameWS;
using System;
using System.Data;
using System.Web.UI;
using System.Xml;

public partial class GameInfo : NewBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (txtStartDate.Text.Equals(String.Empty))
        {
            txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        }
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        Msg.Text = "";
        runProcess();
    }

    private void runProcess()
    {
        bool isok = true;
        bsSQL ssql = new bsSQL();
        DataTable table = null;
        try
        {
            table = ssql.GetGameMemberCount(txtStartDate.Text.Trim());
            gvM.DataSource = table;
            DateTime h1Date = DateTime.Parse(txtStartDate.Text.Trim());
            LiveGameSoapClient ws = new LiveGameSoapClient();
            XmlNode root = ws.GetDeviceCount(h1Date.AddDays(1).ToString("yyyyMMdd"));
            XmlNodeList xStatus = root.SelectNodes("//Status");
            XmlNodeList xDescription = root.SelectNodes("//Description"); 
            if (xStatus[0].InnerText != "1" && xDescription[0].InnerText != "Sucess")
            {
                Msg.Text = "API�^�ǥ��ѡA�T���O�J" + xDescription[0].InnerText.ToString();
                return;
            }
            DataTable dt = DataTableSchema();
            Xml2DataTable(root, dt);

            // ���ø˸m�N�X�GWeb5H(���G�O���եΪ�)
            if (dt.Columns.Contains("�˸m�N�X"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["�˸m�N�X"].ToString() == "Web5H")
                    {
                        row.Delete();
                        break;
                    }
                }
                dt.AcceptChanges();
            }

            gvDevice.DataSource = dt;
        }
        catch(Exception ex)
        {
            Msg.Text = ex.ToString();
            isok = false;
        }
        if (isok)
        {
            this.DataBind();
        }
    }

    private DataTable DataTableSchema()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("LogDate", typeof(String));
        dt.Columns.Add("Device", typeof(String));
        dt.Columns.Add("Count", typeof(String));
        return dt;
    }

    private void Xml2DataTable(XmlNode root, DataTable dt)
    {
        XmlNodeList xlist = root.SelectNodes("//DataInfo");

        if (xlist == null)
        {
            Msg.Text = "<h3>API�o�ͥ����ҥ~!!</h3>";
            return;
        }

        if (xlist.Count <= 0)
        {
            Msg.Text = "<h3>�d�L������!!</h3>";
        }
   
        for (int j = 0; j < xlist.Count; j++)
        {
            DataRow r = null;

            if (xlist[j].HasChildNodes)
            {
                foreach (XmlNode cc in xlist[j].ChildNodes)
                {
                    if (!dt.Columns.Contains(cc.Name.Trim()))
                    {
                        Msg.Text = "<h3>API�ǨӤ��s�b�����W��!!</h3>";
                        return;
                    }

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (cc.Name.Trim().ToUpper() == dc.ColumnName.Trim().ToUpper())
                        {
                            if (r == null)
                                r = dt.NewRow();
                            switch (dc.DataType.Name)
                            {
                                case "Decimal":
                                case "Float":
                                case "Double":
                                    r[dc.ColumnName] = Decimal.Parse(cc.InnerText);
                                    break;
                                default:
                                    r[dc.ColumnName] = cc.InnerText;
                                    break;
                            }
                        }
                    }
                }
            }

            if (r != null)
                dt.Rows.Add(r);
        }

        /* 2020-11-16_��l�{��
        foreach (XmlNode c in root.ChildNodes)
        {
            DataRow r = null;
            foreach (XmlNode cc in c.ChildNodes)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (cc.Name == dc.ColumnName)
                    {
                        if (r == null)
                            r = dt.NewRow();
                        switch (dc.DataType.Name)
                        {
                            case "Decimal":
                            case "Float":
                            case "Double":
                                r[dc.ColumnName] = Decimal.Parse(cc.InnerText);
                                break;
                            default:
                                r[dc.ColumnName] = cc.InnerText;
                                break;
                        }
                    }
                }
            }
            if (r != null)
                dt.Rows.Add(r);
        }*/
    }
}