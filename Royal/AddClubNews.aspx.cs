using Dapper;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddClubNews : System.Web.UI.Page
{
    /// <summary>
    /// 給下拉式選單預設值
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string[] str = { "查詢新會員", "全部會員" };
            this.DropDownList1.DataSource = str;
            this.DropDownList1.DataBind();

            //下接選項綁定完資料,再用FindByText把要預設的值選出來 
            this.DropDownList1.Items.FindByText("查詢新會員").Selected = true;
        }
    }
    /// <summary>
    /// 給欄位預設時間
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (txtAccount.Text.Equals(String.Empty))
        {
            txtAccount.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        }

    }
    /// <summary>
    /// 產生EXCEl
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInquire_Click(object sender, EventArgs e)
    {
        try
        {
            using (FileStream file = new FileStream(Server.MapPath(@"~/Templete/NEWCLUB.xlsx"), FileMode.Open, FileAccess.Read))
            {

                //輸出檔名(我習慣加上日期時間)
                string fileName = "新會員明細_" + DateTime.Now.ToString();
                //開啟workbook檔案
                IWorkbook workbook = new XSSFWorkbook(file);
                //指定為第一個頁籤
                ISheet sheet = workbook.GetSheetAt(0);

                var lst = CreateExportData();
                if (lst.Count == 0)
                { ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('查無資料');", true); }
                else
                {
                    //從第二列到第三列，往下移動lst.Count列
                    sheet.ShiftRows(1, 2, lst.Count);

                    for (int i = 0; i < lst.Count; i++)
                    {
                        //5.依列逐格填入資料
                        var l = lst.ElementAt(i);
                        sheet.CreateRow(i + 1);
                        sheet.GetRow(i + 1).CreateCell(0).SetCellValue(l.Club_Ename);
                        sheet.GetRow(i + 1).CreateCell(1).SetCellValue(l.Franchiser_Ename);
                        sheet.GetRow(i + 1).CreateCell(2).SetCellValue(l.ZuBie);
                        sheet.GetRow(i + 1).CreateCell(3).SetCellValue(l.Game_id);
                        sheet.GetRow(i + 1).CreateCell(4).SetCellValue(l.Datetime.ToString("yyyy-MM-dd HH:mm:ss"));
                        sheet.GetRow(i + 1).CreateCell(5).SetCellValue(l.Quantity);
                        sheet.GetRow(i + 1).CreateCell(6).SetCellValue(l.YaMa);
                    }
                    //重新計算公式內的值
                    sheet.ForceFormulaRecalculation = true;
                    #region 輸出
                    MemoryStream ms = new MemoryStream();
                    workbook.Write(ms);

                    //設定檔名, IE 要特殊處理
                    if (HttpContext.Current.Request.Browser.Browser == "IE" || HttpContext.Current.Request.Browser.Browser == "InternetExplorer") fileName = HttpContext.Current.Server.UrlPathEncode(fileName);

                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Clear();

                    //此列針對xlsx錯誤做修正處理，若使用hssfworkbook則不需要此行
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    HttpContext.Current.Response.Cache.SetNoStore();
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileName + ".xlsx"));
                    HttpContext.Current.Response.BinaryWrite(ms.ToArray());

                    workbook = null;
                    ms.Close();
                    ms.Dispose();

                    HttpContext.Current.Response.End();
                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            var logger = BuildSerilog();
            logger.Error("錯誤內容:{System},{Application},{DB環境},{Error}", "Sovatool", "錯誤資訊", "舊公測", ex);
            ClientScript.RegisterStartupScript(GetType(), "hwa", "alert('有錯誤查詢LOG');", true);
        }
    }

    private void LOG(string ex)
    {
        string path = System.AppDomain.CurrentDomain.BaseDirectory + "/templete/";
        string filename = DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
        File.AppendAllText(path + filename, ex);
    }

    /// <summary>
    /// 抓BD資料
    /// </summary>
    /// <returns></returns>
    private List<Card> CreateExportData()
    {
        AppSettingsReader reader = new AppSettingsReader();
        string connString = reader.GetValue("Mon.ConnectionString", typeof(string)).ToString();
        string Find_DATE = txtAccount.Text.Trim();//輸入的時間
        string selectClub = DropDownList1.Text.Trim();//下拉式選單的資料
        string limit = " ";
        if (selectClub == "查詢新會員")
        {
            limit = "NEW";
        }
        else
        {
            limit = "ALL";
        }
        Find_DATE += " 12:00";
        List<Card> ListCard = null;
        //用sp_executesql 查詢 
        string sqlCommand = @"exec InquireClub @Find_DATE,@limit";
        //dapper
        using (var conn = new SqlConnection(connString))
        {
            ListCard = conn.Query<Card>(sqlCommand, new { Find_DATE = Find_DATE, limit = limit }).ToList();

        }
        return ListCard;
    }
    /// <summary>
    /// 物件
    /// </summary>
    public class Card
    {
        public string Club_Ename { get; set; }
        public string Franchiser_Ename { get; set; }
        public string ZuBie { get; set; }
        public string Game_id { get; set; }
        public DateTime Datetime { get; set; }

        public int Quantity { get; set; }
        public int YaMa { get; set; }
    }
    /// <summary>
    /// 抓SEQ.
    /// </summary>
    /// <returns></returns>
    private static Logger BuildSerilog()
    {
        var logger = new LoggerConfiguration()
                       .ReadFrom.AppSettings()
                       .CreateLogger();

        Log.Logger = logger;
        return logger;
    }



}
