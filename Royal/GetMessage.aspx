<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master"
    MaintainScrollPositionOnPostback="true" Inherits="GetMessage" CodeFile="GetMessage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="javascript/stran.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <h3><%=Resources.Resource.GetMessage %></h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            當前系統公告：
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td>
                            <asp:GridView ID="gvCNow" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="Message_Big5" HeaderText="繁中">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="Message_Gb" HeaderText="簡中">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="Message_En" HeaderText="英文">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Message_Tg" HeaderText="泰文">
                                        <ItemStyle Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Datetime" HeaderText="時間">
                                        <ItemStyle Width="100px" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ServerName" HeaderText="桌別">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
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
                        </td>
                        <td align="center" valign="top">
                            <span style="font-size: 12px;">
                                <input onclick="convert(1)" type="button" value="繁體" style="width: 6em; padding: 4px; font-size: 16px">
                                <input onclick="convert(0)" type="button" value="簡體" style="width: 6em; padding: 4px; font-size: 16px">
                                <input onclick="txt.value = ''" type="button" value="清除" style="width: 6em; padding: 4px; font-size: 16px"><br>
                                <textarea id="txt" rows="25" cols="60" onfocus="clearTextarea('once')">-- 輸入或貼上繁體或簡體後再進行轉換 --</textarea>
                                <br>
                            </span>
                        </td>
                    </tr>
                </tbody>
            </table>
            歷史系統公告：
            <asp:GridView ID="gvCOld" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Message_Big5" HeaderText="繁中">
                        <ItemStyle Width="300px" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Message_Gb" HeaderText="簡中">
                        <ItemStyle Width="300px" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="Message_En" HeaderText="英文">
                        <ItemStyle Width="300px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Message_Tg" HeaderText="泰文">
                        <ItemStyle Width="300px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Datetime" HeaderText="時間">
                        <ItemStyle Width="100px" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ServerName" HeaderText="桌別">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
