export interface IProduct {
    id: number;
    createdOn: Date;
    updatedOn: Date;
    name: string;
    description: string;
    price: number;
    isTaxable: boolean;
    isArchived: boolean;
    idealQuantity: number;
}

export interface IProductInventory {
    id: number;
    product: IProduct;
    quantityOnHand: number;
    idealQuantity: number;
}