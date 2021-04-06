<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="Club_Stake_Current_Insert_ForStar.aspx.cs" Inherits="Club_Stake_Current_Insert_ForStar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.Club_Stake_Current_Insert_ForStar %></h3>
    <span style="color: red">※補 Star 注單，本程式請於過帳後使用</span>
    <br />
    <asp:Label ID="lbl_Id" runat="server" Text="Id"></asp:Label>
    <asp:TextBox ID="tbx_Id" runat="server"></asp:TextBox>
    <span style="color: blue">&nbsp;&nbsp;或&nbsp;&nbsp;</span>
    <asp:Label ID="lblClub_Id" runat="server" Text="會員ID(UserID)"></asp:Label>
    <asp:TextBox ID="tbxClub_Id" runat="server"></asp:TextBox>
    <span style="color: blue">＋</span>
    <asp:Label ID="lblStartSeqNoFlag" runat="server" Text="開分號(SessionID)"></asp:Label>
    <asp:TextBox ID="tbxStartSeqNoFlag" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
    <asp:Button ID="btnAdd" runat="server" Text="新增" OnClick="btnAdd_Click" />
    <br />
    <asp:GridView ID="gvResultBefore" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <br />
    <asp:GridView ID="gvResultAfter" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>
    <br />
    <table id="tabAdd" runat="Server" style="width: 100%">
        <tr>
            <td style="width: 200px">
                <asp:Label ID="Label9" runat="server" Text="Id"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="id" runat="server" Width="100%" BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 200px">
                <asp:Label ID="Label1" runat="server" Text="會員ID(UserID)"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Club_id" runat="server" Width="100%" BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">
                <asp:Label ID="Label2" runat="server" Text="開分號(SessionID)"></asp:Label>
            </td>
            <td style="height: 20px">
                <asp:TextBox ID="StartSeqNoFlag" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">
                <asp:Label ID="Label3" runat="server" Text="遊戲ID(例如：Bacc)"></asp:Label>
            </td>
            <td style="height: 20px">
                <asp:TextBox ID="Game_id" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">
                <asp:Label ID="Label4" runat="server" Text="桌別(例如：M)"></asp:Label>
            </td>
            <td style="height: 20px">
                <asp:TextBox ID="Desk" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="下注金額(TotalBet)"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Stake_Score" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="有效押分(Available)"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="YouXiaoYaFen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="輸贏金額(TotalWin)"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Account_Score" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="明細筆數"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Rows" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center">請輸入密碼：<asp:TextBox ID="tbxPwd" runat="server" TextMode="Password"></asp:TextBox>
                <asp:Button ID="btnInsert" runat="server" Text="確定新增" OnClick="btnInsert_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
