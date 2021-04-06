<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="NewJdbGameRaceResult.aspx.cs" Inherits="NewJdbGameRaceResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="selCal" runat="server" Text="日期" OnClick="selCal_Click" />
    <asp:Calendar ID="seldate" runat="server" OnSelectionChanged="seldate_SelectionChanged"></asp:Calendar>
    <asp:TextBox ID="seldateText" runat="server"></asp:TextBox>
    <asp:HiddenField ID="Hidseldate" runat="server" />
    <asp:Button ID="seldata" runat="server" Text="取得資料" OnClick="seldata_Click" />
         <asp:Literal ID="txtscript" runat="server" ></asp:Literal>
         <asp:Literal ID="Msg" runat="server"></asp:Literal> 
    <br>
    <asp:Repeater ID="RaceRt" runat="server"  OnItemDataBound="RaceRt_ItemDataBound" >
        <HeaderTemplate>
            <table>
                <tr>
        </HeaderTemplate>
        <ItemTemplate>
            <td>
                <asp:Label ID="title" runat="server" Text='<%#Eval("Name") %>' ></asp:Label>
                <asp:GridView runat="server" ID="DataItem"  AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                名次
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("名次") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                UID
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("JDB-UID") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                帳號
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("帳號") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                獎金
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("獎金") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="Msg"  runat="server" ></asp:Label> 
                <asp:HiddenField ID="JdbGameCode" runat="server"  Value='<%#Eval("JDBGameCode") %>'/> 
                <asp:HiddenField ID="H1GameCode" runat="server" Value='<%#Eval("H1GameCode") %>' /> 
                <asp:HiddenField ID="HasData" runat="server" Value="N" /> 
            <td>
        </ItemTemplate>
        <FooterTemplate>
            </tr>
            </table>
        </FooterTemplate>
    </asp:Repeater>
   
    <asp:Button ID="btDeliver" runat="server" Text="派送" OnClick="btDeliver_Click" />
</asp:Content>

