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
 
  _performingReset: boolean = false;

  @ViewChild("chart") chart: ChartComponent;
  public chartOptions!: Partial<ChartOptions> | any;

  groupName$ = new BehaviorSubject<string>("");
  groupValues$ = new BehaviorSubject<GroupValues | null>(null);
  
  selectedPage$ = new BehaviorSubject<number>(-1);
  selectedGroupValueIndex$ = new BehaviorSubject<number>(-1);
  selectedItem$ = new BehaviorSubject<GroupedStatistics | null>(null);
  
  pagingSize$ = new BehaviorSubject<number>(10);
  initialized$ = new BehaviorSubject<boolean>(false);

  get groupValues(): GroupValues | null { return this.groupValues$.value; }
  @Input() set groupValues(value : GroupValues | null) { this.groupValues$.next(value); }
  
  get groupValueName(): string { return this.groupName$.value; }
  @Input() set groupValueName(value: string) { this.groupName$.next(value); }


  constructor() { 

    this.groupName$.subscribe((groupName) => {
      if (groupName !== '') {
        this.initialiseDisplayVariables();
      }
    });

    this.selectedGroupValueIndex$.subscribe(index => {
      this.bindWindowData();
    });

    this.selectedItem$.subscribe(item => {
      this.bindChartData();
    });
  }

  private initialiseDisplayVariables() {
    this._performingReset = true;
    // Setup the default chart options
    this.setupDefaultChartOptions();
    // Clear the indexes
    this.selectedItem$.next(null);
    this.selectedPage$.next(1);
    this._performingReset = false;
    // Set the selected item index
    this.selectedGroupValueIndex$.next(0);
  }


  ngAfterViewInit(): void {
    this.initialized$.next(true);
  }

  ngOnInit(): void {
   
  }

  private bindWindowData() {
    if (this._performingReset === true) return;

    var bindItemGroup = this.groupValues$.value;
    if (bindItemGroup === null || bindItemGroup.statistics.length === 0) return;

    var pageNumber = this.selectedPage$.value;
    var pageOffsetIndex = ((pageNumber - 1) * this.pagingSize$.value) + this.selectedGroupValueIndex$.value;

    this.selectedItem$.next(bindItemGroup.statistics[pageOffsetIndex]);
  }
  
  private bindChartData() {
    setTimeout(() => {
      this.bindChartDataDelayed();
    }, 500);
  }

  private bindChartDataDelayed() {
    if (this.chart === undefined || this.chart === null || typeof this.chart.updateSeries === "undefined") return;

    var selectedItem = this.selectedItem$.value;

    if (selectedItem === null) {
      this.chart.updateSeries([]);
    } else {
      
      
      var seriesData: ApexAxisChartSeries = [
        {
          name: "Network Duration",
          data: [ selectedItem?.networkValues?.minimum?.round(0) ?? 0, selectedItem?.networkValues?.maximum.round(0) ?? 0, selectedItem?.networkValues?.average.round(0) ?? 0  ]
        },
        {
          name: "Total Duration",
          data: [ selectedItem?.totalValues?.minimum.round(0) ?? 0, selectedItem?.totalValues?.maximum.round(0) ?? 0, selectedItem?.totalValues?.average.round(0) ?? 0  ]
        },
        {
          name: "Processing Duration",
          data: [ selectedItem?.processingValues?.minimum.round(0) ?? 0, selectedItem?.processingValues?.maximum.round(0) ?? 0, selectedItem?.processingValues?.average.round(0) ?? 0  ]
        },
        {
          name: "Receiving Duration",
          data: [ selectedItem?.receiveValues?.minimum.round(0) ?? 0, selectedItem?.receiveValues?.maximum.round(0) ?? 0, selectedItem?.receiveValues?.average.round(0) ?? 0  ]
        },
        {
          name: "Sending Duration",
          data: [ selectedItem?.sendValues?.minimum.round(0) ?? 0, selectedItem?.sendValues?.maximum.round(0) ?? 0, selectedItem?.sendValues?.average.round(0) ?? 0  ]
        }
      ];
      this.chart.updateOptions({ series: seriesData });
    }
  }

  public setSelectedItemIndex(index: number) {
    this.selectedGroupValueIndex$.next(index);
  }

  public onListPageIndexChanged(pageIndex: number) {
    this.selectedPage$.next(pageIndex);
  }

  private setupDefaultChartOptions() {
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
          data: [0]
        },
        {
          name: "Total Duration",
          data: [0]
        },
        {
          name: "Processing Duration",
          data: [0]
        },
        {
          name: "Receiving Duration",
          data: [0]
        },
        {
          name: "Sending Duration",
          data: [0]
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

    if (this.chart !== undefined && this.chart !== null) {
      this.chart.updateOptions(this.chartOptions)
    }

  }

}
