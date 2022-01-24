import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridBrowserTimingsComponent } from './datacontract-grid-browser-timings.component';

describe('BrowserTimingsComponent', () => {
  let component: DataContractGridBrowserTimingsComponent;
  let fixture: ComponentFixture<DataContractGridBrowserTimingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridBrowserTimingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridBrowserTimingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
