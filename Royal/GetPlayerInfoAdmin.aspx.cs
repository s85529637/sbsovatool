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

            // �O���|���N�X
            hdnClub_id.Value = dt.Rows[0]["Club_id"].ToStr();
            hdnSession.Value = dt.Rows[0]["SessionNo"].ToStr();
            hdLogin_Game_Id.Value = dt.Rows[0]["GameId"].ToStr();

            // TODO�G�|�����D�A�Ȥ��}��
            //// ��_�|�����C�������s�A�B�z�q�l�C�� Room Room �S���}�������D
            //if (dt.Rows[0]["Login_Game_Id"].ToStr() == "Room" && dt.Rows[0]["Login_Server_Id"].ToStr() == "Room" && dt.Rows[0]["IsEGame"].ToStr() == "�O" && dt.Rows[0]["LockStatus"].ToStr() == "�O")
            //{
            //    btnRecoveryLogin.Visible = true;
            //}
            //else
            //{
            //    btnRecoveryLogin.Visible = false;
            //}
            //���x�ﶵ
            dlplatform.Visible = true;
            //�ˬd�b��
            btChkHasAccount.Visible = true;
            // �j�������s
            btnPowerReturnAccount.Visible = true;
            // �j�������s
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
    /// ��_�|�����C�����A���ݯ��u
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRecoveryLogin_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�S���|�����');", true);
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
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�w�N�|����_���C����');", true);
            this.DataSearch();
        }
    }

    /// <summary>
    /// �j�����|���A���ݵL�b
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPowerReturnAccount_Click(object sender, EventArgs e)
    {
        if (hdnClub_id.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�S���|�����');", true);
        }
        else if (hdnSession.Value.IsBlank())
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�S���}�����');", true);
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

            // �j�������s
            btnPowerReturnAccount.Visible = false;

            //
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('�w�j�����|��');", true);
            this.DataSearch();
        }
    }

    /// <summary>
    /// ��GameID�^�ǩ��ݹC���t�ӥN��
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
    /// �̳]�w�ɧP�_�Ӯa�q�lAPI������
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
    /// �եάӹqAPI(�@�N�ΤG�N)�A�ˬd�O�_�s�b�b��
    /// </summary>
    /// <param name="PlayerId"></param>
    /// <param name="session_id"></param>
    /// <returns>True ���b�ȡAFalse��L�b��</returns>
    private bool GetRoyalHasAccount(string Version, string SubSystem, string WebId, string PlayerId, string Session_id)
    {
        SlotGameWS.SlotGameWSSoapClient objRoyal1 = new SlotGameWS.SlotGameWSSoapClient();   //�@�N
        SlotGameWS2.SlotGameWSSoapClient objRoyal2 = new SlotGameWS2.SlotGameWSSoapClient(); //�G�N
        XmlNode objxml = null;
        XmlNode objflashxml = null;
        string SessionId = string.Empty;
        int DataCount = -999;
        int Royal2Status = 0;
        bool rvalue = false;
        bool isError = false;
        StringBuilder LogMsg = new StringBuilder("");
        string param = string.Format("�ǤJ�Ѽ�=Version:{0},SubSystem:{1},WebId:{2},PlayerId:{3},Session_id:{4}", Version, SubSystem, WebId, PlayerId, Session_id);
        LogMsg.Append(param);
        try
        {
            /*******************************************************/
            //���հϬq
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
                                LogMsg.Append("�եΤ@�NAPI���G�J");
                                LogMsg.Append(objxml.InnerXml.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("�եΤ@�NAPI�o�ͨҥ~�J{0}", ex.ToString()));
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
                                LogMsg.Append("�եΤG�NAPI���G�J");
                                LogMsg.Append(objxml.InnerXml.ToString());
                                Royal2Status = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("�եΤG�NAPI�o�ͨҥ~�J{0}", ex.ToString()));
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
                                    LogMsg.Append("�եγ���API���G�J");
                                    LogMsg.Append(objflashxml.InnerXml.ToString());
                                    Royal2Status = 1;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount",
                            string.Format("�եγ���API�o�ͨҥ~�J{0}", ex.ToString()));
                        Royal2Status = 0;
                    }
                }
                else
                {
                    Lib.MsgBox(this, "�ӮaAPI�����P�O�{�ǵL�k�P�O����!!");
                    return true;
                }
            }
            else
            {
                Lib.MsgBox(this, "����������!!");
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
                            Lib.MsgBox(this, "API�^�Ƕ}������NULL�A���p��RD�d��");
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
                    Lib.MsgBox(this, "API�^��NULL�A���p��RD�d��");
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
                            Lib.MsgBox(this, "API�^�Ƕ}������NULL�A���p��RD�d��");
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
                                Lib.MsgBox(this, "API�^�ǳ������Ƭ��D�Ʀr�A���p��RD�d��");

                                isError = true;
                            }

                            if (DataCount > 0)
                            {
                                rvalue = true;
                            }
                        }
                        else
                        {
                            Lib.MsgBox(this, "API�^�ǳ������Ƭ�NULL�A���p��RD�d��");
                            isError = true;
                        }
                    }
                }

                if (objxml == null && objflashxml == null)
                {
                    Lib.MsgBox(this, "API�^��NULL�A���p��RD�d��");
                    isError = true;
                }
            }

        }
        catch (Exception ex)
        {
            Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount", ex.ToString());
            Lib.MsgBox(this, "�o�ͥ����ҥ~�A���p��RD�d��[" + ex.Message + "]");
            isError = true;
        }

        if (isError)  //�p�G�����~�A�h�^��True�A���ϵ{���L�k�j��^�_�b�����A
        {
            rvalue = true;
        }

        Lib.WritLog("GetPlayerInfoAdmin.aspx.cs.GetRoyalHasAccount", LogMsg.ToString());

        return rvalue;
    }

    /// <summary>
    /// �ˬd�b�ȬO�_�s�b�I���ƥ�
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
                            Lib.MsgBox(this, "�o�ͥ����ҥ~�A���p��RD�d��[" + ex.Message + "]");
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
                                        // �j�������s
                                        btnPowerReturnAccount.Enabled = true;
                                    }
                                    else
                                    {
                                        Lib.MsgBox(this, "API�^�Ǧs�b�b�ȩΦ����`���p�o�͡A���p��RD�d��");
                                    }
                                }
                                else
                                {
                                    Lib.MsgBox(this, "���ˬdweb.config���AGameId=[" + hdLogin_Game_Id.Value + "]�O�_�s�b!!");
                                }
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId���ݪ����x���۲šA�d�ߨ쪺���x�O�J" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "�d�L�t�Ӹ��!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "�d�ּt�Ӹ�ƮɡA�o�ͥ����ҥ~!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id���s�b!!");
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
                                // �j�������s
                                btnPowerReturnAccount.Enabled = true;
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId���ݪ����x���۲šA�d�ߨ쪺���x�O�J" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "�d�L�t�Ӹ��!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "�d�ּt�Ӹ�ƮɡA�o�ͥ����ҥ~!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id���s�b!!");
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
                                // �j�������s
                                btnPowerReturnAccount.Enabled = true;
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId���ݪ����x���۲šA�d�ߨ쪺���x�O�J" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "�d�L�t�Ӹ��!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "�d�ּt�Ӹ�ƮɡA�o�ͥ����ҥ~!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id���s�b!!");
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
                                // �j�������s
                                btnPowerReturnAccount.Enabled = true;
                            }
                            else
                            {
                                Lib.MsgBox(this, "GameId���ݪ����x���۲šA�d�ߨ쪺���x�O�J" + ThirdParty_Id.Trim());
                            }
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "�d�L�t�Ӹ��!!");
                    }
                }
                else
                {
                    Lib.MsgBox(this, "�d�ּt�Ӹ�ƮɡA�o�ͥ����ҥ~!!");
                }
            }
            else
            {
                Lib.MsgBox(this, "Game_Id���s�b!!");
            }
        }
    }

    protected void dlplatform_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPowerReturnAccount.Enabled = false;
    }
}