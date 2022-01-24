import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeModule } from '../../@theme/theme.module';
import { Ng2SmartTableModule }  from 'ng2-smart-table'; 
import { NgxJsonViewerModule } from 'ngx-json-viewer';

import { DataContractGridAvailabilityComponent } from './datacontract-grid-availability/datacontract-grid-availability.component';
import { DataContractGridBrowserTimingsComponent } from './datacontract-grid-browser-timings/datacontract-grid-browser-timings.component';
import { DataContractGridCustomEventsComponent } from './datacontract-grid-custom-events/datacontract-grid-custom-events.component';
import { DataContractGridCustomMetricsComponent } from './datacontract-grid-custom-metrics/datacontract-grid-custom-metrics.component';
import { DataContractGridDependenciesComponent } from './datacontract-grid-dependencies/datacontract-grid-dependencies.component';
import { DataContractGridExceptionsComponent } from './datacontract-grid-exceptions/datacontract-grid-exceptions.component';
import { DataContractGridPageViewsComponent } from './datacontract-grid-page-views/datacontract-grid-page-views.component';
import { DataContractGridPerformanceCountersComponent } from './datacontract-grid-performance-counters/datacontract-grid-performance-counters.component';
import { DataContractGridRequestsComponent } from './datacontract-grid-requests/datacontract-grid-requests.component';
import { DataContractGridTracesComponent } from './datacontract-grid-traces/datacontract-grid-traces.component';
import { NbDialogService } from '@nebular/theme';


@NgModule({
  declarations: [
    DataContractGridAvailabilityComponent,
    DataContractGridBrowserTimingsComponent,
    DataContractGridCustomEventsComponent,
    DataContractGridCustomMetricsComponent,
    DataContractGridDependenciesComponent,
    DataContractGridExceptionsComponent,
    DataContractGridPageViewsComponent,
    DataContractGridPerformanceCountersComponent,
    DataContractGridRequestsComponent,
    DataContractGridTracesComponent,
  ],
  imports: [
    CommonModule,
    ThemeModule,
    Ng2SmartTableModule,
    NgxJsonViewerModule,
  ],
  exports: [
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
export class DatacontractGridModule { }
