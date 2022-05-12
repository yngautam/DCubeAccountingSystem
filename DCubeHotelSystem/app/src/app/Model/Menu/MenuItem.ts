import { UnitType } from './../Inventory/UnitType';
export interface IMenuItem {
    Id: number,
    MenuItemPortionId: number,
    Name: string,
    categoryId: number,
    Barcode: string,
    Tag: string,
    UnitType: string;
    MenuItemPortions: IMenuItemPortion[];
    UnitTypes: UnitType[]
}
export interface IMenuItemPortion {
    Id: number,
    Name: string,
    MenuItemPortionId: number,
    Multiplier: number,
    Price: number,
    OpeningStock:number
}
export interface IScreenMenuItem {
    Id: number,
    Name: string,
    categoryId: number,
    MenuItem_Bool: boolean,
    MenuId: number
}

export class IScreenMenuItems {
    ItemId: number;
    categoryId: number;
    MenuId: number;
    MenuItem_Bool: boolean;
    constructor(ItemId, categoryId, MenuId, MenuItem_Bool) {
        this.ItemId = ItemId;
        this.categoryId = categoryId;
        this.MenuId = MenuId;
        this.MenuItem_Bool = MenuItem_Bool;
    }
}
export interface IMenuCategoryItem {
    Id: number,
    ItemId: number,
    categoryId: number
}
