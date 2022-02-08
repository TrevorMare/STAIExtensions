import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
// @ts-ignore
import jsonview from '@pgrabovets/json-view';

@Component({
  selector: 'st-json-object-viewer',
  templateUrl: './json-object-viewer.component.html',
  styleUrls: ['./json-object-viewer.component.scss']
})
export class JsonObjectViewerComponent implements OnInit, AfterViewInit {
  private _data: any = null;
  private _initialised: boolean = false;
  private _currentTree: any = null;


  @ViewChild('jsonViewerRoot') jsonViewerRoot: ElementRef; 

  get data(): any {
    return this._data;
  }
  @Input() set data(value: any) {
    this._data = value;
    this.renderComponent();
  } 

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this._initialised = true;
    this.renderComponent();
  }

  private renderComponent() {
    if (this._initialised === false) return;
    if (this._data === null) return;

    // Clear the items
    if (this._currentTree !== null) jsonview.destroy(this._currentTree);

    this._currentTree = jsonview.create(this._data);
    jsonview.render(this._currentTree, this.jsonViewerRoot.nativeElement);
    jsonview.expand(this._currentTree);
  }

}
