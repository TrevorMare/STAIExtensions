import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilityNestedTableComponent } from './availability-nested-table.component';

describe('AvailabilityNestedTableComponent', () => {
  let component: AvailabilityNestedTableComponent;
  let fixture: ComponentFixture<AvailabilityNestedTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AvailabilityNestedTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AvailabilityNestedTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
