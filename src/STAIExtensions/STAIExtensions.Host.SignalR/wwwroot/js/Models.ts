interface IDataSetInformation {
    dataSetName: string, 
    dataSetId: string, 
    dataSetType: string
}

interface IViewInformation {
    viewName: string, 
    viewTypeName: string,
    dataSetViewParameterDescriptors?: IDataSetViewParameterDescriptor[]
}

interface IMyView {
    viewId: string,
    viewTypeName: string
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
    ownerId: string,
    expiryDate?: Date,
    lastUpdate?: Date,
    slidingExpiration: Number
}