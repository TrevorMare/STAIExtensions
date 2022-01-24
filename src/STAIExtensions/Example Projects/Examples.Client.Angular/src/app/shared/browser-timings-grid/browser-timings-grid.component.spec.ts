import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowserTimingsGridComponent } from './browser-timings-grid.component';

describe('BrowserTimingsGridComponent', () => {
  let component: BrowserTimingsGridComponent;
  let fixture: ComponentFixture<BrowserTimingsGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BrowserTimingsGridComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowserTimingsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
