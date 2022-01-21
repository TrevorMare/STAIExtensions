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
import { TelemetryOverviewComponent } from './telemetry-overview/telemetry-overview.component';
import { CloudFilterSelectComponent } from './cloud-filter-select/cloud-filter-select.component';


@NgModule({
  declarations: [
    NumberCardComponent,
    TelemetryOverviewComponent,
    CloudFilterSelectComponent
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
  ],
  exports: [
    NumberCardComponent,
    TelemetryOverviewComponent,
    NbSelectComponent,
    NbOptionComponent
  ]
})
export class SharedModule { }
