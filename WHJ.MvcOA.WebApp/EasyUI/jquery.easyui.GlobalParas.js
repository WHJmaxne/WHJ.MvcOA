(function (jqObj) {
    jqObj.extend(jqObj, {
        globalParas: {
            updateDataGridInTab: function () {
                window.top.updateTab();//跟新 datagrid
            },
            progresstimer: null,
            //代理方法：显示父窗体的easyui的进度加载条
            progressShow: function () {
                window.top.$.messager.progress();
                ////3.4显示 父窗体上的 进度条
                //window.top.document.getElementById("progressWin").style.display = "block";
                ////3.5将 父窗体上的 进度条 在父窗体上 居中显示
                //this.center("progressWin", window.top);
                ////3.6进度条 进度数值
                //var time1 = 0;
                ////3.7每个500毫秒，更新进度
                //$.globalParas.progresstimer = setInterval(function () {
                //    window.top.topHelper.commonProgress.progressbar("setValue", time1);
                //    if (time1 < 100) time1 += 20;
                //    else time1 = 0;
                //}, 500);
                
            },
            //代理方法：关闭父窗体的easyui的进度加载条
            progressClose: function () {
                window.top.$.messager.progress("close");
                ////隐藏 父窗体上的 进度条
                //window.top.document.getElementById("progressWin").style.display = "none";
                ////清空进度条计时器
                //clearInterval($.globalParas.progresstimer);
                ////重置进度条
                //window.top.topHelper.commonProgress.progressbar("setValue", 0);
            },
            //代理方法：调用后台主页面的topHelper.showComWindow(title, url, width, height)方法
            showComWindow: function (title, url, width, height) {
                window.top.topHelper.showComWindow(title, url, width, height);
                //this.progressShow();
            },
            //代理方法：调用后台主页面的topHelper.showComWindow(title, url, width, height)方法
            closeComWindow: function () {
                window.top.topHelper.closeComWindow();
            },
            //将序列化成json格式后日期(毫秒数)转成日期格式
            changeDateFormat: function (cellval) {
                var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                return date.getFullYear() + "-" + month + "-" + currentDate;
            },
            datagrid:{
                url:null,
                columns: null,
                fitColumns: true,
                idField: null,
                singleSelect:true,//单选
                rownumbers: true,
                showHeader: true,
                showFooter: false,
                pagination: true,//启用分页
                pageNumber: 1,//第一次请求默认请求的页码
                pageList: [5, 10, 15, 20, 30, 50, 100],//页容量数组
                pageSize: 10,//页容量（必须和 pageList 里某一个值一致）
                onLoadSuccess: function (jsonObj) {
                    //如果Datagrid获取的服务器数据 包含 Statu的话，那就可以证明在过滤器验证权限时失败，那就将过滤器返回的权限消息交给msgAlert统一处理！
                    if (jsonObj.Statu) {
                        $.msgAlert(jsonObj);
                    }
                },
                toolbar: [{
                    text: "新增",
                    iconCls: 'icon-add',
                    handler: function () { alert('新增') }
                }, '-', {
                    text: "删除",
                    iconCls: 'icon-remove',
                    handler: function () { alert('删除') }
                }, '-', {
                    text: "修改",
                    iconCls: 'icon-edit',
                    handler: function () { alert('修改') }
                }],
                init: function (url, columns, idFiled) {
                    this.url = url;
                    this.columns = columns;
                    this.idField = idFiled;
                },
                //绑定指定名字的按钮方法
                bindBtnEvent: function (text, func) {
                    //1.根据按钮名字
                    for (var i = 0; i < this.toolbar.length; i++) {
                        if (this.toolbar[i].text == text) {
                            this.toolbar[i].handler = func;
                            break;
                        }
                    }
                },
                //新增按钮
                addBtn: function (text, func, iconCls) {
                    if (this.toolbar.length > 0) {
                        this.toolbar[this.toolbar.length] = "-";
                    }
                    this.toolbar[this.toolbar.length] = {
                        text: text,
                        iconCls: iconCls ? iconCls : 'icon-ok',
                        handler: func
                    };
                },
                //根据按钮名字，删除按钮
                removeBtn: function (text) {
                    //1.根据按钮名字，找到按钮所在下标
                    var btnIndex = -1;
                    for (var i = 0; i < this.toolbar.length; i++) {
                        if (this.toolbar[i].text == text) {
                            btnIndex = i;
                            break;
                        }
                    }
                    //1.1根据找到的按钮下标，移除按钮
                    this.removeBtnByIndex(btnIndex);
        
                    //2.找到相邻的两个【-】，如果有，则删除一个
                    var splitIndex = -1;
                    for (var i = 0; i < this.toolbar.length; i++) {
                        if (this.toolbar[i]=="-" && this.toolbar[i] == this.toolbar[i + 1]) {
                            splitIndex = i;
                            break;
                        }
                    }
                    //2.1根据找到的 分隔符 下标，移除 分隔符
                    this.removeBtnByIndex(splitIndex);
        
                    if (this.toolbar.length <= 0) return;
                    //3.如果数组的第一个是分隔符，则删除掉此分隔符
                    if (this.toolbar[0] == "-") {
                        this.removeBtnByIndex(0);
                    }
                    //4.如果数组的最后一个是分隔符，则删除掉此分隔符
                    if (this.toolbar[this.toolbar.length-1] == "-") {
                        this.removeBtnByIndex(this.toolbar.length - 1);
                    }
                },
                //根据按钮下标，删除按钮
                removeBtnByIndex: function (itemIndex) {
                    //如果没找到要删除的按钮，则停止执行
                    if (itemIndex <= -1) return;
                    for (var i = itemIndex; i < this.toolbar.length - 1; i++) {
                        this.toolbar[i] = this.toolbar[i + 1];
                    }
                    this.toolbar.length = this.toolbar.length - 1;
                }
            },
            //让元素  居中
            center: function (id, windowParam) {
                var winObj = window;
                if (windowParam != window) winObj = windowParam;
                var eDiv = winObj.document.getElementById(id);

                // 计算left
                // 当前窗口宽度
                var nDivLeft = $(winObj).width();
                // 整体宽度减去当前元素的宽度
                nDivLeft = nDivLeft - parseInt(eDiv.offsetWidth);
                // 计算中间位置
                nDivLeft = nDivLeft / 2;
                // 当前元素是展示在屏幕中间的，加上滚动条滚动的距离
                nDivLeft = nDivLeft + $(document).scrollLeft();
                // 设置对象的left 坐标
                $.style(eDiv, "left", nDivLeft);
                // 计算Top, 原理与上边的原理一致
                var nDivTop = $(winObj).height();
                nDivTop = nDivTop - parseInt(eDiv.offsetHeight);
                nDivTop = nDivTop / 2;
                nDivTop = nDivTop + $(document).scrollTop();
                $.style(eDiv, "top", nDivTop);
            }
        }
    });
})($);
