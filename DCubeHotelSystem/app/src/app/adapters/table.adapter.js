"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var entity_1 = require("@ngrx/entity");
// Entity adapter
exports.TableAdapter = entity_1.createEntityAdapter({
    selectId: function (table) { return table.TableId; },
    sortComparer: false
});
exports.InitialState = exports.TableAdapter.getInitialState({
    CurrentTableId: ""
});
