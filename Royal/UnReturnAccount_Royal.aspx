<%@ Page Language="C#" MasterPageFile="SimpleMasterPage.master" AutoEventWireup="true" CodeFile="UnReturnAccount_Royal.aspx.cs" Inherits="UnReturnAccount_Royal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.UnReturnAccount_Royal %></h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
            最後處理序號：<asp:Label ID="lblSeqNo" runat="server" Text="0"></asp:Label><br/>
            <asp:CheckBox ID="chkAuto" runat="server" Checked="True" Font-Bold="True" Text="Auto"
                AutoPostBack="True" OnCheckedChanged="chkAuto_CheckedChanged" />
            <br />
            <asp:TextBox ID="TextBox1" runat="server" Height="200px" ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
