using System;

public class dbCossMS : dbConnectionBase
{
    #region ªì©l¤Æ
    String ConnectionString = "CossMS.ConnectionString";

    public dbCossMS()
    {
        base.InitClass(ConnectionString);
    }
    #endregion
}
