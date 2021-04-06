<%@ Page Language="C#" MasterPageFile="SovaMasterPage.master" AutoEventWireup="true" CodeFile="ManualReturnJumbo.aspx.cs" Inherits="ManualReturnJumbo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.ManualReturnJumbo %></h3>
    <asp:TextBox ID="txtClubEname" runat="server"></asp:TextBox>
    <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
    <asp:Button ID="btnReturn" runat="server" Text="手動洗分" OnClick="btnReturn_Click" />
    <br />
    <br />
    <asp:GridView ID="gvData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
</asp:Content>