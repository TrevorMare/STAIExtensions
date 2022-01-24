import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridCustomMetricsComponent } from './datacontract-grid-custom-metrics.component';

describe('CustomMetricsComponent', () => {
  let component: DataContractGridCustomMetricsComponent;
  let fixture: ComponentFixture<DataContractGridCustomMetricsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridCustomMetricsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridCustomMetricsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
