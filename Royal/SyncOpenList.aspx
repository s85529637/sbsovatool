<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="SyncOpenList" CodeFile="SyncOpenList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.SyncOpenList %></h3>
    同歩開牌記錄:
    <asp:DropDownList ID="ddlGameServer" runat="server">
    </asp:DropDownList>
    <asp:Button ID="btnRun" runat="server" OnClick="btnRun_Click" Text="Run" />
    <br />
    <asp:TextBox ID="txtMessage" runat="server" Height="500px" ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
</asp:Content>
