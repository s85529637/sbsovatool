using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Configuration;

/// <summary>
/// GoogleAuthenticator 的摘要描述
/// </summary>
public class GoogleAuthenticator
{
    /// <summary>
    /// 初始化验证码生成规则
    /// </summary>
    /// <param name="key">秘钥(手机使用Base32码)</param>
    /// <param name="duration">验证码间隔多久刷新一次（默认30秒和google同步）</param>
    public GoogleAuthenticator(long duration = 30, string key= "NoName@gmail.com")
    {
        this.SERECT_KEY = key;
        this.SERECT_KEY_MOBILE = Base32.ToString(Encoding.UTF8.GetBytes(key));
        this.DURATION_TIME = duration;
    }

    /// <summary>
    /// 间隔时间
    /// </summary>
    private long DURATION_TIME { get; set; }

    /// <summary>
    /// 迭代次数
    /// </summary>
    private long COUNTER
    {
        get
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / DURATION_TIME;
        }
    }

    /// <summary>
    /// 秘钥
    /// </summary>
    private string SERECT_KEY { get; set; }

    /// <summary>
    /// 手机端输入的秘钥
    /// </summary>
    private string SERECT_KEY_MOBILE { get; set; }

    /// <summary>
    /// 到期秒数
    /// </summary>
    public long EXPIRE_SECONDS
    {
        get
        {
            return (DURATION_TIME - (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds % DURATION_TIME);
        }
    }

    /// <summary>
    /// 获取手机端秘钥
    /// </summary>
    /// <returns></returns>
    public string GetMobilePhoneKey()
    {
        if (SERECT_KEY_MOBILE == null)
            throw new ArgumentNullException("SERECT_KEY_MOBILE");
        return SERECT_KEY_MOBILE;
    }

    /// <summary>
    /// 生成认证码
    /// </summary>
    /// <returns>返回验证码</returns>
    public string GenerateCode()
    {
        return GenerateHashedCode(SERECT_KEY, COUNTER);
    }

    /// <summary>
    /// 按照次数生成哈希编码
    /// </summary>
    /// <param name="secret">秘钥</param>
    /// <param name="iterationNumber">迭代次数</param>
    /// <param name="digits">生成位数</param>
    /// <returns>返回验证码</returns>
    private string GenerateHashedCode(string secret, long iterationNumber, int digits = 6)
    {
        byte[] counter = BitConverter.GetBytes(iterationNumber);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(counter);

        byte[] key = Encoding.ASCII.GetBytes(secret);

        HMACSHA1 hmac = new HMACSHA1(key, true);

        byte[] hash = hmac.ComputeHash(counter);

        int offset = hash[hash.Length - 1] & 0xf;

        int binary =
            ((hash[offset] & 0x7f) << 24)
            | ((hash[offset + 1] & 0xff) << 16)
            | ((hash[offset + 2] & 0xff) << 8)
            | (hash[offset + 3] & 0xff);

        int password = binary % (int)Math.Pow(10, digits); // 6 digits

        return password.ToString(new string('0', digits));
    }

    /// <summary>
    ///   Generates a QR code bitmap for provisioning.(暫時加入)
    /// </summary>byte[]
    public void GenerateProvisioningImage(string _identifier, string KeyString, int width, int height)
    {
        return;

        string QR_Code_Path = string.IsNullOrEmpty(ConfigurationManager.AppSettings["QR_Code_Path"]) ? @"C:\" : ConfigurationManager.AppSettings["QR_Code_Path"].ToString();

        string myLocalFilePath = string.Empty;

        string identifier = RemoveWhitespace(_identifier);

        string tmpurl = string.Format("otpauth://totp/{2}:{0}?secret={1}&issuer={2}", identifier, KeyString.TrimEnd('='), new_UrlEncode("Yamaplay"));

        var ProvisionUrl = UrlEncode(RemoveWhitespace(string.Format("otpauth://totp/{2}:{0}?secret={1}&issuer={2}", identifier, KeyString.TrimEnd('='), new_UrlEncode("Yamaplay"))));

        var ChartUrl = string.Format(@"https://chart.googleapis.com/chart?cht=qr&chs={0}x{1}&chl={2}", width, height, ProvisionUrl);

        if (identifier.IndexOf('@') > -1)
        {
            myLocalFilePath = string.Format("{0}{1}{2}.png", QR_Code_Path, DateTime.Now.ToString("yyyyMMddHHmm"), identifier.Split('@')[0]);
        }
        else
        {
            myLocalFilePath = string.Format("{0}{1}{2}.png", QR_Code_Path, DateTime.Now.ToString("yyyyMMddHHmm"), identifier);
        }

        try
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(ChartUrl, myLocalFilePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// 移除空白
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static string RemoveWhitespace(string str)
    {
        return new string(str.Where(c => !Char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string new_UrlEncode(string value)
    {
        StringBuilder result = new StringBuilder();
        string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        foreach (char symbol in value)
        {
            if (validChars.IndexOf(symbol) != -1)
            {
                result.Append(symbol);
            }
            else
            {
                result.Append('%' + String.Format("{0:X2}", (int)symbol));
            }
        }

        return result.ToString().Replace(" ", "%20");
    }

    /// <summary>
    ///   Url Encoding (with upper-case hexadecimal per OATH specification)
    /// </summary>
    public static string UrlEncode(string value)
    {
        const string UrlEncodeAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        var Builder = new StringBuilder();

        for (var i = 0; i < value.Length; i++)
        {
            var Symbol = value[i];

            if (UrlEncodeAlphabet.IndexOf(Symbol) != -1)
            {
                Builder.Append(Symbol);
            }
            else
            {
                Builder.Append('%');
                Builder.Append(((int)Symbol).ToString("X2"));
            }
        }

        return Builder.ToString();
    }
}