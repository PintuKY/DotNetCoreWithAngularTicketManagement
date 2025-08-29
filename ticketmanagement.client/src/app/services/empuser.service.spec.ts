import { TestBed } from '@angular/core/testing';

import { EmpuserService } from './empuser.service';

describe('EmpuserService', () => {
  let service: EmpuserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmpuserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
