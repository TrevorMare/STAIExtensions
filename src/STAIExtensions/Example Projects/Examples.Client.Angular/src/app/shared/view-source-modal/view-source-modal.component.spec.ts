import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewSourceModalComponent } from './view-source-modal.component';

describe('ViewSourceModalComponent', () => {
  let component: ViewSourceModalComponent;
  let fixture: ComponentFixture<ViewSourceModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewSourceModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewSourceModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
