"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var entity_1 = require("@ngrx/entity");
// Entity adapter
exports.TicketAdapter = entity_1.createEntityAdapter({
    selectId: function (ticket) { return ticket.Id; },
    sortComparer: false
});
exports.InitialState = exports.TicketAdapter.getInitialState({
    CurrentTicketId: ""
});
