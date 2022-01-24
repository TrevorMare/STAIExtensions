import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridExceptionsComponent } from './datacontract-grid-exceptions.component';

describe('ExceptionsComponent', () => {
  let component: DataContractGridExceptionsComponent;
  let fixture: ComponentFixture<DataContractGridExceptionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridExceptionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridExceptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
