using System;

public class dbJBTool : dbConnectionBase
{
    #region ªì©l¤Æ
    String ConnectionString = "JBTool.ConnectionString";

    public dbJBTool()
    {
        base.InitClass(ConnectionString);
    }
    #endregion
}
