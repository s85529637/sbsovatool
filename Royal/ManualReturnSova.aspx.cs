using System;
using System.Data;

public partial class ManualReturnSova : NewBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnReturn.Visible = false;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.ManualReturnAccount_Sova(txtClubEname.Text.Trim(),"");

        if (dt.Rows.Count > 0)
        {
            DataRow r = dt.Rows[0];

            String sPlayerId = (String.IsNullOrEmpty(r["PlayerId"].ToString())) ? "" : r["PlayerId"].ToString();
            String sSessionNo = (String.IsNullOrEmpty(r["SessionNo"].ToString())) ? "" : r["SessionNo"].ToString();
            String sLock = (String.IsNullOrEmpty(r["Lock"].ToString())) ? "0" : r["Lock"].ToString();
            String sSovaCount = (String.IsNullOrEmpty(r["SovaCount"].ToString())) ? "0" : r["SovaCount"].ToString();
            String sStakeCount = (String.IsNullOrEmpty(r["StakeCount"].ToString())) ? "0" : r["StakeCount"].ToString();
            String sNowXinYong = (String.IsNullOrEmpty(r["NowXinYong"].ToString())) ? "0.0" : r["NowXinYong"].ToString();
            String sLogoutXinYong = (String.IsNullOrEmpty(r["LogoutXinYong"].ToString())) ? "0.0" : r["LogoutXinYong"].ToString();

            int iLock = int.Parse(sLock);
            int iSovaCount = int.Parse(sSovaCount);
            int iStakeCount = int.Parse(sStakeCount);
            Decimal dNowXinYong = Decimal.Parse(sNowXinYong);
            Decimal dLogoutXinYong = Decimal.Parse(sLogoutXinYong);

            if (sPlayerId != ""
                && sSessionNo != ""
                && iLock == 1
                && dNowXinYong == 0
                && dLogoutXinYong > 0
                && iSovaCount == 0
                && iStakeCount == 0
                )
            {
                btnReturn.Visible = true;
                Session["ClubEname"] = txtClubEname.Text.Trim();
            }
            /*
PlayerId	SessionNo	Active	Login	Lock	DongJie	LoginRoom	LoginEGame	LoginGameId	LoginServerId	JumboStatus	SovaCount	JumboCount	StakeCount	NowXinYong	LogoutXinYong	LastXinYong	GameSeqNoMin	GameSeqNoMax	MNum	MType	Location	StartTime	EndTime	TTLBet	TTLWin	TTLWinGame	TTLWinGamble	TTLWinJackpot	TTLPrepay	TTLNetWin	Rows
1106241705	20160719134945127	1	0	0	0	0	0	Room	Room	0	0	1	0	89998188.00	0.00	0.00	NULL	NULL	0	0	NULL	2016-08-04 17:50:10.607	2016-08-04 17:50:10.607	0.00	0.00	0.00	0.00	0.00	0.00	0.00	0
*/
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        else
        {
            btnReturn.Visible = false;
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.ManualReturnAccount_Jumbo(Session["ClubEname"].ToString(), "Return");

        Session.Remove("ClubEname");

        if (dt.Rows.Count > 0)
        {
            gvData.DataSource = dt;
            gvData.DataBind();
        }        
    }
}