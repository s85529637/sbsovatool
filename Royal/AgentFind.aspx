<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="AgentFind.aspx.cs" Inherits="AgentFind" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3>今日代理紀錄</h3>
    <asp:Button ID="btGetdata" runat="server" Text="取得資料"  OnClick="btGetdata_Click"/>
    <asp:GridView ID="AgentListView" runat="server"></asp:GridView>
</asp:Content>

