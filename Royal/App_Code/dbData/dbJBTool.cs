using System;

public class dbJBTool : dbConnectionBase
{
    #region ��l��
    String ConnectionString = "JBTool.ConnectionString";

    public dbJBTool()
    {
        base.InitClass(ConnectionString);
    }
    #endregion
}
