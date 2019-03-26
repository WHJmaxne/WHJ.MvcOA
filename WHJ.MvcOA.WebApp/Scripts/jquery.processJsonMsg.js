(function (jqObj) {
    jqObj.extend(jqObj, {
        processJsonMsg: function (msgStr,callBack) {
            //1.将 json字符串 转成 js对象
            var jsonObj = null;
            try{
                jsonObj = JSON.parse(msgStr);
            } catch (e) {
                try {
                    jsonObj = eval(msgStr);//将 [....] 转成 js数组对象
                } catch (e1) {
                    jsonObj = eval("(" + msgStr + ")");// 将 {....} 转成 js对象
                }
            }

            function invokeCallBack(callBackA,callBackArgs)
            {
                if (callBackA) callBackA(callBackArgs);
            }

            //2.按照 规定格式 解析 json对象
            switch (jsonObj.Statu)
            {
                case 1://"ok"
                    $.msgBoxObj.showMsgOk(jsonObj.Msg, function () {
                        invokeCallBack(callBack, jsonObj);
                    });
                    break;
                case 2://"nook"
                    $.msgBoxObj.showMsgInfo(jsonObj.Msg, function () {
                        invokeCallBack(callBack, jsonObj);
                    });
                    break;
                case 3://"err"
                    $.msgBoxObj.showMsgErr(jsonObj.Msg, function () {
                        invokeCallBack(callBack, jsonObj);
                    });
                    break;
                case 4://"nologin"
                    $.msgBoxObj.showMsgInfo(jsonObj.Msg, function () {
                        invokeCallBack(callBack, jsonObj);
                    });
                    break;
                case 5://"noperission"
                    $.msgBoxObj.showMsgInfo(jsonObj.Msg, function () {
                        invokeCallBack(callBack, jsonObj);
                    });
                    break;
            }
        }
    });
})($);