<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="EditPassword" CodeFile="EditPassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h3><%=Resources.Resource.EditPassword %></h3>
            <table cellpadding="0" cellspacing="0" class="style1" width="100%" style="border: 1px solid #808080; font-size: 10pt;">
                <tr>
                    <td style="width: 150px">工具登入密碼</td>
                    <td><%=Resources.Resource.原密碼%>
                        <asp:TextBox ID="txtSovaPassOrg" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                        <%=Resources.Resource.新密碼%>
                        <asp:TextBox ID="txtSovaPassNew1" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                        <%=Resources.Resource.確認密碼%>
                        <asp:TextBox ID="txtSovaPassNew2" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                        <asp:Button ID="btnSovaPass" runat="server" Text="修改" OnClick="btnSova_Click" />
                    </td>
                </tr>
                <%if ("H1".Equals(ConfigurationManager.AppSettings["SiteId"].ToString()))
                    { %>
                <tr>
                    <td>JUMBO工具密碼(OP2)</td>
                    <td><%=Resources.Resource.原密碼%>
                        <asp:TextBox ID="txtJumboPassOrg" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                        <%=Resources.Resource.新密碼%>
                        <asp:TextBox ID="txtJumboPassNew1" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                        <%=Resources.Resource.確認密碼%>
                        <asp:TextBox ID="txtJumboPassNew2" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                        <asp:Button ID="btnJumboPass" runat="server" Text="修改" OnClick="btnJumbo_Click" />
                    </td>
                </tr>
                <% } %>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
