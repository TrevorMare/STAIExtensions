import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AvailabilityOverviewComponent } from './availability-overview/availability-overview.component';
import { HomePageComponent } from './home-page/home-page.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { PagesComponent } from './pages.component';
import { TelemetryOverviewComponent } from './telemetry-overview/telemetry-overview.component';
import { TracesOverviewComponent } from './traces-overview/traces-overview.component';

const routes: Routes = [
  {
    path: '',
    component: PagesComponent,
    children: [
      {
        path: 'home-page',
        component: HomePageComponent,
      },
      {
        path: 'telemetry-overview',
        component: TelemetryOverviewComponent,
      },
      {
        path: 'traces-overview',
        component: TracesOverviewComponent,
      },
      {
        path: 'availability-overview',
        component: AvailabilityOverviewComponent,
      },
      {
        path: '**',
        component: NotFoundComponent,
      },
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}

