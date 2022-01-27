import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StaiDashboardTelemetryOverviewComponent } from './stai-dashboard-telemetry-overview/stai-dashboard-telemetry-overview.component';
import { NbCardModule, NbMenuModule, NbTabsetComponent, NbTabsetModule } from '@nebular/theme';
import { ThemeModule } from '../../@theme/theme.module';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    StaiDashboardTelemetryOverviewComponent,
    
  ],
  imports: [
    CommonModule,
    NbCardModule,
    SharedModule,
    NbCardModule,

  ]
})
export class StaiDashboardsModule { }
