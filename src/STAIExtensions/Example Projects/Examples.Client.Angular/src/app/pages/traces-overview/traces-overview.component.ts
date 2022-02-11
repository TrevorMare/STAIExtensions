import { AfterViewInit, Component, ElementRef, Input, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TracesOverviewView } from 'src/app/data/view.traces-overview';
import { TracesOverviewService } from 'src/app/services/service.traces-overview';
import { ListItem } from '../availability-overview/details-panel/details-panel.component';


export class ConsoleItem {
  itemType: string;
  itemMessage: string;
}

@Component({
  selector: 'app-traces-overview',
  templateUrl: './traces-overview.component.html',
  styleUrls: ['./traces-overview.component.scss']
})
export class TracesOverviewComponent implements AfterViewInit {
  
  _maxArraySize: number = 400;
  tracesOverviewView$ = new BehaviorSubject<TracesOverviewView>(null!); 

  @ViewChild('console', {static: false}) console: ElementRef;
  @ViewChildren('consoleitem') itemElements: QueryList<any>;
  
  private consoleElement: any;
  private isNearBottom = true;

  @Input() items: ConsoleItem[] = [];

  constructor(
    public tracesOverviewService: TracesOverviewService
  ) {
    tracesOverviewService.View$.subscribe(value => { 
      this.tracesOverviewView$.next(value);
      if (!!value?.traceItems && value.traceItems.length) {
        this.updateViewTraceItems();
      }
    });
  }

  private updateViewTraceItems() {
    var newItems: ConsoleItem[] = [];

    this.tracesOverviewView$.value.traceItems.forEach((item) => {
      newItems.push({
        itemMessage: item.message, 
        itemType: this.getTraceType(item.severityLevel)
      });
    });

    this.items = this.items.concat(newItems);
    if (this.items.length > this._maxArraySize) {
      this.items = this.items.slice(0, this.items.length - this._maxArraySize);
    }
  }

  private getTraceType(severityType: number): string {
    if (severityType === 0) {
      return 'trace';
    } else if (severityType === 1) {
      return 'info';
    } else if (severityType === 2) {
      return 'warning';
    } else if (severityType === 3) {
      return 'error';
    } else if (severityType === 4) {
      return 'critical';
    }
    return 'trace';
  }

  ngAfterViewInit() {
    this.consoleElement = this.console.nativeElement;
    this.itemElements.changes.subscribe(_ => this.onItemElementsChanged());    
  }
  
  private onItemElementsChanged(): void {
    if (this.isNearBottom) {
      this.scrollToBottom();
    }
  }

  private scrollToBottom(): void {
    this.consoleElement.scroll({
      top: this.consoleElement.scrollHeight,
      left: 0,
      behavior: 'smooth'
    });
  }

  private isUserNearBottom(): boolean {
    const threshold = 150;
    const position = this.consoleElement.scrollTop + this.consoleElement.offsetHeight;
    const height = this.consoleElement.scrollHeight;
    return position > height - threshold;
  }

  scrolled(event: any): void {
    this.isNearBottom = this.isUserNearBottom();
  }

}
