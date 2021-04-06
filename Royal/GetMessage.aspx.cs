using System;
using System.Data;
using System.Web.UI;

public partial class GetMessage : NewBasePage
{
    bsSQL db = new bsSQL();

    protected void Page_Load(object sender, EventArgs e)
    {
        runProcess();
    }

    private void runProcess()
    {
        DataTable dtCNow = db.GetClubMessageNow();
        gvCNow.DataSource = dtCNow;
        gvCNow.DataBind();

        DataTable dtCOld = db.GetClubMessageOld();
        gvCOld.DataSource = dtCOld;
        gvCOld.DataBind();
    }

}