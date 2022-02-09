import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesComponent } from './pages.component';
import { PagesRoutingModule } from './pages-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { SharedModule } from '../shared/shared.module';
import { NavigationMenuComponent } from '../shared/navigation-menu/navigation-menu.component';
import { TelemetryOverviewComponent } from './telemetry-overview/telemetry-overview.component';
import { TableModule } from 'ngx-easy-table';
import { TelemetryCardsComponent } from './telemetry-overview/telemetry-cards/telemetry-cards.component';
import { AvailabilityTableComponent } from './telemetry-overview/availability-table/availability-table.component';
import { BrowserTimingsTableComponent } from './telemetry-overview/browser-timings-table/browser-timings-table.component';
import { CustomEventsTableComponent } from './telemetry-overview/custom-events-table/custom-events-table.component';
import { CustomMetricsTableComponent } from './telemetry-overview/custom-metrics-table/custom-metrics-table.component';
import { DependenciesTableComponent } from './telemetry-overview/dependencies-table/dependencies-table.component';
import { ExceptionsTableComponent } from './telemetry-overview/exceptions-table/exceptions-table.component';
import { PageViewsTableComponent } from './telemetry-overview/page-views-table/page-views-table.component';
import { PerformanceCountersTableComponent } from './telemetry-overview/performance-counters-table/performance-counters-table.component';
import { RequestsTableComponent } from './telemetry-overview/requests-table/requests-table.component';
import { TracesTableComponent } from './telemetry-overview/traces-table/traces-table.component';
import { TracesOverviewComponent } from './traces-overview/traces-overview.component';
import { AvailabilityOverviewComponent } from './availability-overview/availability-overview.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { AvailabilityListComponent } from './availability-overview/availability-list/availability-list.component';
import { AvailabilityChartComponent } from './availability-overview/availability-chart/availability-chart.component';
import { DetailsPanelComponent } from './availability-overview/details-panel/details-panel.component';

@NgModule({
  declarations: [
    PagesComponent,
    HomePageComponent,
    NotFoundComponent,
    TelemetryOverviewComponent,
    TelemetryCardsComponent,
    AvailabilityTableComponent,
    BrowserTimingsTableComponent,
    CustomEventsTableComponent,
    CustomMetricsTableComponent,
    DependenciesTableComponent,
    ExceptionsTableComponent,
    PageViewsTableComponent,
    PerformanceCountersTableComponent,
    RequestsTableComponent,
    TracesTableComponent,
    TracesOverviewComponent,
    AvailabilityOverviewComponent,
    AvailabilityListComponent,
    AvailabilityChartComponent,
    DetailsPanelComponent,
   
  ],
  imports: [
    PagesRoutingModule,
    CommonModule,
    SharedModule,
    TableModule,
    NgApexchartsModule,
  ]
})
export class PagesModule { }
