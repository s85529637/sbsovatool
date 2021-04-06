<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="StarMasterPage.master"
    MaintainScrollPositionOnPostback="true" CodeFile="VideoConfigManage.aspx.cs" Inherits="VideoConfigManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <asp:Label ID="lblNowSetup" runat="server">目前設定：</asp:Label>
    <asp:Label ID="lblSetupDisplay" runat="server"></asp:Label>
    <asp:GridView ID="gvT_Server" runat="server" AutoGenerateColumns="False" Width="95%" DataKeyNames="Server_id">
        <Columns>
            <asp:BoundField DataField="Server_Name" HeaderText="伺服器名稱">
                <HeaderStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="Server_id" HeaderText="伺服器 Id">
                <HeaderStyle Width="120px" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Video_1" HeaderText="Video_1" />
            <asp:BoundField DataField="Video_2" HeaderText="Video_2" />
            <asp:BoundField DataField="Created_Time" DataFormatString="{0:yyyy/MM/dd}" HeaderText="建立日期" Visible="False">
                <HeaderStyle Width="120px" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Modified_Time" DataFormatString="{0:yyyy/MM/dd}" HeaderText="最後修改日期" Visible="False">
                <HeaderStyle Width="120px" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:RadioButtonList ID="rblCategory" runat="server" OnSelectedIndexChanged="rblCategory_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="True">
                </asp:RadioButtonList>
            </td>
            <td>
                <asp:Button ID="btnUpdateProfile" runat="server" Text="更新範本" OnClick="btnUpdateProfile_Click" />
            </td>
            <td>
                <asp:Button ID="btnUpdateT_Server" runat="server" Text="套用範本" OnClick="btnUpdateT_Server_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvT_Video_Profile" runat="server" AutoGenerateColumns="False" Width="95%" DataKeyNames="Profile_Id">
        <Columns>
            <asp:BoundField DataField="Server_Name" HeaderText="伺服器名稱">
                <HeaderStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="Server_id" HeaderText="伺服器 Id">
                <HeaderStyle Width="120px" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Video_1" HeaderText="Video_1" />
            <asp:BoundField DataField="Video_2" HeaderText="Video_2" />
            <asp:BoundField DataField="Created_Time" DataFormatString="{0:yyyy/MM/dd}" HeaderText="建立日期" Visible="False">
                <HeaderStyle Width="120px" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Modified_Time" DataFormatString="{0:yyyy/MM/dd}" HeaderText="最後修改日期" Visible="False">
                <HeaderStyle Width="120px" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</asp:Content>
