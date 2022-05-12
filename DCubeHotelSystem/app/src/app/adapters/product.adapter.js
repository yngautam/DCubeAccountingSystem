"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var entity_1 = require("@ngrx/entity");
// Entity adapter
exports.ProductAdapter = entity_1.createEntityAdapter({
    selectId: function (product) { return product.Id; },
    sortComparer: false
});
exports.InitialState = exports.ProductAdapter.getInitialState();
