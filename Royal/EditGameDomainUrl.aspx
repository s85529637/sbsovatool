<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="EditGameDomainUrl.aspx.cs" Inherits="EditGameDomainUrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3>新增資料</h3>
    <table border="1" cellpadding="0" cellspacing="0">
        <tr>
            <td>域名</td>
            <td>
                <div style="color:red;">範例︰http://www.google.com 或 https://www.google.com</div>
                <asp:TextBox ID="txtWebUrl" runat="server" Width="350"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>域名類型</td>
            <td>
                 <asp:DropDownList ID="dtUrlType" runat="server">
                     <asp:ListItem Value="" >請選擇</asp:ListItem>
                     <asp:ListItem Value="Direct-H1" >H1正式</asp:ListItem>
                     <asp:ListItem Value="Direct-Mini" >Mini正式</asp:ListItem>
                     <asp:ListItem Value="Official-H1" >H1備用</asp:ListItem>
                     <asp:ListItem Value="Official-Mini" >Mini備用</asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td>備註</td>
            <td>
                <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Rows="5" Columns="80"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btAdd" runat="server" Text="新增" OnClick="btAdd_Click" OnClientClick="return chkform3();" />
            </td>
        </tr>
    </table>
     <script type="text/javascript">
         
        function chkform3()
        {
            var txtWebUrl = document.getElementById("<%=txtWebUrl.ClientID%>");
            var dtUrlType = document.getElementById("<%=dtUrlType.ClientID%>");
            if (txtWebUrl.value == "") {
                alert("請填寫域名!!");
                txtWebUrl.focus();
                return false;
            } 

            if (dtUrlType.value == "") {
                alert("請選擇域名類型!!");
                dtUrlType.focus();
                return false;
            }

            return true;
        }
    </script>
    </p>
    <h3>資料查詢</h3>
    <table border="1" cellpadding="0" cellspacing="0">
        <tr>
            <td>域名類型︰</td>
             <td>
                 <asp:DropDownList ID="UrlType" runat="server">
                     <asp:ListItem Value="" >請選擇</asp:ListItem>
                     <asp:ListItem Value="Direct" >正式</asp:ListItem>
                     <asp:ListItem Value="Official" >備用</asp:ListItem>
                 </asp:DropDownList>
             </td>
        </tr>
         <tr>
            <td>平台︰</td>
             <td>
                  <asp:DropDownList ID="SysType" runat="server">
                     <asp:ListItem Value="" >請選擇</asp:ListItem>
                     <asp:ListItem Value="H1" >H1</asp:ListItem>
                     <asp:ListItem Value="Mini" >Mini</asp:ListItem>
                 </asp:DropDownList>
             </td>
        </tr>
         <tr>
            <td>啟用狀態︰</td>
             <td>
                 <asp:DropDownList ID="LiveStatus" runat="server">
                     <asp:ListItem Value="" >請選擇</asp:ListItem>
                     <asp:ListItem Value="all" >全部資料</asp:ListItem>
                     <asp:ListItem Value="true" >啟用</asp:ListItem>
                     <asp:ListItem Value="false" >未啟用</asp:ListItem>
                 </asp:DropDownList>
             </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btGetData" runat="server" Text="查詢"  OnClick="btGetData_Click" OnClientClick="return chkform1();" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function chkform1()
        {
            var UrlType = document.getElementById("<%=UrlType.ClientID%>");
            var SysType = document.getElementById("<%=SysType.ClientID%>");
            var LiveStatus = document.getElementById("<%=LiveStatus.ClientID%>");

            if (UrlType.value == "") {
                alert("請選擇域名類型!!");
                UrlType.focus();
                return false;
            }

            if (SysType.value == "") {
                alert("請選擇平台!!");
                SysType.focus();
                return false;
            }

            if (LiveStatus.value == "") {
                alert("請選擇啟用狀態!!");
                LiveStatus.focus();
                return false;
            }

            return true;
        }
    </script>
    </p>
    <asp:GridView ID="gvDomainUrlList" runat="server" OnRowCommand="gvDomainUrlList_RowCommand">
        <Columns>
             <asp:ButtonField ButtonType="Button" CommandName="Del" Text="刪除"   />
             <asp:ButtonField ButtonType="Button" CommandName="Upt" Text="修改"   />
         </Columns>
    </asp:GridView>
    </p>
    <h3>修改資料</h3>
    <table border="1" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                域名︰
            </td>
             <td>
                 <asp:Label ID="lbdomain" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td>
                域名類型︰
            </td>
             <td>
                <asp:Label ID="lbwebSite" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td>
                啟用狀態︰
            </td>
             <td>
                 <asp:DropDownList ID="dlisLive" runat="server">
                     <asp:ListItem Value="" >請選擇</asp:ListItem>
                     <asp:ListItem Value="true" >啟用</asp:ListItem>
                     <asp:ListItem Value="false" >未啟用</asp:ListItem>
                 </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td>
                修改人員︰
            </td>
             <td>
                <asp:Label ID="lbmodifyUser" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td>
                修改時間︰
            </td>
             <td>
                <asp:Label ID="lbmodifyTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td>
                備註︰
            </td>
             <td>
                 <asp:TextBox ID="tbNoet" TextMode="MultiLine" Columns="100" Rows="5" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btUpdate" runat="server" Text="修改"  OnClick="btUpdate_Click" OnClientClick="return chkform2();"/>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function chkform2()
        {
            var dlisLive = document.getElementById("<%=dlisLive.ClientID%>");

            if (dlisLive.value == "") {
                alert("請選擇啟用狀態!!");
                dlisLive.focus();
                return false;
            } 
            return true;
        }
    </script>
</asp:Content>

