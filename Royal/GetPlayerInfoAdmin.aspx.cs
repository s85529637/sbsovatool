using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;

public partial class GetLockedPlayer : BasePage
{
    private void DataSearch()
    {
        bsSQL sql = new bsSQL();
        DataTable dt = sql.SovaGetPlayerInfo(txtAccount.Text.Trim());
        if (dt.Rows.Count >= 1)
        {
            dv.DataSource = dt;
            dv.DataBind();

            // 記錄會員代碼
            hdnClub_id.Value = dt.Rows[0]["Club_id"].ToStr();
            hdnSession.Value = dt.Rows[0]["SessionNo"].ToStr();
            hdLogin_Game_Id.Value = dt.Rows[0]["GameId"].ToStr();

            // TODO：尚有問題，暫不開放
            //// 恢復會員為遊戲中按鈕，處理電子遊戲 Room Room 又有開分的問題
            //if (dt.Rows[0]["Login_Game_Id"].ToStr() == "Room" && dt.Rows[0]["Login_Server_Id"].ToStr() == "Room" && dt.Rows[0]["IsEGame"].ToStr() == "是" && dt.Rows[0]["LockStatus"].ToStr() == "是")
            //{
            //    btnRecoveryLogin.Visible = true;
            //}
            //else
            //{
            //    btnRecoveryLogin.Visible = false;
            //}
            //平台選項
            dlplatform.Visible = true;
            //檢查帳務
            btChkHasAccount.Visible = true;
            // 強制解鎖按鈕
            btnPowerReturnAccount.Visible = true;
            // 強制解鎖按鈕
            btnPowerReturnAccount.Enabled = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.DataSearch();
    }

    /// <summary>
    /// 恢復會員為遊戲中，對方需能踢線
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRecoveryLogin_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('沒有會員資料');", true);
        }
        else
        {
            dbSQL sql = new dbSQL();
            SqlCommand cmd = new SqlCommand("dbo.C_RecoveryLoginForRoomRoom");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("CAC80563-9052-41C5-9616-0D16700AE919");
            cmd.Parameters.Add("@Club_id", SqlDbType.NChar, 20).Value = hdnClub_id.Value;
            var rows = sql.RunSQL(cmd);

            //
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('已將會員恢復為遊戲中');", true);
            this.DataSearch();
        }
    }

    /// <summary>
    /// 強制解鎖會員，對方需無帳
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPowerReturnAccount_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('沒有會員資料');", true);
        }
        else if (hdnSession.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('沒有開分資料');", true);
        }
        else
        {
            dbSQL sql = new dbSQL();
            SqlCommand cmd = new SqlCommand("dbo.C_PowerReturnAccount");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("F58B47EF-B7ED-486E-A06A-DE99EA3E91C5");
            cmd.Parameters.Add("@Club_id", SqlDbType.NChar, 20).Value = hdnClub_id.Value;
            cmd.Parameters.Add("@SessionNo", SqlDbType.NChar, 50).Value = hdnSession.Value;
            var rows = sql.RunSQL(cmd);

            // 強制解鎖按鈕
            btnPowerReturnAccount.Visible = false;

            //
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('已強制解鎖會員');", true);
            this.DataSearch();
        }
    }

    /// <summary>
    /// 依GameID回傳所屬遊戲廠商代號
    /// </summary>
    /// <param name="GameId"></param>
    /// <returns></returns>
    private DataTable Chkplatform(string GameId)
    {
        dbSQL sql = new dbSQL();
        DataTable dt = null;
        try
        {
            SqlCommand cmd = new SqlCommand("Select [ThirdParty_Id] from [T_Game] t with(nolock) where t.Game_id = @Game_id ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@Game_id", SqlDbType.NVarChar, 10).Value = hdLogin_Game_Id.Value;
            dt = sql.SelectSQL(cmd);
        }
        catch (Exception ex)
        {
            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.Chkplatform", ex.ToString());
        }
        return dt;
    }

    /// <summary>
    /// 依設定檔判斷皇家電子API的版本
    /// </summary>
    /// <param name="GameId"></param>
    /// <returns></returns>
    private string FindRoyalApiVersionByGameId(string GameId)
    {
        string rvalue = string.Empty;

        foreach (string keys in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
        {
            if (keys.StartsWith("RoyalGameId"))
            {
                if (System.Configuration.ConfigurationManager.AppSettings[keys].Trim() == GameId)
                {
                    rvalue = "1";
                    break;
                }
            }

            if (keys.StartsWith("RoyalGame2Id"))
            {
                if (keys.ToUpper().IndexOf(GameId.ToUpper().Trim()) > -1)
                {
                    rvalue = "2";
                    break;
                }
            }
        }

        return rvalue;
    }


    /// <summary>
    /// 調用皇電API(一代或二代)，檢查是否存在帳務
    /// </summary>
    /// <param name="PlayerId"></param>
    /// <param name="session_id"></param>
    /// <returns>True 表有帳務，False表無帳務</returns>
    private bool GetRoyalHasAccount(string Version, string SubSystem, string WebId, string PlayerId, string Session_id)
    {
        SlotGameWS.SlotGameWSSoapClient objRoyal1 = new SlotGameWS.SlotGameWSSoapClient();   //一代
        SlotGameWS2.SlotGameWSSoapClient objRoyal2 = new SlotGameWS2.SlotGameWSSoapClient(); //二代
        XmlNode objxml = null;
        XmlNode objflashxml = null;
        string SessionId = string.Empty;
        int DataCount = -999;
        int Royal2Status = 0;
        bool rvalue = false;
        bool isError = false;
        StringBuilder LogMsg = new StringBuilder("");
        string param = string.Format("傳入參數=Version:{0},SubSystem:{1},WebId:{2},PlayerId:{3},Session_id:{4}", Version, SubSystem, WebId, PlayerId, Session_id);
        LogMsg.Append(param);
        try
        {
            /*******************************************************/
            //測試區段
            /*string xmlContent = @"<root RowCount='1'> 
                                       <DataInfo>
                                           <RowId>1</RowId>
                                           <SequenNumber>1131085</SequenNumber>
                                           <SessionNo>20210113135854640</SessionNo>
                                           <PlayerId>2005293653</PlayerId>
                                           <WebId>Mobile</WebId>
                                           <GameID>48</GameID>
                                           <AccDenom>1.0000</AccDenom>
                                           <PlayDenom>0.0100</PlayDenom>
                                           <BetAmt>5.0000</BetAmt>
                                           <WinAmt>-5.0000</WinAmt>
                                           <JackpotAccumulateAmt>0.0000</JackpotAccumulateAmt>
                                           <GambleWinAmt>0.0000</GambleWinAmt>
                                           <JackpotWinAmt>0.0000</JackpotWinAmt>
                                           <PrepayAmt>0.0000</PrepayAmt>
                                           <BeforeCredit>49452.9400</BeforeCredit>
                                           <AfterCredit>49447.9400</AfterCredit>
                                           <CurrencyRate>1.1110</CurrencyRate>
                                           <PlayTime>2021-01-13 13:59:10</PlayTime>
                                           </DataInfo>
                                           <DataInfo>
                                           <RowId>2</RowId>
                                           <SequenNumber>1131086</SequenNumber>
                                           <SessionNo>20210113135854640</SessionNo>
                                           <PlayerId>2005293653</PlayerId>
                                           <WebId>Mobile</WebId>
                                           <GameID>48</GameID>
                                           <AccDenom>1.0000</AccDenom>
                                           <PlayDenom>0.0100</PlayDenom>
                                           <BetAmt>5.0000</BetAmt>
                                           <WinAmt>-5.0000</WinAmt>
                                           <JackpotAccumulateAmt>0.0000</JackpotAccumulateAmt>
                                           <GambleWinAmt>0.0000</GambleWinAmt>
                                           <JackpotWinAmt>0.0000</JackpotWinAmt>
                                           <PrepayAmt>0.0000</PrepayAmt>
                                           <BeforeCredit>49447.9400</BeforeCredit>
                                           <AfterCredit>49442.9400</AfterCredit>
                                           <CurrencyRate>1.1110</CurrencyRate>
                                           <PlayTime>2021-01-13 13:59:13</PlayTime>
                                        </DataInfo>
                                 </root> ";
           //xmlContent = @"<root RowCount='0'/> ";
           XmlDocument doc = new XmlDocument();
           doc.LoadXml(xmlContent);
           objxml = doc.DocumentElement;
           StringBuilder txtMsg = new StringBuilder("");
           Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount111",
                           objxml.InnerXml.ToString());
            return true;*/
            /*****************************************/

            if (!string.IsNullOrEmpty(Version))
            {
                if (Version == "1")
                {
                    try
                    {
                        objxml = objRoyal1.GetPlayerGameHistory(PlayerId, WebId, Session_id, 1, 10);

                        if (objxml != null)
                        {
                            if (objxml.ChildNodes.Count > 0)
                            {
                                LogMsg.Append("調用一代API結果︰");
                                LogMsg.Append(objxml.InnerXml.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("調用一代API發生例外︰{0}", ex.ToString()));
                    }
                }
                else if (Version == "2")
                {
                    try
                    {
                        objxml = objRoyal2.GetPlayerGameHistory(PlayerId, SubSystem, WebId, Session_id, 1, 10);

                        if (objxml != null)
                        {
                            if (objxml.ChildNodes.Count > 0)
                            {
                                LogMsg.Append("調用二代API結果︰");
                                LogMsg.Append(objxml.InnerXml.ToString());
                                Royal2Status = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("調用二代API發生例外︰{0}", ex.ToString()));
                        Royal2Status = 0;
                    }

                    try
                    {
                        if (Royal2Status == 0)
                        {
                            objflashxml = objRoyal2.GetPlayerFishGameMinuteSummery(PlayerId, SubSystem, WebId, Session_id, 1, 10);

                            if (objflashxml != null)
                            {
                                if (objflashxml.ChildNodes.Count > 0)
                                {
                                    LogMsg.Append("調用魚機API結果︰");
                                    LogMsg.Append(objflashxml.InnerXml.ToString());
                                    Royal2Status = 1;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("調用魚機API發生例外︰{0}", ex.ToString()));
                        Royal2Status = 0;
                    }
                }
                else
                {
                    Lib.MsgBox(this, "皇家API版本判別程序無法判別版本!!");
                    return true;
                }
            }
            else
            {
                Lib.MsgBox(this, "版本號為空!!");
                return true;
            }

            if (Version == "1")
            {
                if (objxml != null)
                {
                    if (objxml.ChildNodes.Count > 0)
                    {
                        SessionId = objxml.SelectNodes("//DataInfo/SessionNo")[0].InnerText;

                        if (SessionId != null)
                        {
                            rvalue = true;
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回傳開分號為NULL，請聯絡RD查看");
                            isError = true;
                        }
                    }
                    else
                    {
                        rvalue = false;
                    }
                }
                else
                {
                    Lib.MsgBox(this, "API回傳NULL，請聯絡RD查看");
                    isError = true;
                }
            }
            else if (Version == "2")
            {
                rvalue = false;

                if (objxml != null)
                {
                    if (objxml.ChildNodes.Count > 0)
                    {
                        SessionId = objxml.SelectNodes("//DataInfo/SessionNo")[0].InnerText;

                        if (SessionId != null)
                        {
                            rvalue = true;
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回傳開分號為NULL，請聯絡RD查看");
                            isError = true;
                        }
                    }
                }

                if (objflashxml != null)
                {
                    if (objflashxml.ChildNodes.Count > 0)
                    {
                        SessionId = objflashxml.SelectNodes("//DataInfo/DataCount")[0].InnerText;

                        if (SessionId != null)
                        {
                            if (!int.TryParse(SessionId, out DataCount))
                            {
                                Lib.MsgBox(this, "API回傳魚機筆數為非數字，請聯絡RD查看");

                                isError = true;
                            }

                            if (DataCount > 0)
                            {
                                rvalue = true;
                            }
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回傳魚機筆數為NULL，請聯絡RD查看");
                            isError = true;
                        }
                    }
                }

                if (objxml == null && objflashxml == null)
                {
                    Lib.MsgBox(this, "API回傳NULL，請聯絡RD查看");
                    isError = true;
                }
            }

        }
        catch (Exception ex)
        {
            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount", ex.ToString());
            Lib.MsgBox(this, "發生未知例外，請聯絡RD查看[" + ex.Message + "]");
            isError = true;
        }

        if (isError)  //如果有錯誤，則回傳True，迫使程式無法強制回復帳號狀態
        {
            rvalue = true;
        }

        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount", LogMsg.ToString());

        return rvalue;
    }

    /// <summary>
    /// 檢查帳務是否存在點擊事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btChkHasAccount_Click(object sender, EventArgs e)
    {
        //GetRoyalHasAccount("1", "h1", "mobile","1111", "2");

        DataTable dt = null;

        string ThirdParty_Id = string.Empty;

        string sVersion = string.Empty;

        if (dlplatform.SelectedValue == "Royal")
        {
            if (hdLogin_Game_Id.Value != null)
            {
                dt = Chkplatform(hdLogin_Game_Id.Value);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ThirdParty_Id = row["ThirdParty_Id"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ThirdParty_Id = ex.ToString();
                            Lib.MsgBox(this, "發生未知例外，請聯絡RD查看[" + ex.Message + "]");
                            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.btChkHasAccount_Click", ex.ToString());
                        }
                        if (!string.IsNullOrEmpty(ThirdParty_Id))
                        {
                            if (ThirdParty_Id.Trim() == "Royal")
                            {
                                sVersion = FindRoyalApiVersionByGameId(hdLogin_Game_Id.Value);

                                if (!string.IsNullOrEmpty(sVersion))
                                {
                                    if (!GetRoyalHasAccount(sVersion, "h1", "mobile", hdnClub_id.Value, hdnSession.Value))
                                    {
                                        // 強制解鎖按鈕
                                        btnPowerReturnAccount.Enabled = true;
                                    }
                                    else
                                    {
                                        Lib.MsgBox(this, "API回傳存在帳務或有異常情況發生，請聯絡RD查看");
                                    }
                                }
                                else
                                {
                                    Lib.MsgBox(this, "請檢查web.config中，GameId=[" + hdLogin_Game_Id.Value + "]是否存在!!");
                                }
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId所屬的平台不相符，查詢到的平台是︰" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "查無廠商資料!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "查核廠商資料時，發生未知例外!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id不存在!!");
            }
        }
        else if (dlplatform.SelectedValue == "JDB")
        {
            if (hdLogin_Game_Id.Value != null)
            {
                dt = Chkplatform(hdLogin_Game_Id.Value);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ThirdParty_Id = row["ThirdParty_Id"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ThirdParty_Id = ex.ToString();
                            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.btChkHasAccount_Click", ex.ToString());
                        }
                        if (!string.IsNullOrEmpty(ThirdParty_Id))
                        {
                            if (ThirdParty_Id.Trim() == "JDB")
                            {
                                // 強制解鎖按鈕
                                btnPowerReturnAccount.Enabled = true;
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId所屬的平台不相符，查詢到的平台是︰" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "查無廠商資料!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "查核廠商資料時，發生未知例外!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id不存在!!");
            }
        }
        else if (dlplatform.SelectedValue == "RTG")
        {
            if (hdLogin_Game_Id.Value != null)
            {
                dt = Chkplatform(hdLogin_Game_Id.Value);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ThirdParty_Id = row["ThirdParty_Id"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ThirdParty_Id = ex.ToString();
                            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.btChkHasAccount_Click", ex.ToString());
                        }
                        if (!string.IsNullOrEmpty(ThirdParty_Id))
                        {
                            if (ThirdParty_Id.Trim() == "RTG")
                            {
                                // 強制解鎖按鈕
                                btnPowerReturnAccount.Enabled = true;
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId所屬的平台不相符，查詢到的平台是︰" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "查無廠商資料!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "查核廠商資料時，發生未知例外!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id不存在!!");
            }
        }
        else if (dlplatform.SelectedValue == "GCLUB")
        {
            if (hdLogin_Game_Id.Value != null)
            {
                dt = Chkplatform(hdLogin_Game_Id.Value);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ThirdParty_Id = row["ThirdParty_Id"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            ThirdParty_Id = ex.ToString();
                            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.btChkHasAccount_Click", ex.ToString());
                        }
                        if (!string.IsNullOrEmpty(ThirdParty_Id))
                        {
                            if (ThirdParty_Id.Trim() == "GCLUB")
                            {
                                // 強制解鎖按鈕
                                btnPowerReturnAccount.Enabled = true;
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId所屬的平台不相符，查詢到的平台是︰" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "查無廠商資料!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "查核廠商資料時，發生未知例外!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id不存在!!");
            }
        }
    }

    protected void dlplatform_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPowerReturnAccount.Enabled = false;
    }
}