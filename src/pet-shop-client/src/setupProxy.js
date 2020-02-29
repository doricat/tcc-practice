const proxy = require("http-proxy-middleware")

module.exports = app => {
    app.use(proxy(["/_configuration", "/.well-known"], {
        target: "http://localhost:6750"
    }));
};