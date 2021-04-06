<%@ Page Title="" Language="C#" MasterPageFile="~/SovaMasterPage.master" AutoEventWireup="true" CodeFile="AutoAlert.aspx.cs" Inherits="AutoAlert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script src="javascript/jquery-3.5.0.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <audio controls id="voiceplay" style="display: none">
                <source  src="AccountAlert.mp3">
            </audio>
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>  
            <h3>黑名單警示</h3> 
            <asp:DropDownList runat="server" ID="dlType" OnSelectedIndexChanged="dlType_SelectedIndexChanged" AutoPostBack="true">
               <asp:ListItem Value="IP" Selected>IP</asp:ListItem>
               <asp:ListItem Value="Account">帳號</asp:ListItem>
               <asp:ListItem Value="Source">來源</asp:ListItem>
            </asp:DropDownList>
             <asp:Label runat="server" ID="Label1" Text=":" ></asp:Label>
            <asp:DropDownList runat="server" ID="dlSource"  Visible="false"></asp:DropDownList>
            <asp:TextBox runat="server" ID="txtTarget" Text=""></asp:TextBox>
            <asp:DropDownList runat="server" ID="dlCondition" >
               <asp:ListItem Value="AND" >和</asp:ListItem>
               <asp:ListItem Value="OR">或</asp:ListItem>
               <asp:ListItem Value="NONE" Selected>無</asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" ID="btAdd" Text="加入" OnClick="btAdd_Click" OnClientClick="return ChkConditionForm()"/>
            <asp:ListBox ID="ConditionList" runat="server" Width="500" Height="300"></asp:ListBox>
             <asp:Button runat="server" ID="btCut" Text="移除"  OnClick="btCut_Click"/>
            <asp:Button runat="server" ID="Button2" Text="立即套用"  OnClick ="Button2_Click"/><p>
<%-- *******************************************************************************************************************--%>
            <asp:CheckBox ID="chkAuto" runat="server" Checked="True" Font-Bold="True" Text="Auto"
                AutoPostBack="True" OnCheckedChanged="chkAuto_CheckedChanged" />
             <asp:CheckBox ID="IsCheck" runat="server" Checked="True" Font-Bold="True" Text="是否確認" 
                AutoPostBack="True" OnCheckedChanged="IsCheck_CheckedChanged" />
           <asp:Label ID="FindRecordDateMsg" runat="server" Text=""></asp:Label>
            <asp:GridView ID="FindRecordDateResult" runat="server"  OnRowCommand="FindRecordDateResult_RowCommand" OnRowDataBound="FindRecordDateResult_RowDataBound" >
                 <Columns> 
                     <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="FindUid" runat="server" CausesValidation="false" CommandName="FINDUID" CommandArgument='<%#Eval("帳號") +"="+ Eval("來源") %>' Text='族譜'  OnClientClick="clearInterval(intervalID);return true;"  />
                            <asp:Button ID="Button" runat="server" CausesValidation ="False" CommandName="MARK" CommandArgument='<%#Eval("ID") %>' Text="解除警示" OnClientClick="clearInterval(intervalID);return true;"  />
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="STOP" CommandArgument='<%#Eval("ID") +"="+ Eval("備註") +"="+ Eval("帳號")   %>' Text="停止追蹤"  OnClientClick="if (confirm('您確定要停止追蹤嗎?')==false) {return false;}else{clearInterval(intervalID);}" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Literal ID="objpage" runat="server"></asp:Literal>
           <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
                <script type="text/javascript">
                     function isNumber(value) {
             var re = /^[0-9]+$/;
             return re.test(value);
         }

         /**
         * @param {String} a String
         * @return {Boolean} true if the String is a valid IPv6 address; false otherwise
         */
         function isIPv6(value) {
             var components = value.split(":");

             if (components.length < 2 || components.length > 8) {
                 return false;
             }

             if (components[0] != "" || components[1] != "") {
                 // Address does not begin with a zero compression ("::")
                 if (!components[0].test(/^[\da-f]{1,4}/i)) {
                     // Component must contain 1-4 hex characters
                     return false;
                 }
             }

             var numberOfZeroCompressions = 0;
             for (var i = 1; i < components.length; ++i) {
                 if (components[i] == "") {
                     // We're inside a zero compression ("::")
                     ++numberOfZeroCompressions;
                     if (numberOfZeroCompressions > 1) {
                         // Zero compression can only occur once in an address
                         return false;
                     }
                     continue;
                 }
                 if (!components[i].test(/^[\da-f]{1,4}/i)) {
                     // Component must contain 1-4 hex characters
                     return false;
                 }
             }
             return true;
         }

         function chkIpV4(ip) {

             var ipArray;

             var isok = false;

             var star = false;

             if (isIPv6(ip)) return true;  //如果是IPV6就返回

             if (ip.indexOf(".") > 0) {
                 ipArray = ip.split(".");

                 if (4 == ipArray.length) {
                     if (isNumber(ipArray[0])) {
                         isok = ipArray[0] >= 0 && ipArray[0] <= 255;
                         if (!isok) return isok;
                     } else {
                         return false;
                     }

                     if (isNumber(ipArray[1]) && isok) {
                         isok = ipArray[1] >= 0 && ipArray[1] <= 255;
                         if (!isok) return isok;
                     } else {
                         isok = ipArray[1] == "*";
                         if (isok == true) {
                             star = true;
                         }

                         if (!isok) return isok;
                     }

                     if (isNumber(ipArray[2]) && isok && !star) {
                         isok = ipArray[2] >= 0 && ipArray[2] <= 255;
                         if (!isok) return isok;
                     } else {
                         isok = ipArray[2] == "*";
                         if (isok == true) {
                             star = true;
                         }

                         if (!isok) return isok;
                     }

                     if (isNumber(ipArray[3]) && isok && !star) {
                         isok = ipArray[3] >= 0 && ipArray[3] <= 255;
                         if (!isok) return isok;
                     } else {
                         isok = ipArray[3] == "*";

                         if (!isok) return isok;
                     }
                 }
             }

             return isok;
         }

         function ChkConditionForm()
         {
             var dlType = document.getElementById("<%=dlType.ClientID%>");  
             var dlSource = document.getElementById("<%=dlSource.ClientID%>");  
             var txtTarget = document.getElementById("<%=txtTarget.ClientID%>"); 

             if (dlType.value == "IP" || dlType.value == "Account")
             {
                 if (txtTarget.value == "" || txtTarget.value == undefined) {
                     if (dlType.value == "IP") {
                         alert("請輸入IP");
                         txtTarget.focus();
                         return false;
                     }

                     if (dlType.value == "Account") {
                         alert("請輸入帳號");
                         txtTarget.focus();
                         return false;
                     }
                 }else {
                     if (dlType.value == "IP") {
                         if (!chkIpV4(txtTarget.value)) {
                             alert("IP格式錯誤");
                             txtTarget.focus();
                             return false;
                         }
                     }
                 }
             }

             return true;
         }
                    var x = document.getElementById("voiceplay");

                    var isplay=<%=a%>;

                    function playAudio() {
                        x.play();
                    }

                    if(isplay>0)
                    {
                        x.play();
                    }
      </script>
           
</asp:Content>

