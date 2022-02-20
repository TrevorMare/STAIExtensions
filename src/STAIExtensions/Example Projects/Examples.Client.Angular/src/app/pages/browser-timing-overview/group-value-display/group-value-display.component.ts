import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ApexAxisChartSeries, ApexChart, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { BehaviorSubject } from 'rxjs';
import { GroupedStatistics, GroupValues } from 'src/app/data/view.browsertimings-overview';
import '../../../shared/shared.extensions';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'browsertimingpage-group-value-display',
  templateUrl: './group-value-display.component.html',
  styleUrls: ['./group-value-display.component.scss']
})
export class GroupValueDisplayComponent implements OnInit, AfterViewInit {

  @ViewChild("chart") chart: ChartComponent;
  
  _groupValues: GroupValues | null;
  _selectedGroupValueIndex = new BehaviorSubject<number>(-1);
  _selectedItem: GroupedStatistics | null = null;
  _selectedPage: number = -1;
  _pagingSize: number = 10; 


  get groupValues(): GroupValues | null { return this._groupValues;}
  @Input() set groupValues(value : GroupValues | null) {
    this._groupValues = value;
    this.setupSelectedIndex();
  }
  @Input() groupValueName: string = "";

  public chartOptions!: Partial<ChartOptions> | any;

  constructor() { 

    this._selectedGroupValueIndex.subscribe((index) => {
      if (this.groupValues != null) {
        this._selectedItem = this.groupValues.statistics[index];
        this.buildChartData();
      }
    });

    this.chartOptions = {
      chart: {
        height: 350,
        type: "bar",
        fontFamily: 'Orbitron, sans-serif',
        foreColor: '#00ebff'
      },
      series: [
        {
          name: "Network Duration",
          data: []
        },
        {
          name: "Total Duration",
          data: []
        },
        {
          name: "Processing Duration",
          data: []
        },
        {
          name: "Receiving Duration",
          data: []
        },
        {
          name: "Sending Duration",
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
        categories: ["Minimum", "Maximum", "Average"]
      }
    };

  }
  ngAfterViewInit(): void {
    this.buildChartData();
  }

  ngOnInit(): void {
   
  }

  private setupSelectedIndex() {
    if (this._selectedGroupValueIndex.value === -1) {
      if (!!this.groupValues && this.groupValues.statistics.length > 0) {
        this._selectedGroupValueIndex.next(0);
        this._selectedPage = 1;
      }
    }
  }

  public setSelectedIndex(index: number, pageNumber: number) {
    if (pageNumber >= 1) {
      var pageOffsetIndex = ((pageNumber - 1) * this._pagingSize) + index;
      this._selectedGroupValueIndex.next(pageOffsetIndex);
    }
  }

  private buildChartData() {
    if (this.chart === undefined || this.chart === null) return;

    var seriesData: ApexAxisChartSeries = [
      {
        name: "Network Duration",
        data: [ this._selectedItem?.networkValues?.minimum?.round(0) ?? 0, this._selectedItem?.networkValues?.maximum.round(0) ?? 0, this._selectedItem?.networkValues?.average.round(0) ?? 0  ]
      },
      {
        name: "Total Duration",
        data: [ this._selectedItem?.totalValues?.minimum.round(0) ?? 0, this._selectedItem?.totalValues?.maximum.round(0) ?? 0, this._selectedItem?.totalValues?.average.round(0) ?? 0  ]
      },
      {
        name: "Processing Duration",
        data: [ this._selectedItem?.processingValues?.minimum.round(0) ?? 0, this._selectedItem?.processingValues?.maximum.round(0) ?? 0, this._selectedItem?.processingValues?.average.round(0) ?? 0  ]
      },
      {
        name: "Receiving Duration",
        data: [ this._selectedItem?.receiveValues?.minimum.round(0) ?? 0, this._selectedItem?.receiveValues?.maximum.round(0) ?? 0, this._selectedItem?.receiveValues?.average.round(0) ?? 0  ]
      },
      {
        name: "Sending Duration",
        data: [ this._selectedItem?.sendValues?.minimum.round(0) ?? 0, this._selectedItem?.sendValues?.maximum.round(0) ?? 0, this._selectedItem?.sendValues?.average.round(0) ?? 0  ]
      }
    ];

    this.chart.updateOptions({ series: seriesData });
  }

}
