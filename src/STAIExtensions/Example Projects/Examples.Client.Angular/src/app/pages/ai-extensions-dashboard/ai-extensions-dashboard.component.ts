import { Component, OnInit } from '@angular/core';
import { TelemetryOverviewService } from '../../@core/utils/telemetry-overview.service';


@Component({
  selector: 'ngx-ai-extensions-dashboard',
  templateUrl: './ai-extensions-dashboard.component.html',
  styleUrls: ['./ai-extensions-dashboard.component.scss']
})
export class AiExtensionsDashboardComponent implements OnInit {


  constructor(public telemetryOverviewService: TelemetryOverviewService) { 
    
  }

  ngOnInit(): void {
    
  }

 

}
