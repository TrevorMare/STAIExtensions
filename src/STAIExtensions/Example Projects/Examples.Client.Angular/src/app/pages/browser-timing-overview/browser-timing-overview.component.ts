import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BrowserTimingsOverviewView } from 'src/app/data/view.browsertimings-overview';
import { BrowserTimingsOverviewService } from 'src/app/services/service.browsertimings-overview';

@Component({
  selector: 'app-browser-timing-overview',
  templateUrl: './browser-timing-overview.component.html',
  styleUrls: ['./browser-timing-overview.component.scss']
})
export class BrowserTimingOverviewComponent implements OnInit {

  browserTimingsOverviewView$ = new BehaviorSubject<BrowserTimingsOverviewView>(null!); 
  
  constructor(
    public browserTimingService: BrowserTimingsOverviewService
  ) { 
    browserTimingService.View$.subscribe(value => { 
      this.browserTimingsOverviewView$.next(value);
    });
  }

  ngOnInit(): void {
  } 

}
