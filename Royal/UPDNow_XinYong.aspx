<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master" 
    CodeFile="UPDNow_XinYong.aspx.cs" Inherits="UPDNow_XinYong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.UPDNow_XinYong %></h3>
    <asp:Label ID="lblAccount" runat="server" Text="請輸入會員帳號："></asp:Label>
    <asp:TextBox ID="txtAccount" runat="server" Width="60px" ></asp:TextBox>
     <asp:Button ID="Inquire" runat="server"  OnClick="btnInquire_Click"  Text="查詢"  OnClientClick ="return chkfindform();"/>
    <br />
    <asp:Label ID="Label2" runat="server" Text="修改額度金額："></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server" Width="60px" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
    <asp:Label ID="Label3" runat="server" Text="驗證碼："></asp:Label>
    <input id="TextBox2" runat="server" type="text" />
    <asp:Button ID="Button1" runat="server"  OnClick="btnQuery_Click"  Text="確定"  OnClientClick ="return chkfindform2();"/>
    <asp:DetailsView ID="dv" runat="server" AutoGenerateRows="False"
        CellPadding="4" ForeColor="#333333" GridLines="None" Height="30px">
        <AlternatingRowStyle BackColor="White" />
        <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
        <EditRowStyle BackColor="#2461BF" />
        <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
        <Fields>
            <asp:BoundField DataField="Club_Id" HeaderText="會員編號" />
            <asp:BoundField DataField="Club_Ename" HeaderText="會員帳號" />
            <asp:BoundField DataField="Now_XinYong" HeaderText="信用額度" />
            <asp:BoundField DataField="Login" HeaderText="是否在線上" />    
            <asp:BoundField DataField="XinYong" HeaderText="修改前額度" />  
        </Fields>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" /> 
    </asp:DetailsView>

    <script type="text/javascript">
        function chkfindform() {
            var f_Club_id = document.getElementById("<%=txtAccount.ClientID%>");
            if (f_Club_id.value == "")
            {
                alert("請輸入會員ID!!");
                f_Club_id.focus();
                return false;
            }
            return true;
        }
        function chkfindform2() {
            var f_Club_id = document.getElementById("<%=txtAccount.ClientID%>");
            var f_Now_XinYong = document.getElementById("<%=TextBox1.ClientID%>");
            var chk = document.getElementById("<%=TextBox2.ClientID%>");

            if (f_Club_id.value == "") {
                alert("請輸入會員ID!!");
                f_Club_id.focus();
                return false;
            }   
            if (f_Now_XinYong.value == "") {
                alert("請輸入修改金額!!");
                f_Now_XinYong.focus();
                return false;
            }
            if (f_Now_XinYong.value != "" & chk.value == "") {
                alert("請輸入驗證碼");
                f_Now_XinYong.focus();
                chk.focus();
                return false;
            }
            return true;

        }

    </script>
</asp:Content>
