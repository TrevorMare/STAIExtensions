import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'st-popup-modal',
  templateUrl: './popup-modal.component.html',
  styleUrls: ['./popup-modal.component.scss']
})
export class PopupModalComponent implements OnInit {

  @Input() my_modal_title: string = "";
  @Input() my_modal_content: any = null;
  
  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
  }

}
