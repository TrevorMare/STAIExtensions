export interface View {
    id: string,
    viewTypeName: string,
    ownerId: string,
    expiryDate?: Date,
    lastUpdate?: Date,
    slidingExpiration: Number
}