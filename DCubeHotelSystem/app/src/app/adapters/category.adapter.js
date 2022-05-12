"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var entity_1 = require("@ngrx/entity");
// Entity adapter
exports.CategoryAdapter = entity_1.createEntityAdapter({
    selectId: function (category) { return category.Id; },
    sortComparer: false
});
exports.InitialState = exports.CategoryAdapter.getInitialState();
