import { AfterViewInit, Component, OnInit } from '@angular/core';
import { BehaviorSubject, interval, timer } from 'rxjs';
import { AvailabilityOverviewService } from 'src/app/services/service.availability-overview';

@Component({
  selector: 'app-telemetry-overview',
  templateUrl: './telemetry-overview.component.html',
  styleUrls: ['./telemetry-overview.component.scss']
})
export class TelemetryOverviewComponent implements OnInit {
  selectedCard: string = "Availability";

  constructor() { }

  ngOnInit(): void {
  }

}
