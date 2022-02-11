import { AfterViewChecked, AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ApexAxisChartSeries, ApexChart, ApexFill, ApexPlotOptions, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { AvailabilityAggregateGroup } from 'src/app/data/view.availability-overview';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  labels: string[],
  stroke: any,
  fill: ApexFill,
  plotOptions: ApexPlotOptions
};

@Component({
  selector: 'st-availability-gauge',
  templateUrl: './availability-gauge.component.html',
  styleUrls: ['./availability-gauge.component.scss']
})
export class AvailabilityGaugeComponent implements OnInit, AfterViewInit, AfterViewChecked {
  _data: AvailabilityAggregateGroup | null = null;
  _initialized: boolean = false;

  @ViewChild("chart") chart: ChartComponent;
  public chartOptions!: Partial<ChartOptions> | any;
 
  get data(): AvailabilityAggregateGroup | null { return this._data; }
 
  @Input() set data(value: AvailabilityAggregateGroup | null) {
    this._data = value;
    this.setChartData();
  }
  
  constructor() { 

    this.chartOptions = {
      series: [],
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
                  background: 'transparent',
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
                      color: '#fff',
                      fontSize: '17px'
                  },
                  value: {
                      formatter: function (val) {
                          return parseInt(val) + '%';
                      },
                      color: '#fff',
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
              gradientToColors: ['#00EBFF'],
              inverseColors: true,
              opacityFrom: 1,
              opacityTo: 1,
              stops: [0, 100]
          }
      },
      stroke: {
          lineCap: 'round'
      },
      labels: ['Success Percent'],
    };
  }
  ngAfterViewChecked(): void {
   
  }
  
  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this._initialized = true;
    this.setChartData();
  }

  private setChartData() {
    if (this._initialized === false) return;
    if (this.chart === undefined || this.chart === null) return;
    if (this._data?.lastItem == null) {
      this.chart.updateOptions({ 
        series: []
      });
    } else {
      this.chart.updateOptions({ 
        series: [ this._data.lastItem.successPercentage.round(0) ],
      });
    }
  }

}
