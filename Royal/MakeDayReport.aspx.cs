using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

public partial class MakeDayReport : NewBasePage
{
    String LoginUser = ConfigurationManager.AppSettings["LoginUser"].ToString();
    bsSQL ssql = new bsSQL();
    String Message = "{0}.{1}:{2}:{3}{4}", TableData = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", 
        NewLine = "\r\n", Step = "", Start = "開始", Error = "錯誤", Success = "成功", Fail = "失敗",
        SumTitle = "總計(秒)", CountTitle = "總計", TimeFormat = "yyyy/MM/dd HH:mm:ss.fff";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Redirect("Logout.aspx");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (txtStartDate.Text.Equals(String.Empty))
        {
            txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        }
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        Response.Redirect("Logout.aspx");

        return;

        btnRun.Text = "結束";
        btnRun.Enabled = false;

        String _Password = ssql.GetSytemParameter(LoginUser);
        if (_Password == null)
        {
            Step = "無法取得密碼";
            txtMessage.Text += String.Format(Message, 0, Step, Error, DateTime.Now.ToString(TimeFormat), NewLine);
        }
        else if (_Password.Equals(txtSova.Text.Trim()))
        {
            runProcess();
        }
        else
        {
            Step = "輸入密碼錯誤";
            txtMessage.Text += String.Format(Message, 0, Step, Error, DateTime.Now.ToString(TimeFormat), NewLine);
        }
    }

    private void runProcess()
    {
        return;

        txtMessage.Text = "";

        int i = 0, iReport = 0, iCount = 0;
        String[] arrGameId = ConfigurationManager.AppSettings["VideoGame"].ToString().Split(',');
        bsSQL ssql = new bsSQL();
        DataTable dt = new DataTable();
        DateTime dtStart, dtEnd; TimeSpan ts;

        try
        {
            #region 總遊戲人數

            iReport++; Step = "總遊戲人數"; dtStart = DateTime.Now;
            //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            i = ssql.GameMembers();
            dtEnd = DateTime.Now;
            ts = dtEnd - dtStart;
            txtReport.Text += String.Format(TableData, Step, i.ToString(), "", "", "", "", NewLine);
            //txtReport.Text += String.Format(Message, iReport, Step , SumTitle, ts.TotalSeconds.ToString(), NewLine);

            #endregion

            #region 各遊戲人數
            for (int j = 0; j < arrGameId.Length; j++)
            {
                try
                {
                    iReport++; Step = "遊戲人數"; dtStart = DateTime.Now;
                    //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
                    i = ssql.GameMembers(arrGameId[j]);
                    dtEnd = DateTime.Now;
                    ts = dtEnd - dtStart;
                    txtReport.Text += String.Format(TableData, arrGameId[j], Step, i.ToString(), "", "", "", NewLine);
                    //txtReport.Text += String.Format(Message, iReport, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
                }
                catch (Exception ex)
                {
                    dtEnd = DateTime.Now;
                    txtReport.Text += String.Format(Message, iReport, Step, ex.Message, dtEnd.ToString(TimeFormat), NewLine);
                    ts = dtEnd - dtStart;
                    txtReport.Text += String.Format(Message, iReport, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
                }
            }
            #endregion

            #region 各注單數量
            txtReport.Text += NewLine;
            iReport++; Step = "各注單數量"; dtStart = DateTime.Now;
            //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.GetMaHaoListCount();
            dtEnd = DateTime.Now;
            if (dt != null && dt.Rows.Count > 0)
                txtReport.Text += String.Format(TableData, "結果", "遊戲", "注區", "注區數量", "輸贏", "實投量", NewLine);

            foreach (DataRow r in dt.Rows)
            {
                txtReport.Text += String.Format(TableData, r["TP"].ToString(), r["Game_id"].ToString(), r["MaHao"].ToString(),
                    r["iCount"].ToString(), r["Account_Score"].ToString(), r["YaMa"].ToString(), NewLine);
            }
            ts = dtEnd - dtStart;
            //txtReport.Text += String.Format(Message, iReport, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region 檢查轉帳
            iCount++; Step = "檢查轉帳"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            i = ssql.MoveAccountCheck();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, ((i == 0) ? Success : Fail), dtStart.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            if (i > 0)
            {
                String ErrorMessage = "";
                switch (i)
                {
                    case 1:
                        ErrorMessage = "檢查狀態為轉帳中";
                        break;
                    case 2:
                    case 3:
                        ErrorMessage = "非轉帳時間(正確為11：30-12：00)";
                        break;
                    case 4:
                    case 5:
                        ErrorMessage = "今日已轉過帳";
                        break;
                }
                txtMessage.Text += String.Format(Message, iCount, Step, ErrorMessage, "作業停止", NewLine);
                return;
            }
            #endregion

            #region 開始轉帳
            iCount++; Step = "開始轉帳"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            i = ssql.SetSytemParameter("ZhuanLiShi_Flag", "1");
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, ((i > 0) ? Success : Fail), dtStart.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);            
            #endregion

            #region 搬移開牌紀錄
            for (int j = 0; j < arrGameId.Length; j++)
            {
                try
                {
                    iCount++; Step = "搬移開牌紀錄-" + arrGameId[j]; dtStart = DateTime.Now;
                    txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
                    dt = ssql.MoveOpenList(arrGameId[j]);
                    dtEnd = DateTime.Now;
                    txtMessage.Text += String.Format(Message, iCount, Step,
                        ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                        dtEnd.ToString(TimeFormat), NewLine);
                    ts = dtEnd - dtStart;
                    txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
                }
                catch (Exception ex)
                {
                    dtEnd = DateTime.Now;
                    txtMessage.Text += String.Format(Message, iCount, Step, ex.Message, dtEnd.ToString(TimeFormat), NewLine);
                    ts = dtEnd - dtStart;
                    txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
                }
            }
            #endregion

            #region 刪除ID
            try
            {
                iCount++; Step = "刪除ID"; dtStart = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
                dt = ssql.DeleteId();
                dtEnd = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step,
                    ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                    dtEnd.ToString(TimeFormat), NewLine);
                ts = dtEnd - dtStart;
                txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step, ex.Message, dtEnd.ToString(TimeFormat), NewLine);
                ts = dtEnd - dtStart;
                txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            }
            #endregion

            #region 過帳初始化
            try
            {
                iCount++; Step = "過帳初始化"; dtStart = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
                dt = ssql.MoveAccountInit();
                dtEnd = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step,
                    ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                    dtEnd.ToString(TimeFormat), NewLine);
                ts = dtEnd - dtStart;
                txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            }
            catch (Exception ex)
            {
                dtEnd = DateTime.Now;
                txtMessage.Text += String.Format(Message, iCount, Step, ex.Message, dtEnd.ToString(TimeFormat), NewLine);
                ts = dtEnd - dtStart;
                txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            }
            #endregion

            #region 更新會員額度
            iCount++; Step = "更新會員額度"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.UpdateClubXinYong();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step,
                ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                dtEnd.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region 轉帳主檔
            iCount++; Step = "轉帳主檔"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.MoveAccountMain();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step,
                ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                dtEnd.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region 轉帳明細
            iCount++; Step = "轉帳明細"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.MoveToMonNetGameHJ2();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step,
                ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                dtEnd.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region 取得近三日注單總量
            txtReport.Text += NewLine;
            iReport++; Step = "取得近三日注單總量"; dtStart = DateTime.Now;
            //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.GetDailyStakeCount();
            dtEnd = DateTime.Now;
            if (dt != null && dt.Rows.Count > 0)
                txtReport.Text += String.Format(TableData, "ID", "報表日期", "最小ID", "最大ID", "注單數量", "新增時間", NewLine);

            foreach (DataRow r in dt.Rows)
            {
                txtReport.Text += String.Format(TableData, r["Id"].ToString(), r["Qishu_Name"].ToString(), r["MinID"].ToString(),
                    r["MaxID"].ToString(), r["RowsCount"].ToString(), r["ctime"].ToString(), NewLine);
            }
            ts = dtEnd - dtStart;
            //txtReport.Text += String.Format(Message, iReport, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion
        }
        catch (Exception ex)
        {
            dtEnd = DateTime.Now;
            txtReport.Text += String.Format(Message, iReport, Step, ex.Message, dtEnd.ToString(TimeFormat), NewLine);
            txtReport.Text += String.Format(Message, iReport, Step, ex.StackTrace, dtEnd.ToString(TimeFormat), NewLine);
            if (ex.InnerException != null)
                txtReport.Text += String.Format(Message, iReport, Step, ex.InnerException.Message, dtEnd.ToString(TimeFormat), NewLine);
        }
    }

}