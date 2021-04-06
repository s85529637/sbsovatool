using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

public partial class MakeDayReport : NewBasePage
{
    String LoginUser = ConfigurationManager.AppSettings["LoginUser"].ToString();
    bsSQL ssql = new bsSQL();
    String Message = "{0}.{1}:{2}:{3}{4}", TableData = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", 
        NewLine = "\r\n", Step = "", Start = "�}�l", Error = "���~", Success = "���\", Fail = "����",
        SumTitle = "�`�p(��)", CountTitle = "�`�p", TimeFormat = "yyyy/MM/dd HH:mm:ss.fff";

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

        btnRun.Text = "����";
        btnRun.Enabled = false;

        String _Password = ssql.GetSytemParameter(LoginUser);
        if (_Password == null)
        {
            Step = "�L�k���o�K�X";
            txtMessage.Text += String.Format(Message, 0, Step, Error, DateTime.Now.ToString(TimeFormat), NewLine);
        }
        else if (_Password.Equals(txtSova.Text.Trim()))
        {
            runProcess();
        }
        else
        {
            Step = "��J�K�X���~";
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
            #region �`�C���H��

            iReport++; Step = "�`�C���H��"; dtStart = DateTime.Now;
            //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            i = ssql.GameMembers();
            dtEnd = DateTime.Now;
            ts = dtEnd - dtStart;
            txtReport.Text += String.Format(TableData, Step, i.ToString(), "", "", "", "", NewLine);
            //txtReport.Text += String.Format(Message, iReport, Step , SumTitle, ts.TotalSeconds.ToString(), NewLine);

            #endregion

            #region �U�C���H��
            for (int j = 0; j < arrGameId.Length; j++)
            {
                try
                {
                    iReport++; Step = "�C���H��"; dtStart = DateTime.Now;
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

            #region �U�`��ƶq
            txtReport.Text += NewLine;
            iReport++; Step = "�U�`��ƶq"; dtStart = DateTime.Now;
            //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.GetMaHaoListCount();
            dtEnd = DateTime.Now;
            if (dt != null && dt.Rows.Count > 0)
                txtReport.Text += String.Format(TableData, "���G", "�C��", "�`��", "�`�ϼƶq", "��Ĺ", "���q", NewLine);

            foreach (DataRow r in dt.Rows)
            {
                txtReport.Text += String.Format(TableData, r["TP"].ToString(), r["Game_id"].ToString(), r["MaHao"].ToString(),
                    r["iCount"].ToString(), r["Account_Score"].ToString(), r["YaMa"].ToString(), NewLine);
            }
            ts = dtEnd - dtStart;
            //txtReport.Text += String.Format(Message, iReport, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region �ˬd��b
            iCount++; Step = "�ˬd��b"; dtStart = DateTime.Now;
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
                        ErrorMessage = "�ˬd���A����b��";
                        break;
                    case 2:
                    case 3:
                        ErrorMessage = "�D��b�ɶ�(���T��11�G30-12�G00)";
                        break;
                    case 4:
                    case 5:
                        ErrorMessage = "����w��L�b";
                        break;
                }
                txtMessage.Text += String.Format(Message, iCount, Step, ErrorMessage, "�@�~����", NewLine);
                return;
            }
            #endregion

            #region �}�l��b
            iCount++; Step = "�}�l��b"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            i = ssql.SetSytemParameter("ZhuanLiShi_Flag", "1");
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, ((i > 0) ? Success : Fail), dtStart.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);            
            #endregion

            #region �h���}�P����
            for (int j = 0; j < arrGameId.Length; j++)
            {
                try
                {
                    iCount++; Step = "�h���}�P����-" + arrGameId[j]; dtStart = DateTime.Now;
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

            #region �R��ID
            try
            {
                iCount++; Step = "�R��ID"; dtStart = DateTime.Now;
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

            #region �L�b��l��
            try
            {
                iCount++; Step = "�L�b��l��"; dtStart = DateTime.Now;
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

            #region ��s�|���B��
            iCount++; Step = "��s�|���B��"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.UpdateClubXinYong();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step,
                ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                dtEnd.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region ��b�D��
            iCount++; Step = "��b�D��"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.MoveAccountMain();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step,
                ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                dtEnd.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region ��b����
            iCount++; Step = "��b����"; dtStart = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.MoveToMonNetGameHJ2();
            dtEnd = DateTime.Now;
            txtMessage.Text += String.Format(Message, iCount, Step,
                ((dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Equals("0")) ? Success : Fail),
                dtEnd.ToString(TimeFormat), NewLine);
            ts = dtEnd - dtStart;
            txtMessage.Text += String.Format(Message, iCount, Step, SumTitle, ts.TotalSeconds.ToString(), NewLine);
            #endregion

            #region ���o��T��`���`�q
            txtReport.Text += NewLine;
            iReport++; Step = "���o��T��`���`�q"; dtStart = DateTime.Now;
            //txtReport.Text += String.Format(Message, iReport, Step, Start, dtStart.ToString(TimeFormat), NewLine);
            dt = ssql.GetDailyStakeCount();
            dtEnd = DateTime.Now;
            if (dt != null && dt.Rows.Count > 0)
                txtReport.Text += String.Format(TableData, "ID", "������", "�̤pID", "�̤jID", "�`��ƶq", "�s�W�ɶ�", NewLine);

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