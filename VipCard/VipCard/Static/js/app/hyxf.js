(function() {
    $(function() {
        var app = new Vue({
            el: "#divVue",
            data: {
                list: [],
                glist: [],
                vlist: [],
                goods: {}
            },
            methods: {
                showAdd: function() {
                    app.$data.goods = {};

                    app.$refs.custom.show({
                        title: "会员消费",
                        el: $("#addDialog")
                    });
                },
                hideAdd: function() {
                    app.$refs.custom.hide();
                },
                addGoods: function() {
                    var gid = app.$data.goods.Goods.gid;
                    delete app.$data.goods.Goods;
                    app.$data.goods.Gid = gid;
                    MyApp.send("/Goods/Add", app.$data.goods, function(data) {
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

                }
            }
        });

        function query() {
            app.$refs.wait.show({
                body: "数据查询中..."
            });

            MyApp.send("/Goods/Query", {}, function(data) {
                app.$refs.wait.hide();
                if (data.Success) {
                    app.$data.list = data.Datas.list;
                    app.$data.glist = data.Datas.glist;
                    app.$data.vlist = data.Datas.vlist;
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