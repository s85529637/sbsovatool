using System;
using System.Data;
using System.Data.SqlClient;

public class bsSQL
{
    #region Jumbo

    #region 取得尊博開關密碼
    public string GetJBGameStartPassword()
    {
        dbJBTool bsql = new dbJBTool();
        string cmdText = string.Empty;
        cmdText = "SELECT User_pwd FROM S_User WHERE User_ID='op02'";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable rt = bsql.SelectSQL(cmdToExecute);

        return (rt.Rows.Count > 0) ? rt.Rows[0][0].ToString() : "";
    }
    #endregion

    #region 設定尊博開關密碼
    public int SetJBGameStartPassword(String Password)
    {
        dbJBTool bsql = new dbJBTool();
        string cmdText = string.Empty;

        cmdText = "UPDATE S_User SET User_pwd=@Password WHERE User_ID='op02'";

        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Password", Password);
        int rt = bsql.RunSQL(cmdToExecute);
        return rt;
    }
    #endregion

    #region Jumbo洗分
    public DataTable ReturnAccount_Jumbo(string playerId,
                                         string webId,
                                         string sessionNo,
                                         string password,
                                         double amount,
                                         double gameSeqNo_min,
                                         double gameSeqNo_max,
                                         int mNum,
                                         int mType,
                                         string location,
                                         int playCurrencyRate,
                                         string startTime,
                                         string endTime,
                                         double ttlBet,
                                         double ttlWin,
                                         double ttlWinGame,
                                         double ttlWinGamble,
                                         double ttlWinJackpot,
                                         double ttlPrepay,
                                         double ttlNetWin,
                                         int Rows)
    {
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "[dbo].[A_wsjb_PlayerReturnAccount]";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@PlayerId", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, playerId));
        cmd.Parameters.Add(new SqlParameter("@WebId", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, webId));
        cmd.Parameters.Add(new SqlParameter("@SessionNo", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, sessionNo));
        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, password));
        cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, amount));
        cmd.Parameters.Add(new SqlParameter("@GameSeqNo_Min", SqlDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, gameSeqNo_min));
        cmd.Parameters.Add(new SqlParameter("@GameSeqNo_Max", SqlDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, gameSeqNo_max));
        cmd.Parameters.Add(new SqlParameter("@MNum", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, mNum));
        cmd.Parameters.Add(new SqlParameter("@MType", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, mType));
        cmd.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, location));
        cmd.Parameters.Add(new SqlParameter("@PlayCurrencyRate", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, playCurrencyRate));
        cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, startTime));
        cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, endTime));
        cmd.Parameters.Add(new SqlParameter("@TTLBet", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlBet));
        cmd.Parameters.Add(new SqlParameter("@TTLWin", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlWin));
        cmd.Parameters.Add(new SqlParameter("@TTLWinGame", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlWinGame));
        cmd.Parameters.Add(new SqlParameter("@TTLWinGamable", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlWinGamble));
        cmd.Parameters.Add(new SqlParameter("@TTLWinJackpot", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlWinJackpot));
        cmd.Parameters.Add(new SqlParameter("@TTLPrepay", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlPrepay));
        cmd.Parameters.Add(new SqlParameter("@TTLNetWin", SqlDbType.Money, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, ttlNetWin));
        cmd.Parameters.Add(new SqlParameter("@Rows", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Rows));
        cmd.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, 0));
        cmd.Parameters.Add(new SqlParameter("@iReturnRows", SqlDbType.Int, 4, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, 0));

        dt = bsql.SelectSQL(cmd);

        return dt;
    }
    #endregion

    #region Jumbo手動洗分
    public DataTable ManualReturnAccount_Jumbo(string PlayerEName, string ReturnFlag)
    {
        dbSQL bsql = new dbSQL();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "[dbo].[M_ManualReturnJumbo]";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@Club_Ename", SqlDbType.VarChar, 50)).Value = PlayerEName;
        cmd.Parameters.Add(new SqlParameter("@ReturnFlag", SqlDbType.VarChar, 50)).Value = ReturnFlag;

        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得Jumbo未洗分會員
    public DataTable GetUnReturnAccount_Jumbo(string ClubEname, string ThirdPartyId)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = @"
SELECT TOP 1 TC.Club_id,
             Club_Ename,
             CASE
               WHEN login = 1
                    AND Lock = 1
                    AND Login_EGame = 0
                    AND Login_Room = 1
                    AND Login_Game_Id = @ThirdPartyId + 'Room'
                    AND Login_Server_Id = @ThirdPartyId + 'Room'
                    AND Now_XinYong >= 0
                    AND Logout_Xinyong = 0
                    THEN '已登入/已洗分'
               WHEN Login_EGame = 1
                    AND Now_XinYong = 0
                    AND Logout_Xinyong >= 0
                    AND Login_Game_Id = @ThirdPartyId + 'Game'
                    AND Login_Server_Id = @ThirdPartyId + 'Game'
                    THEN '已開分'
               WHEN login = 0
                    AND Lock = 0
                    AND Login_EGame = 0
                    AND Login_Room = 0
                    AND Login_Game_Id = 'Room'
                    AND Login_Server_Id = 'Room'
                    AND Logout_Xinyong = 0
                    THEN '已登出'
               ELSE '需人工辨識'
             END AS 會員狀態,
             CASE
               WHEN TJIN.SessionNo IS NOT NULL
					AND TJ.SessionNo IS NOT NULL 
					and TJ.Web_id='Recovery'
					THEN '已工具洗分'
               WHEN TJIN.SessionNo IS NULL 
					AND TJ.SessionNo IS NULL
					THEN '未開過分'
               WHEN TJIN.SessionNo IS NOT NULL 
					AND TJ.SessionNo IS NULL
					THEN '已開分'
               WHEN TJIN.SessionNo IS NOT NULL
					AND TJ.SessionNo IS NOT NULL 
					THEN '已洗分'
               ELSE '需人工辨識'
             END AS 開分狀態,
             Login,
             Lock,
             CONVERT(INT,Login_EGame)AS Login_EGame,
             CONVERT(INT,Login_Room)AS Login_Room,
             Login_Game_Id,
             Login_Server_Id,
             Now_XinYong,
             Logout_Xinyong AS TC_Logout_Xinyong,
             TJIN.SessionNo AS '開SessionNo',
             CASE 
				WHEN TJ.SessionNo IS NULL THEN 'NULL'
				ELSE TJ.SessionNo 
			 END AS '洗SessionNo',
			 TJIN.CreateTime AS '開Time',
			 TJ.CreateTime AS '洗Time',
			 CASE
				WHEN JB.SessionNo IS NULL THEN '0'
				ELSE (SELECT COUNT(1) FROM [Jumbo].online_game.dbo.GameHistory JBC WITH(NOLOCK) WHERE JBC.SessionNo= JB.SessionNo)
			 END AS 'JumboAccountCount'
			 --,JB.*	
FROM   T_Club TC WITH(NOLOCK)
       LEFT JOIN T_A_JBSessionNo_IN TJIN WITH(NOLOCK) ON TC.Club_id = TJIN.Club_id
       LEFT JOIN T_A_JBSessionNo TJ WITH(NOLOCK) ON TC.Club_id = TJ.Club_id AND TJIN.SessionNo = TJ.SessionNo
       LEFT JOIN [Jumbo].online_game.dbo.GameHistory JB WITH(NOLOCK) ON  TJIN.SessionNo = JB.SessionNo
WHERE  ( Club_Ename = @Club_Ename )
ORDER BY TJIN.uid DESC";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Ename", ClubEname);
        cmdToExecute.Parameters.AddWithValue("@ThirdPartyId", ThirdPartyId);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 取得Jumbo未洗分下注資料
    public DataTable GetUnReturnAccount_JumboData(string ClubId, string SessionNo)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = @"
DECLARE @AfterGameCredits [numeric](14, 4)
DECLARE @GameSeqNo_Max [numeric](20, 0)
DECLARE @MNum INT,@MType INT,@PlayCurrencyRate INT
DECLARE @Location [varchar](50)
DECLARE @EndTime DATETIME
DECLARE @GameHistory TABLE
  (
     [GameSeqNo]            [NUMERIC](20, 0) NOT NULL,
     [SessionNo]            [NUMERIC](20, 0) NOT NULL,
     [PlayerId]             [VARCHAR](20) NOT NULL,
     [WebId]                [VARCHAR](6) NOT NULL,
     [MNum]                 [INT] NOT NULL,
     [MType]                [INT] NOT NULL,
     [Location]             [VARCHAR](50) NOT NULL,
     [AccDenom]             [INT] NOT NULL,
     [PlayDenom]            [INT] NOT NULL,
     [PlayNational]         [INT] NOT NULL,
     [PlayCurrencyRate]     [INT] NOT NULL,
     [HasGamble]            [INT] NOT NULL,
     [StartTime]            [DATETIME] NOT NULL,
     [EndTime]              [DATETIME] NOT NULL,
     [TTLBet]               [NUMERIC](14, 4) NOT NULL,
     [TTLWin]               [NUMERIC](14, 4) NOT NULL,
     [TTLWinGame]           [NUMERIC](14, 4) NOT NULL,
     [TTLWinGamble]         [NUMERIC](14, 4) NOT NULL,
     [TTLWinJackpot]        [NUMERIC](14, 4) NOT NULL,
     [TTLPrepay]            [NUMERIC](14, 4) NOT NULL,
     [TTLNetWin]            [NUMERIC](14, 4) NOT NULL,
     [TTLJackpotAccumulate] [NUMERIC](14, 4) NOT NULL,
     [RewardPoints]         [NUMERIC](10, 4) NOT NULL,
     [RewardRate]           [NUMERIC](6, 2) NOT NULL,
     [RewardMultiplier]     [INT] NOT NULL,
     [BeforeGameCredits]    [NUMERIC](14, 4) NOT NULL,
     [AfterGameCredits]     [NUMERIC](14, 4) NOT NULL,
     [Status]               [INT] NOT NULL,
     [LastModifyTime]       [DATETIME] NOT NULL
  )

INSERT INTO @GameHistory
SELECT * FROM [Jumbo].online_game.dbo.GameHistory WITH(NOLOCK)
WHERE  SessionNo = @SessionNo AND PlayerId = @ClubId

SELECT TOP 1 @AfterGameCredits=AfterGameCredits,
             @GameSeqNo_Max=GameSeqNo,
             @MNum=MNum,
             @MType=MType,
             @Location=Location,
             @PlayCurrencyRate=PlayCurrencyRate,
             @EndTime=EndTime
FROM @GameHistory ORDER BY gameseqno DESC

--SELECT * FROM @GameHistory

SELECT
@ClubId										   AS PlayerId
,@SessionNo									   AS SessionNo
,@AfterGameCredits							   AS Amount
,(SELECT MIN(GameSeqNo) FROM @GameHistory)	   AS GameSeqNo_Min
,@GameSeqNo_Max								   AS GameSeqNo_Max
,@MNum										   AS MNum
,@MType										   AS MType
,@Location									   AS Location
,@PlayCurrencyRate							   AS PlayCurrencyRate
,(SELECT MIN(StartTime) FROM @GameHistory)	   AS StartTime
,@EndTime									   AS EndTime
,(SELECT SUM(TTLBet) FROM @GameHistory)		   AS TTLBet
,(SELECT SUM(TTLWin) FROM @GameHistory)        AS TTLWin
,(SELECT SUM(TTLWinGame) FROM @GameHistory)    AS TTLWinGame
,(SELECT SUM(TTLWinGamble) FROM @GameHistory)  AS TTLWinGamble
,(SELECT SUM(TTLWinJackpot) FROM @GameHistory) AS TTLWinJackpot
,(SELECT SUM(TTLPrepay) FROM @GameHistory)     AS TTLPrepay
,(SELECT SUM(TTLNetWin) FROM @GameHistory)     AS TTLNetWin
,(SELECT COUNT(1) FROM @GameHistory)		   AS [Rows] ";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@SessionNo", SessionNo);
        cmdToExecute.Parameters.AddWithValue("@ClubId", ClubId);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 恢復會員狀態Jumbo
    public DataTable RecoveryClubJumbo(string Club_id)
    {
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.CommandText = "A_wsjb_RecoveryClub";
        cmdToExecute.CommandType = CommandType.StoredProcedure;
        cmdToExecute.Parameters.AddWithValue("@Club_id", Club_id);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 取得會員Jumbo最新開分資料
    public DataTable GetUserJumboSessionIn(string Club_Id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = "SELECT TOP 1 Club_id, Web_id, SessionNo, CreateTime FROM T_A_JBSessionNo_IN WITH(NOLOCK) WHERE Club_id=@Club_Id ORDER BY SessionNo DESC";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Id", Club_Id);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 取得會員Jumbo最新洗分資料
    public DataTable GetUserJumboSession(string Club_Id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = "SELECT TOP 1 Club_id, Web_id, SessionNo, CreateTime FROM T_A_JBSessionNo WITH(NOLOCK) WHERE Club_id=@Club_Id ORDER BY SessionNo DESC";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Id", Club_Id);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #endregion

    #region Sova
    public DataTable ManualReturnAccount_Sova(string PlayerEName, string ReturnFlag)
    {
        dbSQL bsql = new dbSQL();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "[dbo].[M_ManualReturnSova]";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@Club_Ename", SqlDbType.VarChar, 50)).Value = PlayerEName;
        cmd.Parameters.Add(new SqlParameter("@ReturnFlag", SqlDbType.VarChar, 50)).Value = ReturnFlag;

        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region Golden

    #region 取得Ruby4未洗分會員
    public DataTable GetUnReturnAccount_Ruby4(string ClubEname, string ThirdPartyId)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = @"
SELECT TOP 1 TC.Club_id,
             Club_Ename,
             CASE
               WHEN login = 1
                    AND Lock = 1
                    AND Login_EGame = 0
                    AND Login_Room = 1
                    AND Login_Game_Id = @ThirdPartyId + 'Room'
                    AND Login_Server_Id = @ThirdPartyId + 'Room'
                    AND Now_XinYong >= 0
                    AND Logout_Xinyong = 0
                    AND TS.Status <> 0 THEN '已登入/已洗分'
               WHEN Login_EGame = 1
                    AND Now_XinYong = 0
                    AND Logout_Xinyong >= 0
                    AND TS.Status = 0 THEN '已開分'
               WHEN login = 0
                    AND Lock = 0
                    AND Login_EGame = 0
                    AND Login_Room = 0
                    AND Login_Game_Id = 'Room'
                    AND Login_Server_Id = 'Room'
                    AND Logout_Xinyong = 0
                    AND TS.Status <> 0 THEN '已登出'
               ELSE '需人工辨識'
             END AS 會員狀態,
             CASE
               WHEN TS.ReturnDateTime IS NOT NULL
                    AND TS.Status <> 0 THEN '已洗分'
               WHEN TS.ReturnDateTime IS NULL
                    AND TS.Status = 0 THEN '已開分'
               WHEN TS.ReturnDateTime IS NULL
                    AND TS.Status <> 0 THEN '已工具恢復'
               ELSE '需人工辨識'
             END AS 開分狀態,
             Login,
             Lock,
             CONVERT(INT,Login_EGame)AS Login_EGame,
             CONVERT(INT,Login_Room)AS Login_Room,
             Login_Game_Id,
             Login_Server_Id,
             Now_XinYong,
             Logout_Xinyong AS TC_Logout_Xinyong,
             TG.ThirdParty_Id,
             TS.GameId,
             TS.SessionNo,
             TS.Status,
             TS.LogoutXinYong AS TS_Logout_Xinyong,
             CASE 
				WHEN TS.ReturnDateTime IS NULL THEN 'NULL'
				ELSE CONVERT(NVARCHAR(20),TS.ReturnDateTime,21) 
			 END AS ReturnDateTime    
FROM   T_Club TC WITH(NOLOCK)
       LEFT JOIN T_Session_Lock TS WITH(NOLOCK) ON TC.Club_id = TS.Club_id
       INNER JOIN T_Game TG WITH(NOLOCK) ON TS.GameId = TG.Game_id
WHERE  ( Club_Ename = @Club_Ename )
ORDER  BY TS.SessionNo DESC ";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Ename", ClubEname);
        cmdToExecute.Parameters.AddWithValue("@ThirdPartyId", ThirdPartyId);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #endregion

    #region 警示系統
    public string GetMyAllUpFranchiser(int Type,string Uid)
    {
        string rvalue = string.Empty;
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();
        cmdText = "F_GetMyAllUpFranchiser";
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.Parameters.Clear();
        cmdToExecute.CommandType = CommandType.StoredProcedure;
        cmdToExecute.CommandText = cmdText;
        cmdToExecute.Parameters.Add("@type", SqlDbType.Int).Value = Type;
        cmdToExecute.Parameters.Add("@Uid", SqlDbType.NVarChar, 20).Value = Uid;
        dt = bsql.SelectSQL(cmdToExecute);
        if (dt.Rows.Count > 0)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (string.IsNullOrEmpty(rvalue))
                {
                    rvalue = dt.Rows[j]["Franchiser_Ename"].ToString();
                }
                else
                {
                    rvalue = string.Format("{0}/{1}", dt.Rows[j]["Franchiser_Ename"].ToString(), rvalue);
                }
            }
        }
       
        return rvalue;
    }
    #endregion

    #region RTG
    /// <summary>
    /// 取得是否正在過帳中
    /// </summary>
    /// <returns>1表示過帳中，過帳完成要查T_ReportDatetime才能確定</returns>
    public string GetZhuanLiShi()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        string rvalue = string.Empty;
        cmdText = "SELECT [Param_value] FROM [T_Sysparameter] where [Param_key] ='ZhuanLiShi_Flag'";  
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable rt = bsql.SelectSQL(cmdToExecute);
        if (rt.Rows.Count > 0)
        {
            foreach (DataRow row in rt.Rows)
            {
                rvalue = row["Param_value"].ToString();
            }
        }else {
            rvalue = "0";
        }

        return rvalue; 
    }

    /// <summary>
    /// 檢查是否已經過帳，若過帳則傳回過帳的日期
    /// </summary>
    /// <returns>傳回已經過帳的日期</returns>
    public DataTable ChkIsHistory()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        string  rvalue = string.Empty;
        cmdText = @"SELECT * FROM [T_ReportDatetime] where [End_Datetime] between @startdate and @enddate ";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.Add("@startdate", SqlDbType.DateTime);
        cmdToExecute.Parameters["@startdate"].Value = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        cmdToExecute.Parameters.Add("@enddate", SqlDbType.DateTime);
        cmdToExecute.Parameters["@enddate"].Value = DateTime.Now.ToString("yyyy-MM-dd 23:59:59"); ;
        DataTable rt = bsql.SelectSQL(cmdToExecute); 
        return rt;
    }

    /// <summary>
    /// 取得目前改單帳務資料最高編號
    /// </summary>
    /// <returns></returns>
    public int GetMaxId()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = "SELECT Param_value FROM T_Sysparameter WHERE (Param_key = 'RTG_WashReviseNo')";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable rt = bsql.SelectSQL(cmdToExecute);
        return (rt.Rows.Count > 0) ? int.Parse(rt.Rows[0][0].ToString()) : 0;
    }

    public int SetMaxId(int NewId)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = "UPDATE T_Sysparameter WITH(ROWLOCK) SET Param_value='{0}' WHERE (Param_key = 'RTG_WashReviseNo')"; 
       SqlCommand cmdToExecute = new SqlCommand(String.Format(cmdText, NewId.ToString()));
        int rt = bsql.RunSQL(cmdToExecute);
        return rt;
    }

    /// <summary>
    /// 從日帳撈資料
    /// </summary>
    /// <param name="sessionid"></param>
    /// <param name="Club_id"></param>
    /// <returns></returns>
    public DataTable GetStakeCurrentData(string sessionid,string Club_id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        /*
        cmdText = @"SELECT   CAST( LEFT(t.[MaHao],CHARINDEX(',',t.[MaHao],1)-1) as int) as Rows,
                             Club_id,
                             Now_XinYong,
                             Account_Score,
                             Datetime,
                             Game_id,   
                             Jackpot_Score,
        					 CAST(SUBSTRING(t.[MaHao],CHARINDEX(',0.00,',t.[MaHao],1)+6,17) as varchar) as StartSeqNoFlag,
						     Id,
							 Stake_Score,
                             'N' as IsHistory,
                              Stake_id,
                              ZhuDan_Type,
                              Active,
                              Desk_id,
                              No_Run,
                              No_Active,
                              JiTai_No            
                    FROM T_Club_Stake_Current t with(nolock) WHERE CAST(SUBSTRING(t.[MaHao],CHARINDEX(',0.00,',t.[MaHao],1)+6,17) as varchar) = @SessionId and Club_id=@Club_id ";
        */
        cmdText = @"SELECT   CAST( LEFT(t.[MaHao],CHARINDEX(',',t.[MaHao],1)-1) as int) as Rows,
                             Club_id,
                             Now_XinYong,
                             Account_Score,
                             Datetime,
                             Game_id,   
                             Jackpot_Score,
        					 EndSeqNoFlag as StartSeqNoFlag,
						     Id,
							 Stake_Score,
                             'N' as IsHistory,
                              Stake_id,
                              ZhuDan_Type,
                              Active,
                              Desk_id,
                              No_Run,
                              No_Active,
                              JiTai_No,
                              [TableFee],
                              [Commission]            
                    FROM T_Club_Stake_Current t with(nolock) WHERE EndSeqNoFlag = @SessionId and Club_id=@Club_id ";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.Add("@SessionId", SqlDbType.VarChar);
        cmdToExecute.Parameters["@SessionId"].Value = sessionid;
        cmdToExecute.Parameters.Add("@Club_id", SqlDbType.VarChar);
        cmdToExecute.Parameters["@Club_id"].Value = Club_id;
        DataTable rt = bsql.SelectSQL(cmdToExecute);
        return rt;
    }


    /// <summary>
    /// 從月帳撈資料
    /// </summary>
    /// <param name="sessionid"></param>
    /// <param name="Club_id"></param>
    /// <returns></returns>
    public DataTable GetStakeHistoryData(string sessionid, string Club_id)
    {
        dbSQL bsql = new dbSQL("Mon.ConnectionString");
        string cmdText = string.Empty;
        /* cmdText = @"SELECT   CAST( LEFT(t.[MaHao],CHARINDEX(',',t.[MaHao],1)-1) as int) as Rows,
                              Club_id,
                              Now_XinYong,
                              Account_Score,
                              Datetime,
                              Game_id,   
                              Jackpot_Score,
                              CAST(SUBSTRING(t.[MaHao],CHARINDEX(',0.00,',t.[MaHao],1)+6,17) as varchar) as StartSeqNoFlag,
                              Id,
                              Stake_Score,
                              'N' as IsHistory,
                               Stake_id,
                               ZhuDan_Type,
                               Active,
                               Desk_id,
                               No_Run,
                               No_Active,
                               JiTai_No          
                     FROM T_Club_Stake_History t with(nolock) WHERE CAST(SUBSTRING(t.[MaHao],CHARINDEX(',0.00,',t.[MaHao],1)+6,17) as varchar) = @SessionId and Club_id=@Club_id ";
         */
        cmdText = @"SELECT   CAST( LEFT(t.[MaHao],CHARINDEX(',',t.[MaHao],1)-1) as int) as Rows,
                             Club_id,
                             Now_XinYong,
                             Account_Score,
                             Datetime,
                             Game_id,   
                             Jackpot_Score,
        					 EndSeqNoFlag as StartSeqNoFlag,
						     Id,
							 Stake_Score,
                             'N' as IsHistory,
                              Stake_id,
                              ZhuDan_Type,
                              Active,
                              Desk_id,
                              No_Run,
                              No_Active,
                              JiTai_No,
                              [TableFee],
                              [Commission]          
                    FROM T_Club_Stake_History t with(nolock) WHERE EndSeqNoFlag = @SessionId and Club_id=@Club_id ";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.Add("@SessionId", SqlDbType.VarChar);
        cmdToExecute.Parameters["@SessionId"].Value = sessionid;
        cmdToExecute.Parameters.Add("@Club_id", SqlDbType.VarChar);
        cmdToExecute.Parameters["@Club_id"].Value = Club_id;
        DataTable rt = bsql.SelectSQL(cmdToExecute);
        return rt;
    }

    /// <summary>
    /// 依SessionId及Club_id是否存在
    /// </summary>
    /// <param name="sessionid"></param>
    /// <param name="Club_id"></param>
    /// <returns>存在則回傳ID，不存在則回傳0</returns>
    public Int64 HasStakeCurrentData(string sessionid, string Club_id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        Int64 id = 0;
        /*
        cmdText = @"SELECT  Id        
                    FROM T_Club_Stake_Current t with(nolock) WHERE CAST(SUBSTRING(t.[MaHao],CHARINDEX(',0.00,',t.[MaHao],1)+6,17) as varchar) = @SessionId and Club_id=@Club_id ";
        */
        cmdText = @"SELECT  Id        
                    FROM T_Club_Stake_Current t with(nolock) WHERE EndSeqNoFlag = @SessionId and Club_id=@Club_id ";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.Add("@SessionId", SqlDbType.VarChar);
        cmdToExecute.Parameters["@SessionId"].Value = sessionid;
        cmdToExecute.Parameters.Add("@Club_id", SqlDbType.VarChar);
        cmdToExecute.Parameters["@Club_id"].Value = Club_id;
        DataTable rt = bsql.SelectSQL(cmdToExecute);
        if (rt.Rows.Count > 0)
        {
            foreach (DataRow row in rt.Rows)
            {
                id = Int64.Parse(row["Id"].ToString());
            }
        }
        return id;
    }

    #endregion

    #region JDB
    public DataTable GetWrongUidInfo(string MemberUid, ref string SResultMessage, ref int IResult)
    {
        DataTable dt = null;
        try
        {
            PointCenterApi.ApiReuslt objApiReuslt = null;

            PointCenterApi.Logs ojbPointCenterApi = new PointCenterApi.Logs();

            objApiReuslt = ojbPointCenterApi.WrongUidInfo(MemberUid);

            dt = objApiReuslt.Reuslt;

            IResult = objApiReuslt.IResult;

            SResultMessage = objApiReuslt.SResultMessage;
        }
        catch (Exception ex)
        {
            Lib.WritLog("bsSQL.cs.GetWrongUidInfo()", ex.ToString());
            dt = new DataTable();
        }

        return dt;
    }
    public DataSet NSP_Member_KaDan_Check(string jqu_subsystem, string jqu_website, string jqu_vendor_id, string MemberUid, string MemberAccount, ref string SResultMessage, ref int IResult)
    {
        DataSet ds;
        try
        {
            PointCenterApi.ApiMultReuslt objApiMultReusl = null;

            PointCenterApi.Logs ojbPointCenterApi = new PointCenterApi.Logs();

            objApiMultReusl = ojbPointCenterApi.NSP_Member_KaDan_Check(jqu_subsystem, jqu_website, jqu_vendor_id, MemberUid, MemberAccount);

            ds = objApiMultReusl.Reuslt;

            IResult = objApiMultReusl.IResult;

            SResultMessage = objApiMultReusl.SResultMessage;
        }
        catch (Exception ex)
        {
            Lib.WritLog("bsSQL.cs.NSP_Member_KaDan_Check()", ex.ToString());
            ds = new DataSet();
        }

        return ds;
    }

    /// <summary>
    /// 取得時間內在線數
    /// </summary>
    /// <param name="_SDateTime"></param>
    /// <param name="_EDateTime"></param>
    /// <param name="_Platform"></param>
    /// <returns></returns>
    public DataTable GetOnlineMemberCounts_List(string _SDateTime, string _EDateTime, string _Platform)
    {
        DataTable dt = null;
        string SDateTime = _SDateTime;
        string EDateTime = _EDateTime;
        string Platform = _Platform;
        DateTime tmpSDateTime;
        DateTime tmpEDateTime;
        bool S_isok = true;
        bool E_isok = true;
        try
        {         
            if (!DateTime.TryParse(SDateTime, out tmpSDateTime))
            {
                S_isok = false;
            }

            if (!DateTime.TryParse(EDateTime, out tmpEDateTime))
            {
                E_isok = false;
            }

            if (S_isok == false || E_isok == false)
            {
                return null;

            }
            else if (tmpSDateTime > tmpEDateTime)
            {
                return null;
            }

            dt = OnlineMemberCounts_List( SDateTime, EDateTime, Platform);

        }
        catch (Exception ex)
        {
            Lib.WritLog("bsSQL.cs.GetOnlineMemberCounts_List()", ex.ToString());
            dt = new DataTable();
        }

        return dt;

    }
    #endregion

    #region Royal

    #region 取得皇家電子手動洗分序號
    public int GetRoyalGameReturnFailNo()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = "SELECT Param_value FROM T_Sysparameter WHERE (Param_key = 'Royal_ReturnFailNo')";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable rt = bsql.SelectSQL(cmdToExecute);
        return (rt.Rows.Count > 0) ? int.Parse(rt.Rows[0][0].ToString()) : 0;
    }
    #endregion

    #region 設定皇家電子手動洗分序號
    public int SetRoyalGameReturnFailNo(int iNo)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = "UPDATE T_Sysparameter WITH(ROWLOCK) SET Param_value='{0}' WHERE (Param_key = 'Royal_ReturnFailNo')";
        SqlCommand cmdToExecute = new SqlCommand(String.Format(cmdText, iNo.ToString()));
        int rt = bsql.RunSQL(cmdToExecute);
        return rt;
    }
    #endregion    #region 取得未洗分清單

    #region 取得未洗分清單
    /// <summary>
    /// JDB手動洗分
    /// </summary>
    /// <param name="ThirdPartyID">第三方廠商ID</param>
    /// <param name="ClubEname">會員帳號</param>
    /// <param name="PageIndex">頁碼</param>
    /// <param name="PageSize">一頁資料筆數</param>
    /// <param name="ITotalCount">總筆數</param>
    /// <param name="SResultMessage">錯誤訊息</param>
    /// <param name="IResult">結果0表成功，非0表錯誤</param>
    /// <returns></returns>
    public DataTable GetUnReturnAccount( string ThirdPartyID, string ClubEname,int PageIndex,int PageSize, ref int ITotalCount,ref string SResultMessage,ref int IResult)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();
        cmdText = "NSP_Club_PlayThirdPartyData";
        /*
        cmdText = @"
SELECT top 2000  b.Club_Id ID,b.Club_Ename 會員帳號,a.SessionNo 開分號,c.Game_Id 遊戲代碼,c.Game_name 遊戲名稱,LogoutXinYong 開分額度, c.ThirdParty_Id 廠商代號, a.IP
FROM    dbo.T_Session_Lock a WITH (NOLOCK)
INNER JOIN dbo.T_Club b WITH (NOLOCK)
ON a.Club_id = b.Club_id
INNER JOIN dbo.T_Game c WITH (NOLOCK)
ON a.GameId=c.Game_id
WHERE   a.Status = 0
AND (@Login='All' OR b.Login=0)
AND (@ThirdPartyID='All' OR c.ThirdParty_Id=@ThirdPartyID)
AND (@ClubEname='' OR b.Club_Ename=@ClubEname)";*/
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.Parameters.Clear();
        cmdToExecute.CommandType = CommandType.StoredProcedure;
        cmdToExecute.CommandText = cmdText;
        cmdToExecute.Parameters.Add("@Login", SqlDbType.NVarChar, 10).Value="ALL";
        cmdToExecute.Parameters.Add("@ThirdPartyID", SqlDbType.NVarChar, 10).Value = ThirdPartyID;
        cmdToExecute.Parameters.Add("@ClubEname", SqlDbType.NVarChar, 20).Value = string.IsNullOrEmpty(ClubEname) ? "" : ClubEname ;
        cmdToExecute.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;
        cmdToExecute.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
        SqlParameter TotalCount = cmdToExecute.Parameters.Add("@TotalCount", SqlDbType.Int);
        TotalCount.Direction = ParameterDirection.Output;
        SqlParameter ResultMessage = cmdToExecute.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 4000);
        ResultMessage.Direction = ParameterDirection.Output;
        SqlParameter Result = cmdToExecute.Parameters.Add("@Result", SqlDbType.Int);
        Result.Direction = ParameterDirection.Output;
        dt = bsql.SelectSQL(cmdToExecute);
        ITotalCount = TotalCount.Value != null ? int.Parse(TotalCount.Value.ToString()) : -1;
        SResultMessage = string.IsNullOrEmpty(ResultMessage.Value.ToString()) ? "" : ResultMessage.Value.ToString();
        IResult = Result.Value != null ? int.Parse(Result.Value.ToString()) : -1;
        return dt; 
    }
    #endregion

    #region 取得時間內在線數
    /// <summary>
    /// 取得時間內在線數
    /// </summary>
    /// <param name="SDateTime"></param>
    /// <param name="EDateTime"></param>
    /// <param name="website"></param>
    /// <returns></returns>
    public DataTable OnlineMemberCounts_List(string SDateTime, string EDateTime, string Platform)
    {
        dbSQL bsql = new dbSQL("Moblie.ConnectionString");
        string cmdText = string.Empty;
        DataSet ds;
        cmdText = "NSP_OnlineMemberCounts_List";
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.Parameters.Clear();
        cmdToExecute.CommandType = CommandType.StoredProcedure;
        cmdToExecute.CommandText = cmdText;
        cmdToExecute.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = SDateTime;
        cmdToExecute.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = EDateTime;
        if (!string.IsNullOrEmpty(Platform))
        {
            cmdToExecute.Parameters.Add("@Platform", SqlDbType.VarChar, 10).Value = Platform;
        }
        try
        {
            ds = bsql.SelectSQL_DataSet(cmdToExecute);
        }
        catch (Exception ex)
        {
            Lib.WritLog("bsSQL.cs.OnlineMemberCounts_List()", ex.ToString());
            return new DataTable();
        }
        return ds.Tables[0];
    }
    #endregion

    #region Royal手動洗分
    /// <summary>
    /// Royal洗分
    /// </summary>
    /// <param name="playerId">會員ID</param>
    /// <param name="webId">分桶</param>
    /// <param name="password">密碼</param>
    /// <param name="location">機台資訊</param>
    /// <param name="amount">傳回額度param>
    /// <param name="sessionNo">開分號碼</param>
    /// <param name="gameId"></param>
    /// <param name="stakeScore">總押注額</param>
    /// <param name="accountScore">總得分結果</param>
    /// <param name="Rows">下注筆數</param>
    /// <param name="maxDateTime">最後押注時間</param>
    /// <param name="ttlJackpot">彩金總得分</param>
    /// <returns></returns>
    public DataTable ReturnAccount_Royal(string playerId, string webId, string password, string location, decimal amount, string sessionNo, string gameId, decimal stakeScore, decimal accountScore, int Rows, string maxDateTime, decimal ttlJackpot)
    {       
        int iErrorCode = 0;
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "[dbo].[A_wsms_PlayerReturnAccount]";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@sPlayerId", SqlDbType.VarChar, 20).Value = playerId;
        cmd.Parameters.Add("@sWebId", SqlDbType.VarChar, 6).Value = webId;
        cmd.Parameters.Add("@sPassword", SqlDbType.VarChar, 50).Value = password;
        cmd.Parameters.Add("@Location", SqlDbType.VarChar, 50).Value = location;
        cmd.Parameters.Add("@dPlayerAccount", SqlDbType.Money, 8).Value = amount;
        cmd.Parameters.Add("@dPlayerAccountCheck", SqlDbType.Money, 8).Value = amount;
        cmd.Parameters.Add("@sSessionId", SqlDbType.VarChar, 17).Value = sessionNo;
        cmd.Parameters.Add("@sGameId", SqlDbType.VarChar, 10).Value = gameId;
        cmd.Parameters.Add("@Stake_Score", SqlDbType.Money, 8).Value = stakeScore;
        cmd.Parameters.Add("@Account_Score", SqlDbType.Money, 8).Value = accountScore;
        cmd.Parameters.Add("@iRows", SqlDbType.Int, 4).Value = Rows;
        cmd.Parameters.Add("@MaxDateTime", SqlDbType.DateTime, 8).Value = maxDateTime;
        cmd.Parameters.Add("@TTLJackpot", SqlDbType.Money, 8).Value = ttlJackpot;
        cmd.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, iErrorCode));

        dt = bsql.SelectSQL(cmd);

        return dt;
    }
    #endregion

    #region Royal手動洗分
    /// <summary>
    /// Royal洗分
    /// </summary>
    /// <param name="playerId">會員ID</param>
    /// <param name="webId">分桶</param>
    /// <param name="password">密碼</param>
    /// <param name="thirdpartyid">廠商代碼</param>
    /// <returns></returns>
    public DataTable Logout_Royal(string playerId, string webId, string password, string thirdpartyid)
    {
        int iErrorCode = 0;
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "[dbo].[A_wsms_PlayerLogout]";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@sPlayerId", SqlDbType.VarChar, 20).Value = playerId;
        cmd.Parameters.Add("@sWebId", SqlDbType.VarChar, 6).Value = webId;
        cmd.Parameters.Add("@sPassword", SqlDbType.VarChar, 50).Value = password;
        cmd.Parameters.Add("@sThirdPartyId", SqlDbType.VarChar, 20).Value = thirdpartyid;
        cmd.Parameters.Add(new SqlParameter("@iErrorCode", SqlDbType.Int, 4, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Proposed, iErrorCode));

        dt = bsql.SelectSQL(cmd);

        return dt;
    }
    #endregion

    #region 恢復會員狀態 Sova/Golden/Royal
    public DataTable RecoveryClub(string Club_id, string SessionNo)
    {
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        SqlCommand cmdToExecute = new SqlCommand();
        cmdToExecute.CommandText = "A_ws_RecoveryClub";
        cmdToExecute.CommandType = CommandType.StoredProcedure;
        cmdToExecute.Parameters.AddWithValue("@Club_id", Club_id);
        cmdToExecute.Parameters.AddWithValue("@SessionNo", SessionNo);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 取得會員未洗分資料 Golden/Sova/Royal
    public DataTable GetUserSovaGoldenSession(string Club_Id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = "SELECT TOP 1 * FROM T_Session_Lock WITH(NOLOCK) WHERE Club_id=@Club_Id AND Status=0 ORDER BY SessionNo DESC";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Id", Club_Id);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #endregion

    #region System

    public DataTable SovaGetPlayerInfo(string Club_Ename)
    {
        dbSQL sql = new dbSQL();
        return sql.SovaGetPlayerInfo(Club_Ename);
    }

    #region 取得系統參數
    public string GetSystemParameter(String Param_key)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = "EXEC [S_Get_SysParameter] @Param_key";
        SqlCommand cmd = new SqlCommand(cmdText);
        cmd.Parameters.AddWithValue("@Param_key", Param_key);
        DataTable rt = bsql.SelectSQL(cmd);
        return (rt.Rows.Count > 0) ? rt.Rows[0][0].ToString() : "Error";
    }
    #endregion

    #region 設定系統參數
    public string SetSystemParameter(String Param_key, String Param_value)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = "EXEC [S_Update_SysParameter] @Param_key,@Param_value";
        SqlCommand cmd = new SqlCommand(cmdText);
        cmd.Parameters.AddWithValue("@Param_key", Param_key);
        cmd.Parameters.AddWithValue("@Param_value", Param_value);
        DataTable rt = bsql.SelectSQL(cmd);
        return (rt.Rows.Count > 0) ? rt.Rows[0][0].ToString() : "Error";
    }
    #endregion

    #region 取得未統計資料
    public DataTable GetUnTongJiStakeAccount()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = @"SET DEADLOCK_PRIORITY -10;DELETE T_Club_Stake_Now WITH(rowlock) OUTPUT DELETED.* INTO T_Club_Stake_Now_Del_Log WHERE ReportTime < DATEADD(SECOND, -175, GETDATE()) AND Id IN (SELECT Id FROM T_Club_Stake_Current WITH(nolock));
SELECT Id ,Club_Ename 會員帳號,a.Game_id 遊戲,c.Server_type 桌別,a.No_Run 輪號,a.No_Active 局號,Stake_Score 押注,Account_Score 輸贏,Now_XinYong 額度,ReportTime 時間,a.Active 狀態,Ip ,MaHao 注區
FROM dbo.T_Club_Stake_Now a WITH(NOLOCK)
--INNER JOIN dbo.FN_GetOpenListNow() b
--ON a.Server_id = b.Server_id
INNER JOIN T_Server c WITH(NOLOCK)
ON a.Server_id = c.Server_id
WHERE a.ReportTime < DATEADD(MINUTE, -3, GETDATE()) AND EXISTS(SELECT 1 FROM dbo.T_TongJi_Log t WITH(nolock) WHERE t.Server_id = CAST(a.Server_id AS varchar(20)) AND t.No_Run = CAST(a.No_Run AS varchar(20)) AND t.No_Active = CAST(a.No_Active AS varchar(20)))";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 搬移未統計資料
    public String MoveUnTongJiStakeAccount(String id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = @"
DECLARE @Error INT
SET @Error = 0
INSERT  INTO T_Club_Stake_Current
        SELECT  *
        FROM    T_Club_Stake_Now WITH (NOLOCK)
        WHERE   id = @id
SET @Error = @Error + @@Error

IF @Error = 0 
    DELETE  T_Club_Stake_Now WITH (ROWLOCK)
    WHERE   id = @id
SET @Error = @Error + @@Error

SELECT  @Error
";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@id", id);
        DataTable dt = bsql.SelectSQL(cmdToExecute);
        return (dt.Rows.Count > 0) ? dt.Rows[0][0].ToString() : "";
    }
    #endregion

    #region 取得視訊遊戲清單 - Game_Class = 1 AND Active = 1

    public DataTable getVideoGameList()
    {
        DataTable result = new DataTable();

        dbSQL dbLink = new dbSQL();

        string cmdText = string.Empty;

        SqlCommand cmdToExecute;

        cmdText =
        @"SELECT
            [T_Server].[Server_id]
         ,  [T_Server].[Server_ip]
         ,  [T_Server].[Server_port]
         ,  [T_Server].[Server_Name]
         ,  [T_Server].[Game_id]
         ,  [T_Server].[Server_Type]
         ,  [T_Server].[Active]
         ,  [T_Server].[Video_1]
         ,  [T_Server].[Video_2]
          FROM
            [T_Server]
          INNER JOIN
            [T_Game]
          ON
            [T_Server].[Game_id]  = [T_Game].[Game_id]
          AND
            [T_Game].[Game_Class] = 1
          AND
            [T_Server].[Active]   = 1
          ORDER BY
            [T_Game].[Game_id]
          , [T_Server].[Server_Type]";

        cmdToExecute = new SqlCommand(cmdText);

        result = dbLink.SelectSQL(cmdToExecute);

        ////

        return result;
    }

    #endregion

    #region 更新視訊遊戲設定 - IP , PORT , VideoURL

    public int setUpdateVGameSetting(string serverId, string serverIP, int serverPort, string video_1, string video_2)
    {
        int result = 0;

        dbSQL bsql = new dbSQL();

        string cmdText = string.Empty;

        DataTable resultTable;

        ////

        cmdText = "EXEC [dbo].[NSP_UpdateVGameSetting] @serverId , @serverIP , @serverPort , @video1 , @video2";

        SqlCommand cmdToExecute = new SqlCommand(cmdText);

        ////

        cmdToExecute.Parameters.AddWithValue("@serverId", serverId);

        cmdToExecute.Parameters.AddWithValue("@serverIP", serverIP);

        cmdToExecute.Parameters.AddWithValue("@serverPort", serverPort);

        cmdToExecute.Parameters.AddWithValue("@video1", video_1);

        cmdToExecute.Parameters.AddWithValue("@video2", video_2);

        ////

        resultTable = bsql.SelectSQL(cmdToExecute);

        ////

        result = int.Parse(resultTable.Rows[0][0].ToString());

        ////

        return result;
    }

    #endregion

    #region 取得會員帳號狀態
    public DataTable GetUserStat(string ClubEname)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        //Club_id, Club_Ename, Club_Cname, Franchiser_id, Password, Max_XinYong, Now_XinYong, Datetime, Active, PanZu, MAC, JieSuan_Time, Login, OnlineTime, IP, ChongZhi, VIP_Flag, Login_Game_Id, Login_Server_Id, TingYong_XinYong, Lock, Open_Server_id, DongJie_Flag, msrepl_tran_version, Logout_Xinyong, Login_EGame, Login_Room, UidKey, FKey, LimitLevel, PlayerReturnTime, Test_Flag
        cmdText = "SELECT Club_id, Now_XinYong, Logout_Xinyong, Login, Lock, Active, DongJie_Flag, Login_Game_Id, Login_Server_Id, Login_EGame, Login_Room, OnlineTime, IP FROM T_Club WITH(NOLOCK) WHERE Club_Ename=@Club_Ename";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Ename", ClubEname);
        dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 復原會員狀態
    public int RecoveryClubStat(string Club_Id)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        DataTable dt = new DataTable();

        cmdText = "UPDATE T_Club WITH(ROWLOCK) SET Login=0,Login_Game_Id='Room',Login_Server_Id='Room',Login_EGame=0,Login_Room=0,Lock=0 WHERE Club_Id = @Club_Id";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@Club_Id", Club_Id);
        int i = bsql.RunSQL(cmdToExecute);
        return i;
    }
    #endregion

    #region 取得遊戲總人數
    public DataTable GetGameMemberCount(string StartDate)
    {
        string SQL = "EXEC [R_SelectGameReport] @StartDate";
        SqlCommand cmd = new SqlCommand(SQL);
        cmd.Parameters.AddWithValue("@StartDate", StartDate);

        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #endregion

    #region Account

    #region 取得遊戲人數
    public int GameMembers()
    {
        string SQL = "SELECT COUNT(DISTINCT Club_id) FROM T_Club_Stake_Current";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        if (dt.Rows.Count > 0)
            return int.Parse(dt.Rows[0][0].ToString());
        else
            return 0;
    }

    public int GameMembers(String GameId)
    {
        switch (GameId)
        {
            case "Bacc":
            case "InsuBacc":
            case "LongHu":
            case "FanTan":
            case "LunPan":
            case "ShaiZi":
            case "YuXiaXie":
                string SQL = "SELECT COUNT(DISTINCT Club_id) FROM T_Club_Stake_Current WHERE Game_id = @Game_id";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@Game_id", GameId);
                dbSQL bsql = new dbSQL();
                DataTable dt = bsql.SelectSQL(cmd);
                if (dt.Rows.Count > 0)
                    return int.Parse(dt.Rows[0][0].ToString());
                else
                    return 0;
            default:
                return 0;
        }
    }
    #endregion

    #region 取得各注單數量
    public DataTable GetMaHaoListCount()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = @"
SELECT Game_id,Account_Score, YaMa,
CASE WHEN Game_id IN (SELECT Game_id FROM T_Game WHERE ThirdParty_Id IS NOT NULL) THEN 'EGame' ELSE MaHao END MaHao
INTO #GameTemp
FROM T_Club_Stake_Current
SELECT '嬴'AS TP, Game_id,MaHao, COUNT(*) AS iCount,SUM(Account_Score) AS Account_Score, SUM(YaMa)AS YaMa FROM #GameTemp
WHERE Account_Score >0
GROUP BY Game_id,MaHao
UNION ALL 
SELECT '輸'AS TP, Game_id,MaHao, COUNT(*) AS iCount ,SUM(Account_Score) AS Account_Score, SUM(YaMa)AS YaMa FROM #GameTemp
WHERE Account_Score < 0
GROUP BY Game_id,MaHao
UNION ALL 
SELECT '合'AS TP, Game_id,MaHao, COUNT(*) AS iCount ,SUM(Account_Score) AS Account_Score, SUM(YaMa)AS YaMa FROM #GameTemp
WHERE Account_Score = 0
GROUP BY Game_id,MaHao
ORDER BY Game_id,TP,MaHao";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 取得近三日注單總量
    public DataTable GetDailyStakeCount()
    {
        dbSQL bsql = new dbSQL();
        string cmdText = string.Empty;
        cmdText = @"
    SELECT TOP 3 [Id]
          ,[Qishu_Name]
          ,[MinID]
          ,[MaxID]
          ,[RowsCount]
          ,[ctime]
    FROM [T_Club_Stake_Count]
    ORDER by ctime DESC";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        DataTable dt = bsql.SelectSQL(cmdToExecute);
        return dt;
    }
    #endregion

    #region 取得轉帳開關
    public int MoveAccountCheck()
    {
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        string cmdText = @"EXEC [M_MoveAccount_Check]";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        dt = bsql.SelectSQL(cmdToExecute);
        if (dt.Rows.Count > 0)
            return int.Parse(dt.Rows[0][0].ToString());
        else
            return 1;
    }
    #endregion

    #region 取得系統參數
    public String GetSytemParameter(String ParamKey)
    {
        dbSQL bsql = new dbSQL();
        DataTable dt = new DataTable();
        string cmdText = @"SELECT Param_value FROM T_Sysparameter WHERE Param_key = @ParamKey";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@ParamKey", ParamKey);
        dt = bsql.SelectSQL(cmdToExecute);
        if (dt.Rows.Count > 0)
            return dt.Rows[0][0].ToString();
        else
            return "";
    }
    #endregion

    #region 設定系統參數
    /// <summary>
    /// 設定系統參數
    /// </summary>
    /// <param name="ParamKey">要更新的參數</param>
    /// <param name="ParamValue">要更新的值</param>
    /// <returns></returns>
    public int SetSytemParameter(String ParamKey, String ParamValue)
    {
        dbSQL bsql = new dbSQL();
        string cmdText = @"
UPDATE T_Sysparameter
SET Param_Value = @ParamValue
WHERE Param_key = @ParamKey";
        SqlCommand cmdToExecute = new SqlCommand(cmdText);
        cmdToExecute.Parameters.AddWithValue("@ParamKey", ParamKey);
        cmdToExecute.Parameters.AddWithValue("@ParamValue", ParamValue);
        int i = bsql.RunSQL(cmdToExecute);
        return i;
    }
    #endregion

    #region 搬移開牌記錄
    public DataTable MoveOpenList(String GameId)
    {
        switch (GameId)
        {
            case "Bacc":
            case "InsuBacc":
            case "LongHu":
            case "FanTan":
            case "LunPan":
            case "ShaiZi":
            case "YuXiaXie":
                string SQL = "EXEC M_MoveOpenList_" + GameId;
                SqlCommand cmd = new SqlCommand(SQL);
                dbSQL bsql = new dbSQL();
                DataTable dt = bsql.SelectSQL(cmd);
                return dt;
            default:
                return null;
        }
    }
    #endregion

    #region 刪除ID
    public DataTable DeleteId()
    {
        string SQL = "EXEC M_DeleteId";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 過帳初始化
    public DataTable MoveAccountInit()
    {
        string SQL = "EXEC M_MoveAccount_Init";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 更新會員額度
    public DataTable UpdateClubXinYong()
    {
        string SQL = "EXEC M_UpdateClub_XinYong";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 轉帳主檔
    public DataTable MoveAccountMain()
    {
        string SQL = "EXEC M_MoveAccount_Main";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 轉帳明細
    public DataTable MoveToMonNetGameHJ2()
    {
        string SQL = "EXEC Z_MoveTo_MonNetGame_HJ2";
        SqlCommand cmd = new SqlCommand(SQL);
        dbMonSQL bsql = new dbMonSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得當前開牌紀錄
    public DataTable GetOpenListNow()
    {
        string SQL = "SELECT * FROM FN_GetOpenListNow()";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得伺服器ID
    public DataTable GetServerId(String GameServer)
    {
        string SQL = "SELECT Server_id FROM T_Server WITH(NOLOCK) WHERE Game_id+Server_type=@GameServer";
        SqlCommand cmd = new SqlCommand(SQL);
        cmd.Parameters.AddWithValue("@GameServer", GameServer);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得當前開牌紀錄
    public DataTable GetOpenListNow(String GameId, String ServerId)
    {
        String TableName = "T_{0}_Openlist_Now", GameName = "";
        switch (GameId)
        {
            case "Bacc":
                GameName = "Baccarat";
                break;
            default:
                GameName = GameId;
                break;
        }
        TableName = String.Format(TableName, GameName);
        string SQL = "SELECT * FROM {0} WITH(NOLOCK) WHERE Server_Id=@ServerId";
        SQL = String.Format(SQL, TableName);
        SqlCommand cmd = new SqlCommand(SQL);
        cmd.Parameters.AddWithValue("@ServerId", ServerId);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得局號總量
    public DataTable GetActiveCount(String GameId, String ServerId)
    {
        String TableName = "T_{0}_Openlist", GameName = "";
        switch (GameId)
        {
            case "Bacc":
                GameName = "Baccarat";
                break;
            default:
                GameName = GameId;
                break;
        }
        TableName = String.Format(TableName, GameName);
        string SQL = "SELECT COUNT(*) FROM {0} WITH(NOLOCK) WHERE Server_Id=@ServerId";
        SQL = String.Format(SQL, TableName);
        SqlCommand cmd = new SqlCommand(SQL);
        cmd.Parameters.AddWithValue("@ServerId", ServerId);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #endregion

    #region Message

    #region 取得會員當前跑馬燈
    public DataTable GetClubMessageNow()
    {
        string SQL = @"
SELECT a.Message_Big5 ,
       a.Message_Gb ,
	   a.Message_En ,
	   a.Message_Tg ,
	   a.Datetime ,
	   --a.Active ,
	   (ISNULL(Game_Id ,'')+ISNULL(server_type ,'')) ServerName
FROM   T_Message a WITH (NOLOCK)
LEFT JOIN T_MessageByServerId b WITH (NOLOCK)
ON a.Message_Id = b.MessageId
LEFT JOIN T_Server c WITH (NOLOCK)
ON b.ServerId = c.Server_Id
WHERE ( (a.Active = 1 AND a.End_Datetime >= GETDATE()) OR a.Active = 2) AND a.Class = 'C'
ORDER BY a.Active
";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得會員歷史跑馬燈
    public DataTable GetClubMessageOld()
    {
        string SQL = @"
SELECT TOP 10 a.Message_Big5 ,
              a.Message_Gb ,
			  a.Message_En ,
			  a.Message_Tg ,
			  a.Datetime ,
			  --Active ,
              (ISNULL(Game_Id ,'')+ISNULL(Server_Type ,'')) ServerName
FROM   T_Message a WITH (NOLOCK)
LEFT JOIN T_MessageByServerId b WITH (NOLOCK)
ON a.Message_Id = b.MessageId
LEFT JOIN T_Server c WITH (NOLOCK)
ON b.ServerId = c.Server_Id
WHERE a.Active = 0 AND a.Class = 'C'
ORDER BY Datetime DESC
";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得代理當前跑馬燈
    public DataTable GetFranMessageNow()
    {
        string SQL = @"
SELECT Message_Big5 ,
       Message_Gb ,
	   Message_En ,
	   Message_Tg ,
	   Datetime 
	   --Active
FROM   T_Message
WHERE ( (Active = 1 AND End_Datetime >= GETDATE()) OR Active = 2) AND Class = 'F'
ORDER BY Active
";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    #region 取得代理歷史跑馬燈
    public DataTable GetFranMessageOld()
    {
        string SQL = @"
SELECT TOP 10 Message_Big5 ,
              Message_Gb ,
			  Message_En ,
			  Message_Tg ,
			  Datetime 
			  --Active ,
			  --Message_id
FROM          T_Message WITH (NOLOCK)
WHERE         Active = 0 AND Class = 'F'
ORDER BY Datetime DESC
";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }
    #endregion

    public DataTable TestDelay(int iSec)
    {
        string SQL = "WAITFOR DELAY '00:" + iSec.ToString("D2") + "';";
        SqlCommand cmd = new SqlCommand(SQL);
        dbSQL bsql = new dbSQL();
        DataTable dt = bsql.SelectSQL(cmd);
        return dt;
    }

    #endregion
}