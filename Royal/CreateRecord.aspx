<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="CreateRecord.aspx.cs" Inherits="CreateRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="1" cellpadding="0" cellspacing="0" style="display:none;">
         <tr>
            <td colspan="2">
                新增記錄
            </td>
        </tr>
        <tr>
            <td>IP︰</td>
             <td><asp:TextBox ID="txtIp" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Account︰</td>
             <td><asp:TextBox ID="txtUid" runat="server"></asp:TextBox></td>
        </tr>
        <tr> 
            <td>Source︰</td>
             <td><asp:TextBox ID="txtSource" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btsend" runat="server" Text="送出" OnClick="btsend_Click1"  OnClientClick ="return chkform1();"/>
            </td>
        </tr>
    </table>
    <asp:Label ID="lbCreateRecordMsg" runat="server" Text=""></asp:Label>
    <script type="text/javascript">
        function chkform1()
        {
            var txtIp = document.getElementById("<%=txtIp.ClientID%>");
            var txtUid = document.getElementById("<%=txtUid.ClientID%>");
            var txtSource = document.getElementById("<%=txtSource.ClientID%>");

            if (txtIp.value == "") {
                alert("請輸入IP!!");
                txtIp.focus();
                return false;
            }

            if (txtUid.value == "") {
                alert("請輸入Account!!");
                txtUid.focus();
                return false;
            }

            if (txtSource.value == "") {
                alert("請輸入Source!!");
                txtSource.focus();
                return false;
            }
            return true;
        }
    </script>
    <!--************************************************************-->
    <P>
    <P>
     <table border="1" cellpadding="0" cellspacing="0" width="400">
         <tr>
            <td colspan="2">
                新增黑名單
            </td>
        </tr>
        <tr>
            <td>追蹤目標︰</td>
             <td>
                 <asp:TextBox ID="txtTarget" runat="server"></asp:TextBox>
             </td>
        </tr>
        <tr>
            <td>類型︰</td> 
             <td>
                  <asp:DropDownList ID="txtType" runat="server">
                      <asp:ListItem Value="IP">IP</asp:ListItem>
                      <asp:ListItem Value="Account">帳號</asp:ListItem>
                  </asp:DropDownList>
                 <%--<asp:TextBox ID="txtType" runat="server"></asp:TextBox>--%>
             </td>
        </tr>
         <tr>
             <td>帳號類型︰</td>
             <td>
                 <asp:DropDownList ID="AccountType" runat="server">
                     <asp:ListItem Value="0">請選擇</asp:ListItem>
                     <asp:ListItem Value="1">通路端</asp:ListItem>
                     <asp:ListItem Value="2">會員端</asp:ListItem>
                 </asp:DropDownList>
             </td>
         </tr>
        <tr> 
            <td>備註︰</td>
             <td><asp:TextBox ID="txtBelong" runat="server"  Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btSent1" runat="server" Text="送出"  OnClick="btSent1_Click" OnClientClick ="return chkform2();" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lbCreateTrackMsg" runat="server" Text=""></asp:Label>
        <script type="text/javascript">

        function isValidIP(ip) {
            var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
            return reg.test(ip);
        }
    
        function isValidIPV6(ip) {
            var reg = /^(([\da-fA-F]{1,4}):){8}$/;
            return reg.test(ip);
        }
          
        function isNumber(value)
        {
            var re = /^[0-9]+$/;
            return re.test(value);
        }
            
        /**
        * @param {String} a String
        * @return {Boolean} true if the String is a valid IPv6 address; false otherwise
        */
        function isIPv6(value) {
            var components = value.split(":");
            
            if (components.length < 2 || components.length > 8) {
                return false;
            }
            
            if (components[0] != "" || components[1] != "") {
                // Address does not begin with a zero compression ("::")
                if (!components[0].test(/^[\da-f]{1,4}/i)) {
                    // Component must contain 1-4 hex characters
                    return false;
                }
            }
            
            var numberOfZeroCompressions = 0;
            for (var i = 1; i < components.length; ++i) {
                if (components[i] == "") {
                    // We're inside a zero compression ("::")
                    ++numberOfZeroCompressions;
                    if (numberOfZeroCompressions > 1) {
                        // Zero compression can only occur once in an address
                        return false;
                    }
                    continue;
                }
                if (!components[i].test(/^[\da-f]{1,4}/i)) {
                    // Component must contain 1-4 hex characters
                    return false;
                }
            }
            return true;
        }
       /*
        暫不用，需要再詳測    
        */
        function isIPv6_bak(ip)
        {
            var expression = /((^\s*((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]))\s*$)|(^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$))/;

            var isok = false;

            alert(expression.test(ip));

            if (expression.test(ip)) {
                isok = true;
            }
            return isok;
        }

        function chkIpV4(ip)
        {           
            var ipArray ;
            
            var isok = false;

            var star = false;

           if (isIPv6(ip)) return true;  //如果是IPV6就返回

           if(ip.indexOf(".") > 0)
           {
               ipArray = ip.split(".");
                 
                if( 4 == ipArray.length)
                {	
                    if (isNumber(ipArray[0])) {
                        isok = ipArray[0] >= 0 && ipArray[0] <= 255;
                        if (!isok) return isok;
                    } else {
                        return false;
                    }
	
                    if(isNumber(ipArray[1]) && isok)
                    {
                        isok = ipArray[1] >= 0 && ipArray[1] <= 255;
                        if (!isok) return isok;
                    }else{
                        isok = ipArray[1] == "*";
                        if (isok == true) {
                            star = true;
                        }

                        if (!isok) return isok;
                    }
	
                    if (isNumber(ipArray[2]) && isok && !star)
                    {
                        isok = ipArray[2] >= 0 && ipArray[2] <= 255;
                        if (!isok) return isok;
                    }else{
                        isok = ipArray[2] == "*";
                        if (isok == true) {
                            star = true;
                        }

                        if (!isok) return isok;
                    }
	
                    if (isNumber(ipArray[3]) && isok && !star)
                    {
                        isok = ipArray[3] >= 0 && ipArray[3] <= 255;
                        if (!isok) return isok;
                    }else{
                        isok = ipArray[3] == "*";

                        if (!isok) return isok;
                    }
                }
           }
           
           return  isok;
        }

        function chkform2()
        {
            var txtTarget = document.getElementById("<%=txtTarget.ClientID%>");
            var txtType = document.getElementById("<%=txtType.ClientID%>");
            var txtBelong = document.getElementById("<%=txtBelong.ClientID%>");
            var AccountType = document.getElementById("<%=AccountType.ClientID%>");

            if (txtTarget.value == "") {
                alert("請輸入追蹤目標!!");
                txtTarget.focus();
                return false;
            }

            if (txtTarget.value != "") {
                if (txtType.value == "IP") {
                    if (!chkIpV4(txtTarget.value)) {
                        alert("IP格式錯誤!!");
                        txtTarget.focus();
                        return false;
                    }
                } else {
                    if (txtType.value == "Account")
                    {
                        if (AccountType.value == 0)
                        {
                            alert("請選擇帳號類型!!");
                            return false;
                        }
                    }
                }
            }

            //if (txtBelong.value == "") {
            //    alert("請輸入備註!!");
            //    txtBelong.focus();
            //    return false;
            //}
            return true;
        }
    </script>
   <!--************************************************************-->
        <p>
     <table border="1" cellpadding="0" cellspacing="0" width="400">
         <tr>
            <td colspan="2">
                修改黑名單
            </td>
        </tr>
        <tr>
            <td>ID︰</td>
             <td>
                 <asp:Label ID="lbId" runat="server" Text=""></asp:Label>
             </td>
        </tr>
        <tr>
            <td>追蹤目標︰</td>
             <td>
                 <asp:Label ID="lbUid" runat="server" Text=""></asp:Label>
             </td>
        </tr>
        <tr>
            <td>是否追蹤︰</td>
             <td>
                 <asp:DropDownList ID="dlIsTrace" runat="server">
                     <asp:ListItem Value="1" Selected>是</asp:ListItem> 
                     <asp:ListItem Value="0" >否</asp:ListItem> 
                 </asp:DropDownList>
             </td>
        </tr>
        <tr> 
            <td>備註︰</td>
             <td><asp:TextBox ID="txteBelong" runat="server" Width="300"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="bteditTrack" runat="server" Text="送出"  OnClick="bteditTrack_Click" />
            </td>
        </tr>
    </table>
    <asp:Label ID="editTrackMsg" runat="server" Text=""></asp:Label>
 
    <!--*********************************************************************************-->
    <p>
    <h3>黑名單記錄</h3><asp:CheckBox ID="cbIsTrace" runat="server" Text="列追蹤資料"  Checked="true"  OnCheckedChanged="cbIsTrace_CheckedChanged" AutoPostBack="true"/>
    <asp:GridView ID="GetTrackDataResult" runat="server" OnRowCommand="GetTrackDataResult_RowCommand">
         <Columns>
             <asp:ButtonField ButtonType="Button" CommandName="Del" Text="刪除"   />
             <asp:ButtonField ButtonType="Button" CommandName="Upt" Text="修改"   />
         </Columns>
    </asp:GridView>
    <asp:Literal ID="objpage" runat="server"></asp:Literal>
    <asp:Label ID="GetTrackDataMsg" runat="server" Text=""></asp:Label>

</asp:Content>

