<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master" CodeFile="GameChart.aspx.cs" Inherits="GameChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="javascript/jquery-3.5.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
    <canvas id="canvas" width="1800" height="300"></canvas>
    <script type="text/javascript">

        //获取上下文
        var canvas = document.getElementById("canvas");
        var ctx = canvas.getContext("2d");
        //描绘背景
        var gradient = ctx.createLinearGradient(0, 0, 0, 300);//createLinearGradient() 方法创建线性的渐变对象。
        gradient.addColorStop(0, "#e0e0e0");
        gradient.addColorStop(1, "#ffffff");
        ctx.fillStyle = gradient;
        ctx.fillRect = (0, 0, canvas.width, canvas.height);
        //描绘边框
        var grid_cols = 60;
        var grid_rows = 10;
        var cell_height = canvas.height / grid_rows;
        var cell_width = canvas.width / grid_cols;
        ctx.lineWidth = 1;
        ctx.strokeStyle = "#a0a0a0";
        //结束边框描绘
        ctx.beginPath();
        //准备画横线
        for (var col = 0; col <= grid_cols; col++) {
            var x = col * cell_width;
            ctx.moveTo(x, 0);
            ctx.lineTo(x, canvas.height);
        }
        //准备画竖线
        for (var row = 0; row <= grid_rows; row++) {
            var y = row * cell_height;
            ctx.moveTo(0, y);
            ctx.lineTo(canvas.width, y);
        }

        //完成描绘
        ctx.stroke();
        //畫斜線
        //var c = document.getElementById("canvas");
        //var cxt = c.getContext("2d");
        //ctx.lineWidth = 2;
        //cxt.moveTo(30, 30);
        //cxt.lineTo(0, 60);
        //ctx.stroke();


    </script>
    <br />
    <br />

    <asp:Label ID="lblRun" runat="server" Text="輪號:"></asp:Label>
    <asp:TextBox ID="txtlblRun" runat="server" Width="60px" class="txtlblRun" ></asp:TextBox><br>
    <asp:DropDownList ID="DropDownList1" runat="server">
    </asp:DropDownList>
    <input type="button" name="name" onclick="GcluekickAll()" value="查詢" />
    <a href="#" download="路圖.png" onclick="this.href=canvas.toDataURL();">下載</a>

    <script type="text/javascript">
        function draw(x, y, a) {//畫圓
            var c = document.getElementById("canvas");
            var ctx = c.getContext("2d");
            ctx.beginPath();
            ctx.arc(x, y, 14, 0, 2 * Math.PI, false);
            if (a.Win_Flag == 1) {
                ctx.fillStyle = "blue";
                ctx.fill();
            }
            if (a.Win_Flag == 2) {
                ctx.fillStyle = "red";
                ctx.fill();
            }
            if (a.Win_Flag == 3) {
                ctx.fillStyle = "green";
                ctx.fill();
            }
            ctx.stroke();
        };
        function number(xx, yy, i) {//文字

            var c = document.getElementById("canvas");
            var ctx = c.getContext("2d");
            ctx.font = "18px orbitron";

            ctx.fillText(i.No_Active, xx, yy);
            ctx.fill();
            ctx.stroke();
        }
        var x = 15;
        var y = 45;
        var xx = 0;
        var yy = 20;
        function right(x, y) {//不相同換行
            x = x + 30;
            y = 45;
            return [x, y];
        };
        function under(x, y) {//相同往下

            y = y + 30;
            return [x, y];
        };
        function bureauunder(xx) {//開頭局數換行

            xx = xx + 30;
            return [xx];
        };

        function GcluekickAll() {
            //$.ajax({
            //    type: "GET",
            //    url: "/AsyncRe/GameChart.ashx",
            //    success: function (jsonStr) {
            //        console.log(jsonStr);
            //        //var c = document.getElementById("canvas");
            //        //var ctx = c.getContext("2d");
            //        //ctx.beginPath();
            //        //ctx.arc(15, 45, 14, 0, 2 * Math.PI, false);
            //        //ctx.fillStyle = "red";
            //        //ctx.fill();
            //    }
            //});


            var DropDownList = $('Select').children("option:selected").val();
            var Run = $('.txtlblRun').val();
            axios.get('/AsyncRe/GameChart.ashx', { params: { DropDownList, Run }})
                .then(function (jsonStr) {//可能第一局是和局
                draw(x, y, jsonStr.data[0]);
                number(xx, yy, jsonStr.data[0]);
                if (jsonStr.data[0].Win_Flag == 3 || jsonStr.data[0].Win_Flag == jsonStr.data[1].Win_Flag) {//相同或和局,第一局和局要分開判斷
                    [x, y] = under(x, y);
                    draw(x, y, jsonStr.data[1]);
                }
                else {//換行,第一局和局要分開判斷
                    [xx] = bureauunder(xx, yy);
                    [x, y] = right(x, y);
                    draw(x, y, jsonStr.data[1]);
                    number(xx, yy, jsonStr.data[1]);
                }

                for (var i = 2; i < jsonStr.data.length; i++) {//相同或和局
                    if (jsonStr.data[i].Win_Flag == jsonStr.data[i - 1].Win_Flag || jsonStr.data[i].Win_Flag == 3) {
                        [x, y] = under(x, y);
                        draw(x, y, jsonStr.data[i]);
                    }
                    else {//換行
                        [x, y] = right(x, y);
                        [xx] = bureauunder(xx, yy);
                        draw(x, y, jsonStr.data[i]);
                        number(xx, yy, jsonStr.data[i]);
                    }
                }

            })
                .catch(function (error) {

                    console.log(error);
                })

        }
    </script>


</asp:Content>



