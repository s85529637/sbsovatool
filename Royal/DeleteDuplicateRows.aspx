<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" CodeFile="DeleteDuplicateRows.aspx.cs" Inherits="DeleteDuplicateRows" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.DeleteDuplicateRows %><span style="color: red">(請於 IT 中午過帳時發現有重複注單時再執行此功能)</span></h3>
    <table>
        <tr>
            <td>
                <asp:CheckBox ID="cbxTemp" runat="server" Text="實際查詢帳務資料表(未勾選代表從暫存表查詢)" ViewStateMode="Disabled" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center">請輸入密碼：<asp:TextBox ID="tbxPwd" runat="server" TextMode="Password"></asp:TextBox>
                <div style="position: absolute; top: 50%; left: 50%; margin-top: -50px; margin-left: -50px">
                    <!-- margin-top、margin-left 設定值是圖片長寬的一半 -->
                    <img id="imgLoading" alt="" src="images/loading.gif" style="display: none" />
                </div>
                <input id="btnSearching" type="button" value="搜尋中..." style="display: none" disabled="disabled" />
                <asp:Button ID="btnSearch" runat="server" Text="開始搜尋" OnClick="btnSearch_Click" OnClientClick="this.setAttribute('style','display:none');document.getElementById('btnSearching').removeAttribute('style');document.getElementById('imgLoading').removeAttribute('style');" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <span style="color: red">※刪到查無資料為止</span>
    <br />
    <asp:Button ID="btnBatDelete" runat="server" Text="批次刪除重複注單最大的流水號" OnClick="btnBatDelete_Click" OnClientClick="return confirm('確定刪除？');" />
    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Id" EmptyDataText="查無資料" ForeColor="#333333" GridLines="None" OnRowDeleting="gvResult_RowDeleting">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="流水號" />
            <asp:BoundField DataField="Club_id" HeaderText="會員ID" />
            <asp:BoundField DataField="Club_Ename" HeaderText="會員帳號" />
            <asp:BoundField DataField="Server_Name" HeaderText="遊戲" />
            <asp:BoundField DataField="DATETIME" DataFormatString="{0:yyyy-MM-dd HH:mm:ss.fff}" HeaderText="押注日期" />
            <asp:BoundField DataField="MaHao" HeaderText="碼號" />
            <asp:BoundField DataField="account_score" HeaderText="輸贏金額" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="btnButton" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('您確定要刪除此筆資料嗎？')" Text="刪除" CommandArgument='<% Eval("Id") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Wrap="False" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" Wrap="False" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>
</asp:Content>
