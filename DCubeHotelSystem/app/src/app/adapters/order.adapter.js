"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var entity_1 = require("@ngrx/entity");
// Entity adapter
exports.OrderAdapter = entity_1.createEntityAdapter({
    selectId: function (order) { return order.OrderNumber; },
    sortComparer: false
});
exports.InitialState = exports.OrderAdapter.getInitialState({
    CurrentOrderId: ""
});
