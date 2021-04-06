<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="CheckGsRoad.aspx.cs" Inherits="CheckGsRoad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script src="javascript/jquery-3.5.0.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3><%=Resources.Resource.CheckGsRoad %></h3>
    <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>    
   <%--***************************************************************** --%>  
       <asp:GridView ID="GwCheckGsRoad" runat="server"  CellPadding="4" GridLines="Both" ForeColor="#333333" >
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" Width="100px" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>  
        <asp:Timer ID="Timer1" runat="server"  OnTick="Timer1_Tick" Interval="20000"></asp:Timer>
</asp:Content>

