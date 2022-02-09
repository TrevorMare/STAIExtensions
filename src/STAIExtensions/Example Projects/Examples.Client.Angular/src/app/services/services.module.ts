import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { STAIExtensionsService } from './staiextensions-data-service';
import { throwIfAlreadyLoaded } from './module-import-guard';
import { TelemetryOverviewService } from './service.telemetry-overview';
import { AvailabilityOverviewService } from './service.availability-overview';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    
  ]
})
export class ServicesModule { 
  constructor(@Optional() @SkipSelf() parentModule: ServicesModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
 
  static forRoot(): ModuleWithProviders<ServicesModule> {
    return {
      ngModule: ServicesModule,
      providers: [
        STAIExtensionsService,
        TelemetryOverviewService,
        AvailabilityOverviewService,
      ],
    };
  }
}
