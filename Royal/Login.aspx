<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" CodeFile="Login.aspx.cs" Inherits="Sova_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" class="style1" width="100%" style="border: 1px solid #808080;
        font-size: 10pt;">
        <tr> 
            <td id="tdTitle" runat="server">
                <b>Login</b>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" Width="60px" TextMode="Password"></asp:TextBox><br>
                <asp:Label ID="Label1" runat="server" Text="驗證碼:"></asp:Label>
                <input id="txtCkeCode" runat="server" type="text" />
                
             </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button ID="Button1" runat="server" onclick="btnQuery_Click" 
                    Text="Login"  OnClientClick="return chkform()"/>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function chkform()
        {
            var pwd = document.getElementById("<%=txtPassword.ClientID%>");
            var chk = document.getElementById("<%=txtCkeCode.ClientID%>");
            if (pwd.value == "")
            {
                alert("請輸入密碼!!");
                pwd.focus();
                return false;
            }

            if (chk.value == "") {
                alert("請輸入驗證碼!!");
                chk.focus();
                return false;
            }
        }
    </script>
</asp:Content>