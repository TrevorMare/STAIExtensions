import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartComponent, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexTitleSubtitle } from "ng-apexcharts";
import { BehaviorSubject } from 'rxjs';
import { AvailabilityAggregateGroup, AvailabilityOverviewView } from 'src/app/data/view.availability-overview';
import { AvailabilityOverviewService } from 'src/app/services/service.availability-overview';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'app-availability-overview',
  templateUrl: './availability-overview.component.html',
  styleUrls: ['./availability-overview.component.scss']
})
export class AvailabilityOverviewComponent implements OnInit {
  
  availabilityOverviewView$ = new BehaviorSubject<AvailabilityOverviewView>(null!); 
  selectedAvailabilityGroup: AvailabilityAggregateGroup | null;
  
  public chartOptions!: Partial<ChartOptions> | any;
  public gaugeChartOptions!: Partial<ChartOptions> | any;

  public listItems: any[] = [
    { label: 'aaa', xx: 123 },
    { label: 'bbb', xx: 124 },
    { label: 'ccc', xx: 125 }
  ];


  public data = [{
    phone: '+1 (934) 551-2224',
    age: 20,
    address: { street: 'North street', number: 12 },
    company: 'ZILLANET',
    name: 'Valentine Webb',
    isActive: false,
  }, {
    phone: '+1 (948) 460-3627',
    age: 31,
    address: { street: 'South street', number: 12 },
    company: 'KNOWLYSIS',
    name: 'Heidi Duncan',
    isActive: true,
  }];

  constructor(
    public availabilityService: AvailabilityOverviewService
  ) {
    availabilityService.View$.subscribe(value => { 
      this.availabilityOverviewView$.next(value);
    });

 


    this.gaugeChartOptions ={
      series: [75],
      chart: {
      height: 350,
      type: 'radialBar',
      toolbar: {
        show: true
      }
    },
    plotOptions: {
      radialBar: {
        startAngle: -135,
        endAngle: 225,
         hollow: {
          margin: 0,
          size: '70%',
          background: '#fff',
          image: undefined,
          imageOffsetX: 0,
          imageOffsetY: 0,
          position: 'front',
          dropShadow: {
            enabled: true,
            top: 3,
            left: 0,
            blur: 4,
            opacity: 0.24
          }
        },
        track: {
          background: '#fff',
          strokeWidth: '67%',
          margin: 0, // margin is in pixels
          dropShadow: {
            enabled: true,
            top: -3,
            left: 0,
            blur: 4,
            opacity: 0.35
          }
        },
    
        dataLabels: {
          show: true,
          name: {
            offsetY: -10,
            show: true,
            color: '#888',
            fontSize: '17px'
          },
          value: {
            formatter: function(val: any){
              return parseInt(val);
            },
            color: '#111',
            fontSize: '36px',
            show: true,
          }
        }
      }
    },
    fill: {
      type: 'gradient',
      gradient: {
        shade: 'dark',
        type: 'horizontal',
        shadeIntensity: 0.5,
        gradientToColors: ['#ABE5A1'],
        inverseColors: true,
        opacityFrom: 1,
        opacityTo: 1,
        stops: [0, 100]
      }
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['Percent'],
    };

  }

  ngOnInit(): void {
   
  }

}
