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
    id: string | null;
    message: string | null;
    outerId: string | null;
    parsedStack: ExceptionParsedStack[];
    type: string | null;
    assembly: string | null;
    method: string | null;
    fileName: string | null;
    level: number | null;
    line: number | null;
}

export interface DataContract {
    recordState: RecordState;
    timeStamp: string;
    customDimensions: CustomDimension | null;
    appId: string | null;
    applicationVersion: string | null;
    appName: string | null;
    clientBrowser: string | null;
    clientCity: string | null;
    clientCountryOrRegion: string | null;
    clientIP: string | null;
    clientModel: string | null;
    clientOS: string | null;
    clientStateOrProvince: string | null;
    clientType: string | null;
    cloudRoleInstance: string | null;
    cloudRoleName: string | null;
    iKey: string | null;
    itemId: string | null;
    itemType: string | null;
    operationId: string | null;
    operationName: string | null;
    operationParentId: string | null;
    operationSyntheticSource: string | null;
    sDKVersion: string | null;
    sessionId: string | null;
    userAccountId: string | null;
    userAuthenticatedId: string | null;
    userId: string | null;
}

export interface AIException extends DataContract {
    customMeasurements: CustomMeasurement | null;
    assembly: string | null;
    details: ExceptionParsedStack[] | null;
    handledAt: string | null;
    innermostAssembly: string | null;
    innermostMessage: string | null;
    innermostMethod: string | null;
    innermostType: string | null;
    itemCount: number | null;
    message: string | null;
    method: string | null;
    outerAssembly: string | null;
    outerMessage: string | null;
    outerMethod: string | null;
    outerType: string | null;
    problemId: string | null;
    severityLevel: number | null;
    type: string | null;
}

export interface Availability extends DataContract {
    customMeasurements: CustomMeasurement | null;
    duration: number | null;
    id: string | null;
    itemCount: number | null;
    location: string | null;
    message: string | null;
    name: string | null;
    performanceBucket: string | null;
    size: number | null;
    success: string | null;
}

export interface BrowserTiming extends DataContract {
    customMeasurements: CustomMeasurement | null;
    itemCount: number | null;
    name: string | null;
    networkDuration: number | null;
    performanceBucket: string | null;
    processingDuration: number | null; 
    receiveDuration: number | null;
    sendDuration: number | null;
    totalDuration: number | null;
    url: string | null;
}

export interface CustomEvent extends DataContract {
    customMeasurements: CustomMeasurement | null;
    itemCount: number | null;
    name: string | null;
}

export interface CustomMetric extends DataContract {
    name: string | null;
    value: number | null;
    valueCount: number | null;
    valueMax: number | null;
    valueMin: number | null;
    valueStdDev: number | null;
    valueSum: number | null;
}

export interface Dependency extends DataContract {
    customMeasurements: CustomMeasurement | null;
    data: string | null;
    duration: number | null;
    id: string | null;
    itemCount: number | null;
    name: string | null;
    performanceBucket: string | null;
    resultCode: string | null;
    success: string | null;
    target: string | null;
    type: string | null;
}

export interface PageView extends DataContract {
    customMeasurements: CustomMeasurement | null;
    duration: number | null;
    id: string | null;
    itemCount: number | null;
    name: string | null;
    performanceBucket: string | null;
    url: string | null;
}

export interface PerformanceCounter extends DataContract {
    category: string | null;
    counter: string | null;
    instance: string | null;
    name: string | null;
    value: number | null;
}

export interface Request extends DataContract {
    customMeasurements: CustomMeasurement | null;
    duration: number | null;
    id: string | null;
    itemCount: number | null;
    name: string | null;
    performanceBucket: string | null;
    resultCode: string | null;
    source: string | null;
    success: string | null;
    url: string | null;
}

export interface Trace extends DataContract {
    customMeasurements: CustomMeasurement | null;
    itemCount: number | null;
    message: string | null;
    severityLevel: number | null;
}