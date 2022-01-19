import { TestBed } from '@angular/core/testing';

import { STAIExtensionsService } from './staiextensions.service';

describe('STAIExtensionsService', () => {
  let service: STAIExtensionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(STAIExtensionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
