<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="GameInfo" CodeFile="GameInfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.GameInfo %></h3>
    報表日期(只能查歷史帳):<asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
    <asp:Button ID="btnRun" runat="server" OnClick="btnRun_Click" Text="Run" />
    <br/><asp:Label ID="Msg" runat="server" Text=""></asp:Label>
    <br /> 
    <br />
    <asp:GridView ID="gvDevice" runat="server" CellPadding="3"
        GridLines="Vertical" Font-Size="10pt" Width="400px" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="False">
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#DCDCDC" />
        <Columns>
            <asp:BoundField DataField="LogDate" HeaderText="報表日期">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Device" HeaderText="裝置代碼">
                <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Count" HeaderText="數量">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
        </Columns>
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#0000A9" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#000065" />
    </asp:GridView>
    <br />
    <asp:GridView ID="gvM" runat="server" CellPadding="3"
        GridLines="Vertical" Font-Size="10pt" AutoGenerateColumns="False" Width="800px" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px">
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="#DCDCDC" />
        <Columns>
            <asp:BoundField DataField="ReportDate" HeaderText="報表日期" Visible="False">
                <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Server_id" HeaderText="伺服器代碼" Visible="False">
                <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Game_id" HeaderText="遊戲代碼" Visible="False">
                <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Server_no" HeaderText="排列序號" Visible="False">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="ThirdParty_Id" HeaderText="遊戲種類">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Server_name" HeaderText="遊戲名稱">
                <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Members" HeaderText="會員數">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Stakes" HeaderText="電子人次/真人注單">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="YaMa" HeaderText="有效押分(NT)">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Account_Score" HeaderText="輸贏(NT)">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
        </Columns>
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#0000A9" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#000065" />
    </asp:GridView>
</asp:Content>
