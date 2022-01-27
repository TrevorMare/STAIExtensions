interface IDataSetInformation {
    dataSetName: string, 
    dataSetId: string, 
    dataSetType: string,
    friendlyDataSetType: string
}

interface IViewInformation {
    viewName: string, 
    viewTypeName: string,
    friendlyViewTypeName: string,
    dataSetViewParameterDescriptors?: IDataSetViewParameterDescriptor[]
}

interface IMyView {
    viewId: string,
    viewTypeName: string,
    friendlyViewTypeName: string,
}

interface IDataSetViewParameterDescriptor {
    required: Boolean,
    name: string,
    type: string,
    description?: string
}

interface IView {
    id: string,
    viewTypeName: string,
    friendlyViewTypeName: string,
    ownerId: string,
    expiryDate?: Date,
    lastUpdate?: Date,
    slidingExpiration: Number
}