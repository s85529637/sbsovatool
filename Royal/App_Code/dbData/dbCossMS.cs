using System;

public class dbCossMS : dbConnectionBase
{
    #region ��l��
    String ConnectionString = "CossMS.ConnectionString";

    public dbCossMS()
    {
        base.InitClass(ConnectionString);
    }
    #endregion
}
