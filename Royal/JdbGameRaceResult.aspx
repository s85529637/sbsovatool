<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="JdbGameRaceResult.aspx.cs" Inherits="JdbGameRaceResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script type="text/javascript" >

       function confirmDelete() {
           if (window.confirm("您確定要派彩嗎?"))
           {
               return true;
           }
           return false;
       }

   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="selCal" runat="server" Text="日期" OnClick="selCal_Click" />
    <asp:Calendar ID="seldate" runat="server" OnSelectionChanged="seldate_SelectionChanged"></asp:Calendar>
    <asp:TextBox ID="seldateText" runat="server"></asp:TextBox>
    <asp:HiddenField ID="Hidseldate" runat="server" />
    <asp:Button ID="seldata" runat="server" Text="查詢" OnClick="seldata_Click" />
         <asp:Literal ID="txtscript" runat="server" ></asp:Literal>
         <asp:Literal ID="Msg" runat="server"></asp:Literal> 
           <table>
               <tr>
                   <td style="vertical-align:top;">
                       <h3><asp:Label ID="lbDragon" runat="server" Text="龍榜"></asp:Label> </h3>
            <asp:GridView ID="DragonList" runat="server" CellPadding="4" GridLines="None" 
                AllowSorting="True"   ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
               <%-- <Columns>
                   <asp:TemplateField HeaderText="派彩"  >
                        <ItemTemplate>
                            <asp:Button runat="server" ID="Send" OnClick="DragonList_Send_Click" CommandArgument='<%# Eval("名次")+","+Eval("UID")+","+Eval("彩金") %>' OnClientClick="return confirmDelete();"  Text="派彩"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>--%>
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
                   </td>
                   <td style="vertical-align:top;">
                        <h3> <asp:Label ID="lbTiger" runat="server" Text="虎榜"></asp:Label></h3> 
            <asp:GridView ID="TigerList" runat="server" CellPadding="4" GridLines="None" 
                AllowSorting="True"   ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
                <%--<Columns>
                      <asp:TemplateField HeaderText="派彩"  >
                        <ItemTemplate>
                            <asp:Button runat="server" ID="Send" CommandArgument='<%# Eval("名次")+","+Eval("UID")+","+Eval("彩金") %>'  OnClick="TigerList_Send_Click"  OnClientClick="return confirmDelete();" Text="派彩" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>--%>
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
                   </td>
                   <tr>
                       <td colspan="2">
                           <asp:Button runat="server" ID="Send"  OnClick="Send_Click"  OnClientClick="return confirmDelete();" Text="全部派彩" />
                       </td>
                   </tr>
                   </table>
</asp:Content>

