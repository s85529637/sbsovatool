using System;

/// <summary>
/// dbMonSQL 的摘要描述
/// </summary>
public class dbMonSQL : dbConnectionBase
{
    #region 初始化
    String ConnectionString = "Mon.ConnectionString";

    public dbMonSQL()
    {
        base.InitClass(ConnectionString);
    }
    #endregion
}

