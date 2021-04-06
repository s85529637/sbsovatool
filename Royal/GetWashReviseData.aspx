<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="GetWashReviseData.aspx.cs" Inherits="GetWashReviseData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h3><%=Resources.Resource.RTGWashRevise %></h3>
     <asp:Button ID="btGetWashRevise" runat="server" Text="刷新資料" OnClick="btGetWashRevise_Click" /><p>     
     <asp:GridView ID="WashReviseList" runat="server" CellPadding="4" GridLines="Both"
        AllowSorting="True" ForeColor="#333333" AutoGenerateColumns="false">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField   >
                <HeaderTemplate>
                    會員ID
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Club_id") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField   >
                <HeaderTemplate>
                    開分號
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_SessionId") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField   >
                <HeaderTemplate>
                    遊戲
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_Game_id") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    原下注
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Stake_Score") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    新下注
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_Stake_Score") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    原輸贏
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Account_Score") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    新輸贏
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_Account_Score") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField  >
                <HeaderTemplate>
                    原筆數
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Rows") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    新筆數
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_Rows") %>
                </ItemTemplate>
            </asp:TemplateField>
             <%--*****************************************--%>
             <asp:TemplateField  >
                <HeaderTemplate>
                    原公點
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_PKPoint") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    新公點
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_PKPoint") %>
                </ItemTemplate>
            </asp:TemplateField>
               <%--*****************************************--%>
             <asp:TemplateField  >
                <HeaderTemplate>
                    原分潤
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_SharePoint") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    新分潤
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_SharePoint") %>
                </ItemTemplate>
            </asp:TemplateField>
             <%--*****************************************--%>
            <asp:TemplateField  >
                <HeaderTemplate>
                    原彩金
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_JackPot") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    新彩金
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("RTG_JackPot") %>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField  >
                <HeaderTemplate>
                    是否已經過帳
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("IsHistory") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    是否處理
                </HeaderTemplate>
                <ItemTemplate>
                    <%# ChkIsToDo(Eval("IsHistory").ToString(),Eval("RTG_Rows").ToString(),Eval("H1_Rows").ToString(), Eval("RTG_Stake_Score").ToString(),Eval("H1_Stake_Score").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    No_Run
                </HeaderTemplate>
                <ItemTemplate>
                     <%#Eval("H1_No_Run") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    No_Active
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_No_Active") %>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField  >
                <HeaderTemplate>
                    Stake_id
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Stake_id") %>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField  >
                <HeaderTemplate>
                    ZhuDan_Type
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_ZhuDan_Type") %>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField  >
                <HeaderTemplate>
                    Active
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Active") %>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField  >
                <HeaderTemplate>
                    Desk_id
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("H1_Desk_id") %>
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
         <p>
    <asp:Button ID="btcomplete" runat="server" Text="修改" OnClick="btcomplete_Click" OnClientClick="return confirm('您確定要進行修改!!');" />
    <asp:Literal ID="Msg" runat="server"></asp:Literal>
</asp:Content>


