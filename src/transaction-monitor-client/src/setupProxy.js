const proxy = require("http-proxy-middleware")

module.exports = app => {
    app.use(proxy("/transaction_hub", { target: "http://localhost:6531", ws: true }))
}