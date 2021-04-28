<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="SovaMasterPage.master" CodeFile="GameChart.aspx.cs" Inherits="GameChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .clubtable td.title {
            border: 1px solid black;
            color: White;
            background-color: #507CD1;
            font-weight: bold;
        }

        .clubtable tbody.content {
            border: 1px solid black;
            background-color: #EFF3FB;
        }
    </style>
    <script src="javascript/jquery-3.5.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
    <canvas id="canvas" width="1800" height="450"></canvas>

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
        var grid_rows = 15;
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
    <asp:Label ID="lbldata" runat="server" Text="請輸入日期："></asp:Label>
    <asp:TextBox ID="txtdata" runat="server" Width="200" class="txtdata"></asp:TextBox>
    <br />
    <asp:Label ID="lblclub" runat="server" Text="會員帳號:"></asp:Label>
    <asp:TextBox ID="txtlblclub" runat="server" Width="200px" class="txtlblclub"></asp:TextBox>
    <asp:Label ID="Label1" runat="server" Text="金錢單位:"></asp:Label>
    <select name="money" id="money">
        <option value="Million">萬</option>
        <option value="thousand">千</option>
    </select>
    <input type="button" name="name" onclick="onclickclub()" value="查詢" />
    <br>
    <select name="server_id" id="server_id" onchange="selecrun()">
        <option value="0">--選擇桌號--</option>
    </select>
    <select name="Run" id="Run">
        <option value="0">--選擇輪號--</option>
    </select>
    <input type="button" name="name" onclick="GcluekickAll()" value="查詢" />
    <input type="button" name="name" onclick="onclickClear()" value="清除" />
    <a href="#" download="路圖.png" onclick="this.href=canvas.toDataURL();">下載</a>
    <br>
    <%--style="display:none"--%>
    <table class="clubtable">
        <tr>
            <td class="title">下注金額</td>
            <td class="title">輸贏金額</td>
            <td class="title">局號</td>
            <td class="title">下注區</td>
        </tr>
        <tbody id="content" class="content">
        </tbody>
    </table>

    <script type="text/javascript">
        function draw(xa, y, a) {//畫圓
            var c = document.getElementById("canvas");
            var ctx = c.getContext("2d");
            ctx.beginPath();
            ctx.arc(xa, y, 14, 0, 2 * Math.PI, false);
            if (a.Win_Flag == 1) {
                ctx.strokeStyle = "blue";
                ctx.stroke();

            }
            if (a.Win_Flag == 2) {
                ctx.strokeStyle = "red";
                ctx.stroke();
            }
            if (a.Win_Flag == 3) {
                ctx.strokeStyle = "green";
                ctx.stroke();
            }

        };
        function number(xx, yy, i) {//文字

            var c = document.getElementById("canvas");
            var ctx = c.getContext("2d");
            ctx.font = "14px orbitron";
            ctx.fillStyle = 'black';
            ctx.fillText(i.No_Active, xx, yy);
        }
        function clubnote(xa, ya, i, MaHao) {//會員下注局數
            xa = xa - 12;
            ya = ya + 5;
            var c = document.getElementById("canvas");
            var ctx = c.getContext("2d");
            ctx.font = "10px orbitron";
            if (MaHao == 'Zhuang') {
                ctx.fillStyle = 'red';
            }
            else { ctx.fillStyle = 'blue'; }
            ctx.fillText(i, xa, ya);
        }
        var xa = 15;
        var ya = 45;
        var xx = 0;
        var yy = 20;
        function right(xa, ya) {//不相同換行
            xa = xa + 30;
            ya = 45;
            return [xa, ya];
        };
        function under(xa, ya) {//相同往下

            ya = ya + 30;
            return [xa, ya];
        };
        function bureauunder(xx) {//開頭局數換行

            xx = xx + 30;
            return [xx];
        };
        function onclickClear() {//清除畫布

            let canvas = document.getElementById("canvas");
            var ctx = canvas.getContext("2d");
            canvas.height = "300";
            canvas.width = "1800";

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
            xa = 15;
            ya = 45;
            xx = 0;
            yy = 20;
            j = 0;
        }

        function getDrawing() {
            var DropDownList = $('#server_id').children("option:selected").val();
            var run = $('#Run').children("option:selected").val();
            return axios.get('/AsyncRe/GameChart.ashx', { params: { DropDownList, run } });
        }

        function getclubmessage() {
            var Club_Ename = $('.txtlblclub').val();
            var DropDownList = $('#server_id').children("option:selected").val();
            var run = $('#Run').children("option:selected").val();
            return axios.get('/AsyncRe/GameChartclubmessage.ashx', { params: { DropDownList, run, Club_Ename } });
        }


        var result;
        var j = 0;

        function GcluekickAll() {
            var run = $('#Run').children("option:selected").val();
            var No_Active = result.data.filter(p =>
               p.No_Run == run && (p.MaHao == 'Zhuang' || p.MaHao == 'Xian')
            );

            axios.all([getDrawing(), getclubmessage()])
                .then(axios.spread((Drawing, clubmess) => {//第一局

                    var Wrapvalue;
                    number(xx, yy, Drawing.data[0]);
                    draw(xa, ya, Drawing.data[0]);

                    if (j != No_Active.length) {
                        if (No_Active[j].No_Active == Drawing.data[0].No_Active) {
                            clubnote(xa, ya, No_Active[j].Stake_Score, No_Active[j].MaHao)
                            j++
                        }
                    }
                    if (Drawing.data[0].Win_Flag == 3 || Drawing.data[0].Win_Flag == Drawing.data[1].Win_Flag) {//相同或和局,判斷第二局         
                        [xa, ya] = under(xa, ya);
                        draw(xa, ya, Drawing.data[1]);
                        if (j != No_Active.length) {
                            if (No_Active[j].No_Active == Drawing.data[1].No_Active) {
                                clubnote(xa, ya, No_Active[j].Stake_Score, No_Active[j].MaHao)
                                j++
                            }
                        }
                    }
                    else {//判斷是否與第一局不同
                        [xx] = bureauunder(xx, yy);
                        [xa, y] = right(xa, ya);
                        number(xx, yy, Drawing.data[1]);
                        draw(xa, ya, Drawing.data[1]);
                        Wrapvalue = Drawing.data[1];
                        if (j != No_Active.length) {
                            if (No_Active[j].No_Active == Drawing.data[1].No_Active) {
                                clubnote(xa, ya, No_Active[j].Stake_Score, No_Active[j].MaHao)
                                j++
                            }
                        }
                    }

                    for (var i = 2; i < Drawing.data.length; i++) {//相同或和局
                        if (Drawing.data[i].Win_Flag == Wrapvalue || Drawing.data[i].Win_Flag == 3
                            ) {

                            [xa, ya] = under(xa, ya);
                            draw(xa, ya, Drawing.data[i]);
                            if (j != No_Active.length) {
                                if (No_Active[j].No_Active == Drawing.data[i].No_Active) {
                                    clubnote(xa, ya, No_Active[j].Stake_Score, No_Active[j].MaHao)
                                    j++
                                }
                            }
                        }
                        else {//換行
                            [xa, ya] = right(xa, ya);
                            [xx] = bureauunder(xx, yy);
                            number(xx, yy, Drawing.data[i]);
                            draw(xa, ya, Drawing.data[i]);
                            Wrapvalue = Drawing.data[i].Win_Flag;
                            if (j != No_Active.length) {
                                if (No_Active[j].No_Active == Drawing.data[i].No_Active) {
                                    clubnote(xa, ya, No_Active[j].Stake_Score, No_Active[j].MaHao)
                                    j++
                                }
                            }
                        }

                    }
                    var item;
                    for (i = 0; i < clubmess.data.length; i++) {
                        item += '<tr><td>' + clubmess.data[i].Stake_Score + '</td><td>' + clubmess.data[i].Account_Score + '</td><td>' + clubmess.data[i].No_Active + '</td><td>' + clubmess.data[i].MaHao + '</td></tr>'
                    }
                    $('#content').html(item);
                }));

            //axios.get('/AsyncRe/GameChart.ashx', { params: { DropDownList, run } })
            //    .then(function (jsonStr) {//可能第一局是和局
            //        draw(x, y, jsonStr.data[0]);
            //        number(xx, yy, jsonStr.data[0]);
            //        if (jsonStr.data[0].Win_Flag == 3 || jsonStr.data[0].Win_Flag == jsonStr.data[1].Win_Flag) {//相同或和局,第一局和局要分開判斷
            //            [x, y] = under(x, y);
            //            draw(x, y, jsonStr.data[1]);
            //        }
            //        else {//換行,第一局和局要分開判斷
            //            [xx] = bureauunder(xx, yy);
            //            [x, y] = right(x, y);
            //            draw(x, y, jsonStr.data[1]);
            //            number(xx, yy, jsonStr.data[1]);
            //        }

            //        for (var i = 2; i < jsonStr.data.length; i++) {//相同或和局
            //            if (jsonStr.data[i].Win_Flag == jsonStr.data[i - 1].Win_Flag || jsonStr.data[i].Win_Flag == 3) {
            //                [x, y] = under(x, y);
            //                draw(x, y, jsonStr.data[i]);
            //            }
            //            else {//換行
            //                [x, y] = right(x, y);
            //                [xx] = bureauunder(xx, yy);
            //                draw(x, y, jsonStr.data[i]);
            //                number(xx, yy, jsonStr.data[i]);
            //            }
            //        }

            //    })
            //    .catch(function (error) {

            //        console.log(error);
            //    })

        }
        function selecrun() {
            var DropDownList = $('#server_id').children("option:selected").val();
            var NO_Run = result.data.filter(p =>
                p.Server_id == DropDownList
            );
            NO_Run = NO_Run.map(function (item, index, array) {

                return item.No_Run;


            });
            NO_Run = Array.from(new Set(NO_Run));
            var select = document.getElementById("Run");
            $('#Run').find('option').remove().end()
            var option2 = document.createElement("option");

            option2.appendChild(document.createTextNode("選擇輪號"));
            select.appendChild(option2);

            for (var x = 0; x < NO_Run.length; x++) {
                var select = document.getElementById("Run");
                select.options[0].selected = true;
                var option = document.createElement("option");
                option.setAttribute("value", NO_Run[x]);
                option.appendChild(document.createTextNode(NO_Run[x]));
                select.appendChild(option);
            }
        }

        function onclickclub() {
            var Club_Ename = $('.txtlblclub').val();
            var selectdata = $('.txtdata').val();
            var money = $('#money').children("option:selected").val();
            axios.get('/AsyncRe/GameChartselect.ashx', { params: { Club_Ename, selectdata, money } })
                .then(function (jsonStr) {
                    result = jsonStr;
                    var Server_id = jsonStr.data.map(function (item, index, array) {
                        return (item.Server_id);
                    });
                    var Server_Name = jsonStr.data.map(function (item, index, array) {
                        return (item.Server_Name);
                    });
                    Server_id = Array.from(new Set(Server_id));
                    Server_Name = Array.from(new Set(Server_Name));
                    var select = document.getElementById("server_id");
                    $('#server_id').find('option').remove().end()
                    var option2 = document.createElement("option");
                    option2.appendChild(document.createTextNode("選擇桌號"));
                    select.appendChild(option2);
                    for (var x = 0; x < Server_id.length; x++) {
                        select.options[0].selected = true;
                        var option = document.createElement("option");
                        option.setAttribute("value", Server_id[x]);
                        option.appendChild(document.createTextNode(Server_Name[x]));
                        select.appendChild(option);
                    }
                }).catch(function (error) {

                    console.log(error);
                })

        }
    </script>


</asp:Content>



