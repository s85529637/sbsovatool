using System.Linq;

namespace bs
{
    #region SqlBuilder
    /// <summary>
    /// SqlBuilder
    /// </summary>
    public sealed partial class SqlBuilder
    {
        #region 欄位
        /// <summary>
        /// 用來存放 SQL 語法
        /// </summary>
        private System.Text.StringBuilder _SqlString = new System.Text.StringBuilder();

        /// <summary>
        /// 用來存放參數值
        /// </summary>
        private System.Collections.Generic.List<object> _SqlParams = new System.Collections.Generic.List<object>();
        #endregion

        #region 建構式
        /// <summary>
        /// 建構式
        /// </summary>
        public SqlBuilder() { }
        #endregion

        #region 加入參數值
        /// <summary>
        /// 加入選擇性參數值
        /// </summary>
        /// <param name="opt">是否執行</param>
        /// <param name="values">不具名參數值(陣列)</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AddOptParams(bool opt, params object[] values)
        {
            if (opt == true)
            {
                this._SqlParams.AddRange(values);
            }

            //
            return this;
        }

        /// <summary>
        /// 加入選擇性參數值
        /// </summary>
        /// <param name="opt">是否執行</param>
        /// <param name="value">不具名參數值</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AddOptParams(bool opt, object value)
        {
            return this.AddOptParams(opt, new object[] { value });
        }

        /// <summary>
        /// 加入參數值
        /// </summary>
        /// <param name="values">不具名參數值(陣列)</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AddParams(params object[] values)
        {
            return this.AddOptParams(true, values);
        }

        /// <summary>
        /// 加入參數值
        /// </summary>
        /// <param name="value">不具名參數值</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AddParams(object value)
        {
            return this.AddOptParams(true, new object[] { value });
        }
        #endregion

        #region 附加 SQL 語法片段
        /// <summary>
        /// 附加 SQL 語法片段
        /// </summary>
        /// <param name="sql">SQL 語法片段</param>
        /// <param name="opt">是否執行</param>
        /// <param name="args">String Format 參數值(陣列)</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AppendLine(string sql, bool opt, params string[] args)
        {
            sql += string.Empty;

            //
            if (opt == true)
            {
                this._SqlString.Append(System.Environment.NewLine);
                if (args == null || args.Length == 0)
                {
                    this._SqlString.Append(sql);
                }
                else
                {
                    this._SqlString.Append(string.Format(sql, args));
                }
            }

            //
            return this;
        }

        /// <summary>
        /// 附加 SQL 語法片段
        /// </summary>
        /// <param name="sql">SQL 語法片段</param>
        /// <param name="opt">是否執行</param>
        /// <param name="arg">String Format 參數值</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AppendLine(string sql, bool opt, string arg)
        {
            return this.AppendLine(sql, opt, new string[] { arg });
        }

        /// <summary>
        /// 附加 SQL 語法片段
        /// </summary>
        /// <param name="sql">SQL 語法片段</param>
        /// <param name="args">String Format 參數值(陣列)</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AppendLine(string sql, params string[] args)
        {
            return this.AppendLine(sql, true, args);
        }

        /// <summary>
        /// 附加 SQL 語法片段
        /// </summary>
        /// <param name="sql">SQL 語法片段</param>
        /// <param name="arg">String Format 參數值</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AppendLine(string sql, string arg)
        {
            return this.AppendLine(sql, true, new string[] { arg });
        }

        /// <summary>
        /// 附加 SqlBuilder 區塊，通常為子查詢，區塊會做縮排
        /// </summary>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder AppendBlock(SqlBuilder sqlBuilder)
        {
            this._SqlString.Append(System.Environment.NewLine);
            this._SqlString.Append(string.Join(System.Environment.NewLine, sqlBuilder.ToString().Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries).Select(x => "        " + x)));
            this._SqlString.Append(System.Environment.NewLine);
            this._SqlParams.AddRange(sqlBuilder.GetParams());

            //
            return this;
        }
        #endregion

        #region 取代字串
        /// <summary>
        /// 取代字串
        /// </summary>
        /// <param name="oldValue">舊字串</param>
        /// <param name="newValue">新字串</param>
        /// <param name="opt">是否執行</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder Replace(string oldValue, string newValue, bool opt)
        {
            oldValue += string.Empty;
            newValue += string.Empty;

            //
            if (opt == true)
            {
                this._SqlString.Replace(oldValue, newValue);
            }

            //
            return this;
        }

        /// <summary>
        /// 取代字串
        /// </summary>
        /// <param name="oldValue">舊字串</param>
        /// <param name="newValue">新字串</param>
        /// <returns>傳回 SqlBuilder</returns>
        public SqlBuilder Replace(string oldValue, string newValue)
        {
            return Replace(oldValue, newValue, true);
        }
        #endregion

        #region 清除 SQL 語法及參數值
        /// <summary>
        /// 清除 SQL 語法及參數值
        /// </summary>
        public void Clear()
        {
            this._SqlString.Clear();
            this._SqlParams.Clear();
        }
        #endregion

        #region 取得所有的參數值
        /// <summary>
        /// 取得所有的參數值
        /// </summary>
        /// <returns>傳回 Object 陣列</returns>
        public object[] GetParams()
        {
            return this._SqlParams.ToArray();
        }
        #endregion

        #region 取得 SQL 語法
        /// <summary>
        /// 取得 SQL 語法
        /// </summary>
        /// <returns>傳回字串</returns>
        public override string ToString()
        {
            return System.Environment.NewLine + string.Join(System.Environment.NewLine, this._SqlString.ToString().Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimEnd())) + System.Environment.NewLine;
        }
        #endregion

        #region 解構式
        /// <summary>
        /// 解構式
        /// </summary>
        ~SqlBuilder()
        {
            this._SqlString.Clear();
            this._SqlParams.Clear();
            this._SqlString = null;
            this._SqlParams = null;
        }
        #endregion
    }
    #endregion

    #region Database Provider Factory
    /// <summary>
    /// Database Provider Factory
    /// </summary>
    public static partial class DbPF
    {
        /// <summary>
        /// SQL 查詢 Timeout 時間
        /// </summary>
        private const int _CommandTimeout = 300;

        #region 取得本機電腦可用的資料庫 Provider
        /// <summary>
        /// 取得本機電腦可用的資料庫 Provider
        /// </summary>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetLocalhostDbProvider()
        {
            return System.Data.Common.DbProviderFactories.GetFactoryClasses();
        }
        #endregion

        #region 取得資料庫相關設定
        /// <summary>
        /// Get Provider Name
        /// </summary>
        /// <param name="tagName">App.config or Web.config Tag Name</param>
        /// <returns>傳回 ProviderName</returns>
        public static string GetProviderName(string tagName)
        {
            tagName += string.Empty;

            // Get Provider Name
            string providerName = System.Configuration.ConfigurationManager.ConnectionStrings[tagName].ProviderName + string.Empty;
            if (providerName.Trim().Length == 0)
            {
                providerName = System.Data.SqlClient.SqlClientFactory.Instance.GetType().Namespace;
            }

            //
            return providerName;
        }

        /// <summary>
        /// Get Provider Name
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <returns>傳回 Provider Name</returns>
        public static string GetProviderName(System.Data.Common.DbConnection dbConnection)
        {
            try
            {
                // Get Provider Name
                string providerName;
                switch (dbConnection.GetType().Namespace)
                {
                    case "System.Data.SqlServerCe":
                        providerName = "Microsoft.SqlServerCe.Client";
                        break;
                    default:
                        providerName = dbConnection.GetType().Namespace;
                        break;
                }

                //
                return providerName;
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Get Parameter Symbol
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <returns>傳回 Parameter Symbol</returns>
        public static string GetParamSymbol(System.Data.Common.DbConnection dbConnection)
        {
            try
            {
                // Get Parameter Symbol
                string paramSymbol;
                switch (dbConnection.GetType().Namespace)
                {
                    case "System.Data.OracleClient":
                        paramSymbol = ":";
                        break;
                    case "System.Data.OleDb":
                        paramSymbol = "?";
                        break;
                    case "System.Data.Odbc":
                        paramSymbol = "?";
                        break;
                    default:
                        paramSymbol = "@";
                        break;
                }

                //
                return paramSymbol;
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Get ConnectionString
        /// </summary>
        /// <param name="tagName">App.config or Web.config Tag Name</param>
        /// <returns>傳回 ConnectionString</returns>
        public static string GetConnString(string tagName)
        {
            tagName += string.Empty;

            //
            return System.Configuration.ConfigurationManager.ConnectionStrings[tagName].ConnectionString;
        }
        #endregion

        #region 解析 SQL 語法的參數名稱
        /// <summary>
        /// 取得所有的參數名稱
        /// </summary>
        /// <param name="paramSymbol">參數符號：SQL Server 請用【@】、Oracle 請用【:】、OleDb and Odbc 請用【?】</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <returns>傳回參數名稱 List</returns>
        public static System.Collections.Generic.List<string> GetParamNames(string paramSymbol, string sqlStatement)
        {
            paramSymbol += string.Empty;
            sqlStatement += string.Empty;

            // 包含 SQL 語法的 StringBuilder
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder(sqlStatement + " ");

            // 用來暫存正在解析的參數名稱的 StringBuilder
            System.Text.StringBuilder sbParamName = new System.Text.StringBuilder();

            // 用來存放解析完的參數名稱的 List
            System.Collections.Generic.List<string> listParamNames = new System.Collections.Generic.List<string>();

            // 參數名稱解析
            while (sbSql.ToString().IndexOf(paramSymbol, System.StringComparison.Ordinal) >= 0)
            {
                // 移除 SQL 語法第一個參數符號前的字串
                sbSql.Remove(0, sbSql.ToString().IndexOf(paramSymbol, System.StringComparison.Ordinal));

                // 逐個解析參數名稱
                sbParamName.Remove(0, sbParamName.Length);
                for (int i = 0; i < int.MaxValue; i++)
                {
                    // 要剖析的符號必須包含在規則運算式中
                    if (System.Text.RegularExpressions.Regex.IsMatch(sbSql.ToString(0, 1), "[" + paramSymbol + "0-9A-Za-z_]") == true)
                    {
                        sbParamName.Append(sbSql.ToString(0, 1));
                        sbSql.Remove(0, 1);
                    }
                    else
                    {
                        listParamNames.Add(sbParamName.ToString());
                        break;
                    }
                }
            }

            //
            return listParamNames;
        }
        #endregion

        #region 將不具名參數(?)的 SQL 語法轉為指定具名參數(例如：@)的 SQL 語法
        /// <summary>
        /// 將不具名參數(?)的 SQL 語法轉為指定具名參數(例如：@)的 SQL 語法
        /// </summary>
        /// <param name="paramSymbol">指定的參數符號</param>
        /// <param name="sqlStatement">要轉換的 SQL 語法</param>
        /// <returns>傳回轉換完成的 SQL 語法</returns>
        public static string ToNamedParamSql(string paramSymbol, string sqlStatement)
        {
            paramSymbol += string.Empty;
            sqlStatement += string.Empty;

            // 如果指定的符號為 ? 則不用再轉換
            if (paramSymbol.Trim() == "?")
            {
                return sqlStatement;
            }
            else
            {
                // 存放舊 SQL 語法的 StringBuilder
                System.Text.StringBuilder sbOldSql = new System.Text.StringBuilder(sqlStatement);

                // 存放新 SQL 語法的 StringBuilder
                System.Text.StringBuilder sbNewSql = new System.Text.StringBuilder(sqlStatement.Length);

                // 執行轉換
                int i = 0;
                while (sbOldSql.ToString().IndexOf("?", System.StringComparison.Ordinal) >= 0)
                {
                    i++;
                    sbNewSql.Append(sbOldSql.ToString(), 0, sbOldSql.ToString().IndexOf("?", System.StringComparison.Ordinal));
                    sbNewSql.Append(paramSymbol + "P" + i.ToString());
                    sbOldSql.Remove(0, sbOldSql.ToString().IndexOf("?", System.StringComparison.Ordinal) + 1);
                }
                sbNewSql.Append(sbOldSql.ToString());

                //
                return sbNewSql.ToString();
            }
        }
        #endregion

        #region 將特殊符號取代為底線(通常用來處理資料表名稱或欄位名稱的特殊符號)
        /// <summary>
        /// 將特殊符號取代為底線(通常用來處理資料表名稱或欄位名稱的特殊符號)
        /// </summary>
        /// <param name="name">原本的名稱</param>
        /// <returns>傳回替代後的名稱</returns>
        public static string GetReplacedName(string name)
        {
            // 參考：https://docs.microsoft.com/zh-tw/dotnet/standard/base-types/character-classes-in-regular-expressions
            // 參考：https://docs.microsoft.com/zh-tw/dotnet/standard/base-types/regular-expression-language-quick-reference
            const string cstReplacePattern = @"[\s\p{C}\p{M}\p{P}\p{S}\p{Z}-[.]]"; // 排除【.】

            //
            return System.Text.RegularExpressions.Regex.Replace(name + string.Empty, cstReplacePattern, "_");
        }
        #endregion

        #region 將 DataRow 轉換成 TEntity
        /// <summary>
        /// 將 DataRow 轉換成 TEntity
        /// </summary>
        /// <typeparam name="TEntity">要轉換的型別</typeparam>
        /// <param name="dataRow">DataRow</param>
        /// <param name="entity">Entity</param>
        /// <returns>傳回 TEntity</returns>
        public static TEntity DataRowToEntity<TEntity>(System.Data.DataRow dataRow, TEntity entity = null) where TEntity : class, new()
        {
            if (entity == null) { entity = new TEntity(); }

            //
            if (dataRow != null)
            {
                using (System.Data.DataTable tmpTable = dataRow.Table.Clone())
                {
                    tmpTable.Rows.Add(dataRow.ItemArray);

                    // 處理欄位名稱的特殊符號(規則與 ADO.ORM 相同)
                    foreach (System.Data.DataColumn column in tmpTable.Columns)
                    {
                        column.ColumnName = GetReplacedName(column.ColumnName);
                    }

                    //
                    var members = entity.GetType().GetProperties().Where(x => x.CanWrite == true && tmpTable.Columns.Contains(x.Name));
                    foreach (var member in members)
                    {
                        // TODO：型別判斷的規則可能需要修改
                        if (member.PropertyType == tmpTable.Columns[member.Name].DataType || (member.PropertyType.IsValueType == true && tmpTable.Columns[member.Name].DataType.IsValueType == true))
                        {
                            // 設定值，真的無法設定值就略過
                            try
                            {
                                member.SetValue(entity, tmpTable.Rows[0][member.Name] == System.DBNull.Value ? null : tmpTable.Rows[0][member.Name], null);
                            }
                            catch { }
                        }
                    }
                }
            }

            //
            return entity;
        }
        #endregion

        #region 將 DataTable 轉換成 TEntity 列舉值
        /// <summary>
        /// 將 DataTable 轉換成 TEntity 列舉值
        /// </summary>
        /// <typeparam name="TEntity">要轉換的型別</typeparam>
        /// <param name="dataTable">DataTable</param>
        /// <returns>傳回 List</returns>
        public static System.Collections.Generic.IEnumerable<TEntity> DataTableToEntities<TEntity>(System.Data.DataTable dataTable) where TEntity : class, new()
        {
            //// 舊版寫法
            //System.Collections.Generic.List<TEntity> entities = new System.Collections.Generic.List<TEntity>();
            //if (dataTable != null)
            //{
            //    foreach (System.Data.DataRow dataRow in dataTable.Rows)
            //    {
            //        entities.Add(DataRowToEntity<TEntity>(dataRow));
            //    }
            //}

            ////
            //return entities;

            ////////////////////////////////////////////////////////////////////////////////////////////////////

            // 新版寫法(效能較好)
            System.Collections.Generic.List<TEntity> entities = new System.Collections.Generic.List<TEntity>();
            if (dataTable != null)
            {
                using (System.Data.DataTable tmpTable = dataTable.Clone())
                {
                    // 處理欄位名稱的特殊符號(規則與 ADO.ORM 相同)
                    foreach (System.Data.DataColumn column in tmpTable.Columns)
                    {
                        column.ColumnName = GetReplacedName(column.ColumnName);
                    }

                    // 取得相同名稱的 Properties
                    var members = new TEntity().GetType().GetProperties().Where(x => x.CanWrite == true && tmpTable.Columns.Contains(x.Name));

                    // 逐筆進行轉換
                    foreach (System.Data.DataRow dataRow in dataTable.Rows)
                    {
                        TEntity entity = new TEntity();

                        //
                        //if (dataRow != null)
                        //{
                        tmpTable.Rows.Clear();
                        tmpTable.Rows.Add(dataRow.ItemArray);

                        //
                        foreach (var member in members)
                        {
                            // TODO：型別判斷的規則可能需要修改
                            if (member.PropertyType == tmpTable.Columns[member.Name].DataType || (member.PropertyType.IsValueType == true && tmpTable.Columns[member.Name].DataType.IsValueType == true))
                            {
                                // 設定值，真的無法設定值就略過
                                try
                                {
                                    member.SetValue(entity, tmpTable.Rows[0][member.Name] == System.DBNull.Value ? null : tmpTable.Rows[0][member.Name], null);
                                }
                                catch { }
                            }
                        }
                        //}

                        //
                        entities.Add(entity);
                    }
                }
            }

            //
            return entities;
        }
        #endregion

        #region DbProviderFactory
        /// <summary>
        /// 加入參數
        /// </summary>
        /// <param name="dbCommand">DbCommand</param>
        /// <param name="parameters">Parameters</param>
        private static void AddParams(System.Data.Common.DbCommand dbCommand, params object[] parameters)
        {
            // Get Parameter Names
            System.Collections.Generic.List<string> listParamNames = GetParamNames(GetParamSymbol(dbCommand.Connection), dbCommand.CommandText);

            try
            {
                //if (listParamNames.Count != parameters.Length)
                //{
                //    throw new System.Exception("參數數量與參數值數量不符");
                //}

                // Add Parameters
                dbCommand.Parameters.Clear();
                for (int i = 0; i < listParamNames.Count; i++)
                {
                    System.Data.Common.DbParameter param = dbCommand.CreateParameter();

                    if (listParamNames[i].Substring(0, 1) == "?")
                    {
                        param.ParameterName = "P" + i.ToString();
                    }
                    else
                    {
                        param.ParameterName = listParamNames[i];
                    }

                    // 參數值例外處理
                    if (parameters[i] == null || parameters[i] == System.DBNull.Value)
                    {
                        // 值是 Null 就替換為 System.DBNull.Value
                        param.Value = System.DBNull.Value;
                    }
                    else if (parameters[i].GetType().Namespace == typeof(DbPF).Namespace)
                    {
                        // 相同 Namespace 轉成字串
                        param.Value = parameters[i] + string.Empty;
                    }
                    ////else if (parameters[i].GetType() == typeof(bool))
                    ////{
                    ////    // 布林值處理
                    ////    bool bln = (bool)parameters[i];
                    ////    param.Value = bln == true ? 1 : 0;
                    ////}
                    else if (parameters[i].GetType() == typeof(System.DateTime))
                    {
                        // 不是 SQL Server，時間精確度只取到秒
                        if (dbCommand.GetType().Namespace == typeof(System.Data.SqlClient.SqlCommand).Namespace)
                        {
                            //param.Value = parameters[i];

                            // TODO：這一段可能有問題
                            // 轉成 SQL Server DateTime2
                            System.Data.SqlClient.SqlParameter sqlParam = new System.Data.SqlClient.SqlParameter();
                            sqlParam.ParameterName = param.ParameterName;
                            sqlParam.DbType = System.Data.DbType.DateTime2;
                            sqlParam.Value = parameters[i];
                            param = sqlParam;
                        }
                        else
                        {
                            // 時間精確度只取到秒，有些資料庫，時間精確度小於秒會產生例外，例如：Access
                            System.DateTime dateTime = (System.DateTime)parameters[i];
                            param.Value = new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
                        }
                    }
                    else
                    {
                        // 其他
                        param.Value = parameters[i];
                    }

                    //
                    dbCommand.Parameters.Add(param);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(System.Environment.NewLine + "參數數量：" + listParamNames.Count.ToString() + System.Environment.NewLine + "參數值數量：" + parameters.Length.ToString() + System.Environment.NewLine + ex.ToString(), ex);
            }
        }

        /// <summary>
        /// 加入參數
        /// </summary>
        /// <param name="dbCommand">DbCommand</param>
        /// <param name="parameters">Parameters</param>
        private static void AddParams(System.Data.Common.DbCommand dbCommand, System.Collections.ArrayList parameters)
        {
            AddParams(dbCommand, parameters.ToArray());
        }

        /// <summary>
        /// 加入參數
        /// </summary>
        /// <param name="dbCommand">DbCommand</param>
        /// <param name="parameters">Parameters</param>
        private static void AddParams(System.Data.Common.DbCommand dbCommand, System.Collections.Generic.List<object> parameters)
        {
            AddParams(dbCommand, parameters.ToArray());
        }

        // ****************************************************************************************************

        /// <summary>
        /// Create DbConnection
        /// </summary>
        /// <param name="tagName">App.config or Web.config Tag Name</param>
        /// <returns>傳回 DbConnection</returns>
        public static System.Data.Common.DbConnection CreateDbConnection(string tagName)
        {
            // Get DbConnection
            System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(GetProviderName(tagName));
            System.Data.Common.DbConnection dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = GetConnString(tagName);

            //
            return dbConnection;
        }

        /// <summary>
        /// Create DbConnection
        /// </summary>
        /// <param name="providerName">ProviderName：若為空字串，預設為 System.Data.SqlClient</param>
        /// <param name="connString">Database ConnectionString</param>
        /// <returns>傳回 DbConnection</returns>
        public static System.Data.Common.DbConnection CreateDbConnection(string providerName, string connString)
        {
            if (string.IsNullOrWhiteSpace(providerName)) { providerName = "System.Data.SqlClient"; }

            // Get DbConnection
            System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(providerName);
            System.Data.Common.DbConnection dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = connString;

            //
            return dbConnection;
        }

        // ****************************************************************************************************

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            try
            {
                // Create DbCommand
                System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
                //dbCommand.Connection = dbConnection;
                dbCommand.CommandTimeout = _CommandTimeout;
                dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                // Add Parameters
                AddParams(dbCommand, paramValues);

                //
                return dbCommand.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return CreateDbDataReader(dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return CreateDbDataReader(dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return CreateDbDataReader(dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            System.Data.Common.DbConnection dbConnection = dbTransaction.Connection;
            try
            {
                // Create DbCommand
                System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
                //dbCommand.Connection = dbConnection;
                dbCommand.Transaction = dbTransaction;
                dbCommand.CommandTimeout = _CommandTimeout;
                dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                // Add Parameters
                AddParams(dbCommand, paramValues);

                //
                return dbCommand.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                dbTransaction.Rollback();
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return CreateDbDataReader(dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return CreateDbDataReader(dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// Create DbDataReader
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DbDataReader</returns>
        public static System.Data.Common.DbDataReader CreateDbDataReader(System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return CreateDbDataReader(dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        // ****************************************************************************************************

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            try
            {
                object value;

                // 判斷連接狀態是否關閉，若是關閉則自動開啟
                bool connState = true;
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    connState = false;
                    dbConnection.Open();
                }

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Get Object
                    value = dbCommand.ExecuteScalar();
                }

                // 若連接狀態原本是關閉，執行完就自動關閉
                if (connState == false)
                {
                    dbConnection.Close();
                }

                //
                return value;
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultScalar(dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultScalar(dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return GetResultScalar(dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            System.Data.Common.DbConnection dbConnection = dbTransaction.Connection;
            try
            {
                object value;

                // 判斷連接狀態是否關閉，若是關閉則自動開啟
                bool connState = true;
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    connState = false;
                    dbConnection.Open();
                }

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.Transaction = dbTransaction;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Get Object
                    value = dbCommand.ExecuteScalar();
                }

                // 若連接狀態原本是關閉，執行完就自動關閉
                if (connState == false)
                {
                    dbConnection.Close();
                }

                //
                return value;
            }
            catch (System.Exception ex)
            {
                dbTransaction.Rollback();
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultScalar(dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultScalar(dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(Object)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回結果集中第一個資料列的第一個資料行</returns>
        public static object GetResultScalar(System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return GetResultScalar(dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        // ****************************************************************************************************

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            try
            {
                // Get DbProviderFactory
                System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(GetProviderName(dbConnection));

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Create DbDataAdapter
                    using (System.Data.Common.DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;

                        // Fill Data
                        dbDataAdapter.Fill(dataSet);
                    }
                }

                //
                return dataSet;
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultSet(dataSet, dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultSet(dataSet, dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return GetResultSet(dataSet, dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            return GetResultSet(new System.Data.DataSet(), dbConnection, sqlStatement, paramValues);
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultSet(new System.Data.DataSet(), dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultSet(new System.Data.DataSet(), dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return GetResultSet(new System.Data.DataSet(), dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            System.Data.Common.DbConnection dbConnection = dbTransaction.Connection;
            try
            {
                // Get DbProviderFactory
                System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(GetProviderName(dbConnection));

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.Transaction = dbTransaction;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Create DbDataAdapter
                    using (System.Data.Common.DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;

                        // Fill Data
                        dbDataAdapter.Fill(dataSet);
                    }
                }

                //
                return dataSet;
            }
            catch (System.Exception ex)
            {
                dbTransaction.Rollback();
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultSet(dataSet, dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultSet(dataSet, dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataSet</returns>
        private static System.Data.DataSet GetResultSet(System.Data.DataSet dataSet, System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return GetResultSet(dataSet, dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            return GetResultSet(new System.Data.DataSet(), dbTransaction, sqlStatement, paramValues);
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultSet(new System.Data.DataSet(), dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultSet(new System.Data.DataSet(), dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataSet)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataSet</returns>
        public static System.Data.DataSet GetResultSet(System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return GetResultSet(new System.Data.DataSet(), dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        // ****************************************************************************************************

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            try
            {
                // Get DbProviderFactory
                System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(GetProviderName(dbConnection));

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Create DbDataAdapter
                    using (System.Data.Common.DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;

                        // Fill Data
                        dbDataAdapter.Fill(dataTable);
                    }
                }

                //
                return dataTable;
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultTable(dataTable, dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultTable(dataTable, dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return GetResultTable(dataTable, dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            return GetResultTable(new System.Data.DataTable(), dbConnection, sqlStatement, paramValues);
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultTable(new System.Data.DataTable(), dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultTable(new System.Data.DataTable(), dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return GetResultTable(new System.Data.DataTable(), dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            System.Data.Common.DbConnection dbConnection = dbTransaction.Connection;
            try
            {
                // Get DbProviderFactory
                System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(GetProviderName(dbConnection));

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.Transaction = dbTransaction;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Create DbDataAdapter
                    using (System.Data.Common.DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;

                        // Fill Data
                        dbDataAdapter.Fill(dataTable);
                    }
                }

                //
                return dataTable;
            }
            catch (System.Exception ex)
            {
                dbTransaction.Rollback();
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultTable(dataTable, dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultTable(dataTable, dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataTable</returns>
        private static System.Data.DataTable GetResultTable(System.Data.DataTable dataTable, System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return GetResultTable(dataTable, dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            return GetResultTable(new System.Data.DataTable(), dbTransaction, sqlStatement, paramValues);
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return GetResultTable(new System.Data.DataTable(), dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return GetResultTable(new System.Data.DataTable(), dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 取得查詢結果(DataTable)
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回 DataTable</returns>
        public static System.Data.DataTable GetResultTable(System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return GetResultTable(new System.Data.DataTable(), dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        // ****************************************************************************************************

        /// <summary>
        /// 異動資料(新增、修改、刪除)，會自動 Open、Close Connection
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 異動資料語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbConnection dbConnection, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            try
            {
                int affectRows;

                // 判斷連接狀態是否關閉，若是關閉則自動開啟
                bool connState = true;
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    connState = false;
                    dbConnection.Open();
                }

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Get Affect Rows
                    affectRows = dbCommand.ExecuteNonQuery();
                }

                // 若連接狀態原本是關閉，執行完就自動關閉
                if (connState == false)
                {
                    dbConnection.Close();
                }

                //
                return affectRows;
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，會自動 Open、Close Connection
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 異動資料語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return ExecNonQuery(dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，會自動 Open、Close Connection
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlStatement">SQL 異動資料語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbConnection dbConnection, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return ExecNonQuery(dbConnection, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，會自動 Open、Close Connection
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbConnection dbConnection, SqlBuilder sqlBuilder)
        {
            return ExecNonQuery(dbConnection, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，不自動 Open Connection，因 Transaction 需在同一個 Connection 下進行
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 異動資料語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, params object[] paramValues)
        {
            sqlStatement += string.Empty;

            //
            System.Data.Common.DbConnection dbConnection = dbTransaction.Connection;
            try
            {
                int affectRows;

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.Transaction = dbTransaction;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = ToNamedParamSql(GetParamSymbol(dbCommand.Connection), sqlStatement);

                    // Add Parameters
                    AddParams(dbCommand, paramValues);

                    // Get Affect Rows
                    affectRows = dbCommand.ExecuteNonQuery();
                }

                //
                return affectRows;
            }
            catch (System.Exception ex)
            {
                dbTransaction.Rollback();
                dbConnection.Close();
                throw new System.Exception(ex.ToString() + System.Environment.NewLine + sqlStatement, ex);
            }
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，不自動 Open Connection，因 Transaction 需在同一個 Connection 下進行
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 異動資料語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.ArrayList paramValues)
        {
            return ExecNonQuery(dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，不自動 Open Connection，因 Transaction 需在同一個 Connection 下進行
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlStatement">SQL 異動資料語法</param>
        /// <param name="paramValues">參數值</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbTransaction dbTransaction, string sqlStatement, System.Collections.Generic.List<object> paramValues)
        {
            return ExecNonQuery(dbTransaction, sqlStatement, paramValues.ToArray());
        }

        /// <summary>
        /// 異動資料(新增、修改、刪除)，不自動 Open Connection，因 Transaction 需在同一個 Connection 下進行
        /// </summary>
        /// <param name="dbTransaction">DbTransaction</param>
        /// <param name="sqlBuilder">SqlBuilder</param>
        /// <returns>傳回受影響的資料筆數</returns>
        public static int ExecNonQuery(System.Data.Common.DbTransaction dbTransaction, SqlBuilder sqlBuilder)
        {
            return ExecNonQuery(dbTransaction, sqlBuilder.ToString(), sqlBuilder.GetParams());
        }

        // ****************************************************************************************************

        /* 不常用，暫不公開 */
        /// <summary>
        /// 依 TableName 及 DataTable 的 RowState 異動資料
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="tableName">要異動的資料表名稱</param>
        /// <param name="dataTable">DataTable</param>
        /// <param name="readOnlyColumnNames">DataTable 設為唯讀不異動的欄位名稱</param>
        /// <returns>傳回受影響的資料筆數</returns>
        private static int DataUpdate(System.Data.Common.DbConnection dbConnection, string tableName, System.Data.DataTable dataTable, params string[] readOnlyColumnNames)
        {
            tableName += string.Empty;

            //
            try
            {
                // 設定唯讀欄位
                foreach (string columnName in readOnlyColumnNames)
                {
                    if (dataTable.Columns.Contains(columnName))
                    {
                        dataTable.Columns[columnName].ReadOnly = true;
                    }
                }

                // 組 SQL 語法欄位名稱的部分
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.Data.DataColumn column in dataTable.Columns)
                {
                    sb.Append(", ");
                    sb.Append(column.ColumnName);
                }
                sb.Remove(0, 2);

                // Get DbProviderFactory
                System.Data.Common.DbProviderFactory dbProviderFactory = System.Data.Common.DbProviderFactories.GetFactory(GetProviderName(dbConnection));

                // Create DbCommand
                using (System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand())
                {
                    //dbCommand.Connection = dbConnection;
                    dbCommand.CommandTimeout = _CommandTimeout;
                    dbCommand.CommandText = "SELECT " + sb.ToString() + " FROM " + tableName + " WHERE 1 = 2";

                    // Create DbDataAdapter
                    using (System.Data.Common.DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;

                        // Create DbCommandBuilder
                        using (System.Data.Common.DbCommandBuilder dbCommandBuilder = dbProviderFactory.CreateCommandBuilder())
                        {
                            dbCommandBuilder.DataAdapter = dbDataAdapter;

                            // 異動資料
                            int affectRows;
                            affectRows = dbDataAdapter.Update(dataTable);
                            dataTable.AcceptChanges();

                            //
                            return affectRows;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                dbConnection.Close();
                throw new System.Exception(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// 依 TableName 及 DataTable 的 RowState 異動資料
        /// </summary>
        /// <param name="dbConnection">DbConnection</param>
        /// <param name="tableName">要異動的資料表名稱</param>
        /// <param name="dataTable">DataTable</param>
        /// <param name="readOnlyColumnNames">DataTable 設為唯讀不異動的欄位名稱</param>
        /// <returns>傳回受影響的資料筆數</returns>
        private static int DataUpdate(System.Data.Common.DbConnection dbConnection, string tableName, System.Data.DataTable dataTable, System.Collections.Generic.List<string> readOnlyColumnNames)
        {
            return DataUpdate(dbConnection, tableName, dataTable, readOnlyColumnNames.ToArray());
        }
        /* 不常用，暫不公開 */
        #endregion
    }
    #endregion
}