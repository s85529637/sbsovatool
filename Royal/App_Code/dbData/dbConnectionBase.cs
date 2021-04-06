using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public abstract class dbConnectionBase : IDisposable
{
    #region 初始化
    protected int _errorCode;
    private bool _isDisposed;
    protected SqlConnection _mainConnection;
    protected bool _mainConnectionIsCreatedLocal;
    protected dbConnectionProvider _mainConnectionProvider;
    protected int _rowsAffected;
    protected string _connectionStringKey = "";

    public dbConnectionBase()
    {
        this.InitClass();
    }
    public dbConnectionBase(String ConnectionString)
    {
        this.InitClass(ConnectionString);
    }

    public virtual bool Delete()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (!this._isDisposed && isDisposing)
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Close();
                this._mainConnection.Dispose();
                this._mainConnectionIsCreatedLocal = false;
            }
            this._mainConnectionProvider = null;
            this._mainConnection = null;
        }
        this._isDisposed = true;
    }

    protected void InitClass(string ConnectionStringKey = "Main.ConnectionString")
    {
        this._connectionStringKey = ConnectionStringKey;
        this._mainConnection = new SqlConnection();
        this._mainConnectionIsCreatedLocal = true;
        this._mainConnectionProvider = null;
        AppSettingsReader reader = new AppSettingsReader();
        this._mainConnection.ConnectionString = reader.GetValue(ConnectionStringKey, typeof(string)).ToString();
        this._errorCode = 0;
        this._isDisposed = false;
    }
    #endregion

    #region 共用方法
    public DataTable SelectSQL(SqlCommand cmdToExecute)
    {
        DataTable table2;
        DataTable dataTable = new DataTable("RT");
        SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);
        cmdToExecute.Connection = this._mainConnection;
        AppSettingsReader reader = new AppSettingsReader();
        cmdToExecute.CommandTimeout = int.Parse(reader.GetValue("TimeOut", typeof(string)).ToString());
        try
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Open();
            }
            else if (this._mainConnectionProvider.IsTransactionPending)
            {
                cmdToExecute.Transaction = this._mainConnectionProvider.CurrentTransaction;
            }
            adapter.Fill(dataTable);
            table2 = dataTable;
        }
        catch (Exception exception)
        {
            throw new Exception("SelectSQL::" + cmdToExecute.CommandText + "::Error occured.", exception);
        }
        finally
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Close();
            }
            cmdToExecute.Dispose();
            adapter.Dispose();
        }
        return table2;
    }

    /// <summary>
    /// 傳回DataSet
    /// </summary>
    /// <param name="cmdToExecute">SqlCommand物件</param>
    /// <returns></returns>
    public DataSet SelectSQL_DataSet(SqlCommand cmdToExecute)
    {
        DataSet ds = new DataSet("Root");
        DataTable[] Td;
        SqlDataAdapter adapter = new SqlDataAdapter(cmdToExecute);
        cmdToExecute.Connection = this._mainConnection;
        AppSettingsReader reader = new AppSettingsReader();
        cmdToExecute.CommandTimeout = int.Parse(reader.GetValue("TimeOut", typeof(string)).ToString());
        try
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Open();
            }
            else if (this._mainConnectionProvider.IsTransactionPending)
            {
                cmdToExecute.Transaction = this._mainConnectionProvider.CurrentTransaction;
            }

            adapter.Fill(ds);
        }
        catch (Exception exception)
        {
            throw new Exception("SelectSQL::" + cmdToExecute.CommandText + "::Error occured.", exception);
        }
        finally
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Close();
            }
            cmdToExecute.Dispose();
            adapter.Dispose();
        }
        return ds;
    }


    public int RunSQL(SqlCommand cmdToExecute)
    {
        int num;
        cmdToExecute.Connection = this._mainConnection;
        AppSettingsReader reader = new AppSettingsReader();
        cmdToExecute.CommandTimeout = int.Parse(reader.GetValue("TimeOut", typeof(string)).ToString());
        try
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Open();
            }
            else if (this._mainConnectionProvider.IsTransactionPending)
            {
                cmdToExecute.Transaction = this._mainConnectionProvider.CurrentTransaction;
            }
            num = cmdToExecute.ExecuteNonQuery();
        }
        catch (Exception exception)
        {
            throw new Exception("RunSQL::" + cmdToExecute.CommandText + "::Error occured.", exception);
        }
        finally
        {
            if (this._mainConnectionIsCreatedLocal)
            {
                this._mainConnection.Close();
            }
            cmdToExecute.Dispose();
        }
        return num;
    }
    #endregion

    #region 未使用
    public dbConnectionProvider MainConnectionProvider
    {
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(_connectionStringKey, "Null passed as value to this property which is not allowed.");
            }
            if (this._mainConnection != null)
            {
                if (this._mainConnectionIsCreatedLocal)
                {
                    this._mainConnection.Close();
                    this._mainConnection.Dispose();
                }
                this._mainConnection = null;
            }
            this._mainConnectionProvider = value;
            this._mainConnection = this._mainConnectionProvider.DBConnection;
            this._mainConnectionIsCreatedLocal = false;
        }
    }
    #endregion
}

