using System;

/// <summary>
/// dbSovaSQL 的摘要描述
/// </summary>
public class dbSovaSQL : dbConnectionBase
{
    #region 初始化
    String ConnectionString = "Sova.ConnectionString";

    public dbSovaSQL()
    {
        base.InitClass(ConnectionString);
    }
    #endregion
}

