(function(win) {
  var app = {
    "title": "会员卡管理",
    "server": ""
  };
  app.send = function(url, data, cb) {
    var senddata = {
      "jstimeStamp": new Date().getTime()
    };
    if (data) {
      senddata = data;
      senddata.jstimeStamp = new Date().getTime();
    }

    $.post(app.server + url, senddata, function(data) {
      if (cb) {
        cb(data);
      }
    }, "json");

  };

  win.MyApp = app;
})(window);