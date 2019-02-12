(function() {
    $(function() {
        var app = new Vue({
            el: "#divVue",
            data: {
                welcome: MyApp.title,
                showajax: false,
                ajaxdata: {}
            },
            methods: {
                btnAjax: function() {
                    app.$refs.wait.show({
                        body: "程序处理中..."
                    });
                    MyApp.send("/Test", {}, function(data) {
                        app.$refs.wait.hide();
                        app.$data.showajax = true;
                        app.$data.ajaxdata = data;
                    });
                },
                closeMe: function() {
                    app.$refs.custom.hide();
                },
                btnDialog: function() {

                    app.$refs.confirm.show({
                        title: "标题",
                        body: "提示内容",
                        yes: "哈哈",
                        no: "嘻嘻",
                        cby: function() {
                            app.$refs.alert.show({
                                body: "确认",
                                cb: function() {
                                    app.$refs.custom.show({
                                        title: "添加用户信息",
                                        el: $("#divDialog")
                                    });
                                }
                            });
                        },
                        cbn: function() {
                            app.$refs.wait.show({
                                title: "等待对话框",
                                body: "程序处理中...",
                                cb: function() {
                                    console.log("处理完毕");
                                }
                            });

                            setTimeout(function() {
                                app.$refs.wait.hide();
                            }, 2000);
                        }

                    });
                }
            }
        });
    });
})();