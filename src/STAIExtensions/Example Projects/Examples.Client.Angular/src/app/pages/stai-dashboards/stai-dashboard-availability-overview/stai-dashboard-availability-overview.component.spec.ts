import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaiDashboardAvailabilityOverviewComponent } from './stai-dashboard-availability-overview.component';

describe('StaiDashboardAvailabilityOverviewComponent', () => {
  let component: StaiDashboardAvailabilityOverviewComponent;
  let fixture: ComponentFixture<StaiDashboardAvailabilityOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaiDashboardAvailabilityOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaiDashboardAvailabilityOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
