import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridPageViewsComponent } from './datacontract-grid-page-views.component';

describe('PageViewsComponent', () => {
  let component: DataContractGridPageViewsComponent;
  let fixture: ComponentFixture<DataContractGridPageViewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridPageViewsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridPageViewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
