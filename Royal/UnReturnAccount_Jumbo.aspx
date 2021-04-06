<%@ Page Language="C#" MasterPageFile="SovaMasterPage.master" AutoEventWireup="true" CodeFile="UnReturnAccount_Jumbo.aspx.cs" Inherits="UnReturnAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.UnReturnAccount_Jumbo %></h3>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <asp:Label ID="lblClubEname" runat="server" Text="輸入會員帳號："></asp:Label>
            <asp:TextBox ID="txtClubEname" runat="server"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查詢" onclick="btnQuery_Click" />
            <br/>
            <asp:Label ID="lblQueryResult" runat="server" Text=""></asp:Label>
            <br/>
            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999"
                BorderStyle="None" BorderWidth="1px" CellPadding="1"  >
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            </asp:GridView>
            <br/>
            <asp:Label ID="lblClubStatus" runat="server" Text=""></asp:Label><br/>
            <asp:Label ID="lblSessionNoStatus" runat="server" Text=""></asp:Label><br/>
            <asp:Label ID="lblJumboAccountCount" runat="server" Text=""></asp:Label><br/>
            <br/>
            <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#999999"
                BorderStyle="None" BorderWidth="1px" CellPadding="1"  >
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            </asp:GridView>
            <br/>

            <asp:Label ID="lblJumboAccountCountTxt" runat="server" Text=""></asp:Label>
            <asp:Button ID="btnReturnAccount" runat="server" Text="手動洗分" OnClientClick="if (confirm('確定洗分?')==false) {return false;}" UseSubmitBehavior="False" OnClick="btnReturnAccount_Click"/><br/>
            <asp:Button ID="btnRecovery" runat="server" Text="恢復會員狀態" OnClientClick="if (confirm('確定恢復會員狀態?')==false) {return false;}" UseSubmitBehavior="False" OnClick="btnRecovery_Click"/><br/>

            <asp:Label ID="lblReturnAccount" runat="server" Text=""></asp:Label><br/>
            <asp:Label ID="lblRecoveryAccount" runat="server" Text=""></asp:Label><br/>
            <asp:Label ID="lblReturnValue" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label><br/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
