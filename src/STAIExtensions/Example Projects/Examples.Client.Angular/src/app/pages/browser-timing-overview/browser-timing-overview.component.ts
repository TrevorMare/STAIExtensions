import { AfterViewInit, Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BrowserTimingsOverviewView, GroupValues } from 'src/app/data/view.browsertimings-overview';
import { BrowserTimingsOverviewService } from 'src/app/services/service.browsertimings-overview';

@Component({
  selector: 'app-browser-timing-overview',
  templateUrl: './browser-timing-overview.component.html',
  styleUrls: ['./browser-timing-overview.component.scss']
})
export class BrowserTimingOverviewComponent implements OnInit, AfterViewInit {
  public selectedViewLabel: string = "None";

  browserTimingsOverviewView$ = new BehaviorSubject<BrowserTimingsOverviewView>(null!); 
  selectedViewName$ = new BehaviorSubject<string>("");
  selectedGroup$ = new BehaviorSubject<GroupValues | null>(null);
  
  constructor(
    public browserTimingService: BrowserTimingsOverviewService
  ) { 
   
    browserTimingService.View$.subscribe(value => { 
      this.browserTimingsOverviewView$.next(value);
      this.setSelectedViewFromName();
    });

    this.selectedViewName$.subscribe((_) => {
      this.setSelectedViewFromName();
    });
  }

  ngOnInit(): void {
  } 

  ngAfterViewInit(): void {
    this.selectedViewName$.next("overall");
  }

  public setViewName(viewName: string) {
    this.selectedViewName$.next(viewName);
  }

  private setSelectedViewFromName(): void {
    if (this.selectedViewName$.value === '' || this.browserTimingsOverviewView$.value === null) return;
    var viewName = this.selectedViewName$.value;
    var groupValues: GroupValues | null = null;
    this.selectedViewLabel = "";

    if (viewName === 'overall') {
      groupValues = this.browserTimingsOverviewView$.value.operationNameStatistics;
      this.selectedViewLabel = "Overall";
    } else if (viewName === 'clientBrowser') {
      groupValues = this.browserTimingsOverviewView$.value.clientBrowserStatistics;
      this.selectedViewLabel = "Client Browser";
    } else if (viewName === 'clientCity') {
      groupValues = this.browserTimingsOverviewView$.value.clientCityStatistics;
      this.selectedViewLabel = "Client City";
    } else if (viewName === 'countryOrRegion') {
      groupValues = this.browserTimingsOverviewView$.value.countryOrRegionStatistics;
      this.selectedViewLabel = "Client Country";
    } else if (viewName === 'userSession') {
      groupValues = this.browserTimingsOverviewView$.value.userSessionStatistics;
      this.selectedViewLabel = "User Session";
    } else if (viewName === 'userId') {
      groupValues = this.browserTimingsOverviewView$.value.userIdStatistics;
      this.selectedViewLabel = "User Id";
    }
    this.selectedGroup$.next(groupValues);
  }

}
