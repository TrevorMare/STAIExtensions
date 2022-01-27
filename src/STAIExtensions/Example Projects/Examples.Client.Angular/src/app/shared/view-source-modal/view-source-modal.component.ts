import { Component, Input, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'ngx-view-source-modal',
  templateUrl: './view-source-modal.component.html',
  styleUrls: ['./view-source-modal.component.scss']
})
export class ViewSourceModalComponent implements OnInit {

  @Input() source: any;

  constructor(protected ref: NbDialogRef<ViewSourceModalComponent>) {}

  dismiss() {
    this.ref.close();
  }

  ngOnInit(): void {
  }

}
