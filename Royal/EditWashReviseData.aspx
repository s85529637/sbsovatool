<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="EditWashReviseData.aspx.cs" Inherits="EditWashReviseData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.EditWashReviseData %></h3>
    <asp:Label ID="Label1" runat="server" Text="會員ID︰"></asp:Label><asp:TextBox ID="btClub_id" runat="server"></asp:TextBox><br>
    <asp:Label ID="Label2" runat="server" Text="開分號︰"></asp:Label><asp:TextBox ID="btSessioinid" runat="server"></asp:TextBox>
    <asp:Button ID="btsend" runat="server" Text="查詢" OnClick="btsend_Click" OnClientClick="return chkfindform();" /><br>
    <asp:Label ID="Msg" runat="server" Text="會員ID︰"></asp:Label>
    <table border="1" cellpadding="2" cellspacing="2">
        <tr>
            <td>帳務狀態︰</td>
            <td>
                <asp:Label ID="lbstatus" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>會員ID︰</td>
            <td>
                <asp:Label ID="lbPlayerId" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>開分號︰</td>
            <td>
                <asp:Label ID="lbSessionId" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>下注︰</td>
            <td>
                <asp:TextBox ID="btStakeScore" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>輸贏︰</td>
            <td>
                <asp:TextBox ID="btAccount_Score" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>筆數︰</td>
            <td>
                <asp:TextBox ID="btRows" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>公點︰</td>
            <td>
                <asp:TextBox ID="txtTableFee" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>分潤︰</td>
            <td>
                <asp:TextBox ID="txtCommission" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>彩金︰</td>
            <td>
                <asp:TextBox ID="textJackpot" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btupdate" runat="server" Text="送出" OnClick="btupdate_Click" OnClientClick="return chkupdateform();" /></td>
        </tr>
    </table>

    <script type="text/javascript">
        function chkfindform() {
            var f_Club_id = document.getElementById("<%=btClub_id.ClientID%>");
            var f_Sessioinid = document.getElementById("<%=btSessioinid.ClientID%>");

            if (f_Club_id.value == "") {
                alert("請輸入會員ID!!");
                f_Club_id.focus();
                return false;
            }

            if (f_Sessioinid.value == "") {
                alert("請輸入開分號!!");
                f_Sessioinid.focus();
                return false;
            }

            return true;
        }

        function chkupdateform() {
            var f_StakeScore = document.getElementById("<%=btStakeScore.ClientID%>");
            var f_Account_Score = document.getElementById("<%=btAccount_Score.ClientID%>");
            var f_Rows = document.getElementById("<%=btRows.ClientID%>");
            var f_TableFee = document.getElementById("<%=txtTableFee.ClientID%>");
            var f_Commission = document.getElementById("<%=txtCommission.ClientID%>");

            if (f_StakeScore.value == "") {
                alert("請輸入下注!!");
                f_StakeScore.focus();
                return false;
            }
            if (f_Account_Score.value == "") {
                alert("請輸入輸贏!!");
                f_Account_Score.focus();
                return false;
            }
            if (f_Rows.value == "") {
                alert("請輸入筆數!!");
                f_Rows.focus();
                return false;
            }

            if (txtTableFee.value == "") {
                alert("請輸入公點!!");
                txtTableFee.focus();
                return false;
            }

            if (txtCommission.value == "") {
                alert("請輸入分潤!!");
                txtCommission.focus();
                return false;
            }
            return true;
        }
    </script>

</asp:Content>

