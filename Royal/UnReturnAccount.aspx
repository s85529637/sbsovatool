<%@ Page Language="C#" MasterPageFile="SovaMasterPage.master" AutoEventWireup="true"
    CodeFile="UnReturnAccount.aspx.cs" Inherits="UnReturnAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="javascript/jquery-3.5.0.min.js"></script>
     <script type="text/javascript"> 
         var child = [];

         function ShowJDBStatus(id, valus, sid, tid, device)
         {
             child.push(id);
             jdbchgvalue(id, valus, sid, tid, device);
         }

         //RTG 
         function ShowRTGStatus(id, valus, sid, tid, device, ZuBie) {
             child.push(id);
             rtgchgvalue(id, valus, sid, tid, device, ZuBie);
         }

         //RTG
         function rtgchgvalue(id, valus, sid, tid, device, ZuBie) {
             var data = "uid=" + valus + "&tid=" + tid + "&sid=" + sid + "&ZuBie=" + ZuBie;
             $.ajax({
                 type: "get",
                 url: "/AsyncRe/OnlineStatus.ashx",
                 data: data,
                 success: function (msg) {
                     var status = msg;
                     if (status == "1") {
                         alert("在線");
                     } else if (status == "0") {
                         if (confirm("玩家離線，您要手動洗分嗎?")) {
                             RTGreturnaccount(sid, valus, ZuBie);
                         }
                     } else if (status == "-1") {
                         alert("報錯");
                         if (confirm("玩家離線，您要手動洗分嗎?")) {
                             RTGreturnaccount(sid, valus, ZuBie);
                         }
                     } else {
                         alert("皇家棋牌回應訊息︰" + msg);
                         if (confirm("玩家離線，您要手動洗分嗎?")) {
                             RTGreturnaccount(sid, valus, ZuBie);
                         }
                     }
                 }
             })
         }

         function ShowRoyalStatus(id, valus, sid, tid, device) {
             royalchgvalue(id, valus, sid, tid, device);
         }

         function jdbchgvalue(id, valus, sid, tid, device)
         {
             var data = "uid=" + valus + "&tid=" + tid + "&sid=" + sid ;
            $.ajax({
                type: "get",
                url: "/AsyncRe/OnlineStatus.ashx",
                data: data,
                success: function (msg) {
                    var status = msg;
                    if (status == "1") {
                        alert("在線");
                    } else if (status == "0") {
                        if (confirm("玩家離線，您要手動洗分嗎?")){
                            returnaccount(sid, valus);
                        }
                    } else if (status == "-1") {
                        alert("報錯");
                        if (confirm("玩家離線，您要手動洗分嗎?")) {
                            returnaccount(sid, valus);
                        }
                    } else {
                        alert(msg);
                        if (confirm("玩家離線，您要手動洗分嗎?")) {
                            returnaccount(sid, valus);
                        }
                    }
                }
            })
        }

         function royalchgvalue(id, valus, sid, tid, device) {
             var data = "uid=" + valus + "&tid=" + tid + "&sid=" + sid ;
            $.ajax({
                type: "get",
                url: "/AsyncRe/OnlineStatus.ashx",
                data: data,
                success: function (msg) {
                    var status = msg;
                    if (status == "1") {
                        alert("在線");
                    } else if (status == "0") {
                        alert("離線");
                    } else if (status == "-1") {
                        alert("報錯");
                    } else {
                        alert(msg);
                    }
                }
            })
         }

         function RTGreturnaccount(sid,uid,ZuBie) {
             var data = "sid=" + sid + "&uid=" + uid + "&ZuBie=" + ZuBie;
             $.ajax({
                 type: "get",
                 url: "/AsyncRe/RTGReturnAccount.ashx",
                 data: data,
                 success: function (msg) {
                     var Result;
                     var tmpResult;
                     if (msg.indexOf('|') > -1) {
                         alert(msg);
                     } else {
                         if (msg.indexOf(',') > -1) {
                             Result = msg.split(',');
                             if (Result[2] == "1") {
                                 alert("操作成功!!");
                                 $("#" + child.pop()).parent().parent().hide();
                             } else if (Result[2] == "0") {
                                 alert("操作失敗!!");
                                 child.pop();
                             } else {
                                 alert("發生未知例外︰" + msg);
                                 child.pop();
                             }
                         }
                     }
                 }
             })
         }

        function returnaccount(sid,uid)
        {
            var data = "sid=" + sid + "&uid=" + uid;
            $.ajax({
                type: "get",
                url: "/AsyncRe/ReturnAccount.ashx",
                data: data,
                success: function (msg) {
                    var Result;
                    var tmpResult;
                    if (msg.indexOf('|') > -1) {
                        alert(msg);
                    } else {
                        if (msg.indexOf(',') > -1) {
                            Result = msg.split(',');
                            if (Result[2] == "1"){
                                alert("操作成功!!");
                                $("#" + child.pop()).parent().parent().hide();
                            } else if (Result[2] == "0"){
                                alert("操作失敗!!");
                                child.pop();
                            } else if (Result[2] == "-1") {
                                alert("操作失敗!!，點數中心傳回︰" + Result[3]);
                                child.pop();
                            } else if (Result[2] == "-6") {
                                alert("操作失敗!!，點數中心傳回︰" + Result[3]);
                                child.pop();
                            } else if (Result[2] == "-2" || Result[2] == "-3") {
                                alert("操作失敗!!，調用手動洗分API發生意外︰" + Result[3]);
                                child.pop();
                            } else if (Result[2] == "-999") {
                                alert("操作失敗!!，調用點數中心API得到︰" + Result[3]);
                                child.pop();
                            } else {
                                alert("發生未知例外︰" + msg);
                                child.pop();
                            }  
                        }
                    } 
                }
            })   
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3><%=Resources.Resource.UnReturnAccount %></h3>
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>    
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate> --%>
            <asp:Label ID="Label2" runat="server" Text="廠商代號："></asp:Label>
            <asp:DropDownList ID="ddlThirdPartyId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlThirdPartyId_SelectedIndexChanged">
                <asp:ListItem Selected="True" Value="All">全部</asp:ListItem>
                <asp:ListItem Value="Royal">皇家</asp:ListItem>
                <asp:ListItem Value="Golden">黃金</asp:ListItem>
                <%--<asp:ListItem Value="Sova">微妙</asp:ListItem>--%>               
                <asp:ListItem Value="JDB">JDB</asp:ListItem> 
                <asp:ListItem Value="RTG">皇家棋牌</asp:ListItem> 
                <asp:ListItem Value="GCLUB">Gclub</asp:ListItem> 
            </asp:DropDownList>   
            <%--***************************************************************** --%>                     
            <asp:CheckBox ID="chkAuto" runat="server" Checked="True" Font-Bold="True" Text="Auto"
                AutoPostBack="True" OnCheckedChanged="chkAuto_CheckedChanged" />
            &nbsp;<asp:Label ID="lblClubEname" runat="server" Text="過濾會員："></asp:Label>
            <asp:TextBox ID="txtClubEname" runat="server"></asp:TextBox>
             <%--***************************************************************** --%> 
            <asp:Button ID="btnQuery" runat="server" Text="查詢" onclick="btnQuery_Click" />
               <%--***************************************************************** --%> 
            <asp:GridView ID="gvList" runat="server" CellPadding="4" GridLines="None" 
                AllowSorting="True" onsorting="gvList_Sorting" OnRowCommand="gvList_RowCommand" OnRowCreated="gvList_RowCreated"  OnRowDataBound="gvList_RowDataBound" ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="Kick" Text="踢線" />
                    <asp:TemplateField HeaderText="狀態"  >
                        <ItemTemplate>
                            <asp:HyperLink ID="mbrstats" runat="server" Text=""></asp:HyperLink>
                            <%--<asp:Label ID="mbrstats" runat="server" Text=""></asp:Label>--%>
                            <asp:Literal ID="txtscript" runat="server" ></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
    
            <asp:Literal ID="objpage" runat="server"></asp:Literal>
            <asp:Timer ID="Timer1" runat="server"  OnTick="Timer1_Tick">
            </asp:Timer>         
     <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
       
</asp:Content>
