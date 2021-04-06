using System;
using System.Data;

public partial class UnReturnAccount : NewBasePage
{
    public string ClubStatus = string.Empty;
    public string SessionNoStatus = string.Empty;
    public string LoginGameId = string.Empty;
    public double TC_Logout_Xinyong = 0, Now_XinYong = 0;
    public string ClubId = string.Empty;
    public string WebId = "Recovery";
    public string SessionNo = string.Empty;
    public string SessionCreateTime = string.Empty;
    public string PassWord = string.Empty;
    public double Amount = 0, GameSeqNo_Min = 0, GameSeqNo_Max = 0, ttlBet = 0, ttlWin = 0, ttlWinGame = 0, ttlWinGamble = 0, ttlWinJackpot = 0, ttlPrepay = 0, ttlNetWin = 0;
    public int JumboAccountCount = 0, mNum = 0, mType = 0, playCurrencyRate = 0;
    public string Location = string.Empty, StartTime = string.Empty, EndTime = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        PageIni();
    }

    protected void PageIni()
    {
        lblJumboAccountCount.Text = "";
        lblQueryResult.Text = "";
        lblClubStatus.Text = "";
        lblSessionNoStatus.Text = "";
        lblJumboAccountCountTxt.Text = "";
        lblReturnAccount.Text = "";
        lblRecoveryAccount.Text = "";
        lblDescription.Text = "";
        lblReturnValue.Text = "";
        btnRecovery.Visible = false;
        btnReturnAccount.Visible = false;
        GridView2.DataSource = null;
        GridView2.DataBind();
    }

    protected void PageProcess() //page顯示處理
    {
        if (ClubStatus == "已開分" && SessionNoStatus == "已開分" && LoginGameId == "JumboGame") //卡遊戲
        {
            if (JumboAccountCount > 0) //有帳
            {
                lblJumboAccountCount.Text = "Jumbo帳務筆數:" + JumboAccountCount.ToString() + "筆";
                lblJumboAccountCountTxt.Text = "有電子帳需執行Jumbo手動洗分";
                GetUnReturnAccount_JumboData(ClubId, SessionNo);
            }
            else //沒帳
            {
                lblJumboAccountCountTxt.Text = "有開分過需執行Jumbo手動洗分";
            }
            btnReturnAccount.Visible = true;
        }
        else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已洗分" && LoginGameId == "JumboRoom") //卡大廳
        {
            btnRecovery.Visible = true; //恢復會員狀態按鈕
        }
        else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已工具洗分" && LoginGameId == "JumboRoom") //卡大廳
        {
            btnRecovery.Visible = true; //恢復會員狀態按鈕
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        PageIni();
        GetUnReturnAccount_Jumbo_Club();
        PageProcess();
    }

    private void GetUnReturnAccount_Jumbo_Club() //取得會員狀態
    {
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetUnReturnAccount_Jumbo(txtClubEname.Text.Trim(), "Jumbo");
        if (dt.Rows.Count == 0)
        {
            lblQueryResult.Text = "會員帳號:" + txtClubEname.Text.Trim() + "不存在";
        }
        else
        {
            ClubStatus = dt.Select("")[0]["會員狀態"].ToString();
            SessionNoStatus = dt.Select("")[0]["開分狀態"].ToString();
            SessionNo = dt.Select("")[0]["開SessionNo"].ToString();
            ClubId = dt.Select("")[0]["Club_id"].ToString();
            LoginGameId = dt.Select("")[0]["Login_Game_Id"].ToString();
            int.TryParse(dt.Select("")[0]["JumboAccountCount"].ToString(), out JumboAccountCount);
            double.TryParse(dt.Select("")[0]["TC_Logout_Xinyong"].ToString(), out TC_Logout_Xinyong);
            double.TryParse(dt.Select("")[0]["Now_XinYong"].ToString(), out Now_XinYong);
            if (dt.Select("")[0]["開time"].ToString() != "")
            {
                SessionCreateTime = ((DateTime)dt.Select("")[0]["開time"]).ToString("yyyy-MM-dd HH:mm:sss");
            }
            lblClubStatus.Text = "會員狀態:" + ClubStatus;
            lblSessionNoStatus.Text = "開分狀態:" + SessionNoStatus;
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private void GetUnReturnAccount_JumboData(string ClubId, string SessionNo) //取得Jumbo下注資料
    {
        bsSQL bssql = new bsSQL();
        DataTable dt = bssql.GetUnReturnAccount_JumboData(ClubId, SessionNo);

        StartTime = ((DateTime)dt.Select("")[0]["StartTime"]).ToString("yyyy-MM-dd HH:mm:sss");
        EndTime = ((DateTime)dt.Select("")[0]["EndTime"]).ToString("yyyy-MM-dd HH:mm:sss");
        Location = dt.Select("")[0]["Location"].ToString();
        double.TryParse(dt.Select("")[0]["Amount"].ToString(), out Amount);
        double.TryParse(dt.Select("")[0]["GameSeqNo_Min"].ToString(), out GameSeqNo_Min);
        double.TryParse(dt.Select("")[0]["GameSeqNo_Max"].ToString(), out GameSeqNo_Max);
        double.TryParse(dt.Select("")[0]["TTLBet"].ToString(), out ttlBet);
        double.TryParse(dt.Select("")[0]["TTLWin"].ToString(), out ttlWin);
        double.TryParse(dt.Select("")[0]["TTLWinGame"].ToString(), out ttlWinGame);
        double.TryParse(dt.Select("")[0]["TTLWinGamble"].ToString(), out ttlWinGamble);
        double.TryParse(dt.Select("")[0]["TTLWinJackpot"].ToString(), out ttlWinJackpot);
        double.TryParse(dt.Select("")[0]["TTLPrepay"].ToString(), out ttlPrepay);
        double.TryParse(dt.Select("")[0]["TTLNetWin"].ToString(), out ttlNetWin);
        int.TryParse(dt.Select("")[0]["MNum"].ToString(), out mNum);
        int.TryParse(dt.Select("")[0]["MType"].ToString(), out mType);
        int.TryParse(dt.Select("")[0]["PlayCurrencyRate"].ToString(), out playCurrencyRate);

        GridView2.DataSource = dt;
        GridView2.DataBind();
    }

    protected void btnReturnAccount_Click(object sender, EventArgs e)
    {
        ReturnAccount();
    }

    protected void btnRecovery_Click(object sender, EventArgs e)
    {
        RecoveryClub();
    }

    protected void RecoveryClub() //恢復會員狀態
    {
        GetUnReturnAccount_Jumbo_Club(); //重新取會員資訊

        bsSQL bssql = new bsSQL();

        if (ClubStatus == "已開分" && SessionNoStatus == "已開分" && LoginGameId == "JumboGame") //卡遊戲
        {
            if (JumboAccountCount > 0) //有帳
            {
                lblJumboAccountCount.Text = "Jumbo帳務筆數:" + JumboAccountCount.ToString() + "筆";
                lblJumboAccountCountTxt.Text = "有電子帳需執行Jumbo手動洗分";
                GetUnReturnAccount_JumboData(ClubId, SessionNo);
            }
            else //沒帳
            {
                lblJumboAccountCountTxt.Text = "有開分過需執行Jumbo手動洗分";
            }
            btnReturnAccount.Visible = true;
        }
        else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已洗分" && LoginGameId == "JumboRoom") //卡大廳
        {
            DataTable dt = bssql.RecoveryClubJumbo(ClubId); //Jumbo SessionNo沒用
            if (dt.Rows.Count > 0)
            {
                lblDescription.Text = "結果 " + dt.Rows[0]["Description"].ToString();
                lblReturnValue.Text = "回傳代碼 " + dt.Rows[0]["ReturnValue"].ToString();
            }
            else
            {
                lblRecoveryAccount.Text = "調用恢復SP發生錯誤,無法進行會員狀態恢復";
            }
        }
        else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已工具洗分" && LoginGameId == "JumboRoom") //卡大廳
        {
            DataTable dt = bssql.RecoveryClubJumbo(ClubId); //Jumbo SessionNo沒用
            if (dt.Rows.Count > 0)
            {
                lblDescription.Text = "結果 " + dt.Rows[0]["Description"].ToString();
                lblReturnValue.Text = "回傳代碼 " + dt.Rows[0]["ReturnValue"].ToString();
            }
            else
            {
                lblRecoveryAccount.Text = "調用恢復SP發生錯誤,無法進行會員狀態恢復";
            }
        }
    }

    protected void ReturnAccount() //手動洗分
    {
        bsSQL bssql = new bsSQL();
        DataRow resultRow;
        DataTable dt;

        GetUnReturnAccount_Jumbo_Club(); //重新取會員資訊

        if (ClubStatus == "已開分" && SessionNoStatus == "已開分" && LoginGameId == "JumboGame") //卡遊戲
        {
            if (JumboAccountCount > 0) //有帳
            {
                lblJumboAccountCount.Text = "Jumbo帳務筆數:" + JumboAccountCount.ToString() + "筆";
                lblJumboAccountCountTxt.Text = "有電子帳需執行Jumbo手動洗分";
                GetUnReturnAccount_JumboData(ClubId, SessionNo);

                dt = bssql.ReturnAccount_Jumbo(ClubId, WebId, SessionNo, PassWord, Amount,
                                         GameSeqNo_Min,
                                         GameSeqNo_Max,
                                         mNum,
                                         mType,
                                         Location,
                                         playCurrencyRate,
                                         StartTime,
                                         EndTime,
                                         ttlBet,
                                         ttlWin,
                                         ttlWinGame,
                                         ttlWinGamble,
                                         ttlWinJackpot,
                                         ttlPrepay,
                                         ttlNetWin,
                                         JumboAccountCount);

            }
            else //沒帳
            {
                DateTime now = DateTime.Now;
                dt = bssql.ReturnAccount_Jumbo(ClubId, WebId, SessionNo, PassWord, TC_Logout_Xinyong,
                                          0,
                                          0,
                                          0,
                                          0,
                                          "",
                                          0,
                                          SessionCreateTime,
                                          now.ToString("yyyy-MM-dd HH:mm:ss"),
                                          0,
                                          0,
                                          0,
                                          0,
                                          0,
                                          0,
                                          0,
                                          0);
            }

            if (dt.Rows.Count > 0) //返回處理
            {
                resultRow = dt.Rows[0];

                if (JumboAccountCount>0 &&dt.Rows[0][0].ToString() == "1")  //有帳成功
                {
                    lblReturnAccount.Text = "結果:洗分成功";

                    //重取會員狀態
                    GetUnReturnAccount_Jumbo_Club();

                    //show恢復會員狀態按鈕
                    if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已洗分" && LoginGameId == "JumboRoom") //卡大廳
                    {
                        btnRecovery.Visible = true; //恢復會員狀態按鈕
                    }
                    else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已工具洗分" && LoginGameId == "JumboRoom") //卡大廳
                    {
                        btnRecovery.Visible = true; //恢復會員狀態按鈕
                    }
                }
                else if (JumboAccountCount == 0 && dt.Columns.Count > 1)  //沒帳成功
                {
                    lblReturnAccount.Text = "結果:洗分成功";

                    //重取會員狀態
                    GetUnReturnAccount_Jumbo_Club();

                    //show恢復會員狀態按鈕
                    if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已洗分" && LoginGameId == "JumboRoom") //卡大廳
                    {
                        btnRecovery.Visible = true; //恢復會員狀態按鈕
                    }
                    else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已工具洗分" && LoginGameId == "JumboRoom") //卡大廳
                    {
                        btnRecovery.Visible = true; //恢復會員狀態按鈕
                    }
                }
                else  //失敗
                {
                    lblReturnAccount.Text = "結果:洗分失敗 返回:" + resultRow["Status"].ToString();
                }
            }
            else
            {
                lblReturnAccount.Text = "調用Jumbo洗分SP發生錯誤,無法進行會員洗分";
            }

        }
        else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已洗分" && LoginGameId == "JumboRoom") //卡大廳
        {
            btnRecovery.Visible = true; //恢復會員狀態按鈕
        }
        else if (ClubStatus == "已登入/已洗分" && SessionNoStatus == "已工具洗分" && LoginGameId == "JumboRoom") //卡大廳
        {
            btnRecovery.Visible = true; //恢復會員狀態按鈕
        }
    }

}