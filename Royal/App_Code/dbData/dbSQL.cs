using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

public class dbSQL : dbConnectionBase
{
    #region 初始化
    String ConnectionString = "Main.ConnectionString";

    public dbSQL()
    {
        base.InitClass(ConnectionString);
    }

    public dbSQL(string DBConnectionName)
    {
        base.InitClass(DBConnectionName);
    }

    #endregion

    #region Sova
    public DataTable SovaGetPlayerInfo(string Club_Ename)
    {
        DataTable dt;
        SqlCommand cmd = new SqlCommand();
        StringBuilder sql = new StringBuilder();
        sql.AppendLine("    SELECT TOP 1 t.*");
        sql.AppendLine("         , CASE t.Login_EGame WHEN 1 THEN '是' ELSE '否' END IsEGame");
        sql.AppendLine("         , CASE l.Status WHEN 0 THEN '是' ELSE '否' END LockStatus");
        sql.AppendLine("         , l.SessionNo");
        sql.AppendLine("         , l.GameId");
        sql.AppendLine("      FROM T_Club AS t WITH (nolock)");
        sql.AppendLine(" LEFT JOIN T_Session_Lock l WITH (nolock)");
        sql.AppendLine("        ON t.Club_id = l.Club_id");
        sql.AppendLine("       AND l.Status = 0");
        sql.AppendLine("     WHERE (t.Club_id = @Club_Ename OR t.Club_Ename = @Club_Ename)");  //增加可以用club_id 搜尋
        cmd.CommandText = sql.ToString();
        cmd.Parameters.AddWithValue("@Club_Ename", Club_Ename);
        DataTable dataTable = new DataTable("T_Club");
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        cmd.Connection = base._mainConnection;
        try
        {
            if (base._mainConnectionIsCreatedLocal)
            {
                base._mainConnection.Open();
            }
            else if (base._mainConnectionProvider.IsTransactionPending)
            {
                cmd.Transaction = base._mainConnectionProvider.CurrentTransaction;
            }
            adapter.Fill(dataTable);
            dt = dataTable;
        }
        catch (Exception exception)
        {
            throw new Exception("db::SovaGetPlayerInfo::Error occured.", exception);
        }
        finally
        {
            if (base._mainConnectionIsCreatedLocal)
            {
                base._mainConnection.Close();
            }
            cmd.Dispose();
            adapter.Dispose();
        }
        return dt;
    }
    #endregion

    #region Star
    /// <summary>
    /// Star 補注單(查詢)
    /// </summary>
    /// <param name="id">資料表的 PK</param>
    /// <param name="club_id">會員ID</param>
    /// <param name="startSeqNoFlag">SessionID</param>
    /// <param name="sReturnStateDescription">執行結果簡易說明</param>
    /// <returns></returns>
    public DataSet A_ms_InsertStakeData_ForStar(long? id, string club_id, decimal? startSeqNoFlag, out string sReturnStateDescription)
    {
        using (SqlConnection conn = this._mainConnection)
        {
            using (SqlCommand cmd = new SqlCommand("dbo.A_ms_InsertStakeData_ForStar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("86AD8AB1-5AE6-478C-AEF8-626863F8AA26");
                cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
                cmd.Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = club_id;
                cmd.Parameters.Add("@StartSeqNoFlag", SqlDbType.Decimal).Value = startSeqNoFlag;
                cmd.Parameters.Add("@Mode", SqlDbType.TinyInt).Value = 1;

                // Output 參數
                int iReturnState = 0;
                cmd.Parameters.Add(new SqlParameter("@ReturnState", iReturnState)
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                });

                // Output 參數
                sReturnStateDescription = "";
                cmd.Parameters.Add(new SqlParameter("@ReturnStateDescription", sReturnStateDescription)
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Output,
                });

                //
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet("root");
                    da.Fill(ds);
                    ds.Tables[0].TableName = "T_Club_Stake_Current";
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[1].TableName = "T_Club_Stake_History";
                    }
                    base._errorCode = iReturnState = (cmd.Parameters["@ReturnState"].Value as int?).GetValueOrDefault();
                    sReturnStateDescription = cmd.Parameters["@ReturnStateDescription"].Value + string.Empty;
                    return ds;
                }
            }
        }
    }

    /// <summary>
    /// Star 補注單(寫入)
    /// </summary>
    /// <param name="club_id">會員ID</param>
    /// <param name="startSeqNoFlag">SessionID</param>
    /// <param name="game_id">遊戲ID，例如：Bacc</param>
    /// <param name="desk">桌別，例如：M</param>
    /// <param name="stake_Score">下注金額(Star 提供的 TotalBet)</param>
    /// <param name="youXiaoYaFen">有效押分(實投量)(Star 提供的 Available)</param>
    /// <param name="account_Score">輸贏金額(Star 提供的 TotalWin)</param>
    /// <param name="rows">明細筆數</param>
    /// <param name="sReturnStateDescription">執行結果簡易說明</param>
    /// <returns></returns>
    public DataSet A_ms_InsertStakeData_ForStar(string club_id, decimal? startSeqNoFlag, string game_id, string desk, decimal? stake_Score, decimal? youXiaoYaFen, decimal? account_Score, int? rows, out string sReturnStateDescription)
    {
        using (SqlConnection conn = this._mainConnection)
        {
            using (SqlCommand cmd = new SqlCommand("dbo.A_ms_InsertStakeData_ForStar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("86AD8AB1-5AE6-478C-AEF8-626863F8AA26");
                cmd.Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = club_id;
                cmd.Parameters.Add("@StartSeqNoFlag", SqlDbType.Decimal).Value = startSeqNoFlag;
                cmd.Parameters.Add("@Game_id", SqlDbType.NVarChar).Value = game_id;
                cmd.Parameters.Add("@Desk", SqlDbType.NVarChar).Value = desk;
                cmd.Parameters.Add("@Stake_Score", SqlDbType.Money).Value = stake_Score;
                cmd.Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = youXiaoYaFen;
                cmd.Parameters.Add("@Account_Score", SqlDbType.Money).Value = account_Score;
                cmd.Parameters.Add("@Rows", SqlDbType.Int).Value = rows;
                cmd.Parameters.Add("@Mode", SqlDbType.TinyInt).Value = 2;

                // Output 參數
                int iReturnState = 0;
                cmd.Parameters.Add(new SqlParameter("@ReturnState", iReturnState)
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                });

                // Output 參數
                sReturnStateDescription = "";
                cmd.Parameters.Add(new SqlParameter("@ReturnStateDescription", sReturnStateDescription)
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Output,
                });

                //
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet("root");
                    da.Fill(ds);
                    ds.Tables[0].TableName = "T_Club_Stake_Current";
                    ds.Tables[1].TableName = "T_Club_Stake_History";
                    //ds.Tables[2].TableName = "ReturnState";
                    base._errorCode = iReturnState = (cmd.Parameters["@ReturnState"].Value as int?).GetValueOrDefault();
                    sReturnStateDescription = cmd.Parameters["@ReturnStateDescription"].Value + string.Empty;
                    return ds;
                }
            }
        }
    }

    /// <summary>
    /// Star 修改注單(查詢)
    /// </summary>
    /// <param name="id">資料表的 PK</param>
    /// <param name="club_id">會員ID</param>
    /// <param name="startSeqNoFlag">SessionID</param>
    /// <returns></returns>
    public DataSet A_ms_UpdateStakeData_ForStar(long? id, string club_id, decimal? startSeqNoFlag)
    {
        using (SqlConnection conn = this._mainConnection)
        {
            using (SqlCommand cmd = new SqlCommand("dbo.A_ms_UpdateStakeData_ForStar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("86AD8AB1-5AE6-478C-AEF8-626863F8AA26");
                cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                cmd.Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = club_id;
                cmd.Parameters.Add("@StartSeqNoFlag", SqlDbType.Decimal).Value = startSeqNoFlag;
                cmd.Parameters.Add("@Mode", SqlDbType.TinyInt).Value = 1;

                //
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet("root");
                    da.Fill(ds);
                    return ds;
                }
            }
        }
    }

    /// <summary>
    /// Star 修改注單(更新)
    /// </summary>
    /// <param name="id">資料表的 PK</param>
    /// <param name="club_id">會員ID</param>
    /// <param name="startSeqNoFlag">SessionID</param>
    /// <param name="stake_Score">下注金額(Star 提供的 TotalBet)</param>
    /// <param name="youXiaoYaFen">有效押分(實投量)(Star 提供的 Available)</param>
    /// <param name="account_Score">輸贏金額(Star 提供的 TotalWin)</param>
    /// <param name="shengDian">勝點，如果遊戲輸嬴改完為 > 0 勝點也要變更, 輸錢時就設為 0</param>
    /// <param name="rows">明細筆數</param>
    /// <returns></returns>
    public DataSet A_ms_UpdateStakeData_ForStar(long? id, string club_id, decimal? startSeqNoFlag, decimal? stake_Score, decimal? youXiaoYaFen, decimal? account_Score, decimal? shengDian, int? rows)
    {
        using (SqlConnection conn = this._mainConnection)
        {
            using (SqlCommand cmd = new SqlCommand("dbo.A_ms_UpdateStakeData_ForStar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("86AD8AB1-5AE6-478C-AEF8-626863F8AA26");
                cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                cmd.Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = club_id;
                cmd.Parameters.Add("@StartSeqNoFlag", SqlDbType.Decimal).Value = startSeqNoFlag;
                cmd.Parameters.Add("@Stake_Score", SqlDbType.Money).Value = stake_Score;
                cmd.Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = youXiaoYaFen;
                cmd.Parameters.Add("@Account_Score", SqlDbType.Money).Value = account_Score;
                cmd.Parameters.Add("@ShengDian", SqlDbType.Money).Value = shengDian;
                cmd.Parameters.Add("@Rows", SqlDbType.Int).Value = rows;
                cmd.Parameters.Add("@Mode", SqlDbType.TinyInt).Value = 2;

                //
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet("root");
                    da.Fill(ds);
                    return ds;
                }
            }
        }
    }
    #endregion

    #region JDB
    /// <summary>
    /// 用於JDB 補單
    /// </summary>
    /// <param name="tbxClub"></param>
    /// <param name="tbxSessionNo"></param>
    /// <param name="tbxJDBSessionId"></param>
    /// <param name="tbxGame_id"></param>
    /// <param name="tbxBet"></param>
    /// <param name="tbxJackpot"></param>
    /// <param name="tbxNetWin"></param>
    /// <param name="tbxRows"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataTable A_ms_InsertStakeData_JDB(string tbxClub, string tbxSessionNo, string tbxJDBSessionId, string tbxGame_id, string tbxBet, string tbxJackpot, string tbxNetWin, int tbxRows, out string value)
    {
        DataTable dtb = null;

        try
        {
            using (SqlCommand cmd = new SqlCommand("EXEC A_ms_InsertStakeData_JDB @KeyFlag, @Club, @SessionNo, @JDBSessionId, @Game_id, @Bet, @Jackpot, @NetWin, @Rows, @ReturnState OUTPUT", this._mainConnection))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@KeyFlag", new Guid("99FBECDB-05B4-4C0A-968B-2BEF85976945"));
                cmd.Parameters.AddWithValue("@Club", tbxClub.ToStr().Trim());
                cmd.Parameters.AddWithValue("@SessionNo", tbxSessionNo.ToStr().Trim());
                cmd.Parameters.AddWithValue("@JDBSessionId", tbxJDBSessionId.ToStr().Trim());
                cmd.Parameters.AddWithValue("@Game_id", tbxGame_id.ToStr().Trim());
                cmd.Parameters.AddWithValue("@Bet", tbxBet.ToSingle());
                cmd.Parameters.AddWithValue("@Jackpot", tbxJackpot.ToSingle());
                cmd.Parameters.AddWithValue("@NetWin", tbxNetWin.ToSingle());
                cmd.Parameters.AddWithValue("@Rows", tbxRows);
                SqlParameter returnState = cmd.Parameters.Add("@ReturnState", SqlDbType.Int);
                returnState.Direction = ParameterDirection.Output;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    dtb = new DataTable();
                    da.Fill(dtb);
                    value = returnState.Value.ToStr();
                }
            }
        }
        catch (Exception ex)
        {
            Lib.WritLog("dbSQL.cs.A_ms_InsertStakeData_JDB", ex.ToString());
            dtb = new DataTable();
            value = string.Empty;
        }

        return dtb;
    }

    /// <summary>
    /// 刪除JDB注單
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int A_ms_DeleteStakeData_JDB(long id)
    {
        int effect = 0;

        try
        {
            using (var conn = this._mainConnection)
            {
                if (conn.State == ConnectionState.Closed )
                {
                    conn.Open();
                }
                using (SqlCommand cmd = new SqlCommand("DELETE T_Club_Stake_Current WHERE Id = @Id", conn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id", id);
                    effect = cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Lib.WritLog("dbSQL.cs.A_ms_DeleteStakeData_JDB", ex.ToString());
            effect = -1;
        }

        return effect;

    }

    /// <summary>
    /// 查詢是否已經派過彩
    /// </summary>
    /// <returns></returns>
    public bool IsSendAmount()
    {
        DataTable dtb = null;

        int num = 0;

        bool rvalue = false;

        try
        {
            using (SqlCommand cmd = new SqlCommand("select count(*) as ttt from T_Club_Stake_Current  where [Game_id]=@Game_id and [EndSeqNoFlag]=@EndSeqNoFlag ", this._mainConnection))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Game_id", "JDBRoom");
                cmd.Parameters.AddWithValue("@EndSeqNoFlag", 88888888888888888);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    dtb = new DataTable();
                    da.Fill(dtb);
                }
            }
        }
        catch (Exception ex)
        {
            Lib.WritLog("dbSQL.cs.IsSendAmount()", ex.ToString());

            num = -999;
        }

        if (dtb != null)
        {
            foreach (DataRow row in dtb.Rows)
            {
                if (row["ttt"] != null)
                {
                    num =int.Parse(row["ttt"].ToString());
                }
            }
        }

        if (num == 0)
        {
            rvalue = true;

        }else {

            rvalue = false;
        }

        return rvalue;
    }

    /// <summary>
    /// JDB派送彩金
    /// </summary>
    /// <param name="awardData"></param>
    /// <param name="JDBdress"></param>
    /// <param name="GameCode"></param>
    /// <param name="IsFinalyDay"></param>
    /// <returns></returns>
    public bool NewSendAmount(DataTable awardData, string JDBdress,out string ErrorMsg)
    {
        string Rank = string.Empty;

        string Uid = string.Empty;

        string Amount = string.Empty;

        string GameCode = string.Empty;

        StringBuilder Errmsg = new StringBuilder("");

        SqlTransaction objTrans = null;

        bool isok = false;

        int effect = 0;

        int iReturnState = 0;

        bool rvalue = false;

        int k = 0;

        try
        {
            if (awardData == null)
            {
                Lib.WritLog("dbSQL.cs.NewSendAmount", "awardData為NULL，尚未執行派彩!!!");

                ErrorMsg = "awardData為NULL，尚未執行派彩!!!";

                return false;
            }

            if (!awardData.Columns.Contains("名次") || !awardData.Columns.Contains("JDB-UID") || !awardData.Columns.Contains("獎金") || !awardData.Columns.Contains("GameCode"))
            {
                Lib.WritLog("dbSQL.cs.NewSendAmount", "DataTable有缺少必要欄位，尚未執行派彩!!!");

                ErrorMsg = "DataTable有缺少必要欄位，尚未執行派彩!!!";

                return false;
            }

            if (this._mainConnection.State == ConnectionState.Closed)
            {
                try
                {
                    this._mainConnection.Open();
                }
                catch (Exception ex)
                {
                    Lib.WritLog("dbSQL.cs.NewSendAmount", string.Format( "連接資料庫時發生例外︰{0} ",ex.ToString()));

                    ErrorMsg = string.Format("連接資料庫時發生例外︰{0} ", ex.ToString());

                    return false;
                }
            }

            objTrans = this._mainConnection.BeginTransaction();

            SqlCommand[] _cmd1 = new SqlCommand[awardData.Rows.Count];

            SqlCommand[] _cmd2 = new SqlCommand[awardData.Rows.Count];
  
            foreach (DataRow row in awardData.Rows)
            {
                Thread.Sleep(10);

                Rank = row["名次"].ToString();

                Uid = row["JDB-UID"].ToString().Replace(JDBdress, "");

                Amount = row["獎金"].ToString();

                GameCode = row["GameCode"].ToString();

                /*--------------------------------------------------*/
                using (_cmd1[k] = new SqlCommand("A_ms_InsertStakeData_JDB", this._mainConnection, objTrans))
                {
                    _cmd1[k].CommandType = CommandType.StoredProcedure;
                    _cmd1[k].Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("99FBECDB-05B4-4C0A-968B-2BEF85976945");
                    _cmd1[k].Parameters.Add("@Club", SqlDbType.NVarChar).Value = Uid;
                    _cmd1[k].Parameters.Add("@SessionNo", SqlDbType.NVarChar).Value = DBNull.Value;
                    _cmd1[k].Parameters.Add("@JDBSessionId", SqlDbType.VarChar).Value = string.Format("1,{0},{1}", GameCode, Rank);
                    _cmd1[k].Parameters.Add("@Game_id", SqlDbType.VarChar).Value = "JDBRoom";
                    _cmd1[k].Parameters.Add("@Bet", SqlDbType.Money).Value = 0;
                    _cmd1[k].Parameters.Add("@Jackpot", SqlDbType.Money).Value = 0;
                    _cmd1[k].Parameters.Add("@NetWin", SqlDbType.Money).Value = Amount;
                    _cmd1[k].Parameters.Add("@Rows", SqlDbType.VarChar).Value = 0;
                    SqlParameter TotalCount = _cmd1[k].Parameters.Add("@ReturnState", SqlDbType.Int);
                    TotalCount.Direction = ParameterDirection.Output;

                    try
                    {
                        _cmd1[k].ExecuteNonQuery();

                        iReturnState = TotalCount.Value != null ? int.Parse(TotalCount.Value.ToString()) : -999;

                        if (iReturnState == 1)
                        {
                            isok = true;
                        }
                        else if (iReturnState == -999)
                        {
                            Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3}", Uid, Rank, Amount, GameCode));
                            Errmsg.Append("A_ms_InsertStakeData_JDB 執行失敗OutPut參數值為NULL︰");
                            Errmsg.Append(iReturnState);
                            Errmsg.Append("\r\n");
                            isok = false;
                        }
                        else
                        {
                            Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3}", Uid, Rank, Amount, GameCode));
                            Errmsg.Append("A_ms_InsertStakeData_JDB 執行失敗回傳值︰");
                            Errmsg.Append(iReturnState);
                            Errmsg.Append("\r\n");
                            isok = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        isok = false;
                        Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3}", Uid, Rank, Amount, GameCode));
                        Errmsg.Append("A_ms_InsertStakeData_JDB 執行發生例外︰");
                        Errmsg.Append(ex.ToString());
                        Errmsg.Append("\r\n");
                    }
                }

                if (isok)
                {
                    using (_cmd2[k] = new SqlCommand("UPDATE T_Club SET Now_XinYong = Now_XinYong + @money WHERE Club_id = @Club_id ", this._mainConnection, objTrans))
                    {
                        _cmd2[k].CommandType = CommandType.Text;
                        _cmd2[k].Parameters.Add("@money", SqlDbType.Money).Value = Amount;
                        _cmd2[k].Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = Uid;

                        try
                        {
                            effect = _cmd2[k].ExecuteNonQuery();

                            if (effect == 1)
                            {
                                isok = true;
                            }
                            else
                            {
                                Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3},Club_id:{4},money:{5}", Uid, Rank, Amount, GameCode, Uid, Amount));
                                Errmsg.Append("UPDATE T_Club 執行失敗回傳受影響筆數︰");
                                Errmsg.Append(effect);
                                Errmsg.Append("\r\n");
                                isok = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            isok = false;
                            Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3},Club_id:{4},money:{5}", Uid, Rank, Amount, GameCode, Uid, Amount));
                            Errmsg.Append("UPDATE T_Club 執行發生例外︰");
                            Errmsg.Append(ex.ToString());
                            Errmsg.Append("\r\n");
                        }
                    }
                }
                /*------------------------------------*/

                if (!isok)
                {
                    break;
                }

                k++;
            }
        }
        catch (Exception ex)
        {
            isok = false;
            Errmsg.Append("SendAmount 執行發生例外︰");
            Errmsg.Append(ex.ToString());
            Errmsg.Append("\r\n");
        }

        if (this._mainConnection != null)
        {
            if (this._mainConnection.State == ConnectionState.Open)
            {
                if (isok)
                {
                    objTrans.Commit();
                    rvalue = true;
                }
                else
                {
                    objTrans.Rollback();
                    rvalue = false;
                }

                this._mainConnection.Close();
            }
        }

        if (Errmsg.ToString() != "")
        {
            Lib.WritLog("dbSQL.cs.NewSendAmount", Errmsg.ToString());
        }

       ErrorMsg = Errmsg.ToString();

        return rvalue;
    }
    /// <summary>
    /// JDB派送彩金
    /// </summary>
    /// <param name="awardData"></param>
    /// <param name="JDBdress"></param>
    /// <returns></returns>
    public bool SendAmount(System.Collections.Generic.IList<AwardData> awardData,string JDBdress)
    {
        string Rank = string.Empty;

        string Uid = string.Empty; 

        string Amount = string.Empty;

        string GameCode = string.Empty;

        StringBuilder Errmsg = new StringBuilder("");

        SqlTransaction objTrans = null;

        bool isok = false;

        int effect = 0;

        int iReturnState = 0;

        bool rvalue = false;

        try
        {

            if (awardData == null)
            {
                Lib.WritLog("dbSQL.cs.SendAmount", "awardData為NULL，尚未執行派彩!!!");

                return false;
            }

            if (this._mainConnection.State == ConnectionState.Closed)
            {
                try
                {
                    this._mainConnection.Open();
                }
                catch (Exception ex)
                {
                    Lib.WritLog("dbSQL.cs.SendAmount", "連接資料庫時發生例外︰" + ex.ToString());
                    return false;
                }
            }

            objTrans = this._mainConnection.BeginTransaction();

            for (int i = 0; i < awardData.Count; i++)
            {
                if (awardData[i].rankData == null)
                {
                    Errmsg.Append("dbSQL.cs.SendAmount,awardData[" + i + "].rankData為NULL，尚未執行派彩或已回滾資料!!!");
                    Errmsg.Append("\r\n");
                    isok = false;
                    break;
                }

                if (awardData[i].awardName.ToUpper() == "DRAGON")
                {
                    GameCode = "1";
                }

                if (awardData[i].awardName.ToUpper() == "TIGER")
                {
                    GameCode = "2";
                }

                SqlCommand[] _cmd1 = new SqlCommand[awardData[i].rankData.Count];

                SqlCommand[] _cmd2 = new SqlCommand[awardData[i].rankData.Count];

                if (awardData[i].rankData.Count > 0)
                {
                    for (int k = 0; k < awardData[i].rankData.Count; k++)
                    {
                        Thread.Sleep(10);

                        Rank = awardData[i].rankData[k].rank;

                        Uid = awardData[i].rankData[k].uid.Replace(JDBdress, "");

                        Amount = awardData[i].rankData[k].amount;
                        /*--------------------------------------------------*/
                        using (_cmd1[k] = new SqlCommand("A_ms_InsertStakeData_JDB", this._mainConnection, objTrans))
                        {
                            _cmd1[k].CommandType = CommandType.StoredProcedure;
                            _cmd1[k].Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("99FBECDB-05B4-4C0A-968B-2BEF85976945");
                            _cmd1[k].Parameters.Add("@Club", SqlDbType.NVarChar).Value = Uid;
                            _cmd1[k].Parameters.Add("@SessionNo", SqlDbType.NVarChar).Value = DBNull.Value;
                            _cmd1[k].Parameters.Add("@JDBSessionId", SqlDbType.VarChar).Value = string.Format("1,{0},{1}", GameCode, Rank);
                            _cmd1[k].Parameters.Add("@Game_id", SqlDbType.VarChar).Value = "JDBRoom";
                            _cmd1[k].Parameters.Add("@Bet", SqlDbType.Money).Value = 0;
                            _cmd1[k].Parameters.Add("@Jackpot", SqlDbType.Money).Value = 0;
                            _cmd1[k].Parameters.Add("@NetWin", SqlDbType.Money).Value = Amount;
                            _cmd1[k].Parameters.Add("@Rows", SqlDbType.VarChar).Value = 0;
                            SqlParameter TotalCount = _cmd1[k].Parameters.Add("@ReturnState", SqlDbType.Int);
                            TotalCount.Direction = ParameterDirection.Output;

                            try
                            {
                                _cmd1[k].ExecuteNonQuery();

                                iReturnState = TotalCount.Value != null ? int.Parse(TotalCount.Value.ToString()) : -999;

                                if (iReturnState == 1)
                                {
                                    isok = true;
                                }
                                else if (iReturnState == -999)
                                {
                                    Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3}", Uid, Rank, Amount, GameCode));
                                    Errmsg.Append("A_ms_InsertStakeData_JDB 執行失敗OutPut參數值為NULL︰");
                                    Errmsg.Append(iReturnState);
                                    Errmsg.Append("\r\n");
                                    isok = false;
                                }
                                else
                                {
                                    Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3}", Uid, Rank, Amount, GameCode));
                                    Errmsg.Append("A_ms_InsertStakeData_JDB 執行失敗回傳值︰");
                                    Errmsg.Append(iReturnState);
                                    Errmsg.Append("\r\n");
                                    isok = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                isok = false;
                                Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3}", Uid, Rank, Amount, GameCode));
                                Errmsg.Append("A_ms_InsertStakeData_JDB 執行發生例外︰");
                                Errmsg.Append(ex.ToString());
                                Errmsg.Append("\r\n");
                            }
                        }

                        if (isok)
                        {
                            using (_cmd2[k] = new SqlCommand("UPDATE T_Club SET Now_XinYong = Now_XinYong + @money WHERE Club_id = @Club_id ", this._mainConnection, objTrans))
                            {
                                _cmd2[k].CommandType = CommandType.Text;
                                _cmd2[k].Parameters.Add("@money", SqlDbType.Money).Value = Amount;
                                _cmd2[k].Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = Uid;

                                try
                                {
                                    effect = _cmd2[k].ExecuteNonQuery();

                                    if (effect == 1)
                                    {
                                        isok = true;
                                    }
                                    else
                                    {
                                        Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3},Club_id:{4},money:{5}", Uid, Rank, Amount, GameCode, Uid, Amount));
                                        Errmsg.Append("UPDATE T_Club 執行失敗回傳受影響筆數︰");
                                        Errmsg.Append(effect);
                                        Errmsg.Append("\r\n");
                                        isok = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    isok = false;
                                    Errmsg.Append(string.Format("UID:{0},Rank:{1},Amount:{2},GameCode:{3},Club_id:{4},money:{5}", Uid, Rank, Amount, GameCode, Uid, Amount));
                                    Errmsg.Append("UPDATE T_Club 執行發生例外︰");
                                    Errmsg.Append(ex.ToString());
                                    Errmsg.Append("\r\n");
                                }
                            }
                        }
                        /*------------------------------------*/

                        if (!isok)
                        {
                            break;
                        }
                    }
                }

                if (!isok)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            isok = false;
            Errmsg.Append("SendAmount 執行發生例外︰");
            Errmsg.Append(ex.ToString());
            Errmsg.Append("\r\n");
        }

        if (this._mainConnection != null)
        {
            if (this._mainConnection.State == ConnectionState.Open)
            {
                if (isok)
                {
                    objTrans.Commit();
                    rvalue = true;
                }
                else
                {
                    objTrans.Rollback();
                    rvalue = false;
                }

                this._mainConnection.Close();
            }
        }

        if (Errmsg.ToString() != "")
        {
            Lib.WritLog("dbSQL.cs.SendAmount", Errmsg.ToString());
        }

        return rvalue;
    }

    /// <summary>
    /// JDB活動，派發彩金 https://www.c-sharpcorner.com/article/transaction-in-net/
    /// </summary>
    /// <param name="Rank"></param>
    /// <param name="Uid"></param>
    /// <param name="Amount"></param>
    /// <param name="GameCode"></param>
    /// <param name="_ReturnState"></param>
    /// <param name="_effect"></param>
    /// <param name="_Errormsg"></param>
    /// <returns></returns>
    public bool SendAmount_bak(string Rank, string Uid, string Amount,string GameCode,out int _ReturnState,out int _effect,out string _Errormsg)
    {
        bool isok = false;

        int effect = 0;

        int iReturnState = 0;

        StringBuilder Errmsg = new StringBuilder("");

        SqlTransaction objTrans = null;

        try
        {
            if (this._mainConnection.State == ConnectionState.Closed)
            {
                this._mainConnection.Open();
            }

            objTrans = this._mainConnection.BeginTransaction();

            using (SqlCommand cmd = new SqlCommand("A_ms_InsertStakeData_JDB", this._mainConnection, objTrans))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@KeyFlag", SqlDbType.UniqueIdentifier).Value = new Guid("99FBECDB-05B4-4C0A-968B-2BEF85976945");
                cmd.Parameters.Add("@Club", SqlDbType.NVarChar).Value = Uid;
                cmd.Parameters.Add("@SessionNo", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@JDBSessionId", SqlDbType.VarChar).Value = string.Format("1,{0},{1}",GameCode, Rank);
                cmd.Parameters.Add("@Game_id", SqlDbType.VarChar).Value = "JDBRoom";
                cmd.Parameters.Add("@Bet", SqlDbType.Money).Value = 0;
                cmd.Parameters.Add("@Jackpot", SqlDbType.Money).Value = 0;
                cmd.Parameters.Add("@NetWin", SqlDbType.Money).Value = Amount;
                cmd.Parameters.Add("@Rows", SqlDbType.VarChar).Value = 0;
                SqlParameter TotalCount = cmd.Parameters.Add("@ReturnState", SqlDbType.Int);
                TotalCount.Direction = ParameterDirection.Output;

                try
                {
                    cmd.ExecuteNonQuery();

                    iReturnState = TotalCount.Value != null ? int.Parse(TotalCount.Value.ToString()) : -1;

                    if (iReturnState == 1)
                    {
                        isok = true;

                    }else if (iReturnState == -1) {
                        Errmsg.Append("A_ms_InsertStakeData_JDB 執行失敗OutPut參數值為NULL︰");
                        Errmsg.Append(iReturnState);
                        Errmsg.Append("\r\n");
                        isok = false;
                    } else{
                        Errmsg.Append("A_ms_InsertStakeData_JDB 執行失敗回傳值︰");
                        Errmsg.Append(iReturnState);
                        Errmsg.Append("\r\n");
                        isok = false;
                    }
                }
                catch (Exception ex )
                {
                    isok = false;
                    Errmsg.Append("A_ms_InsertStakeData_JDB 執行發生例外︰");
                    Errmsg.Append(ex.ToString());
                    Errmsg.Append("\r\n");
                }
            }

            if (isok)
            {
                using (SqlCommand cmd1 = new SqlCommand("UPDATE T_Club SET Now_XinYong = Now_XinYong + @money WHERE Club_id = @Club_id ", this._mainConnection, objTrans))
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.Add("@money", SqlDbType.Money).Value = Amount;
                    cmd1.Parameters.Add("@Club_id", SqlDbType.NVarChar).Value = Uid;
                    try
                    {
                        effect = cmd1.ExecuteNonQuery();

                        if (effect == 1)
                        {
                            isok = true;
                        }else{
                            Errmsg.Append("UPDATE T_Club 執行失敗回傳受影響筆數︰");
                            Errmsg.Append(effect);
                            Errmsg.Append("\r\n");
                            isok = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        isok = false;
                        Errmsg.Append("UPDATE T_Club 執行發生例外︰");
                        Errmsg.Append(ex.ToString());
                        Errmsg.Append("\r\n"); 
                    }
                }
            }            
        }
        catch (Exception ex)
        {
            isok = false;
            Errmsg.Append("SendAmount 執行發生例外︰");
            Errmsg.Append(ex.ToString());
            Errmsg.Append("\r\n");

        }finally {

            if (this._mainConnection != null)
            {
                if (isok)
                {
                    objTrans.Commit();
                }
                else
                {
                    objTrans.Rollback();
                }

                if (this._mainConnection.State == ConnectionState.Open)
                {
                    this._mainConnection.Close();
                } 
            }
        }

        _ReturnState = iReturnState;

        _Errormsg = Errmsg.ToString();

        _effect = effect;

        return isok;
    }
    #endregion

    #region RTG
    public bool updateStakeCurrent(DataTable td,string logname)
    {
        if (td == null)
        {
            return false;

        }else if(td.Rows.Count==0) {

            return false;
        }
        bsSQL objbsSQL = new bsSQL();
        SqlCommand[] cmdToExecute = new SqlCommand[td.Rows.Count];
        int[] InsertResult = new int[td.Rows.Count];
        string cmdText = string.Empty;
        string MaHao = string.Empty;
        int effecttotal = 0;
        int j = 0;
        bool isok = true;
        double diff_Stake_Score = 0;
        double diff_Account_Score = 0;
        double diff_PKPoint = 0;    //公點
        double diff_SharePoint = 0; //分潤
        string NewSessionId = string.Empty;
        Int64 stake_id = 0;
        int diff_Rows = 0;
        bool CanCommit = true;
        StringBuilder logs = new StringBuilder("");
        SqlTransaction objTrans = null;
        if (this._mainConnection.State != ConnectionState.Open)
        {
            this._mainConnection.Open();
        }

        objTrans = this._mainConnection.BeginTransaction();
        foreach (DataRow row in td.Rows)
        {
            try
            { 
                if (row["IsHistory"].ToString().ToUpper() == "Y")  //表示過帳完
                {
                    /*因為筆數及下注無法是負數(只有過完帳才需要判斷) 2020-11-26 註解，目前已經發生過注金為負，所以將其註解
                    if (int.Parse(row["RTG_Rows"].ToString()) < int.Parse(row["H1_Rows"].ToString()) || double.Parse(row["RTG_Stake_Score"].ToString()) < double.Parse(row["H1_Stake_Score"].ToString()))
                    {
                        InsertResult[j] = 1;  
                        j++;
                        continue;
                    }*/

                     diff_Stake_Score = double.Parse(row["RTG_Stake_Score"].ToString())- double.Parse(row["H1_Stake_Score"].ToString());
                     diff_Account_Score = double.Parse(row["RTG_Account_Score"].ToString()) - double.Parse(row["H1_Account_Score"].ToString());
                     diff_Rows = int.Parse(row["RTG_Rows"].ToString()) - int.Parse(row["H1_Rows"].ToString());
                     diff_PKPoint = double.Parse(row["RTG_PKPoint"].ToString()) - double.Parse(row["H1_PKPoint"].ToString()); ;          //公點
                     diff_SharePoint = double.Parse(row["RTG_SharePoint"].ToString()) - double.Parse(row["H1_SharePoint"].ToString()); ; //分潤
                     //2020-12-30 配合做帳不計算公點和分潤，所以將公點和分潤，就算出現差異，也一律設定為0
                     diff_PKPoint = 0;
                     diff_SharePoint = 0;
                    dbSQL bsql = new dbSQL();
                    if (diff_Stake_Score == 0 && diff_Account_Score == 0 && diff_Rows == 0 && diff_PKPoint==0 && diff_SharePoint==0)  //表示帳務資料未改變，不予處理
                    {
                        InsertResult[j] = 1;  //因為新增注單的SP已經綁交易，故增加這個陣列先判斷注單SP是否成功，再決定其它交是否提交
                        j++;
                        continue;
                    }
                    stake_id = objbsSQL.HasStakeCurrentData(row["H1_SessionId"].ToString(), row["H1_Club_id"].ToString());
                    NewSessionId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    MaHao = string.Format("{0},,0.00,{1},0", diff_Rows.ToString(), row["RTG_SessionId"].ToString());  
                    if (stake_id == 0)  //等於0，表示要新增補單
                    {
                        cmdToExecute[j] = new SqlCommand();
                        cmdToExecute[j].CommandType = CommandType.StoredProcedure;
                        cmdToExecute[j].Connection = this._mainConnection;
                        cmdToExecute[j].CommandText = "A_ms_InsertStakeData";
                        cmdToExecute[j].Parameters.Add("@Club_id", SqlDbType.VarChar).Value = row["H1_Club_id"].ToString();
                        cmdToExecute[j].Parameters.Add("@Stake_id", SqlDbType.VarChar).Value = row["H1_Stake_id"].ToString();
                        cmdToExecute[j].Parameters.Add("@ZhuDan_Type", SqlDbType.VarChar).Value = row["H1_ZhuDan_Type"].ToString();
                        cmdToExecute[j].Parameters.Add("@Stake_Score", SqlDbType.Money).Value = diff_Stake_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@Account_Score", SqlDbType.Money).Value = diff_Account_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@Now_XinYong", SqlDbType.Money).Value = row["H1_PlayerAccount"].ToString();
                        cmdToExecute[j].Parameters.Add("@Active", SqlDbType.Int).Value = row["H1_Active"].ToString();
                        cmdToExecute[j].Parameters.Add("@Desk_id", SqlDbType.Int).Value = row["H1_Desk_id"].ToString();
                        cmdToExecute[j].Parameters.Add("@No_Run", SqlDbType.VarChar).Value = row["H1_No_Run"].ToString();
                        cmdToExecute[j].Parameters.Add("@No_Active", SqlDbType.VarChar).Value = row["H1_No_Active"].ToString();
                        cmdToExecute[j].Parameters.Add("@JiTai_No", SqlDbType.VarChar).Value = row["H1_JiTai_No"].ToString(); 
                        cmdToExecute[j].Parameters.Add("@MaHao", SqlDbType.VarChar).Value = MaHao.ToString();
                        cmdToExecute[j].Parameters.Add("@StartSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(NewSessionId);
                        cmdToExecute[j].Parameters.Add("@EndSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(row["H1_SessionId"].ToString()); ;
                        cmdToExecute[j].Parameters.Add("@TTLJackpot", SqlDbType.Money).Value = row["H1_JackPot"].ToString();
                        cmdToExecute[j].Parameters.Add("@ActionTime", SqlDbType.DateTime).Value = DateTime.Now.ToString();//row["H1_MaxDateTime"].ToString();
                        cmdToExecute[j].Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = diff_Stake_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@YaMa", SqlDbType.Money).Value = diff_Stake_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@TableFee", SqlDbType.Money).Value = diff_PKPoint;      //公點  //row["RTG_PKPoint"].ToString();
                        cmdToExecute[j].Parameters.Add("@Commission", SqlDbType.Money).Value = diff_SharePoint; //分潤  //row["RTG_SharePoint"].ToString();
                        SqlParameter returnState = cmdToExecute[j].Parameters.Add("@ReturnState", SqlDbType.Int);
                        returnState.Direction = ParameterDirection.Output;
                        //cmdToExecute[j].ExecuteNonQuery();
                        bsql.RunSQL(cmdToExecute[j]);
                        InsertResult[j] = int.Parse(returnState.Value.ToString()); 
                    }else {  //表示已經有補單了，只要針對現有補單再修改
                        cmdText = @"UPDATE TOP(1) t
			                           SET t.Stake_Score   = ISNULL(@Stake_Score, t.Stake_Score)
                                         , t.Account_Score = ISNULL(@Account_Score, t.Account_Score)
                                         , t.YouXiaoYaFen  = ISNULL(@YouXiaoYaFen, t.YouXiaoYaFen)
                                         , t.YaMa          = ISNULL(@YouXiaoYaFen, t.YaMa)
                                         , t.MaHao         = ISNULL(@MaHao, t.MaHao) --如果有要異動就去掉注解
                                         , t.StartSeqNoFlag  = ISNULL(@StartSeqNoFlag, t.StartSeqNoFlag) 
                                         , t.EndSeqNoFlag    = ISNULL(@EndSeqNoFlag, t.EndSeqNoFlag) 
                                         , t.ReportTime      = ISNULL(@ReportTime, t.ReportTime) 
                                         , t.Datetime        = ISNULL(@Datetime, t.Datetime)
                                         , t.TableFee        = ISNULL(@TableFee, t.TableFee)  
                                         , t.Commission        = ISNULL(@Commission, t.Commission)
                                      FROM T_Club_Stake_Current t with(RowLock)
			                          WHERE t.Id = @Id";
                        MaHao = string.Format("{0},,0.00,{1},0", row["RTG_Rows"].ToString(), row["RTG_SessionId"].ToString());
                        cmdToExecute[j] = new SqlCommand(cmdText, this._mainConnection, objTrans);
                        cmdToExecute[j].CommandType = CommandType.Text;
                        cmdToExecute[j].Parameters.Add("@id", SqlDbType.BigInt).Value = stake_id;
                        cmdToExecute[j].Parameters.Add("@Stake_Score", SqlDbType.Money).Value = diff_Stake_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = diff_Stake_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@Account_Score", SqlDbType.Money).Value = diff_Account_Score.ToString();
                        cmdToExecute[j].Parameters.Add("@MaHao", SqlDbType.VarChar).Value = MaHao.ToString();
                        cmdToExecute[j].Parameters.Add("@StartSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(NewSessionId.ToString());
                        cmdToExecute[j].Parameters.Add("@EndSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(row["H1_SessionId"].ToString());
                        cmdToExecute[j].Parameters.Add("@ReportTime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                        cmdToExecute[j].Parameters.Add("@Datetime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                        cmdToExecute[j].Parameters.Add("@TableFee", SqlDbType.Money).Value = diff_PKPoint;      //公點  
                        cmdToExecute[j].Parameters.Add("@Commission", SqlDbType.Money).Value = diff_SharePoint; //分潤  
                        effecttotal = cmdToExecute[j].ExecuteNonQuery();
                        InsertResult[j] = 1;
                    }
                }else{
                    cmdText = @"UPDATE TOP(1) t
			           SET t.Stake_Score   = ISNULL(@Stake_Score, t.Stake_Score)
                         , t.Account_Score = ISNULL(@Account_Score, t.Account_Score)
                         , t.YouXiaoYaFen  = ISNULL(@YouXiaoYaFen, t.YouXiaoYaFen)
                         , t.YaMa          = ISNULL(@YouXiaoYaFen, t.YaMa)
                         , t.MaHao         = ISNULL(@MaHao, t.MaHao) --如果有要異動就去掉注解
                         , t.TableFee      = ISNULL(@TableFee, t.TableFee)  
                         , t.Commission    = ISNULL(@Commission, t.Commission)
                      FROM T_Club_Stake_Current t with(RowLock)
			         WHERE t.Id = @Id";
                    MaHao = string.Format("{0},,0.00,{1},0", row["RTG_Rows"].ToString(), row["RTG_SessionId"].ToString());
                    cmdToExecute[j] = new SqlCommand(cmdText, this._mainConnection, objTrans);
                    cmdToExecute[j].CommandType = CommandType.Text;
                    cmdToExecute[j].Parameters.Add("@id", SqlDbType.BigInt).Value = row["H1_Id"].ToString();
                    cmdToExecute[j].Parameters.Add("@Stake_Score", SqlDbType.Money).Value = row["RTG_Stake_Score"].ToString();
                    cmdToExecute[j].Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = row["RTG_Stake_Score"].ToString();
                    cmdToExecute[j].Parameters.Add("@Account_Score", SqlDbType.Money).Value = row["RTG_Account_Score"].ToString();
                    cmdToExecute[j].Parameters.Add("@MaHao", SqlDbType.VarChar).Value = MaHao.ToString();
                    //2020-12-30 配合做帳不計算公點和分潤，所以將公點和分潤，就算出現差異，也一律設定為0
                    cmdToExecute[j].Parameters.Add("@TableFee", SqlDbType.Money).Value = 0;    //公點  
                    cmdToExecute[j].Parameters.Add("@Commission", SqlDbType.Money).Value = 0;  //分潤  
                    /* 2020-12-30 原始程式
                    cmdToExecute[j].Parameters.Add("@TableFee", SqlDbType.Money).Value = row["RTG_PKPoint"].ToString();      //公點  
                    cmdToExecute[j].Parameters.Add("@Commission", SqlDbType.Money).Value = row["RTG_SharePoint"].ToString(); //分潤  
                    */
                    effecttotal = cmdToExecute[j].ExecuteNonQuery();
                    InsertResult[j] = 1;
                }

                j++;
            }
            catch (Exception ex)
            {
                isok = false;
                logs.Append(string.Format("第{0}筆執行發生例外︰{0}", ex.ToString()));
                logs.Append(string.Format(@"參數︰MaHao︰{0},
                                            id︰{0},
                                            Stake_Score︰{0},
                                            YouXiaoYaFen︰{0},
                                            Account_Score︰{0},
                                            Rows︰{0}",
                                            MaHao, row["H1_Id"].ToString(),
                                            row["RTG_Stake_Score"].ToString(),
                                            row["RTG_Stake_Score"].ToString(),
                                            row["RTG_Account_Score"].ToString(),
                                            row["RTG_Rows"].ToString()
                                            ));
                Lib.WritLog(logname, logs.ToString());
            }
        }

        if (isok)
        {
            for (int i = 0; i < InsertResult.Length; i++)
            {
                if (InsertResult[i] != 1)
                {
                    CanCommit = false;
                    break;
                }
            }

            if (CanCommit)
            {
                objTrans.Commit();
            }else {
                objTrans.Rollback();
                isok = false;
            }

        }else {

            objTrans.Rollback();
        }

        return isok;
    }
    /// <summary>
    /// 手動針對過帳後的改單
    /// </summary>
    /// <param name="td"></param>
    /// <param name="logname"></param>
    /// <returns></returns>
    public bool HandleEditStakeHistory(string Rows,string Club_id,string Now_XinYong,
        string Account_Score,string Datetime,string Game_id,string Jackpot_Score,string StartSeqNoFlag,
        string Id,string Stake_Score,string IsHistory,string Stake_id,string ZhuDan_Type,
        string Active,string Desk_id,string No_Run,string No_Active,string JiTai_No,string TableFee,string Commission)
    {
        string NewSessionId = string.Empty;
        string MaHao = string.Empty;
        string cmdText = string.Empty;
        Int64 stake_id = 0;
        int InsertResult = 0;
        bsSQL objbsSQL = new bsSQL();
        SqlCommand cmdToExecute = new SqlCommand();
        StringBuilder objstrg = new StringBuilder("");
        objstrg.Append("預計修改資料︰");
        objstrg.Append(string.Format("資料查詢參數︰StartSeqNoFlag︰{0}，Club_id︰{1}", StartSeqNoFlag.ToString(), Club_id.ToString()));
        stake_id = objbsSQL.HasStakeCurrentData(StartSeqNoFlag.ToString(), Club_id.ToString());
        objstrg.Append(string.Format("資料ID︰ID︰{0}", stake_id));
        NewSessionId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        MaHao = string.Format("{0},,0.00,{1},0", Rows,StartSeqNoFlag);
        objstrg.Append(string.Format("MaHao︰{0}", MaHao));
        objstrg.Append(string.Format("NewSessionId︰{0}", NewSessionId));
        if (this._mainConnection.State != ConnectionState.Open)
        {
            this._mainConnection.Open();
        }
        if (stake_id == 0)  //等於0，表示要新增補單
        {
            objstrg.Append("，新增補單A_ms_InsertStakeData，");
            cmdToExecute = new SqlCommand();
            cmdToExecute.CommandType = CommandType.StoredProcedure;
            cmdToExecute.Connection = this._mainConnection;
            cmdToExecute.CommandText = "A_ms_InsertStakeData";
            cmdToExecute.Parameters.Add("@Club_id", SqlDbType.VarChar).Value = Club_id.ToString();
            cmdToExecute.Parameters.Add("@Stake_id", SqlDbType.VarChar).Value = Stake_id.ToString();
            cmdToExecute.Parameters.Add("@ZhuDan_Type", SqlDbType.VarChar).Value = ZhuDan_Type.ToString();
            cmdToExecute.Parameters.Add("@Stake_Score", SqlDbType.Money).Value = Stake_Score.ToString();
            cmdToExecute.Parameters.Add("@Account_Score", SqlDbType.Money).Value = Account_Score.ToString();
            cmdToExecute.Parameters.Add("@Now_XinYong", SqlDbType.Money).Value = Now_XinYong.ToString();
            cmdToExecute.Parameters.Add("@Active", SqlDbType.Int).Value = Active.ToString();
            cmdToExecute.Parameters.Add("@Desk_id", SqlDbType.Int).Value = Desk_id.ToString();
            cmdToExecute.Parameters.Add("@No_Run", SqlDbType.VarChar).Value = No_Run.ToString();
            cmdToExecute.Parameters.Add("@No_Active", SqlDbType.VarChar).Value = No_Active.ToString();
            cmdToExecute.Parameters.Add("@JiTai_No", SqlDbType.VarChar).Value = JiTai_No.ToString();
            cmdToExecute.Parameters.Add("@MaHao", SqlDbType.VarChar).Value = MaHao.ToString();
            cmdToExecute.Parameters.Add("@StartSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(NewSessionId);
            cmdToExecute.Parameters.Add("@EndSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(StartSeqNoFlag.ToString()); ;
            cmdToExecute.Parameters.Add("@TTLJackpot", SqlDbType.Money).Value = Jackpot_Score.ToString();
            cmdToExecute.Parameters.Add("@ActionTime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
            cmdToExecute.Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = Stake_Score.ToString();
            cmdToExecute.Parameters.Add("@YaMa", SqlDbType.Money).Value = Stake_Score.ToString();
            cmdToExecute.Parameters.Add("@TableFee", SqlDbType.Money).Value = TableFee;
            cmdToExecute.Parameters.Add("@Commission", SqlDbType.Money).Value = Commission;
            SqlParameter returnState = cmdToExecute.Parameters.Add("@ReturnState", SqlDbType.Int);
            returnState.Direction = ParameterDirection.Output;
            cmdToExecute.ExecuteNonQuery();
            InsertResult = int.Parse(returnState.Value.ToString());
        }
        else
        {
            objstrg.Append(string.Format("，修改補單，ID︰{0}", stake_id));
            //表示已經有補單了，只要針對現有補單再修改
            cmdText = @"UPDATE TOP(1) t
			                           SET t.Stake_Score   = ISNULL(@Stake_Score, t.Stake_Score)
                                         , t.Account_Score = ISNULL(@Account_Score, t.Account_Score)
                                         , t.YouXiaoYaFen  = ISNULL(@YouXiaoYaFen, t.YouXiaoYaFen)
                                         , t.YaMa          = ISNULL(@YouXiaoYaFen, t.YaMa)
                                         , t.MaHao         = ISNULL(@MaHao, t.MaHao) --如果有要異動就去掉注解
                                         , t.StartSeqNoFlag  = ISNULL(@StartSeqNoFlag, t.StartSeqNoFlag) 
                                         , t.EndSeqNoFlag    = ISNULL(@EndSeqNoFlag, t.EndSeqNoFlag) 
                                         , t.ReportTime      = ISNULL(@ReportTime, t.ReportTime) 
                                         , t.Datetime        = ISNULL(@Datetime, t.Datetime)  
                                         , t.TableFee        = ISNULL(@TableFee, t.TableFee) 
                                         , t.Commission      = ISNULL(@Commission, t.Commission)
                                         , t.Jackpot_Score   = ISNULL(@Jackpot_Score, t.Jackpot_Score)
                                      FROM T_Club_Stake_Current t with(RowLock)
			                          WHERE t.Id = @Id";
            cmdToExecute = new SqlCommand(cmdText, this._mainConnection);
            cmdToExecute.CommandType = CommandType.Text;
            cmdToExecute.Parameters.Add("@id", SqlDbType.BigInt).Value = stake_id;
            cmdToExecute.Parameters.Add("@Stake_Score", SqlDbType.Money).Value = Stake_Score.ToString();
            cmdToExecute.Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = Stake_Score.ToString();
            cmdToExecute.Parameters.Add("@Account_Score", SqlDbType.Money).Value = Account_Score.ToString();
            cmdToExecute.Parameters.Add("@MaHao", SqlDbType.VarChar).Value = MaHao.ToString();
            cmdToExecute.Parameters.Add("@StartSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(NewSessionId.ToString());
            cmdToExecute.Parameters.Add("@EndSeqNoFlag", SqlDbType.BigInt).Value = Int64.Parse(StartSeqNoFlag.ToString());
            cmdToExecute.Parameters.Add("@ReportTime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
            cmdToExecute.Parameters.Add("@Datetime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
            cmdToExecute.Parameters.Add("@TableFee", SqlDbType.Money).Value = TableFee.ToString();
            cmdToExecute.Parameters.Add("@Commission", SqlDbType.Money).Value = Commission.ToString();
            cmdToExecute.Parameters.Add("@Jackpot_Score", SqlDbType.Money).Value = Jackpot_Score.ToString();
            InsertResult = cmdToExecute.ExecuteNonQuery();
        }

        objstrg.Append(string.Format("，結果︰{0}", InsertResult == 1 ? "true" : "false"));

        Lib.WritLog("dbSQL.cs.HandleEditStakeHistory()", objstrg.ToString());

        return InsertResult == 1 ? true : false;
    }
    /// <summary>
    /// 針對過帳前改單
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Stake_Score"></param>
    /// <param name="Account_Score"></param>
    /// <param name="StartSeqNoFlag"></param>
    /// <param name="Rows"></param>
    /// <returns></returns>
    public bool HandleEditStakeCurrent(string Id,string Stake_Score,string Account_Score,string StartSeqNoFlag,string Rows,string TableFee,string Commission)
    {
        string MaHao = string.Empty;
        string cmdText = string.Empty;
        int InsertResult = 0;
        SqlCommand cmdToExecute = new SqlCommand();
        cmdText = @"UPDATE TOP(1) t
			           SET t.Stake_Score   = ISNULL(@Stake_Score, t.Stake_Score)
                         , t.Account_Score = ISNULL(@Account_Score, t.Account_Score)
                         , t.YouXiaoYaFen  = ISNULL(@YouXiaoYaFen, t.YouXiaoYaFen)
                         , t.YaMa          = ISNULL(@YouXiaoYaFen, t.YaMa)
                         , t.MaHao         = ISNULL(@MaHao, t.MaHao) --如果有要異動就去掉注解
                         , t.TableFee      = ISNULL(@TableFee, t.TableFee)
                         , t.Commission    = ISNULL(@Commission, t.Commission)
                      FROM T_Club_Stake_Current t with(RowLock)
			         WHERE t.Id = @Id";
        if (this._mainConnection.State != ConnectionState.Open)
        {
            this._mainConnection.Open();
        }
        MaHao = string.Format("{0},,0.00,{1},0", Rows.ToString(), StartSeqNoFlag.ToString());
        cmdToExecute = new SqlCommand(cmdText, this._mainConnection);
        cmdToExecute.CommandType = CommandType.Text;
        cmdToExecute.Parameters.Add("@id", SqlDbType.BigInt).Value = Id.ToString();
        cmdToExecute.Parameters.Add("@Stake_Score", SqlDbType.Money).Value = Stake_Score.ToString();
        cmdToExecute.Parameters.Add("@YouXiaoYaFen", SqlDbType.Money).Value = Stake_Score.ToString();
        cmdToExecute.Parameters.Add("@Account_Score", SqlDbType.Money).Value = Account_Score.ToString();
        cmdToExecute.Parameters.Add("@MaHao", SqlDbType.VarChar).Value = MaHao.ToString();
        cmdToExecute.Parameters.Add("@TableFee", SqlDbType.Money).Value = TableFee;
        cmdToExecute.Parameters.Add("@Commission", SqlDbType.Money).Value = Commission;
        InsertResult = cmdToExecute.ExecuteNonQuery();
        return InsertResult == 1 ? true : false;
    }

    #endregion

}