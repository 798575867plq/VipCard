(function() {
    $(function() {
        var app = new Vue({
            el: "#divVue",
            data: {
                list: []
            },
            methods: {

            }
        });

        function query() {
            app.$refs.wait.show({
                body: "数据查询中..."
            });

            MyApp.send("/VipCard/QueryRecord", {}, function(data) {
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