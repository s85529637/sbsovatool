<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="GameSwitch" CodeFile="GameSwitch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .SwitchTD {
            width: 150px;
        }

        .auto-style2 {
            width: 384px;
        }

        .auto-style3 {
            width: 511px;
        }
    </style>
    <script src="javascript/jquery-3.5.0.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                var InputPass = '<%=Resources.Resource.InputPass%>';
            </script>
            <h3><%=Resources.Resource.GameSwitch %></h3>
            <table cellpadding="0" cellspacing="0" class="style1" width="100%" style="border: 1px solid #808080;">

                <%if (ThirdPartyId.IndexOf(STAR) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblStar" runat="server" Text="STAR真人開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbStar" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtStar" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnStar" runat="server" OnClick="btnStar_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtStar.value=='') {alert(InputPass); return false;}" />
                    </td>
                </tr>
                <% } %>

                <%
                    if (ThirdPartyId.IndexOf(GCLUB) > -1)
                    {
                %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="Label2" runat="server" Text="Gclub開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbGclub" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtGclub" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btGclub" runat="server" Text="修改" OnClick="btGclub_Click"
                            OnClientClick="if (document.getElementById('ContentPlaceHolder1_txtGclub').value=='') { alert(InputPass);  return false;}else{ if(document.getElementById('ContentPlaceHolder1_rbGclub_1').checked) { GcluekickAll(); } if(document.getElementById('ContentPlaceHolder1_rbGclub_0').checked){ GclueOpenHall(); } }" />
                    </td>
                    <td class="SwitchTD">
                        <asp:Label ID="Label3" runat="server" Text="Gclub廠商開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbGclub2" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtGclub2" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" Text="開啟" OnClick="btGclub_Click2"
                            OnClientClick="if (document.getElementById('ContentPlaceHolder1_txtGclub2').value=='') { alert(InputPass);  return false;}else{ if(document.getElementById('ContentPlaceHolder1_rbGclub2_1').checked) { GcluekickAll(); } if(document.getElementById('ContentPlaceHolder1_rbGclub2_0').checked){ GclueOpenHall(); } }" />
                    </td>

                </tr>

                <%
                    }
                %>

                <%if (ThirdPartyId.IndexOf(RTG) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="Label1" runat="server" Text="棋牌遊戲開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbRTG" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtRTG" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnRTG" runat="server" OnClick="btnRTG_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtRTG.value=='') {alert(InputPass);  return false;}else{ if(document.getElementById('ContentPlaceHolder1_rbRTG_1').checked) kickAll(2, 'ALL'); }" />
                    </td>
                </tr>
                <% } %>

                <%if (ThirdPartyId.IndexOf(JUMBO) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblJumbo" runat="server" Text="尊博電子開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbJumbo" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtJumbo" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnJumbo" runat="server" OnClick="btnJumbo_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtJumbo.value=='') {alert(InputPass); return false;}" />
                    </td>
                </tr>
                <% } %>

                <%if (ThirdPartyId.IndexOf(SOVA) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblSova" runat="server" Text="微妙電子開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbSova" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtSova" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnSova" runat="server" OnClick="btnSova_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtSova.value=='') {alert(InputPass); return false;}" />
                    </td>
                </tr>
                <% } %>

                <%if (ThirdPartyId.IndexOf(GOLDEN) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblGolden" runat="server" Text="黃金電子開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbGolden" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtGolden" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnGolden" runat="server" OnClick="btnGolden_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtGolden.value=='') {alert(InputPass); return false;}" />
                    </td>
                </tr>
                <% } %>

                <%if (ThirdPartyId.IndexOf(ROYAL) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblRoyal" runat="server" Text="皇家電子開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbRoyal" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtRoyal" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnRoyal" runat="server" OnClick="btnRoyal_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtRoyal.value=='') {alert(InputPass); return false;}" />
                    </td>
                </tr>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblRoyal2" runat="server" Text="皇家電子二館開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbRoyal2" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0">開啟</asp:ListItem>
                            <asp:ListItem Value="1">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtRoyal2" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnRoyal2" runat="server" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtRoyal2.value=='') {alert(InputPass); return false;}" OnClick="btnRoyal2_Click" />
                    </td>
                </tr>
                <% } %>

                <%if (ThirdPartyId.IndexOf(JDB) > -1)
                    { %>
                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblJDB" runat="server" Text="JDB電子開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbJDB" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtJDB" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnJDB" runat="server" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtJDB.value=='') {alert(InputPass); return false;}" OnClick="btnJDB_Click" />
                    </td>
                </tr>
                <% } %>

                <tr>
                    <td class="SwitchTD">
                        <asp:Label ID="lblFrontEnd" runat="server" Text="官網維護開關"></asp:Label>
                    </td>
                    <td class="SwitchTD">
                        <asp:RadioButtonList ID="rbFrontEnd" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0">開啟</asp:ListItem>
                            <asp:ListItem Value="1">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtFrontEnd" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnFrontEnd" runat="server" OnClick="btnFrontEnd_Click" Text="修改"
                            OnClientClick="if (document.all.ContentPlaceHolder1_txtFrontEnd.value=='') {alert(InputPass); return false;}" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <br />
    <div runat="server" id="DisplayPanel">
        <table style="width: 50%">
            <tr>
                <% if (_SiteId.ToString().ToLower().Trim() != MAESOT)
                    { %>
                <td style="width: 25%">
                    <h3>超帳及代理開關</h3>
                </td>
                <% } %>
                <td style="width: 25%">
                    <asp:RadioButtonList ID="rdoButtonAll" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoButtonAll_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="1">全開</asp:ListItem>
                        <asp:ListItem Value="0">全關</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="batSave" runat="server" OnClick="batSave_Click" Text="批次修改" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvResult" runat="server" CellPadding="4" DataKeyNames="Param_key" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowCreated="gvResult_RowCreated" Width="50%">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Param_name" HeaderText="開關名稱">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="開或關">
                    <EditItemTemplate>
                        <asp:RadioButtonList ID="rdoButton" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:RadioButtonList ID="rdoButton" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
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

        <br />
        <% if (ThirdPartyId.IndexOf(RTG) > -1)
            { %>
        <table style="width: 50%">
            <tr>
                <td style="width: 25%">
                    <h3>棋牌單一遊戲開關</h3>
                </td>
                <td style="width: 25%">
                    <asp:RadioButtonList ID="rdoButtonAllRTG" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoButtonAllRTG_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="1">全開</asp:ListItem>
                        <asp:ListItem Value="0">全關</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="batSaveRTG" runat="server" Text="批次修改" OnClick="batSaveRTG_Click" />
                </td>
            </tr>
        </table>

        <!------------------------------------------------------------------------------------------------------------------------------------------------>
        <asp:GridView ID="gvRTG" runat="server" CellPadding="4" DataKeyNames="Category,Id" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            OnRowCreated="gvRTG_RowCreated" Width="50%">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Game_Id" HeaderText="遊戲ID"></asp:BoundField>
                <asp:BoundField DataField="Game_Name" HeaderText="遊戲名稱">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="開或關">
                    <EditItemTemplate>
                        <asp:RadioButtonList ID="rdoButton" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:RadioButtonList ID="rdoButton" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <SortedAscendingCellStyle BackColor="#FDF5AC" />
            <SortedAscendingHeaderStyle BackColor="#4D0000" />
            <SortedDescendingCellStyle BackColor="#FCF6C0" />
            <SortedDescendingHeaderStyle BackColor="#820000" />
        </asp:GridView>
        <!-------------------------------------------------------------------------------->
        <% }  %>
        <br />
        <% if (ThirdPartyId.IndexOf(JDB) > -1)
            { %>
        <table style="width: 50%">
            <tr>
                <td style="width: 25%">
                    <h3>JDB 單一遊戲開關</h3>
                </td>
                <td style="width: 25%">
                    <asp:RadioButtonList ID="rdoButtonAllJDB" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoButtonAllJDB_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="1">全開</asp:ListItem>
                        <asp:ListItem Value="0">全關</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="batSaveJDB" runat="server" Text="批次修改" OnClick="batSaveJDB_Click" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvJDB" runat="server" CellPadding="4" DataKeyNames="Category,Id" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            OnRowCreated="gvJDB_RowCreated" Width="50%">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Game_Id" HeaderText="遊戲ID"></asp:BoundField>
                <asp:BoundField DataField="Game_Name" HeaderText="遊戲名稱">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="開或關">
                    <EditItemTemplate>
                        <asp:RadioButtonList ID="rdoButton" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:RadioButtonList ID="rdoButton" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">開啟</asp:ListItem>
                            <asp:ListItem Value="0">關閉</asp:ListItem>
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <SortedAscendingCellStyle BackColor="#FDF5AC" />
            <SortedAscendingHeaderStyle BackColor="#4D0000" />
            <SortedDescendingCellStyle BackColor="#FCF6C0" />
            <SortedDescendingHeaderStyle BackColor="#820000" />
        </asp:GridView>
        <% }  %>
    </div>
    <script type="text/javascript">
        function GcluekickAll() {
            var data = "isopen=N";
            $.ajax({
                type: "GET",
                url: "/AsyncRe/Gclub.ashx",
                dataType: "text",
                data: data,
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                },
                success: function (msg) {
                    alert(msg);
                }
            });
        }

        function GclueOpenHall() {
            var data = "isopen=Y";
            $.ajax({
                type: "GET",
                url: "/AsyncRe/Gclub.ashx",
                dataType: "text",
                data: data,
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                },
                success: function (msg) {
                    alert(msg);
                }
            });
        }

        function kickAll(KickType, gameid) {
            var data = "KickType=" + KickType + "&GameId=" + gameid;
            $.ajax({
                type: "GET",
                url: "/AsyncRe/KickAll.ashx",
                dataType: "text",
                data: data,
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                },
                success: function (msg) {
                    var result = msg;
                    var status;
                    var message;
                    if (result.indexOf('|') > -1) {
                        status = result.split('|')[0];
                        message = result.split('|')[1];
                    } else {
                        status = "-999";
                        message = "您沒有權限或已被登出";
                    }

                    if (status == "0") {
                        alert("Kick " + message);
                    } else {
                        alert(message);
                        if (confirm("您要重新踼線嗎?")) {
                            redokickall(KickType, gameid);
                        }
                    }
                }
            });
        }

        function redokickall(KickType, gameid) {
            kickAll(KickType, gameid);
        }

        function kickonegame(_gamelist) {
            var gamelist;

            var game;

            var chkvalue = 0;

            if (_gamelist == "" || _gamelist == undefined) {
                alert("未關閉任何遊戲，未執行踼線");
                return;
            }

            if (_gamelist.indexOf(',') > -1) {

                gamelist = _gamelist.split(',');

                for (j = 0; j < gamelist.length; j++) {
                    if (gamelist[j].indexOf('|') > -1) //1是開，0是關
                    {
                        game = gamelist[j].split('|');

                        if (game[0] == 0) {
                            chkvalue++;
                        }
                    }
                }

                if (chkvalue == gamelist.length)  //表示所有遊戲都要關
                {
                    kickAll(2, "ALL");
                } else {
                    if (chkvalue == 0) {
                        alert("未關閉任何遊戲，未執行踼線");
                    }
                    for (j = 0; j < gamelist.length; j++) {
                        if (gamelist[j].indexOf('|') > -1) //1是開，0是關
                        {
                            game = gamelist[j].split('|');

                            if (game[0] == 0) {
                                kickAll(1, game[1]);
                            }
                        }
                    }
                }
            }
        }
    </script>
</asp:Content>
