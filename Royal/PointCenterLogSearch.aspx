<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="PointCenterLogSearch.aspx.cs" Inherits="PointCenterLogSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script src="javascript/jquery-3.5.0.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3><%=Resources.Resource.PointCenterLogSearch %></h3>
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>    
    <%--*******************************************************************************************************--%>    
        <asp:Label ID="lbsubsystem" runat="server" Text="子系統︰" ></asp:Label>
            <asp:TextBox ID="txtsubsystem" runat="server" Text="H1" ></asp:TextBox> 
     <%--***************************************************************** --%> 
               <asp:Label ID="lbwebsite" runat="server" Text="網站︰" ></asp:Label>
            <asp:TextBox ID="txtwebsite" runat="server" Text="H1AG" ></asp:TextBox> 
     <%--***************************************************************** --%> 
               <asp:Label ID="lbvendor_id" runat="server" Text="廠商_識別碼︰" ></asp:Label>
            <asp:TextBox ID="txtvendor_id" runat="server" Text="JDB168" ></asp:TextBox> 
     <%--***************************************************************** --%> 
               <asp:Label ID="lb_MemberUid" runat="server" Text="登入帳號UID︰"  ForeColor="Red" ></asp:Label>
            <asp:TextBox ID="txt_MemberUid" runat="server" Text="" ></asp:TextBox> 
     <%--***************************************************************** --%>   
           <asp:Label ID="lbMemberAccount" runat="server" Text="登入帳號︰" ForeColor="Red" ></asp:Label>
            <asp:TextBox ID="txtMemberAccount" runat="server" Text="" ></asp:TextBox> 
     <%--***************************************************************** --%>  
           <asp:Button ID="Button1" runat="server" Text="查詢" onclick="btMember_KaDan_Check_Click" OnClientClick="return  Member_KaDan_Check();" />
     <%--***************************************************************** --%> 
        <asp:GridView ID="Member_KaDan_Check1" runat="server" >
             <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
        </p>
        <asp:GridView ID="Member_KaDan_Check2" runat="server" >
             <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
          </p>
        <asp:GridView ID="Member_KaDan_Check3" runat="server"   OnRowDataBound="Member_KaDan_Check3_RowDataBound" >
             <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
         </p>
        <asp:GridView ID="Member_KaDan_Check4" runat="server"  OnRowDataBound="Member_KaDan_Check4_RowDataBound">
             <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
         <asp:Literal ID="objMsg1" runat="server"></asp:Literal>
    
       <hr style="width='80%'" />

     <%--*******************************************************************************************************--%>
           <div style="color:red;">※第一段查不到資料再使用此查詢</div> 
           <asp:Label ID="LbMemberUid" runat="server" Text="登入帳號UID︰"  ForeColor="Red"></asp:Label>
            <asp:TextBox ID="txtMemberUid" runat="server" Text="" ></asp:TextBox> 
            <%--***************************************************************** --%>                     
            <asp:Button ID="btnQuery" runat="server" Text="查詢" onclick="btnQuery_Click"  OnClientClick="return chkWrongUidInfoForm();"/>
               <%--***************************************************************** --%>  
       <asp:GridView ID="WrongUidInfo" runat="server"  CellPadding="4" GridLines="Both" ForeColor="#333333"  OnRowDataBound="WrongUidInfo_RowDataBound"  >
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>  
        <asp:Literal ID="objMsg" runat="server"></asp:Literal>
   

    <script type="text/javascript" >

  
        function openTextWindow(me)
        {
            var w = window.open();
            w.document.write( me.innerHTML);         
        }

        function chkWrongUidInfoForm()
        {
            var txtMemberUid = document.getElementById("<%=txtMemberUid.ClientID%>");
            if (txtMemberUid.value == "" || txtMemberUid == undefined)
            {
                alert("請輸入登入帳號UID!!");
                txtMemberUid.focus();
                return false;
            }
            return true;
        }

        $(document).ready(function () {
           $("#<%=txtsubsystem.ClientID%>").focus(function () {
                $("#<%=txtsubsystem.ClientID%>").val("");
           });

            $("#<%=txtwebsite.ClientID%>").focus(function () {
                $("#<%=txtwebsite.ClientID%>").val("");
           });

            $("#<%=txtvendor_id.ClientID%>").focus(function () {
                $("#<%=txtvendor_id.ClientID%>").val("");
           }); 
        });

        function Member_KaDan_Check()
        {
            var txtsubsystem = document.getElementById("<%=txtsubsystem.ClientID%>");
            var txtwebsite = document.getElementById("<%=txtwebsite.ClientID%>");
            var txtvendor_id = document.getElementById("<%=txtvendor_id.ClientID%>");
            var txt_MemberUid = document.getElementById("<%=txt_MemberUid.ClientID%>");
            var txtMemberAccount = document.getElementById("<%=txtMemberAccount.ClientID%>");
            var MemberUid = false;
            var MemberAccount = false;


            if (txtsubsystem.value == "" || txtsubsystem.value == undefined)
            {
                alert("請輸入子系統!!");
                txtsubsystem.focus();
                return false;
            }

            if (txtwebsite.value == "" || txtwebsite.value == undefined) {
                alert("請輸入網站!!");
                txtwebsite.focus();
                return false;
            }

            if (txtvendor_id.value == "" || txtvendor_id.value == undefined) {
                alert("請輸入廠商_識別碼!!");
                txtvendor_id.focus();
                return false;
            }

            if (txt_MemberUid.value == "" || txt_MemberUid.value == undefined) {
                MemberUid = true;
            }

            if (txtMemberAccount.value == "" || txtMemberAccount.value == undefined) {
                MemberAccount = true
            }

            if ( MemberUid == true &&  MemberAccount == true )
            {
                if (txt_MemberUid.value == "" || txt_MemberUid.value == undefined) {
                    alert("請輸入登入帳號UID!!");
                    txt_MemberUid.focus();
                    return false;
                } 
            }

            return true;
        }
    </script>
</asp:Content>


