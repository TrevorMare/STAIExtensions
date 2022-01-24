import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NumberCardComponent } from './number-card/number-card.component';
import { ThemeModule } from '../@theme/theme.module';

import {
  NbActionsModule,
  NbButtonModule,
  NbCardModule,
  NbTabsetModule,
  NbUserModule,
  NbRadioModule,
  NbSelectModule,
  NbListModule,
  NbIconModule,
  NbSelectComponent,
  NbOptionComponent,
} from '@nebular/theme';
import { Ng2SmartTableComponent, Ng2SmartTableModule }  from 'ng2-smart-table'; 
import { NgxJsonViewerModule } from 'ngx-json-viewer';

import { CloudFilterSelectComponent } from './cloud-filter-select/cloud-filter-select.component';
import { TelemetryOverviewComponent } from './telemetry-overview/telemetry-overview.component';
import { ViewSourceModalComponent } from './view-source-modal/view-source-modal.component';
import { JsonColumnComponent } from './json-column/json-column.component';
import { ToggleCardComponent } from './toggle-card/toggle-card.component';
import { DatacontractGridModule } from './datacontract-grid/datacontract-grid.module';
import { DataContractGridAvailabilityComponent } from './datacontract-grid/datacontract-grid-availability/datacontract-grid-availability.component';
import { DataContractGridBrowserTimingsComponent } from './datacontract-grid/datacontract-grid-browser-timings/datacontract-grid-browser-timings.component';
import { DataContractGridCustomEventsComponent } from './datacontract-grid/datacontract-grid-custom-events/datacontract-grid-custom-events.component';
import { DataContractGridCustomMetricsComponent } from './datacontract-grid/datacontract-grid-custom-metrics/datacontract-grid-custom-metrics.component';
import { DataContractGridDependenciesComponent } from './datacontract-grid/datacontract-grid-dependencies/datacontract-grid-dependencies.component';
import { DataContractGridExceptionsComponent } from './datacontract-grid/datacontract-grid-exceptions/datacontract-grid-exceptions.component';
import { DataContractGridPageViewsComponent } from './datacontract-grid/datacontract-grid-page-views/datacontract-grid-page-views.component';
import { DataContractGridPerformanceCountersComponent } from './datacontract-grid/datacontract-grid-performance-counters/datacontract-grid-performance-counters.component';
import { DataContractGridRequestsComponent } from './datacontract-grid/datacontract-grid-requests/datacontract-grid-requests.component';
import { DataContractGridTracesComponent } from './datacontract-grid/datacontract-grid-traces/datacontract-grid-traces.component';



@NgModule({
  declarations: [
    NumberCardComponent,
    TelemetryOverviewComponent,
    ViewSourceModalComponent,
    JsonColumnComponent,
    ToggleCardComponent,
    CloudFilterSelectComponent,
  ],
  imports: [
    CommonModule,
    ThemeModule,
    NbCardModule,
    NbUserModule,
    NbButtonModule,
    NbTabsetModule,
    NbActionsModule,
    NbRadioModule,
    NbSelectModule,
    NbListModule,
    NbIconModule,
    NbButtonModule,
    Ng2SmartTableModule,
    NgxJsonViewerModule,
    DatacontractGridModule,
  ],
  exports: [
    NbSelectComponent,
    NbOptionComponent,
    Ng2SmartTableComponent,

    NumberCardComponent,
    TelemetryOverviewComponent,
    ViewSourceModalComponent,

    CloudFilterSelectComponent,
    DataContractGridAvailabilityComponent,
    DataContractGridBrowserTimingsComponent,
    DataContractGridCustomEventsComponent,
    DataContractGridCustomMetricsComponent,
    DataContractGridDependenciesComponent,
    DataContractGridExceptionsComponent,
    DataContractGridPageViewsComponent,
    DataContractGridPerformanceCountersComponent,
    DataContractGridRequestsComponent,
    DataContractGridTracesComponent
  
  ]
})
export class SharedModule { }
