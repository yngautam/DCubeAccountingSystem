export interface MenuConsumption {
    Id: number,
    CategoryId: number,
    ProductId: number,
    ProductPortionId: number,
    MenuConsumptionDetails: MenuConsumptionDetail[];
}

export interface MenuConsumptionDetail {
    Id: number,
    InventoryItemId: number,
    MenuConsumptionId: number,
    Quantity: number,
}