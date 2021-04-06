<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="NewPointCenterLogSearch.aspx.cs" Inherits="NewPointCenterLogSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <script src="javascript/jquery-3.5.0.min.js"></script>
     <script  type="text/javascript"  src="dateselecter/js/jscal2.js"></script>
    <script type="text/javascript"  src="dateselecter/js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="dateselecter/css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="dateselecter/css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="dateselecter/css/steel/steel.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DropDownList ID="OpeaterDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged ="OpeaterDropDownList_SelectedIndexChanged">
        <asp:ListItem Value="0" Text="-----請選擇-----" ></asp:ListItem>
        <asp:ListItem Value="1" Text="取得會員活動LOG" ></asp:ListItem>
        <asp:ListItem Value="2" Text="取得會員下注LOG" ></asp:ListItem>
        <asp:ListItem Value="3" Text="取得JDB網址設定" ></asp:ListItem>
        <asp:ListItem Value="4" Text="設定JDB網址" ></asp:ListItem>
        <asp:ListItem Value="5" Text="重啟Server" ></asp:ListItem>
    </asp:DropDownList>
    <asp:Panel ID="MemberAction" runat="server" Visible="false">
        <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>會員帳號︰</td>
                <td><asp:TextBox ID="A_ename" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>開始時間︰</td>
                <td><input size="30" id="p_MemberActionSTime" runat="server"/><button id="f_MemberActionS">...</button></td>
            </tr>
            <tr>
                <td>結束時間︰</td>
                <td><input size="30" id="p_MemberActionETime" runat="server"/><button id="f_MemberActionE">...</button></td>
            </tr>
            <tr>
                <td>會員UID︰</td>
                <td><asp:TextBox ID="A_uid" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>JdbSessionId︰</td>
                <td><asp:TextBox ID="A_sid" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>API名稱︰</td>
                <td><asp:TextBox ID="A_apin" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="MemberAction_Sent" runat="server" Text="送出" OnClick="MemberAction_Sent_Click" OnClientClick="return chkformA();" /></td> 
            </tr>
        </table>
        <asp:Label ID="MemberActionMsg" runat="server" Text=""></asp:Label>
        <asp:GridView ID="MemberActionView" runat="server"  OnRowDataBound="MemberActionView_RowDataBound"></asp:GridView>
        <script type="text/javascript">//<![CDATA[
        var cal1 = Calendar.setup({
            onSelect: function (cal) { cal.hide() },
            showTime: true
        });
        cal1.manageFields("f_MemberActionS", "<%=this.p_MemberActionSTime.ClientID%>", "%Y-%m-%d %H:%M:%S");
        cal1.manageFields("f_MemberActionE", "<%=this.p_MemberActionETime.ClientID%>", "%Y-%m-%d %H:%M:%S");

        function openTextWindow(me) {
            var w = window.open();
            w.document.write(me.innerHTML);
        }
        
        function chkformA()
        {
            var A_ename = document.getElementById("<%=this.A_ename.ClientID%>");
            var p_MemberActionSTime = document.getElementById("<%=this.p_MemberActionSTime.ClientID%>");
            var p_MemberActionETime = document.getElementById("<%=this.p_MemberActionETime.ClientID%>");
            var A_uid = document.getElementById("<%=this.A_uid.ClientID%>");
            var A_sid = document.getElementById("<%=this.A_sid.ClientID%>");
            var A_apin = document.getElementById("<%=this.A_apin.ClientID%>");

            if ((A_ename.value == "" || A_ename.value == undefined)  && (A_uid.value == "" || A_uid.value == undefined))
            {
                alert("會員帳號或會員UID必須至少填一項!!");
                return false;
            }

            if (p_MemberActionSTime.value == "" || p_MemberActionSTime.value == undefined) {
                alert("請輸入開始時間!!");
                return false;
            }

            if (p_MemberActionETime.value == "" || p_MemberActionETime.value == undefined) {
                alert("請輸入結束時間!!");
                return false;
            }

            return true;
        }

        //]]></script>
    </asp:Panel>
 <%--**************************************************************************************************************************************--%>
    <asp:Panel ID="MemberQueue" runat="server"  Visible="false">
         <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>會員帳號︰</td>
                <td><asp:TextBox ID="B_ename" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>開始時間︰</td>
                <td><input size="30" id="p_MemberQueueSTime" runat="server"/><button id="f_MemberQueueS">...</button></td>
            </tr>
            <tr>
                <td>結束時間︰</td>
                <td><input size="30" id="p_MemberQueueETime" runat="server"/><button id="f_MemberQueueE">...</button></td>
            </tr>
            <tr>
                <td>會員UID︰</td>
                <td><asp:TextBox ID="B_uid" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>JdbSessionId︰</td>
                <td><asp:TextBox ID="B_sid" runat="server"></asp:TextBox></td>
            </tr>
             <tr>
                <td colspan="2"> <asp:Button ID="MemberQueue_Send" runat="server" Text="送出"  OnClick="MemberQueue_Send_Click" OnClientClick="return chkformB();"/></td> 
            </tr>
        </table>
         <asp:Label ID="MemberQueueMsg" runat="server" Text=""></asp:Label>
        <asp:GridView ID="MemberQueueView" runat="server"></asp:GridView>
        <script type="text/javascript">//<![CDATA[
        var cal2 = Calendar.setup({
            onSelect: function (cal) { cal.hide() },
            showTime: true
        });
        cal2.manageFields("f_MemberQueueS", "<%=this.p_MemberQueueSTime.ClientID%>", "%Y-%m-%d %H:%M:%S");
        cal2.manageFields("f_MemberQueueE", "<%=this.p_MemberQueueETime.ClientID%>", "%Y-%m-%d %H:%M:%S");

        function chkformB()
        {
            var B_ename = document.getElementById("<%=this.B_ename.ClientID%>");
            var p_MemberQueueSTime = document.getElementById("<%=this.p_MemberQueueSTime.ClientID%>");
            var p_MemberQueueETime = document.getElementById("<%=this.p_MemberQueueETime.ClientID%>");
            var B_uid = document.getElementById("<%=this.B_uid.ClientID%>");

            if ((B_ename.value == "" || B_ename.value == undefined) && (B_uid.value == "" || B_uid.value == undefined)) {
                alert("會員帳號或會員UID必須至少填一項!!");
                return false;
            }

            if (p_MemberQueueSTime.value == "" || p_MemberQueueSTime.value == undefined) {
                alert("請輸入開始時間!!");
                return false;
            }

            if (p_MemberQueueETime.value == "" || p_MemberQueueETime.value == undefined) {
                alert("請輸入結束時間!!");
                return false;
            }
            return true;
        }

        //]]></script>
    </asp:Panel>
<%--**************************************************************************************************************************************--%>
    <asp:Panel ID="JdbUrlConfig" runat="server"  Visible="false">
         <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>取得JDB網址設定︰</td>
                <td><asp:Button ID="JdbUrlConfigSend" runat="server" Text="送出"  OnClick="JdbUrlConfigSend_Click"/></td>
            </tr> 
           </table>
         <p>
         內容︰<asp:Label ID="lbJdbUrlConfig" runat="server" Text=""></asp:Label> <br> 
         註解︰<asp:Label ID="lbJdbUrlConfigNoet" runat="server" Text=""></asp:Label>      
    </asp:Panel>
<%--**************************************************************************************************************************************--%>
    <asp:Panel ID="SetJdbUrlConfig" runat="server"  Visible="false">
         <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>網址︰</td>
                <td><asp:TextBox ID="txtUrl" runat="server" Width="500"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="btSetJdbUrlConfig" runat="server" Text="送出" OnClick="btSetJdbUrlConfig_Click" OnClientClick="return chkformC();" /></td> 
            </tr> 
          </table>
        <p>
        狀態︰<asp:Label ID="SetJdbUrlConfigDescription" runat="server" Text=""></asp:Label></br>
        資料︰ <asp:Label ID="SetJdbUrlConfigData" runat="server" Text=""></asp:Label>
            <script type="text/javascript">//<![CDATA[
                function chkformC()
                {
                    var txtUrl = document.getElementById("<%=this.txtUrl.ClientID%>");

                    if (txtUrl.value == "" || txtUrl.value == undefined)
                    {
                        alert("請輸入網址!!");
                        txtUrl.focus();
                        return false;
                    }

                    return true;
                }
            //]]></script>
    </asp:Panel>
     <asp:Panel ID="RestartAllServer" runat="server"  Visible="false">
        
         <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>重啟Server︰</td>
                <td><asp:Button ID="btRestartAllServer" runat="server" Text="送出" OnClick="btRestartAllServer_Click"  /></td>
            </tr> 
           </table>
         <p>
         狀態︰<asp:Label ID="lbRestartAllServer" runat="server" Text=""></asp:Label> <br> 
         資料︰<asp:Label ID="lbRestartAllServerNote" runat="server" Text=""></asp:Label>      

     </asp:Panel>
</asp:Content>


