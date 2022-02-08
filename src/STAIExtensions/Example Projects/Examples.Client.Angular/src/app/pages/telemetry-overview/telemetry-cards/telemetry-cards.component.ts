import { Component, EventEmitter, Input, NgZone, OnInit, Output } from '@angular/core';
import { BehaviorSubject, interval } from 'rxjs';
import { TelemetryOverviewView } from 'src/app/data/telemetry-overview';
import { AvailabilityOverviewService } from 'src/app/services/service-availability-overview';
import { TelemetryOverviewService } from 'src/app/services/service-telemetry-overview';


@Component({
  selector: 'st-telemetry-cards',
  templateUrl: './telemetry-cards.component.html',
  styleUrls: ['./telemetry-cards.component.scss']
})
export class TelemetryCardsComponent implements OnInit {

  _selectedCard: string = "";
 
  get selectedCard(): string { return this._selectedCard; }
  @Input() set selectedCard(value: string) {
    if (this._selectedCard !== value) {
      this._selectedCard = value;
      this.selectedCardChanged.emit(value);
    }
  }
  @Output() selectedCardChanged: EventEmitter<string> = new EventEmitter();

  constructor(
    private zone: NgZone,
    public telemetryOverviewService: TelemetryOverviewService
  ) { 
    telemetryOverviewService.View$.subscribe(value => { 
      this.zone.run(() => {
        this.telemetryOverviewView$.next(value)
      })
    });
  }

  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null!); 

  source = interval(5000);
  counter$ = new BehaviorSubject<number>(9999); 
  
  ngOnInit(): void {
    this.source.subscribe(val => {

      this.counter$.next(this.counter$.value + 1);
    });
  }

}
