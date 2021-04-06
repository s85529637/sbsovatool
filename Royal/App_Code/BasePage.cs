using System.Linq;

/// <summary>
/// BasePage
/// </summary>
public abstract class BasePage : System.Web.UI.Page
{
    #region Const
    private const string _Event = "Event";
    #endregion

    /// <summary>
    /// 用來記錄 Log
    /// </summary>
    private readonly System.Collections.Generic.Dictionary<string, object> _LogDictionary = new System.Collections.Generic.Dictionary<string, object>();

    /// <summary>
    /// HttpRequest
    /// </summary>
    private System.Web.HttpRequest _HttpRequest = System.Web.HttpContext.Current.Request;

    /// <summary>
    /// 新增 Log 要記錄的項目，若 Key 值已經存在，則後面的值會覆蓋前面的值
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    public void AddLogItem(string key, object value)
    {
        if (this._LogDictionary.ContainsKey(key))
        {
            this._LogDictionary[key] = value;
        }
        else
        {
            this._LogDictionary.Add(key, value);
        }
    }

    /// <summary>
    /// 記錄 Log
    /// </summary>
    public void SaveLog()
    {
        try
        {
            this.AddLogItem("RequestTime", System.DateTime.Now.ToTwTimeUtc().ToDateTimeMillisecondString());
            this.AddLogItem("UserHostName", this._HttpRequest.UserHostName);
            this.AddLogItem("UserHostAddress", this._HttpRequest.UserHostAddress);
            this.AddLogItem("ContentType", this._HttpRequest.ContentType);
            this.AddLogItem("LocalUrl", this._HttpRequest.Url.AbsoluteUri);
            this.AddLogItem("RemoteUrl", this._HttpRequest.UrlReferrer == null ? "" : this._HttpRequest.UrlReferrer.AbsoluteUri);
            this.AddLogItem("HttpMethod", this._HttpRequest.HttpMethod);

            // 組請求端傳入的參數
            var dicParams = new System.Collections.Generic.Dictionary<string, object>();
            switch (this._HttpRequest.HttpMethod.ToUpperInvariant())
            {
                case "GET":
                    foreach (string key in this._HttpRequest.QueryString.AllKeys.Where(x => x.StartsWith("__") == false))
                    {
                        if (this._LogDictionary.ContainsKey(key) == false)
                        {
                            dicParams.Add(key, this._HttpRequest[key]);
                        }
                    }
                    break;
                case "POST":
                    foreach (string key in this._HttpRequest.Form.AllKeys.Where(x => x.StartsWith("__") == false))
                    {
                        if (this._LogDictionary.ContainsKey(key) == false)
                        {
                            dicParams.Add(key, this._HttpRequest[key]);
                        }
                    }
                    break;
                default:
                    foreach (string key in this._HttpRequest.Params.AllKeys.Where(x => x.StartsWith("__") == false))
                    {
                        if (this._LogDictionary.ContainsKey(key) == false)
                        {
                            dicParams.Add(key, this._HttpRequest[key]);
                        }
                    }
                    break;
            }
            this.AddLogItem("Params", dicParams);

            //
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this._LogDictionary);

            // 記錄 Log
            if (System.IO.Directory.Exists(Server.MapPath("~/LogFiles")) == false)
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/LogFiles"));
            }
            System.IO.File.AppendAllText(Server.MapPath("~/LogFiles/Log_" + System.DateTime.UtcNow.AddHours(8).ToDateString() + ".txt"), json + System.Environment.NewLine + System.Environment.NewLine, System.Text.Encoding.Default);
        }
        catch (System.Exception ex)
        {
            string msg = ex.ToStr();
        }
    }

    #region 建構式
    /// <summary>
    /// BasePage，一些網頁共用的功能可以寫在這裡
    /// </summary>
    public BasePage()
    {
        this.AddLogItem(_Event, "PageStart");
        this.SaveLog();
    }
    #endregion

    #region 覆寫的事件
    // 參考資料：http://www.dotblogs.com.tw/hatelove/archive/2009/12/18/pagelifecycle.aspx

    /// <summary>
    /// 覆寫 OnError
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnError(System.EventArgs e)
    {
        this.AddLogItem("Exception", System.Web.HttpContext.Current.Server.GetLastError().ToStr());

        //
        base.OnError(e);
    }

    /// <summary>
    /// 覆寫 OnPreInit
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnPreInit(System.EventArgs e)
    {
        base.OnPreInit(e);
    }

    /// <summary>
    /// 覆寫 OnInit
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnInit(System.EventArgs e)
    {
        base.OnInit(e);
    }

    /// <summary>
    /// 覆寫 OnInitComplete
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnInitComplete(System.EventArgs e)
    {
        base.OnInitComplete(e);
    }

    /// <summary>
    /// 覆寫 OnPreLoad
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnPreLoad(System.EventArgs e)
    {
        base.OnPreLoad(e);
        if (Session["isLogin"] == null) Response.Redirect("Logout.aspx");
    }

    /// <summary>
    /// 覆寫 OnLoad
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);
        if (Session["isLogin"] == null) Response.Redirect("Logout.aspx");
    }

    /// <summary>
    /// 覆寫 OnLoadComplete
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnLoadComplete(System.EventArgs e)
    {
        base.OnLoadComplete(e);
    }

    /// <summary>
    /// 覆寫 OnPreRender
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnPreRender(System.EventArgs e)
    {
        base.OnPreRender(e);

        // 用戶端在 PostBack 後畫面維持在相同的位置
        this.MaintainScrollPositionOnPostBack = true;
        if (Session["isLogin"] == null) Response.Redirect("Logout.aspx");
    }

    /// <summary>
    /// 覆寫 OnPreRenderComplete
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnPreRenderComplete(System.EventArgs e)
    {
        base.OnPreRenderComplete(e);
    }

    /// <summary>
    /// 覆寫 OnSaveStateComplete
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnSaveStateComplete(System.EventArgs e)
    {
        base.OnSaveStateComplete(e);
    }

    /// <summary>
    /// 覆寫 OnUnload
    /// </summary>
    /// <param name="e">System.EventArgs</param>
    protected override void OnUnload(System.EventArgs e)
    {
        this.AddLogItem(_Event, "PageEnd");
        this.SaveLog();

        //
        base.OnUnload(e);
    }
    #endregion
}