import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridPerformanceCountersComponent } from './datacontract-grid-performance-counters.component';

describe('PerformanceCountersComponent', () => {
  let component: DataContractGridPerformanceCountersComponent;
  let fixture: ComponentFixture<DataContractGridPerformanceCountersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridPerformanceCountersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridPerformanceCountersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
