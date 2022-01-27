import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridTracesComponent } from './datacontract-grid-traces.component';

describe('TracesComponent', () => {
  let component: DataContractGridTracesComponent;
  let fixture: ComponentFixture<DataContractGridTracesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridTracesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridTracesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
