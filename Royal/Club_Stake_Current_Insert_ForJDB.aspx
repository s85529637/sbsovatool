<%@ Page Title="" Language="C#" MasterPageFile="SovaMasterPage.master" AutoEventWireup="true" 
CodeFile="Club_Stake_Current_Insert_ForJDB.aspx.cs" Inherits="Club_Stake_Current_Insert_ForJDB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblClub" runat="server" Text="會員ID或帳號"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxClub" runat="server"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblSessionNo" runat="server" Text="H1SessionNo"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxSessionNo" runat="server"></asp:TextBox>
            </td>
            <td>若無開分號可以不填</td>
        </tr>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblJDBSessionId" runat="server" Text="JDBSessionId"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxJDBSessionId" runat="server"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblGame_id" runat="server" Text="Game_id"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxGame_id" runat="server"></asp:TextBox>
            </td>
            <td>H1 遊戲 Id</td>
        </tr>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblBet" runat="server" Text="投注"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxBet" runat="server"></asp:TextBox>
            </td>
            <td>請填數字</td>
        </tr>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblJackpot" runat="server" Text="彩金"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxJackpot" runat="server"></asp:TextBox>
            </td>
            <td>請填數字</td>
        </tr>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblNetWin" runat="server" Text="淨輸贏"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxNetWin" runat="server"></asp:TextBox>
            </td>
            <td>請填數字</td>
        </tr>
        <tr>
            <td style="text-align: right">
                <font color="red">※</font>
                <asp:Label ID="lblRows" runat="server" Text="筆數"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbxRows" runat="server"></asp:TextBox>
            </td>
            <td>請填數字</td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: center">
                <asp:Button ID="btnSave" runat="server" Text="儲存" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="清除" OnClick="btnClear_Click" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:GridView ID="gvResult" runat="server" DataKeyNames="Id" EmptyDataText="無新增資料" Width="80%" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDeleting="gvResult_RowDeleting">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="Club_id" HeaderText="會員代碼" />
            <asp:BoundField DataField="Club_Ename" HeaderText="會員帳號" />
            <asp:BoundField DataField="Game_id" HeaderText="Game_id" />
            <asp:BoundField DataField="Stake_Score" HeaderText="投注金額" />
            <asp:BoundField DataField="Account_Score" HeaderText="輸贏金額" />
            <asp:BoundField DataField="Jackpot_Score" HeaderText="彩金" />
            <asp:BoundField DataField="MaHao" HeaderText="碼號" />
            <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
        </Columns>
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
</asp:Content>
