export interface IInventoryItem {
    Id: number,
    Name?: string,
    //GroupCode: string,
    BaseUnit: string,
    TransactionUnit: string,
    TransactionUnitMultiplier: number,
    Category: ICategory[];
}

export class ICategory {
    Id: number;
    Name: string;
}

export class InventoryItem {
    Id: number;
    Name: string;
    GroupCode: string;
    BaseUnit: string;
    TransactionUnit: string;
    TransactionUnitMultiplier: number;
    Category: string;

    public constructor(Name: string) {
        this.Name = Name;
    }
}
