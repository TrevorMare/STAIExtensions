import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AiExtensionsDashboardComponent } from './ai-extensions-dashboard.component';

describe('AiExtensionsDashboardComponent', () => {
  let component: AiExtensionsDashboardComponent;
  let fixture: ComponentFixture<AiExtensionsDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AiExtensionsDashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AiExtensionsDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
