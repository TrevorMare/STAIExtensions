import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomEventsGridComponent } from './custom-events-grid.component';

describe('CustomEventsGridComponent', () => {
  let component: CustomEventsGridComponent;
  let fixture: ComponentFixture<CustomEventsGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomEventsGridComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomEventsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
