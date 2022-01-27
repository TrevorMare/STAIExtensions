import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CloudFilterSelectComponent } from './cloud-filter-select.component';

describe('CloudFilterSelectComponent', () => {
  let component: CloudFilterSelectComponent;
  let fixture: ComponentFixture<CloudFilterSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CloudFilterSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CloudFilterSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
