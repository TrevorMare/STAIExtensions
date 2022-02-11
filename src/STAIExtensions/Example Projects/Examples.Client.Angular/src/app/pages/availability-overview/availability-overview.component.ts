import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartComponent, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexTitleSubtitle } from "ng-apexcharts";
import { BehaviorSubject } from 'rxjs';
import { AvailabilityAggregateGroup, AvailabilityOverviewView } from 'src/app/data/view.availability-overview';
import { AvailabilityOverviewService } from 'src/app/services/service.availability-overview';



@Component({
  selector: 'app-availability-overview',
  templateUrl: './availability-overview.component.html',
  styleUrls: ['./availability-overview.component.scss']
})
export class AvailabilityOverviewComponent implements OnInit {
  
  availabilityOverviewView$ = new BehaviorSubject<AvailabilityOverviewView>(null!); 
  selectedAvailabilityGroup: AvailabilityAggregateGroup | null;

  constructor(
    public availabilityService: AvailabilityOverviewService
  ) {
    availabilityService.View$.subscribe(value => { 
      this.availabilityOverviewView$.next(value);
    });
  }

  ngOnInit(): void {
   
  }

}
