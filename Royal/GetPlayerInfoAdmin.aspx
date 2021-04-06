<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="GetLockedPlayer" CodeFile="GetPlayerInfoAdmin.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.GetPlayerInfo %></h3>
    <asp:Label ID="lblAccount" runat="server" Text="請輸入會員帳號："></asp:Label>
    <asp:TextBox ID="txtAccount" runat="server" Width="60px"></asp:TextBox>
    <asp:Button ID="btnQuery" runat="server" Font-Bold="False" OnClick="btnQuery_Click" Text="查詢" />
    <br />
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
            <asp:BoundField DataField="ChongZhi" HeaderText="充值額度" />
            <asp:BoundField DataField="OnlineTime" HeaderText="登入時間" />
            <asp:BoundField DataField="IP" HeaderText="登入位址" />
            <asp:BoundField DataField="Active" HeaderText="是否啟用" />
            <asp:BoundField DataField="Login" HeaderText="是否登入" />
            <asp:BoundField DataField="TingYong_XinYong" HeaderText="是否停用" />
            <asp:BoundField DataField="DongJie_Flag" HeaderText="是否凍結" />
            <asp:BoundField DataField="Lock" HeaderText="是否鎖定" />
            <asp:BoundField DataField="Logout_Xinyong" HeaderText="登出額度" />
            <asp:BoundField DataField="Login_Game_Id" HeaderText="登入遊戲" />
            <asp:BoundField DataField="Login_Server_Id" HeaderText="登入伺服器" />
            <asp:BoundField DataField="IsEGame" HeaderText="是否登入電子遊戲" SortExpression="Login_EGame" />
            <asp:BoundField DataField="SessionNo" HeaderText="開分號" SortExpression="Status" />
        </Fields>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
    </asp:DetailsView>
    <asp:HiddenField ID="hdnClub_id" runat="server" />
    <asp:HiddenField ID="hdnSession" runat="server" />
    <asp:HiddenField ID="hdLogin_Game_Id" runat ="server" />
    <br />
    <asp:DropDownList ID="dlplatform" runat="server" Visible="False" AutoPostBack="true" OnSelectedIndexChanged="dlplatform_SelectedIndexChanged">
         <asp:ListItem Value="">請選擇</asp:ListItem>
        <asp:ListItem Value="Royal">皇家電子</asp:ListItem>
        <asp:ListItem Value="JDB">JDB電子</asp:ListItem>
        <asp:ListItem Value="RTG">棋牌遊戲</asp:ListItem>
        <asp:ListItem Value="GCLUB">GCLUB電子</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btChkHasAccount" runat="server" Text="檢查是否存在帳務" Visible="False"  OnClick="btChkHasAccount_Click"/><br />
    <asp:Button ID="btnPowerReturnAccount" runat="server" Text="強制解鎖會員(請白馬確認該會員無帳務紀錄才能使用此功能)" OnClick="btnPowerReturnAccount_Click" Visible="False" OnClientClick="return confirm('確定該會員無帳務紀錄？');" ForeColor="Red" Enabled="false" />
    <br />
    <asp:Button ID="btnRecoveryLogin" runat="server" Text="恢復會員為遊戲中" Visible="False" OnClick="btnRecoveryLogin_Click" />
</asp:Content>
