import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JsonColumnComponent } from './json-column.component';

describe('JsonColumnComponent', () => {
  let component: JsonColumnComponent;
  let fixture: ComponentFixture<JsonColumnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JsonColumnComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JsonColumnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
