import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridRequestsComponent } from './datacontract-grid-requests.component';

describe('RequestsComponent', () => {
  let component: DataContractGridRequestsComponent;
  let fixture: ComponentFixture<DataContractGridRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridRequestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
