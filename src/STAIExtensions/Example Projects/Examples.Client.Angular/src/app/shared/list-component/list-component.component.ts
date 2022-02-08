import { Component, ContentChild, Input, OnInit, TemplateRef } from '@angular/core';

@Component({
  selector: 'st-list-component',
  templateUrl: './list-component.component.html',
  styleUrls: ['./list-component.component.scss']
})
export class ListComponentComponent implements OnInit {


  @Input() items: any[];
  @ContentChild('itemTemplate', {read: TemplateRef, static: false }) itemTemplate: TemplateRef<any>;
  @ContentChild('headerTemplate', {read: TemplateRef, static: false }) headerTemplate: TemplateRef<any>;

  constructor() { }

  ngOnInit(): void {
  }

}
