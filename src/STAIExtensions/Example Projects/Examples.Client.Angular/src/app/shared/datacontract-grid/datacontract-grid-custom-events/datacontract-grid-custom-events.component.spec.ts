import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridCustomEventsComponent } from './datacontract-grid-custom-events.component';

describe('CustomEventsComponent', () => {
  let component: DataContractGridCustomEventsComponent;
  let fixture: ComponentFixture<DataContractGridCustomEventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridCustomEventsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridCustomEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
