<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="Club_Stake_Current_Update_ForStar.aspx.cs" Inherits="Club_Stake_Current_Update_ForStar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.Club_Stake_Current_Update_ForStar %></h3>
    <span style="color: red">※修改 Star 注單，本程式請於過帳前使用</span>
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
    <asp:Button ID="btnModify" runat="server" Text="修改" OnClick="btnModify_Click" />
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
    <table id="tabModify" runat="Server" style="width: 100%">
        <tr>
            <td style="width: 200px">Id</td>
            <td>
                <asp:TextBox ID="id" runat="server" Width="100%" BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 200px">會員ID</td>
            <td>
                <asp:TextBox ID="Club_id" runat="server" Width="100%" BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">開分號(SessionID)</td>
            <td style="height: 20px">
                <asp:TextBox ID="StartSeqNoFlag" runat="server" Width="100%" BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>下注金額(TotalBet)</td>
            <td>
                <asp:TextBox ID="Stake_Score" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>有效押分(Available)</td>
            <td>
                <asp:TextBox ID="YouXiaoYaFen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>輸贏金額(TotalWin)</td>
            <td>
                <asp:TextBox ID="Account_Score" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>勝點</td>
            <td>
                <asp:TextBox ID="ShengDian" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>明細筆數</td>
            <td>
                <asp:TextBox ID="Rows" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center">請輸入密碼：<asp:TextBox ID="tbxPwd" runat="server" TextMode="Password"></asp:TextBox>
                <asp:Button ID="btnUpdate" runat="server" Text="確定修改" OnClick="btnUpdate_Click" />
            </td>
        </tr>
    </table>
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
</asp:Content>
