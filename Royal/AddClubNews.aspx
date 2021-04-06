<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master" CodeFile="AddClubNews.aspx.cs" Inherits="AddClubNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.AddClubNews %></h3>
    <asp:Label ID="lblAccount" runat="server" Text="請輸入日期："></asp:Label>
    <asp:TextBox ID="txtAccount" runat="server" Width="200"></asp:TextBox>
    <asp:DropDownList ID="DropDownList1" runat="server">
    </asp:DropDownList>
    <asp:Button ID="Inquire" runat="server" OnClick="btnInquire_Click" Text="查詢" OnClientClick="return chkfindform();" />
</asp:Content>
