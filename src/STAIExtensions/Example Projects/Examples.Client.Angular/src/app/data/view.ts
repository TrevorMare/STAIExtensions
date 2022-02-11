export interface View {
    id: string;
    viewTypeName: string;
    ownerId: string | null;
    expiryDate: Date;
    lastUpdate: Date;
    slidingExpiration: number;
}

