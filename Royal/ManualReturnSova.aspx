<%@ Page Language="C#" MasterPageFile="SovaMasterPage.master" AutoEventWireup="true" CodeFile="ManualReturnSova.aspx.cs" Inherits="ManualReturnSova" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.ManualReturnSova %></h3>
    <asp:TextBox ID="txtClubEname" runat="server"></asp:TextBox>
    <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
    <asp:Button ID="btnReturn" runat="server" Text="手動洗分" OnClick="btnReturn_Click" />
    <br />
    <br />
    <asp:GridView ID="gvData" runat="server" BackColor="White" BorderColor="#999999"
        BorderStyle="None" BorderWidth="1px" CellPadding="1">
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
    </asp:GridView>
</asp:Content>
