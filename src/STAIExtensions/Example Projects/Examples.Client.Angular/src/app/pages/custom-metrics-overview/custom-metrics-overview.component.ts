import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApexAxisChartSeries, ApexChart, ApexGrid, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { BehaviorSubject } from 'rxjs';
import { CustomMetric } from 'src/app/data/data-contracts';
import { CustomMetricsOverviewView } from 'src/app/data/view.custommetrics-overview';
import { CustomMetricsOverviewService } from 'src/app/services/service.custommetrics-overview';
import { JsonObjectViewerModalComponent } from 'src/app/shared/json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';


export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
  grid: ApexGrid;
};

@Component({
  selector: 'app-custom-metrics-overview',
  templateUrl: './custom-metrics-overview.component.html',
  styleUrls: ['./custom-metrics-overview.component.scss']
})
export class CustomMetricsOverviewComponent implements OnInit {
  @ViewChild("chart") chart: ChartComponent;
  customMetricsOverviewView$ = new BehaviorSubject<CustomMetricsOverviewView>(null!); 
  jsonViewerData: any = null;

  public configuration: Config;
  public columns: Columns[];
  public data: CustomMetric[] = [];
  
  public chartOptions!: Partial<ChartOptions> | any;

  constructor(
    public customMetricsOverviewService: CustomMetricsOverviewService,
    private modalService: NgbModal,
  ) { 
    customMetricsOverviewService.View$.subscribe(value => { 
      this.customMetricsOverviewView$.next(value);
      if (!!value?.lastCustomMetricItems) {
        this.data = value.lastCustomMetricItems;
      }
      this.buildChartData();
    });

    this.chartOptions = {
      chart: {
        height: 350,
        type: "scatter",
        fontFamily: 'Orbitron, sans-serif',
        foreColor: '#00ebff',
        zoom: {
          type: 'xy'
        }
      },
      series: [
        
      ],
      dataLabels: {
        enabled: false
      },
      title: {
        text: ""
      },
      theme: {
        mode: 'dark',
        palette: 'palette2', 
      },
      grid: {
          xaxis: {
            lines: {
              show: true
            }
          },
          yaxis: {
            lines: {
              show: true
            }
          },
        },
        xaxis: {
          type: 'datetime',
        },
        yaxis: {
          max: 70
        }
    };

  }

  ngOnInit(): void {
    this.configuration = { ...DefaultConfig };
    this.configuration.searchEnabled = true;
    
    this.columns = [
      { key: 'itemId', title: 'Actions', searchEnabled: false, orderEnabled: false },
      { key: 'name', title: 'Name' },
      { key: 'timeStamp', title: 'Timestamp' },
      { key: 'itemCount', title: 'Item Count' },
      { key: 'operationName', title: 'Operation Name' },
      { key: 'cloudRoleInstance', title: 'Role Instance' },
      { key: 'cloudRoleName', title: 'Role Name' },
    ];

  }

  private buildChartData() {
    if (this.chart === undefined || this.chart === null) return;
    var chartData = this.customMetricsOverviewView$.value?.aggregateGroups;

    var seriesData: ApexAxisChartSeries = [];

    if (chartData === null || chartData.length === 0) {
      this.chart.updateOptions({ series: seriesData });
      return;
    } 

    for (let chartDataItem of chartData) {
      var data: any[] = [];
      for (let item of chartDataItem.items) {
        data.push([item.endDate, item.numberOfCalls]);
      }

      var chartSeriesData = {
        name: chartDataItem.groupName,
        data: data
      }
      seriesData.push(chartSeriesData);
    }
    
    this.chart.updateOptions({ series: seriesData });
  }

  public onViewJsonItemClick($event: any, index: number): void {
    var item = this.data[index];
    this.jsonViewerData = item;
    const modalRef = this.modalService.open(JsonObjectViewerModalComponent, { size: 'xl', centered: true });
    modalRef.componentInstance.data = item;
  }

}
