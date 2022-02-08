import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NbCardModule, NbMenuModule, NbTabsetComponent, NbTabsetModule, NbTreeGridModule } from '@nebular/theme';
import { ThemeModule } from '../../@theme/theme.module';
import { SharedModule } from '../../shared/shared.module';
import { StaiDashboardAvailabilityOverviewComponent } from './stai-dashboard-availability-overview/stai-dashboard-availability-overview.component';
import { StaiDashboardTelemetryOverviewComponent } from './stai-dashboard-telemetry-overview/stai-dashboard-telemetry-overview.component';
import { AvailabilityNestedTableComponent } from './stai-dashboard-availability-overview/availability-nested-table/availability-nested-table.component';
import { RowDetailComponent } from './stai-dashboard-availability-overview/availability-nested-table/row-detail/row-detail.component';

@NgModule({
  declarations: [
    StaiDashboardTelemetryOverviewComponent,
    StaiDashboardAvailabilityOverviewComponent,
    AvailabilityNestedTableComponent,
    RowDetailComponent,
    
  ],
  imports: [
    CommonModule,
    NbCardModule,
    SharedModule,
    NbCardModule,
    NbTreeGridModule,

  ]
})
export class StaiDashboardsModule { }
