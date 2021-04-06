using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Xml;

public partial class UnReturnAccount : NewBasePage
{
    public string ClubStatus = string.Empty;
    public string SessionNoStatus = string.Empty;
    public string SessionNo = string.Empty;
    public string GameId = string.Empty;
    public string ThirdPartyId = string.Empty;
    public string ClubId = string.Empty;
    public double TC_Logout_Xinyong = 0;
    public double TS_Logout_Xinyong = 0;
    public double Now_XinYong = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        PageIni();
    }

    protected void PageIni()
    {
        lblQueryResult.Text = "";
        lblClubStatus.Text = "";
        lblSessionNoStatus.Text = "";
        lblUrl.Text = "";
        lblGetPlayerAccountLog.Text = "";
        lblGetPlayerAccountResult.Text = "";
        lblKickLog.Text = "";
        lblKickResult.Text = "";
        lblGetMemberWinloseCountLog.Text = "";
        lblRecoveryAccount.Text = "";
        lblDescription.Text = "";
        lblReturnValue.Text = "";
        btnRecovery.Visible = false;
    }

    protected void PageProcess() //page顯示處理
    {
        if (ClubStatus == "已開分" && SessionNoStatus == "已開分" && ThirdPartyId == "Golden")
        {
            int count = Ruby4Ws_GetMemberWinloseCount(txtClubEname.Text.Trim(), SessionNo, GameId);
            if (count > 0)
            {
                lblUrl.Text = "有電子帳請至黃金後台執行手動洗分";
            }
            else
            {
                btnRecovery.Visible = true; //恢復會員狀態按鈕
            }
        } 
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        PageIni();
        GetUnReturnAccount_Ruby4_Club();
        PageProcess();
    }

    private void GetUnReturnAccount_Ruby4_Club() //取得會員狀態
    {
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetUnReturnAccount_Ruby4(txtClubEname.Text.Trim(), "Golden");
        if (dt.Rows.Count == 0)
        {
            lblQueryResult.Text = "會員帳號:" + txtClubEname.Text.Trim() + "不存在";
        }
        else
        {
            ClubStatus = dt.Select("")[0]["會員狀態"].ToString();
            SessionNoStatus = dt.Select("")[0]["開分狀態"].ToString();
            SessionNo = dt.Select("")[0]["SessionNo"].ToString();
            GameId = dt.Select("")[0]["GameId"].ToString();
            ThirdPartyId = dt.Select("")[0]["ThirdParty_Id"].ToString();
            ClubId = dt.Select("")[0]["Club_id"].ToString();
            double.TryParse(dt.Select("")[0]["TC_Logout_Xinyong"].ToString(), out TC_Logout_Xinyong);
            double.TryParse(dt.Select("")[0]["TS_Logout_Xinyong"].ToString(), out TS_Logout_Xinyong);
            double.TryParse(dt.Select("")[0]["Now_XinYong"].ToString(), out Now_XinYong);
            lblClubStatus.Text = "會員狀態:" + ClubStatus;
            lblSessionNoStatus.Text = "開分狀態:" + SessionNoStatus;
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void btnRecovery_Click(object sender, EventArgs e)
    {
        RecoveryClub();
    }

    protected void RecoveryClub() //恢復會員狀態
    {
        GetUnReturnAccount_Ruby4_Club(); //重新取會員資訊

        bsSQL bssql = new bsSQL();

        int count = Ruby4Ws_GetMemberWinloseCount(txtClubEname.Text.Trim(), SessionNo, GameId); //取得明細帳筆數
        if (count > 0)
        {
            lblUrl.Text = "有電子帳請至黃金後台執行手動洗分";
        }
        else
        {
            if (Ruby4Ws_KickUser(txtClubEname.Text.Trim()) == "1") //踢出指定玩家
            {
		/*
                string Result = Ruby4Ws_GetPlayerAccount(txtClubEname.Text.Trim(), SessionNo, GameId); //取得會員於該遊戲中最終的額度
                double Account;
                double.TryParse(Result, out Account);
		*/
	
                //if ((Now_XinYong == 0) && ((Logout_Xinyong == Account)|| Math.Abs(Account - Logout_Xinyong) < 0.1))
		if ((Now_XinYong == 0) && (TS_Logout_Xinyong == TC_Logout_Xinyong)) 	
                {
                    DataTable dt = bssql.RecoveryClub(ClubId, SessionNo);
                    if (dt.Rows.Count > 0)
                    {
                        lblDescription.Text = "結果 "+dt.Rows[0]["Description"].ToString();
                        lblReturnValue.Text = "回傳代碼 "+dt.Rows[0]["ReturnValue"].ToString();      
                    }
                    else
                    {
                        lblRecoveryAccount.Text = "調用恢復SP發生錯誤,無法進行會員狀態恢復";
                    }
                }
                else
                {
                    lblRecoveryAccount.Text = "會員額度不同,無法進行會員狀態恢復";
                }
            }
            else
            {
                lblKickResult.Text = "踢線不成功,無法執行會員狀態恢復";
            }
        }
    }

    protected int Ruby4Ws_GetMemberWinloseCount(string ClubEname, string SessionNo, string GameId) //取得明細帳筆數
    {
        /*  MemberAccount：會員帳號(string)
            WID：開洗分單號(long)
            GameID：遊戲編號(string) wtf.. */

        int Result = 0;
        long SessionNo_LongType;
        long.TryParse(SessionNo, out SessionNo_LongType);

        using (ruby4ws.GameCommand ws = new ruby4ws.GameCommand())
        {
            ws.Url = ConfigurationManager.AppSettings["Golden.WebService"].ToString();
            ws.Discover();

            String GoldenGameNo = Lib.GetGoldenGameNo(GameId).ToString();
            Result = ws.GetMemberWinloseCount(ClubEname, SessionNo_LongType, GoldenGameNo);
            lblGetMemberWinloseCountLog.Text = "調用GetMemberWinloseCount(" + ClubEname + "," + SessionNo_LongType.ToString() + "," + GoldenGameNo + ") 返回結果:" + Result.ToString();
        }
        return Result;
    }

    protected string Ruby4Ws_KickUser(string ClubEname) //踢出指定玩家
    {
        /* MemberAccount:玩家帳號
           回傳格式:string  1:成功 其他:失敗 */
        string Result = string.Empty;
        using (ruby4ws.GameCommand ws = new ruby4ws.GameCommand())
        {
            ws.Url = ConfigurationManager.AppSettings["Golden.WebService"].ToString();
            ws.Discover();

            Result = ws.KickUser(ClubEname);

            lblKickLog.Text = "調用KickUser(" + ClubEname + ") 返回結果:" + Result.ToString();
        }
        return Result;
    }

    protected string Ruby4Ws_GetPlayerAccount(string ClubEname, string SessionNo, string GameId) //取得會員於該遊戲中最終的額度，用做驗證
    {
        /*  PlayerId:玩家編號
            SessionId:開分識別碼
            GameId:遊戲編號
            回傳XML格式:<double>-1.0000</double>  */

        XmlNode Result;

        using (ruby4ws.GameCommand ws = new ruby4ws.GameCommand())
        {
            ws.Url = ConfigurationManager.AppSettings["Golden.WebService"].ToString();
            ws.Discover();

            String GoldenGameNo = Lib.GetGoldenGameNo(GameId).ToString();
            Result = ws.GetPlayerAccount(ClubEname, SessionNo, GoldenGameNo);
            lblGetPlayerAccountLog.Text = "調用GetPlayerAccount(" + ClubEname + "," + SessionNo + "," + GoldenGameNo + ") 返回結果:" + Result.InnerXml;
        }
        return Result.InnerXml;
    }
}