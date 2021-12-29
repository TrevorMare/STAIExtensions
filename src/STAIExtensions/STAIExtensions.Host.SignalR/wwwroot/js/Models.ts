interface IDataSetInformation {
    dataSetName: string, 
    dataSetId: string, 
    dataSetType: string
}

interface IViewInformation {
    viewName: string, 
    viewTypeName: string
}

interface IView {
     id: string,
     ownerId : string,
     expiryDate : Date,
     lastUpdate: Date,
     slidingExpiration: Number
}