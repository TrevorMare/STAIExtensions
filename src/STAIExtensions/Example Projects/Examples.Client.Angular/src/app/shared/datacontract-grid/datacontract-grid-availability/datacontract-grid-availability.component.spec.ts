import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilityComponent } from './datacontract-grid-availability.component';

describe('AvailabilityComponent', () => {
  let component: AvailabilityComponent;
  let fixture: ComponentFixture<AvailabilityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AvailabilityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AvailabilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});