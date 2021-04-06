<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="GetPlayerInfoAdminBatch.aspx.cs" Inherits="GetPlayerInfoAdminBatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btGetData" runat="server" Text="取得資料" OnClick="btGetData_Click" /> 
    <asp:Button ID="btCheckedAll" runat="server" Text="全選"  OnClick="btCheckedAll_Click" Visible="false"/>
    <asp:Button ID="btprocess" runat="server" Text="開始處理" OnClick="btprocess_Click"  Visible="false"/>
     <asp:GridView ID="gvList" runat="server" CellPadding="4" GridLines="None" 
            AllowSorting="True" onsorting="gvList_Sorting" ForeColor="#333333">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="狀態"  >
                    <ItemTemplate>
                        <asp:CheckBox ID="iReSet"  runat="server" Text="復原" Checked="false" ToolTip='<%#Eval("會員帳號") + "_" + Eval("開分號")  + "_" + Eval("ID") + "_" + Eval("遊戲代碼") %>' />
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
        
</asp:Content>

