<%@ WebHandler Language="C#" Class="AlertRecord" %>

using System;
using System.Web;
using System.Data;
using System.Web.Caching;
using Newtonsoft.Json;

public class AlertRecord : IHttpHandler,System.Web.SessionState.IRequiresSessionState {

    private RqFindRecord GetRecordDate()
    {
        RqFindRecord objResult = Tracklib.FindRecord(Tracklib.Page,Tracklib.Row);

        return objResult;
    }

    public void ProcessRequest (HttpContext context) {

        string strKey = Guid.NewGuid().ToString();

        RqFindRecord objRecord = null;

        string jsonRecord = string.Empty;

        try
        {
            if (System.Web.HttpRuntime.Cache.Get(strKey) == null)
            {
                try
                {
                    objRecord = GetRecordDate();
                    jsonRecord = JsonConvert.SerializeObject(objRecord);
                }
                catch (Exception ex)
                {
                    objRecord = new RqFindRecord();
                    objRecord.success = false;
                    objRecord.message = ex.Message;
                    objRecord.code = "-999";
                    jsonRecord = JsonConvert.SerializeObject(objRecord);
                }

                if (string.IsNullOrEmpty(jsonRecord)) {
                    return;
                }
                /*  context.Cache.Add 說明
                    第一個參數是設定索引值。
                    第二個則是要快取的物件。
                    第三個則是設定與某個檔案的相依性，當該檔案有變更時快取的物件就會被移除，如果不設定相依性，直接設null就可以。
                    而第四、第五個參數是有關聯的，是設定快取的失效時間，
                    第四個參數可以設為NoAbsoluteExpiration或是設定要在哪一個時間過後失效，
                    而第五個則是設定在該快取的最後一次存取後，要過多久才失效，如果前一個參數有設定時間，那該參數就要設為NoSildingExpiration。
                    而最後兩個分別是設定移除快取的優先權及被移除時的call back function。
                    */
                System.Web.HttpRuntime.Cache.Add(
                                strKey,
                                jsonRecord,
                                null,
                                DateTime.Now.AddSeconds(1),
                                System.Web.Caching.Cache.NoSlidingExpiration,
                                CacheItemPriority.High,
                                null
                                );
            }
            else
            {
                jsonRecord = System.Web.HttpRuntime.Cache.Get(strKey).ToString();
            }
        }
        catch (Exception ex)
        { 
            try
            {
                objRecord = GetRecordDate();
                jsonRecord = JsonConvert.SerializeObject(objRecord);
            }
            catch (Exception err)
            {
                objRecord = new RqFindRecord();
                objRecord.success = false;
                objRecord.message = err.Message;
                objRecord.code = "-999";
                jsonRecord = JsonConvert.SerializeObject(objRecord);
            }
               
            Lib.WritLog("AlertRecord.ashx.ProcessRequest()", ex.ToString());
        }

        context.Response.Write(jsonRecord);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}