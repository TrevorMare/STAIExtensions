import { AfterViewInit, Component, ElementRef, Input, QueryList, ViewChild, ViewChildren } from '@angular/core';


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

  @ViewChild('console', {static: false}) console: ElementRef;
  @ViewChildren('consoleitem') itemElements: QueryList<any>;
  
  private consoleElement: any;
  private isNearBottom = true;

  @Input() items: ConsoleItem[] = [];


  ngAfterViewInit() {
    this.consoleElement = this.console.nativeElement;
    this.itemElements.changes.subscribe(_ => this.onItemElementsChanged());    



    setInterval(() => {
      this.items.push({ itemMessage : "This is a trace message", itemType : "trace"  });
      this.items.push({ itemMessage : "This is an info message", itemType : "info"  });
      this.items.push({ itemMessage : "This is a warning message", itemType : "warning"  });
      this.items.push({ itemMessage : "This is an error message", itemType : "error"  });
    }, 2000);

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
