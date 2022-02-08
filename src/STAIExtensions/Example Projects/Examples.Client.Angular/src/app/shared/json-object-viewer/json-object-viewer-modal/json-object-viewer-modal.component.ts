import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-json-object-viewer-modal',
  templateUrl: './json-object-viewer-modal.component.html',
  styleUrls: ['./json-object-viewer-modal.component.scss']
})
export class JsonObjectViewerModalComponent implements OnInit {

  @Input() data: any = null;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
  }

}
