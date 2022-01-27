import { Component, OnInit } from '@angular/core';
import { TelemetryType } from '../../../@core/data/data-contracts';
import { TelemetryOverviewService } from '../../../@core/utils/telemetry-overview.service';

@Component({
  selector: 'ngx-stai-dashboard-telemetry-overview',
  templateUrl: './stai-dashboard-telemetry-overview.component.html',
  styleUrls: ['./stai-dashboard-telemetry-overview.component.scss']
})
export class StaiDashboardTelemetryOverviewComponent implements OnInit {


  selectedTelemetryType: TelemetryType = TelemetryType.Availability;

  constructor(public telemetryOverviewService: TelemetryOverviewService) { 
    
  }

  ngOnInit(): void {
  }

  onSelectedTelemetryTypeChanged($event: any) {
    this.selectedTelemetryType = $event;
  }
 


}
