export interface ICustomer {
    id?: number;
    createdOn?: Date;
    updatedOn?: Date;
    firstName?: string;
    lastName?: string;
    primaryAddress?: ICustomerAddress;
}

export interface ICustomerAddress {
    id?:number;
    createdOn?: Date;
    updatedOn?: Date;
    addressLine1?: string;
    addressLine2?: string;
    city?: string;
    province?: string;
    postalCode?: string;
    country?: string;
}