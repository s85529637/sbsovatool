using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Text;

public partial class Club_Stake_Current_Insert_ForJDB : NewBasePage
{
    private readonly string _DayDbConnStr = ConfigurationManager.AppSettings["Main.ConnectionString"].ToStr();

    private void DataClear()
    {
        this.tbxBet.Text = "";
        this.tbxClub.Text = "";
        this.tbxGame_id.Text = "";
        this.tbxJDBSessionId.Text = "";
        this.tbxRows.Text = "";
        this.tbxSessionNo.Text = "";
        this.tbxNetWin.Text = "";
        this.tbxJackpot.Text = "";
        this.gvResult.DataSource = null;
        //this.gvResult.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.DataClear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (tbxClub.Text.ToStr().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblClub.Text + " 必填');", true);
            this.gvResult.Visible = false;
            return;
        }

        if (tbxJDBSessionId.Text.ToStr().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblJDBSessionId.Text + " 必填');", true);
            this.gvResult.Visible = false;
            return;
        }

        if (tbxGame_id.Text.ToStr().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblGame_id.Text + " 必填');", true);
            this.gvResult.Visible = false;
            return;
        }

        if (tbxBet.Text.ToStr().IsSingle() == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblBet.Text + " 必需是數字');", true);
            this.gvResult.Visible = false;
            return;
        }

        if (tbxJackpot.Text.ToStr().IsSingle() == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblJackpot.Text + " 必需是數字');", true);
            this.gvResult.Visible = false;
            return;
        }

        if (tbxNetWin.Text.ToStr().IsSingle() == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblNetWin.Text + " 必需是數字');", true);
            this.gvResult.Visible = false;
            return;
        }

        if (tbxRows.Text.ToStr().IsSingle() == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('" + lblRows.Text + " 必需是整數');", true);
            this.gvResult.Visible = false;
            return;
        }

        dbSQL objdbSql = new dbSQL();

        var value = string.Empty;

        StringBuilder Msg = new System.Text.StringBuilder("");
        Msg.Append("[InPut]");
        Msg.Append("tbxBet.Text=");
        Msg.Append(this.tbxBet.Text); 
        Msg.Append(",tbxClub.Text=");
        Msg.Append(this.tbxClub.Text);
        Msg.Append(",tbxGame_id.Text=");
        Msg.Append(this.tbxGame_id.Text);
        Msg.Append(",tbxJDBSessionId.Text=");
        Msg.Append(this.tbxJDBSessionId.Text);
        Msg.Append(",tbxRows.Text=");
        Msg.Append(this.tbxRows.Text);
        Msg.Append(",tbxSessionNo.Text=");
        Msg.Append(this.tbxSessionNo.Text);
        Msg.Append(",tbxNetWin.Text=");
        Msg.Append(this.tbxNetWin.Text); 
        Msg.Append(",tbxJackpot.Text=");
        Msg.Append(this.tbxJackpot.Text);
        DataTable dtb = objdbSql.A_ms_InsertStakeData_JDB(tbxClub.Text, tbxSessionNo.Text, tbxJDBSessionId.Text, tbxGame_id.Text, tbxBet.Text, tbxJackpot.Text, tbxNetWin.Text,int.Parse(tbxRows.Text.ToString()), out value);
        dtb.TableName = "Result";
        Msg.Append("[OutPut]");
        Msg.Append("value=");
        Msg.Append(value);

        // SP 執行不成功就不顯示資料
        if (value == "1")
        {
            this.gvResult.Visible = true;
            Msg.Append(",DataTableToXML=");
            Msg.Append(CDataToXml(dtb));
            Lib.WritLog("Club_Stake_Current_Insert_ForJDB.cs.btnSave_Click", Msg.ToString());
            this.gvResult.DataSource = dtb;
            this.gvResult.DataBind();
            this.DataClear();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), "alert('執行失敗，代碼：" + value + "');", true);
            Msg.Append(",執行失敗，代碼=");
            Msg.Append(value);
            Lib.WritLog("Club_Stake_Current_Insert_ForJDB.cs.btnSave_Click", Msg.ToString());
            this.gvResult.Visible = false;
            return;
        }
    }

    /// <summary>
    /// 將DataTable對象轉換成XML字符串
    /// </summary>
    /// <param name="dt">DataTable對象</param>
    /// <returns>XML字符串</returns>
    public  string CDataToXml(DataTable dt)
    {
        if (dt != null)
        {
            MemoryStream ms = null;
            XmlTextWriter XmlWt = null;
            try
            {
                ms = new MemoryStream();
                //根據ms實例化XmlWt
                XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                //獲取ds中的數據
                dt.WriteXml(XmlWt);
                int count = (int)ms.Length;
                byte[] temp = new byte[count];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(temp, 0, count);
                //返回Unicode編碼的文本
                UnicodeEncoding ucode = new UnicodeEncoding();
                string returnValue = ucode.GetString(temp).Trim();
                return returnValue;
            }catch (System.Exception ex){
                Lib.WritLog("Club_Stake_Current_Insert_ForJDB.cs.CDataToXml", ex.ToString());
                return ex.ToString();
            }finally{
                //釋放資源
                if (XmlWt != null)
                {
                    XmlWt.Close();
                    ms.Close();
                    ms.Dispose();
                }
            }
        }else{
            return "";
        }
    }

    protected void gvResult_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        long id = this.gvResult.DataKeys[e.RowIndex].Value.ToStr().ToInt64().GetValueOrDefault();

        int effect = 0;

        if (id > 0)
        {
            dbSQL objdbSql = new dbSQL();

            StringBuilder Msg = new System.Text.StringBuilder("");

            Msg.Append("[InPut]");

            Msg.Append("id=");

            Msg.Append(id.ToString());

            effect = objdbSql.A_ms_DeleteStakeData_JDB(id);

            Msg.Append("[OutPut]");

            Msg.Append("影響筆數=");

            Msg.Append(effect);

            Lib.WritLog("Club_Stake_Current_Insert_ForJDB.cs.gvResult_RowDeleting", Msg.ToString());

            if (effect > 0)
            {
                this.gvResult.Visible = false;
            }
        }
    }
}