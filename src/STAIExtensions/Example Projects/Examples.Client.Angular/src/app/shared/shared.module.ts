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
import { TelemetryOverviewComponent } from './telemetry-overview/telemetry-overview.component';
import { CloudFilterSelectComponent } from './cloud-filter-select/cloud-filter-select.component';
import { AvailabilityGridComponent } from './availability-grid/availability-grid.component';
import { ViewSourceModalComponent } from './view-source-modal/view-source-modal.component';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { JsonColumnComponent } from './json-column/json-column.component';
import { ToggleCardComponent } from './toggle-card/toggle-card.component';
import { BrowserTimingsGridComponent } from './browser-timings-grid/browser-timings-grid.component';
import { CustomEventsGridComponent } from './custom-events-grid/custom-events-grid.component';
import { CustomMetricsGridComponent } from './custom-metrics-grid/custom-metrics-grid.component';


@NgModule({
  declarations: [
    NumberCardComponent,
    TelemetryOverviewComponent,
    CloudFilterSelectComponent,
    AvailabilityGridComponent,
    ViewSourceModalComponent,
    JsonColumnComponent,
    ToggleCardComponent,
    BrowserTimingsGridComponent,
    CustomEventsGridComponent,
    CustomMetricsGridComponent
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
    NgxJsonViewerModule ,
  ],
  exports: [
    NumberCardComponent,
    TelemetryOverviewComponent,
    AvailabilityGridComponent,
    BrowserTimingsGridComponent,
    CustomEventsGridComponent,
    CustomMetricsGridComponent,
    ViewSourceModalComponent,
    NbSelectComponent,
    NbOptionComponent,
    Ng2SmartTableComponent
  ]
})
export class SharedModule { }
