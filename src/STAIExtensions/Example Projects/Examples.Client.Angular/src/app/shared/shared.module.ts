import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NumberCardComponent } from './number-card/number-card.component';
import { ThemeModule } from '../@theme/theme.module';
import {
  NbActionsModule,
  NbButtonModule,
  NbCardModule,
  NbTabsetModule,
  NbUserModule,
  NbRadioModule,
  NbSelectModule,
  NbListModule,
  NbIconModule,
} from '@nebular/theme';


@NgModule({
  declarations: [
    NumberCardComponent
  ],
  imports: [
    CommonModule,
    ThemeModule,
    NbCardModule,
    NbUserModule,
    NbButtonModule,
    NbTabsetModule,
    NbActionsModule,
    NbRadioModule,
    NbSelectModule,
    NbListModule,
    NbIconModule,
    NbButtonModule,
  ],
  exports: [
    NumberCardComponent
  ]
})
export class SharedModule { }
