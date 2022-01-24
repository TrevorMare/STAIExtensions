import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataContractGridDependenciesComponent } from './datacontract-grid-dependencies.component';

describe('DependenciesComponent', () => {
  let component: DataContractGridDependenciesComponent;
  let fixture: ComponentFixture<DataContractGridDependenciesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataContractGridDependenciesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataContractGridDependenciesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
