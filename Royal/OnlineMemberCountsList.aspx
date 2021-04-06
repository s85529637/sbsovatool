<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="OnlineMemberCountsList.aspx.cs" Inherits="OnlineMemberCountsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script  type="text/javascript"  src="dateselecter/js/jscal2.js"></script>
    <script type="text/javascript"  src="dateselecter/js/lang/en.js"></script>
    <link rel="stylesheet" type="text/css" href="dateselecter/css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="dateselecter/css/border-radius.css" />
    <link rel="stylesheet" type="text/css" href="dateselecter/css/steel/steel.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:Label ID="Label1" runat="server" Text="館別︰"></asp:Label>
    <asp:DropDownList ID="Pavilion" runat="server">
        <asp:ListItem Value="JDB" Selected>JDB</asp:ListItem>
        <asp:ListItem Value="RTG">皇家棋牌</asp:ListItem>
    </asp:DropDownList>
    <asp:Label ID="sldate" runat="server" Text="開始時間︰"></asp:Label>
    <input size="30" id="a_startdatetime" runat="server"/><button id="f_btn1">...</button>
    <asp:Label ID="eldate" runat="server" Text="結束時間︰"></asp:Label>
    <input size="30" id="a_enddatetime" runat="server"/><button id="f_btn2">...</button>

      <asp:Button ID="btsent" runat="server" Text="查詢" OnClick="btsent_Click" />

      <asp:GridView ID="OnlineMemberCounts_List" runat="server" >
             <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
       </asp:GridView>

       <asp:Literal ID="objMsg1" runat="server"></asp:Literal>

      <script type="text/javascript">//<![CDATA[

        var cal = Calendar.setup({
            onSelect: function (cal) { cal.hide() },
            showTime: true
        });
        cal.manageFields("f_btn1", "<%=this.a_startdatetime.ClientID%>", "%Y-%m-%d %H:%M:%S");
        cal.manageFields("f_btn2", "<%=this.a_enddatetime.ClientID%>", "%Y-%m-%d %H:%M:%S");

        //]]></script>
</asp:Content>

