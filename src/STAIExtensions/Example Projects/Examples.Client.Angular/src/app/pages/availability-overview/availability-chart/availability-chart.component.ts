import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ApexAxisChartSeries, ApexChart, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { AvailabilityAggregateGroup } from 'src/app/data/view.availability-overview';
import '../../../shared/shared.extensions';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'availabilitypage-availability-chart',
  templateUrl: './availability-chart.component.html',
  styleUrls: ['./availability-chart.component.scss']
})
export class AvailabilityChartComponent implements OnInit {

  _data: AvailabilityAggregateGroup | null = null;

  @ViewChild("chart") chart: ChartComponent;
  public chartOptions!: Partial<ChartOptions> | any;
  get data(): AvailabilityAggregateGroup | null { return this._data; }
  @Input() set data(value: AvailabilityAggregateGroup | null) {
    this._data = value;
    this.setChartData();
  }

  constructor() { 
    this.chartOptions = {
      chart: {
        height: 350,
        type: "line",
        fontFamily: 'Orbitron, sans-serif',
        foreColor: '#00ebff'
      },
      series: [
        {
          name: "Availability Percentage",
          data: []
        },
        {
          name: "Success Count",
          data: []
        },
        {
          name: "Failure Count",
          data: []
        }
      ],
      title: {
        text: ""
      },
      theme: {
        mode: 'dark',
        palette: 'palette2', 
      },
      tooltip: {
        style: {
          backgroundColor: '#00ebff'
        },
        x: {
          format: 'yyyy MM dd hh:mm:ss'
        }
      },
      xaxis: { 
        categories: [], 
        type: "datetime" 
      }
    };
  }

  private setChartData() {
    if (this.chart === undefined || this.chart === null) return;
    if (this._data?.items == null || this._data?.items.length == 0) {

      this.chart.updateOptions({
        series: [
          {
            name: "Availability Percentage",
            data: []
          },
          {
            name: "Success Count",
            data: []
          },
          {
            name: "Failure Count",
            data: []
          }
        ],
        xaxis: { 
          categories: [], 
          type: "datetime" 
        }
      });

    } else {
      var availabilityPercentageSeriesData: number[] = [];
      var successfulCountSeriesData: number[] = [];
      var failureCountSeriesData: number[] = [];
      var categories : number[] = [];
      for (let item of this._data.items) {
        categories.push(item.endDate.getTime());
        availabilityPercentageSeriesData.push(item.successPercentage.round(0));
        successfulCountSeriesData.push(item.successfulCount.round(0));
        failureCountSeriesData.push(item.failureCount.round(0));
      }

      this.chart.updateOptions({
        series: [
          {
            name: "Availability Percentage",
            data: availabilityPercentageSeriesData
          },
          {
            name: "Success Count",
            data: successfulCountSeriesData
          },
          {
            name: "Failure Count",
            data: failureCountSeriesData
          }
        ],
        xaxis: {  
          categories: categories,
          type: 'datetime',
          labels: {
            datetimeFormatter: {
              year: 'yyyy',
              month: 'MMM \'yy',
              day: 'dd MMM',
              hour: 'HH:mm'
            }
          }
        }
      });
    }
  }
  
  ngOnInit(): void {
  }

}
