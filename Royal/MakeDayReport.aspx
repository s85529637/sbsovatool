<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="MakeDayReport" CodeFile="MakeDayReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sm" runat="server" AsyncPostBackTimeout="3600">
    </asp:ScriptManager>
    <h3><%=Resources.Resource.MakeDayReport %> 請於中午11:30-12:30之間執行</h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            轉帳報表日期:<asp:Label ID="txtStartDate" runat="server" ForeColor="Red"></asp:Label>
            輸入轉帳密碼:<asp:TextBox ID="txtSova" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnRun" runat="server" OnClick="btnRun_Click" Text="開始" />
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <img src="images/loading.gif" alt="" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <br />
            <table width="100%">
                <tr>
                    <td width="50%">
                        <asp:TextBox ID="txtMessage" runat="server" Height="600px" ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                    <td width="50%">
                        <asp:TextBox ID="txtReport" runat="server" Height="600px" ReadOnly="True" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
