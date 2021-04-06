<%@ Page Language="C#" MasterPageFile="SovaMasterPage.master" AutoEventWireup="true"
    CodeFile="MoveStakeAccount.aspx.cs" Inherits="MoveStakeAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>><%=Resources.Resource.MoveStakeAccount %></title>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <audio controls id="voiceplay" style="display: none">
        <source  src="google.mp3">
    </audio>
     
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <label ID="labcount"><%=a %></label>
            <h3>遊戲卡單處理</h3>
            <asp:CheckBox ID="chkAuto" runat="server" Checked="True" Font-Bold="True" Text="Auto"
                AutoPostBack="True" OnCheckedChanged="chkAuto_CheckedChanged" />
            <asp:GridView ID="OpenList" runat="server" BackColor="White" BorderColor="#999999"
                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnRowCommand="OpenList_RowCommand">
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                CommandName='<%# Eval("Id") %>' OnClientClick="if (confirm('您確定要移動此筆注單嗎?')==false) {return false;}"
                                Text="移動此筆" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#000065" />
            </asp:GridView>
            <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var x = document.getElementById("voiceplay");
        

        function playAudio() {
            x.play();
        }
         var intervalID = setInterval(function () {
            var lbltext = document.getElementById('labcount').innerHTML;
             var a = lbltext;
             console.log(lbltext);

            if (lbltext > 0)
                playAudio();
        }, 5000);
    </script>
   
</asp:Content>
