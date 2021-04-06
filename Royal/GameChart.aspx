<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master" CodeFile="GameChart.aspx.cs" Inherits="GameChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
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

	</script>
    <br />
    <br />
    <asp:Button ID="Inquire" runat="server"   Text="查詢"  Onclick="btnInquire_Click" />
</asp:Content>


