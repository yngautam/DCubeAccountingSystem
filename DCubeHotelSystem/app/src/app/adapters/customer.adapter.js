"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var entity_1 = require("@ngrx/entity");
// Entity adapter
exports.CustomerAdapter = entity_1.createEntityAdapter({
    selectId: function (customer) { return customer.Id; },
    sortComparer: false
});
exports.InitialState = exports.CustomerAdapter.getInitialState({
    CurrentCustomerId: ""
});
