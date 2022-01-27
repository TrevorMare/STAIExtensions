import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaiDashboardTelemetryOverviewComponent } from './stai-dashboard-telemetry-overview.component';

describe('StaiDashboardTelemetryOverviewComponent', () => {
  let component: StaiDashboardTelemetryOverviewComponent;
  let fixture: ComponentFixture<StaiDashboardTelemetryOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaiDashboardTelemetryOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaiDashboardTelemetryOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
