using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class SyncOpenList : NewBasePage
{
    String Message = "{0}.{1}:{2}:{3}{4}", NewLine = "\r\n", Step = "", Start = "開始", Error = "錯誤", Success = "成功", Fail = "失敗",
    TimeFormat = "yyyy/MM/dd hh:mm:ss.fff";
    DateTime dtStart, dtEnd; TimeSpan ts;int iCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String[] arrGameServer = ConfigurationManager.AppSettings["VideoServer"].ToString().Split(',');
            for (int i = arrGameServer.Length - 1; i > -1; i--)
            {
                ListItem oItem = new ListItem();
                oItem.Text = arrGameServer[i];
                oItem.Value = arrGameServer[i];
                ddlGameServer.Items.Insert(0, oItem);
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        runProcess();
    }

    private void runProcess()
    {
        txtMessage.Text = "";
        String ServerId, GameId, EndActiveNo;
        int iNoActive = 0, iRealCount = 0;
        GameId = ddlGameServer.SelectedValue.Substring(0, ddlGameServer.SelectedValue.Length - 1);
        bsSQL ssql = new bsSQL();
        DataTable dt = ssql.GetServerId(ddlGameServer.SelectedValue);

        if (dt != null && dt.Rows.Count > 0)
        {
            ServerId = dt.Rows[0][0].ToString();
            iCount++; Step = "ServerId"; dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, ServerId, dtEnd.ToString(TimeFormat), NewLine);
            dt = ssql.GetOpenListNow(GameId,ServerId);
            if (dt != null && dt.Rows.Count > 0)
            {
                iNoActive = int.Parse(dt.Rows[0]["No_Active"].ToString());
                iNoActive = (iNoActive > 0) ? iNoActive - 1 : iNoActive;
                EndActiveNo = iNoActive.ToString("0000");
                iCount++; Step = "EndActiveNo"; dtEnd = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step, EndActiveNo, dtEnd.ToString(TimeFormat), NewLine);

                if (iNoActive > 0)
                {
                    dt = ssql.GetActiveCount(GameId, ServerId);
                    iRealCount = int.Parse(dt.Rows[0][0].ToString());

                    if (iNoActive.Equals(iRealCount))
                        return;

                    Mobile.MobileLogWrapperSoapClient ws = new Mobile.MobileLogWrapperSoapClient();
                    XmlNode root = ws.GetOpenListBatch(ServerId, "", "0001", EndActiveNo);

                    String Status, sValue;
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
                            case "Run_Active":
                                Run_Active(cn);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    private void Run_Active(XmlNode cn)
    {

    }
}