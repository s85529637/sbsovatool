using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditGameDomainUrl : System.Web.UI.Page
{
    public static string Param = string.Empty;

    private DataTable CreateRecordDataTable()
    {
        DataTable dtRecord = new DataTable();
        dtRecord.Columns.Add("域名", typeof(string));
        dtRecord.Columns.Add("是否啟用", typeof(string));
        dtRecord.Columns.Add("域名類型", typeof(string));
        dtRecord.Columns.Add("建立時間", typeof(string));
        dtRecord.Columns.Add("異動人員", typeof(string));
        dtRecord.Columns.Add("異動時間", typeof(string));
        dtRecord.Columns.Add("備註", typeof(string));
        return dtRecord;
    }

    protected void Page_Load(object sender, EventArgs e){
        if (!Page.IsPostBack)
        {
            if (Request["WebSite"] != null && Request["System"] != null && Request["IsLive"] != null)
            {
                LoadData(false);
            } 
        }
    }

    private void LoadData(bool isClick)
    {
        DataTable rTable = CreateRecordDataTable();
        if (Request["WebSite"] != null && Request["System"] != null && Request["IsLive"] != null && isClick == false)
        {
            for (int j = 0; j < UrlType.Items.Count; j++)
            {
                if (UrlType.Items[j].Value.ToString().ToUpper() == Request["WebSite"].ToString().ToUpper())
                {
                    UrlType.Items[j].Selected = true;
                }
                else
                {
                    UrlType.Items[j].Selected = false;
                }
            }
            for (int j = 0; j < SysType.Items.Count; j++)
            {
                if (SysType.Items[j].Value.ToString().ToUpper() == Request["System"].ToString().ToUpper())
                {
                    SysType.Items[j].Selected = true;
                }
                else
                {
                    SysType.Items[j].Selected = false;
                }
            }
            for (int j = 0; j < LiveStatus.Items.Count; j++)
            {
                if (LiveStatus.Items[j].Value.ToString().ToUpper() == Request["IsLive"].ToString().ToUpper())
                {
                    LiveStatus.Items[j].Selected = true;
                }
                else
                {
                    LiveStatus.Items[j].Selected = false;
                }
            }
        }
        string WebSite = UrlType.SelectedValue;
        string System = SysType.SelectedValue;
        string IsLive = LiveStatus.SelectedValue;
        if (WebSite != "" && System != "" && IsLive != "" )
        {
            Param = string.Format("EditGameDomainUrl.aspx?WebSite={0}&System={1}&IsLive={2}", WebSite, System, IsLive);
        }
        string Domain = string.Empty;
        string dIsLive = string.Empty;
        string dWebSite = string.Empty;
        string CreateTime = string.Empty;
        string ModifyUser = string.Empty;
        string ModityTime = string.Empty;
        string Note = string.Empty;
        GetGameDomainUrlList Result = CDNlib.GetGameDomainUrlList(WebSite, System, IsLive);
        try
        {
            if (Result != null)
            {
                if (Result.success)
                {
                    if (Result.data != null)
                    {
                        if (Result.data.Count > 0)
                        {
                            for (int j = 0; j < Result.data.Count; j++)
                            {
                                Domain = Result.data[j].domain.ToString();
                                dIsLive = Result.data[j].isLive.ToString();
                                dWebSite = Result.data[j].webSite.ToString();
                                CreateTime = Result.data[j].createTime.ToString();
                                ModifyUser = "*****";
                                ModityTime = Result.data[j].modifyTime.ToString();
                                Note = Result.data[j].note.ToString();
                                rTable.Rows.Add(
                                        Domain,
                                        dIsLive,
                                        dWebSite,
                                        CreateTime,
                                        ModifyUser,
                                        ModityTime,
                                        Note);
                            }
                            gvDomainUrlList.DataSource = rTable;
                            gvDomainUrlList.DataBind();
                        }
                        else
                        {
                            Lib.MsgBox(this, "API回應成功，但資料數為0");
                        }
                    }
                    else
                    {
                        Lib.MsgBox(this, "API回應成功，但DATA物件為NULL");
                    }
                }
                else
                {
                    if (Result.message != null)
                    {
                        gvDomainUrlList.DataSource = null;
                        Lib.MsgBox(this, Result.message);
                    }
                    else
                    {
                        Lib.MsgBox(this, "API回應的訊息為NULL");
                    }
                }
            }
            else
            {
                Lib.MsgBox(this, "API回應的JSON物件為NULL");
            }
        }
        catch (Exception ex)
        { 
            Lib.WritLog("EditGameDomainUrl.aspx.cs", ex.ToString());
            Lib.MsgBox(this, ex.Message);
        }
    }

    protected void btGetData_Click(object sender, EventArgs e)
    {
        LoadData(true);
    }

    protected void gvDomainUrlList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)e.CommandSource;
        int iIndex = int.Parse(e.CommandArgument.ToString());
        GridViewRow row = gv.Rows[iIndex];
        string Domain = row.Cells[2].Text.Trim();
        string dIsLive = row.Cells[3].Text.Trim();
        string dWebSite = row.Cells[4].Text.Trim();
        string CreateTime = row.Cells[5].Text.Trim();
        string ModifyUser = row.Cells[6].Text.Trim();
        string ModityTime = row.Cells[7].Text.Trim();
        string Note = row.Cells[8].Text.Trim();
        
        if ("Del".Equals(e.CommandName))
        {
            CDelGameUrlResult Result = null;
            try
            {
                 Result = CDNlib.DelGameUrl(Domain, dWebSite);

                if (Result != null)
                {
                    if (Result.message != null)
                    {
                        Lib.MsgBoxAndJump(Page, Result.message, Param);
                    }
                    else
                    {
                        Lib.MsgBoxAndJump(Page, "API回應訊息為NULL", Param);
                    }
                }else {  
                    Lib.MsgBoxAndJump(Page, "API回應Json物件為NULL", Param);
                }
            }
            catch (Exception ex)
            {
                Lib.AlertWritLog("EditGameDomainUrl.aspx.cs.gvDomainUrlList_RowCommand", ex.ToString());
                Lib.MsgBoxAndJump(Page,ex.Message, Param);
            }
        }
        else if ("Upt".Equals(e.CommandName))
        {
            lbdomain.Text = Domain;
            lbwebSite.Text = dWebSite;

            for (int j = 0; j < dlisLive.Items.Count; j++)
            {
                if (dlisLive.Items[j].Value.ToString().ToLower() == dIsLive.ToString().ToLower())
                {
                    dlisLive.Items[j].Selected = true;
                }else {
                    dlisLive.Items[j].Selected = false;
                }
            }
            lbmodifyUser.Text = ModifyUser;
            lbmodifyTime.Text = ModityTime;
            tbNoet.Text = Note.Replace("&nbsp;","");
        }
    }

    /// <summary>
    /// 修改資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btUpdate_Click(object sender, EventArgs e)
    {
        string Domain = lbdomain.Text.Trim();
        string dIsLive = dlisLive.SelectedValue;
        string dWebSite = lbwebSite.Text.Trim();
        string ModifyUser = lbmodifyUser.Text.Trim();
        string ModityTime = lbmodifyTime.Text.Trim();
        string Note = tbNoet.Text.Trim();

        if (Domain == "" || dWebSite == "" || ModifyUser == "" || ModityTime == "")
        {
            Lib.MsgBoxAndJump(Page, "修改失敗，參數為空", Param);
            return;
        }

        if (Domain != "")
        {
            Domain = Domain.Trim();
        }

        CUpdateGameUrlResult objResult = CDNlib.UpdateGameUrl(Domain, dWebSite,dIsLive, ModifyUser, Note);

        if (objResult != null)
        {
            if (objResult.message != null)
            {
                Lib.MsgBoxAndJump(Page, objResult.message, Param);
            }
            else
            {
                Lib.MsgBoxAndJump(Page, "API回應成功，但Message為NULL", Param);
            }
        }else {
            Lib.MsgBoxAndJump(Page, "API回應Json物件為NULL", Param);
        }
    }

    /// <summary>
    /// 使用正則式檢驗網址
    /// </summary>
    /// <param name="strUrl"></param>
    /// <returns></returns>
    private bool chkUrl(string strUrl)
    {
        bool rvalue = false;
        Regex rgx = new Regex(@"(http://|https://)[\w-_@]{1,}[.]{1}[\w-_@]{1,}[.]{1,}[\w-_@]{1,}[.]?[\w]{1,}([/]?[\w-_]{0,})*[.]?[\w-_]{0,}");
        if (rgx.IsMatch(strUrl))
        {
            rvalue = true;
        }
        return rvalue;
    }

    /// <summary>
    /// 新增資料
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btAdd_Click(object sender, EventArgs e)
    {
        string Domain = string.IsNullOrEmpty(txtWebUrl.Text) ? "" : txtWebUrl.Text;
        string WebSite = string.IsNullOrEmpty(dtUrlType.Text) ? "" : dtUrlType.Text;
        string ModifyUser = "******";
        string Note = string.IsNullOrEmpty(txtNote.Text) ? "" : txtNote.Text;
        if (Domain != "")
        {
            Domain = Domain.Trim();
        }
        if (!chkUrl(Domain))
        {
            Lib.MsgBoxAndJump(Page, "域名格式錯誤，請重新輸入", Param);
        }else {

            CAddGameUrlResult objResult = CDNlib.AddGameUrl(Domain, WebSite, ModifyUser, Note);
            if (objResult != null)
            {
                if (objResult.message != null)
                {
                    Lib.MsgBoxAndJump(Page, objResult.message, Param);
                }
                else
                {
                    Lib.MsgBoxAndJump(Page, "API回應成功，但Message為NULL", Param);
                }
            }
        }
    }
}