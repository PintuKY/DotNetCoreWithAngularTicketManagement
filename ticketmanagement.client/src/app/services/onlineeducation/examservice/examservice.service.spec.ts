import { TestBed } from '@angular/core/testing';

import { ExamserviceService } from './examservice.service';

describe('ExamserviceService', () => {
  let service: ExamserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExamserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
