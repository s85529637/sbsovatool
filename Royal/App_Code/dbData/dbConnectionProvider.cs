using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class dbConnectionProvider : IDisposable
{
    private SqlTransaction _currentTransaction;
    private SqlConnection _dBConnection;
    private bool _isDisposed;
    private bool _isTransactionPending;
    private ArrayList _savePoints;

    public dbConnectionProvider()
    {
        this.InitClass();
    }

    public bool BeginTransaction(string transactionName)
    {
        bool flag;
        try
        {
            if (this._isTransactionPending)
            {
                throw new Exception("BeginTransaction::Already transaction pending. Nesting not allowed");
            }
            if ((this._dBConnection.State & ConnectionState.Open) == ConnectionState.Closed)
            {
                throw new Exception("BeginTransaction::Connection is not open.");
            }
            this._currentTransaction = this._dBConnection.BeginTransaction(IsolationLevel.ReadCommitted, transactionName);
            this._isTransactionPending = true;
            this._savePoints.Clear();
            flag = true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
        return flag;
    }

    public bool CloseConnection(bool commitPendingTransaction)
    {
        bool flag;
        try
        {
            if ((this._dBConnection.State & ConnectionState.Open) == ConnectionState.Closed)
            {
                return false;
            }
            if (this._isTransactionPending)
            {
                if (commitPendingTransaction)
                {
                    this._currentTransaction.Commit();
                }
                else
                {
                    this._currentTransaction.Rollback();
                }
                this._isTransactionPending = false;
                this._currentTransaction.Dispose();
                this._currentTransaction = null;
                this._savePoints.Clear();
            }
            this._dBConnection.Close();
            flag = true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
        return flag;
    }

    public bool CommitTransaction()
    {
        bool flag;
        try
        {
            if (!this._isTransactionPending)
            {
                throw new Exception("CommitTransaction::No transaction pending.");
            }
            if ((this._dBConnection.State & ConnectionState.Open) == ConnectionState.Closed)
            {
                throw new Exception("CommitTransaction::Connection is not open.");
            }
            this._currentTransaction.Commit();
            this._isTransactionPending = false;
            this._currentTransaction.Dispose();
            this._currentTransaction = null;
            this._savePoints.Clear();
            flag = true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
        return flag;
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
            if (this._currentTransaction != null)
            {
                this._currentTransaction.Dispose();
                this._currentTransaction = null;
            }
            if (this._dBConnection != null)
            {
                this._dBConnection.Close();
                this._dBConnection.Dispose();
                this._dBConnection = null;
            }
        }
        this._isDisposed = true;
    }

    private void InitClass()
    {
        this._dBConnection = new SqlConnection();
        AppSettingsReader reader = new AppSettingsReader();
        this._dBConnection.ConnectionString = reader.GetValue("Main.ConnectionString", typeof(string)).ToString();
        this._isDisposed = false;
        this._currentTransaction = null;
        this._isTransactionPending = false;
        this._savePoints = new ArrayList();
    }

    public bool OpenConnection()
    {
        bool flag;
        try
        {
            if ((this._dBConnection.State & ConnectionState.Open) > ConnectionState.Closed)
            {
                throw new Exception("OpenConnection::Connection is already open.");
            }
            this._dBConnection.Open();
            this._isTransactionPending = false;
            flag = true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
        return flag;
    }

    public bool RollbackTransaction(string transactionToRollback)
    {
        bool flag;
        try
        {
            if (!this._isTransactionPending)
            {
                throw new Exception("RollbackTransaction::No transaction pending.");
            }
            if ((this._dBConnection.State & ConnectionState.Open) == ConnectionState.Closed)
            {
                throw new Exception("RollbackTransaction::Connection is not open.");
            }
            this._currentTransaction.Rollback(transactionToRollback);
            if (!this._savePoints.Contains(transactionToRollback))
            {
                this._isTransactionPending = false;
                this._currentTransaction.Dispose();
                this._currentTransaction = null;
                this._savePoints.Clear();
            }
            flag = true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
        return flag;
    }

    public bool SaveTransaction(string savePointName)
    {
        bool flag;
        try
        {
            if (!this._isTransactionPending)
            {
                throw new Exception("SaveTransaction::No transaction pending.");
            }
            if ((this._dBConnection.State & ConnectionState.Open) == ConnectionState.Closed)
            {
                throw new Exception("SaveTransaction::Connection is not open.");
            }
            this._currentTransaction.Save(savePointName);
            this._savePoints.Add(savePointName);
            flag = true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
        return flag;
    }

    public SqlTransaction CurrentTransaction
    {
        get
        {
            return this._currentTransaction;
        }
    }

    public SqlConnection DBConnection
    {
        get
        {
            return this._dBConnection;
        }
    }

    public bool IsTransactionPending
    {
        get
        {
            return this._isTransactionPending;
        }
    }
}

