(function() {
    $(function() {
        var app = new Vue({
            el: "#divVue",
            data: {
                loginform: {
                    Username: "admin",
                    Password: "123456"
                }
            },
            methods: {
                login: function() {
                    app.$refs.wait.show({
                        body: "登陆处理中..."
                    });
                    MyApp.send("/User/Login", app.$data.loginform, function(data) {
                        app.$refs.wait.hide();
                        if (data.Success) {
                            app.$refs.alert.show({
                                body: data.ServerMessage,
                                cb: function() {
                                    location = "index.html";
                                }
                            });

                        } else {
                            app.$refs.alert.show({
                                body: data.ServerMessage
                            });
                        }
                    });
                },
                reset: function() {
                    app.$data.loginform = {};
                }
            }
        });
    });
})();