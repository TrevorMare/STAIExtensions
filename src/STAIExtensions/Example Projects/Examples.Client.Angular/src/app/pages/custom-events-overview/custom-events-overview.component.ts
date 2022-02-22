import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApexAxisChartSeries, ApexChart, ApexGrid, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { BehaviorSubject } from 'rxjs';
import { CustomEventsOverviewView } from 'src/app/data/view.customevents-overview';
import { CustomEventsOverviewService } from 'src/app/services/service.customevents-overview';
import { CustomEvent } from 'src/app/data/data-contracts';
import { JsonObjectViewerModalComponent } from 'src/app/shared/json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';


export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
  grid: ApexGrid;
};

@Component({
  selector: 'app-custom-events-overview',
  templateUrl: './custom-events-overview.component.html',
  styleUrls: ['./custom-events-overview.component.scss']
})
export class CustomEventsOverviewComponent implements OnInit {
  @ViewChild("chart") chart: ChartComponent;
  customEventsOverviewView$ = new BehaviorSubject<CustomEventsOverviewView>(null!); 
  jsonViewerData: any = null;

  public configuration: Config;
  public columns: Columns[];
  public data: CustomEvent[] = [];
  
  public chartOptions!: Partial<ChartOptions> | any;

  constructor(
    public customEventsOverviewService: CustomEventsOverviewService,
    private modalService: NgbModal,
  ) { 
    
    this.initialiseChartOptions();

    customEventsOverviewService.View$.subscribe(value => { 
      this.customEventsOverviewView$.next(value);
      if (!!value?.lastCustomEventItems) {
        this.data = value.lastCustomEventItems;
      }
      this.buildChartData();
    });
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

  private buildChartData(): void {
    setTimeout(() => {
      this.buildChartDataDelayed();
    }, 500);
  }

  private buildChartDataDelayed() {
    if (this.chart === undefined || this.chart === null) return;
    var chartData = this.customEventsOverviewView$.value?.aggregateGroups;

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

  private initialiseChartOptions(): void {
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

}
