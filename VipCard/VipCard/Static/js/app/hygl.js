(function() {
    $(function() {
        var app = new Vue({
            el: "#divVue",
            data: {
                list: [],
                vipcard: {}
            },
            methods: {
                showAdd: function() {
                    app.$data.vipcard = {
                        "Sex": "m"
                    };
                    app.$refs.custom.show({
                        title: "会员开卡",
                        el: $("#addDialog")
                    });
                },
                hideAdd: function() {
                    app.$refs.custom.hide();
                },
                addCard: function() {
                    app.$refs.wait.show({
                        body: "数据处理中..."
                    });

                    MyApp.send("/VipCard/Add", app.$data.vipcard, function(data) {
                        app.$refs.wait.hide();
                        if (data.Success) {
                            app.$refs.alert.show({
                                body: data.ServerMessage,
                                cb: query
                            });
                        } else {
                            app.$refs.alert.show({
                                body: data.ServerMessage
                            });
                        }

                    });
                },
                showChange: function() {
                    app.$refs.custom.show({
                        title: "会员换卡",
                        el: $("#changeDialog")
                    });
                },
                hideChange: function() {
                    app.$refs.custom.hide();
                },
                showCharge: function() {
                    app.$refs.custom.show({
                        title: "会员充值",
                        el: $("#chargeDialog")
                    });
                },
                hideCharge: function() {
                    app.$refs.custom.hide();
                },

            }
        });

        function query() {
            app.$refs.wait.show({
                body: "数据查询中..."
            });

            MyApp.send("/VipCard/Query", {}, function(data) {
                app.$refs.wait.hide();
                if (data.Success) {
                    app.$data.list = data.Datas.list;
                } else {
                    app.$refs.alert.show({
                        body: data.ServerMessage
                    });
                }
            });
        }

        query();

    });

})();