import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TelemetryOverviewComponent } from './telemetry-overview.component';

describe('TelemetryOverviewComponent', () => {
  let component: TelemetryOverviewComponent;
  let fixture: ComponentFixture<TelemetryOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TelemetryOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TelemetryOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
