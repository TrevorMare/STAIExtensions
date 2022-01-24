import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomMetricsGridComponent } from './custom-metrics-grid.component';

describe('CustomMetricsGridComponent', () => {
  let component: CustomMetricsGridComponent;
  let fixture: ComponentFixture<CustomMetricsGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomMetricsGridComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomMetricsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
