import { NgModule } from '@angular/core';
import { NbCardModule, NbMenuModule, NbTabsetComponent, NbTabsetModule } from '@nebular/theme';

import { ThemeModule } from '../@theme/theme.module';
import { PagesComponent } from './pages.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { ECommerceModule } from './e-commerce/e-commerce.module';
import { PagesRoutingModule } from './pages-routing.module';
import { MiscellaneousModule } from './miscellaneous/miscellaneous.module';
import { SharedModule } from '../shared/shared.module';
import { AiExtensionsDashboardComponent } from './ai-extensions-dashboard/ai-extensions-dashboard.component';


@NgModule({
  imports: [
    PagesRoutingModule,
    ThemeModule,
    NbMenuModule,
    NbCardModule,
    DashboardModule,
    ECommerceModule,
    MiscellaneousModule,
    SharedModule,
    NbTabsetModule

  ],
  declarations: [
    PagesComponent,
    AiExtensionsDashboardComponent,
   
  ],
})
export class PagesModule {
}
