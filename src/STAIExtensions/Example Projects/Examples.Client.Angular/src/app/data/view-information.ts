export interface IDataSetViewParameterDescriptor {
    required: boolean;
    name: string;
    type: string;
    description: string;
}

export interface ViewInformation {
    viewName: string; 
    viewTypeName: string;
    dataSetViewParameterDescriptors: IDataSetViewParameterDescriptor[];
}