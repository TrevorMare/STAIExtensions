export interface CustomDimension extends Record<string, string> {}
export interface CustomMeasurement extends Record<string, number> {}
export enum TelemetryType {
    DataContract = 0, 
    Availability = 1,
    BrowserTiming = 2,
    CustomEvent = 3,
    CustomMetric = 4,
    Dependency = 5,
    Exceptions = 6,
    PageView = 7,
    PerformanceCounter = 8,
    Request = 9,
    Trace = 10,
}

export enum RecordState {
    New = 1,
    Existing = 2
}


export interface ExceptionParsedStack {
    id: string;
    message: string;
    outerId: string;
    parsedStack: ExceptionParsedStack[];
    type: string;
    assembly: string;
    method: string;
    fileName: string;
    level: number;
    line: number;
}

export interface DataContract {
    recordState: RecordState;
    timeStamp: string;
    customDimensions: CustomDimension;
    appId: string;
    applicationVersion: string;
    appName: string;
    clientBrowser: string;
    clientCity: string;
    clientCountryOrRegion: string;
    clientIP: string;
    clientModel: string;
    clientOS: string;
    clientStateOrProvince: string;
    clientType: string;
    cloudRoleInstance: string;
    cloudRoleName: string;
    iKey: string;
    itemId: string;
    itemType: string;
    operationId: string;
    operationName: string;
    operationParentId: string;
    operationSyntheticSource: string;
    sDKVersion: string;
    sessionId: string;
    userAccountId: string;
    userAuthenticatedId: string;
    userId: string;
}

export interface AIException extends DataContract {
    customMeasurements: CustomMeasurement;
    assembly: string;
    details: ExceptionParsedStack[];
    handledAt: string;
    innermostAssembly: string;
    innermostMessage: string;
    innermostMethod: string;
    innermostType: string;
    itemCount: number;
    message: string;
    method: string;
    outerAssembly: string;
    outerMessage: string;
    outerMethod: string;
    outerType: string;
    problemId: string;
    severityLevel: number;
    type: string;
}

export interface Availability extends DataContract {
    customMeasurements: CustomMeasurement;
    duration: number;
    id: string;
    itemCount: number;
    location: string;
    message: string;
    name: string;
    performanceBucket: string;
    size: number;
    success: string;
}

export interface BrowserTiming extends DataContract {
    customMeasurements: CustomMeasurement;
    itemCount: number;
    name: string;
    networkDuration: number;
    performanceBucket: string;
    processingDuration: number; 
    receiveDuration: number;
    sendDuration: number;
    totalDuration: number;
    url: string;
}

export interface CustomEvent extends DataContract {
    customMeasurements: CustomMeasurement;
    itemCount: number;
    name: string;
}

export interface CustomMetric extends DataContract {
    name: string;
    value: number;
    valueCount: number;
    valueMax: number;
    valueMin: number;
    valueStdDev: number;
    valueSum: number;
}

export interface Dependency extends DataContract {
    customMeasurements: CustomMeasurement;
    data: string;
    duration: number;
    id: string;
    itemCount: number;
    name: string;
    performanceBucket: string;
    resultCode: string;
    success: string;
    target: string;
    type: string;
}

export interface PageView extends DataContract {
    customMeasurements: CustomMeasurement;
    duration: number;
    id: string;
    itemCount: number;
    name: string;
    performanceBucket: string;
    url: string;
}

export interface PerformanceCounter extends DataContract {
    category: string;
    counter: string;
    instance: string;
    name: string;
    value: number;
}

export interface Request extends DataContract {
    customMeasurements: CustomMeasurement;
    duration: number;
    id: string;
    itemCount: number;
    name: string;
    performanceBucket: string;
    resultCode: string;
    source: string;
    success: string;
    url: string;
}

export interface Trace extends DataContract {
    customMeasurements: CustomMeasurement;
    itemCount: number;
    message: string;
    severityLevel: number;
}