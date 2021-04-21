<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Clublogin.aspx.cs" Inherits="Clublogin" MasterPageFile="SovaMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .clubtable td.title {
            border: 1px solid black;
            color: White;
            background-color: #507CD1;
            font-weight: bold;
        }

        .clubtable tbody.content {
            border: 1px solid black;
            background-color: #EFF3FB;
        }
    </style>
    <script src="javascript/jquery-3.5.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
    <asp:Label ID="lbldata" runat="server" Text="請輸入日期："></asp:Label>
    <asp:TextBox ID="txtdata" runat="server" Width="200" class="txtdata"></asp:TextBox>
    <br />
    <asp:Label ID="lblclub" runat="server" Text="會員帳號:"></asp:Label>
    <asp:TextBox ID="txtlblclub" runat="server" Width="200px" class="txtlblclub"></asp:TextBox>
    <input type="button" name="name" onclick="onclickclub()" value="查詢" />

    <table class="clubtable">
        <tr>
            <td class="title">會員帳號</td>
            <td class="title">登入館別</td>s
            <td class="title">登入登出時間</td>
            <td class="title">狀態</td>
        </tr>
        <tbody id="content" class="content">
        </tbody>
    </table>
    <script type="text/javascript">

        function onclickclub() {
            var Club_Ename = $('.txtlblclub').val();
            var selectdata = $('.txtdata').val();
            if (Club_Ename == "") {
                alert("請輸入帳號!!");
                return false;
            }
            axios.get('/AsyncRe/Clublogin.ashx', { params: { Club_Ename, selectdata } })
                .then(function (jsonStr) {
                    
                    if (jsonStr.data.length == 0) {
                        alert("查無資料!!");
                        return false;}
                    var item = "";
                    for (i = 0; i < jsonStr.data.length; i++) {
                        item += '<tr><td>' + jsonStr.data[i].user_id + '</td><td>' + jsonStr.data[i].login_local + '</td><td>' + jsonStr.data[i].login_time + '</td><td>' + jsonStr.data[i].login_flag + '</td></tr>'
                    }
                    $('#content').html(item);
                }).catch(function (error) {
                    console.log(error);
                })
        }


    </script>
</asp:Content>
