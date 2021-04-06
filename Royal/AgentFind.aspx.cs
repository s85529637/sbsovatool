using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AgentFind : NewBasePage
{
    private DataTable CreateRecordDataTable()
    {
        DataTable dtRecord = new DataTable();
        dtRecord.Columns.Add("帳號", typeof(string));
        dtRecord.Columns.Add("IP", typeof(string));
        dtRecord.Columns.Add("建立時間", typeof(string));
        dtRecord.Columns.Add("備註", typeof(string));
        return dtRecord;
    }

    private void GetData()
    {
        BackgroundRecord tmpResult = Tracklib.GetBackgroundRecord();

        DataTable Result = CreateRecordDataTable();

        if (tmpResult != null)
        {
            if (tmpResult.success)
            {
                if (tmpResult.data != null)
                {
                    if (tmpResult.data.Count > 0)
                    {
                        for (int j = 0; j < tmpResult.data.Count; j++)
                        {
                            Result.Rows.Add(tmpResult.data[j].name,
                                tmpResult.data[j].ip,
                                tmpResult.data[j].createTime,
                                tmpResult.data[j].note);
                        }

                        AgentListView.DataSource = Result;
                        AgentListView.DataBind();
                    }else { 
                        Lib.MsgBoxAndJump(this, "查無資料!!", "AgentFind.aspx");
                    }
                }
                else
                {
                    Lib.MsgBoxAndJump(this, "API回傳成功，但無資料!!", "AgentFind.aspx");
                }
            }
            else
            {
                if (tmpResult.message != null)
                {
                    Lib.MsgBoxAndJump(this, tmpResult.message, "AgentFind.aspx");
                }
                else
                {
                    Lib.MsgBoxAndJump(this, "API回傳失敗，無錯誤訊息!!", "AgentFind.aspx");
                }
            }
        }else { 
            Lib.MsgBoxAndJump(this, "Json物件為空!!", "AgentFind.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {}

    protected void btGetdata_Click(object sender, EventArgs e)
    {
        GetData();
    }
}