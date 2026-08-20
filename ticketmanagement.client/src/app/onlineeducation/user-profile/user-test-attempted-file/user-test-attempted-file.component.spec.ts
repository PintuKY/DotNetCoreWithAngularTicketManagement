import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
import { UserTestAttemptedFileComponent } from './user-test-attempted-file.component';

describe('UserTestAttemptedFileComponent', () => {
  let component: UserTestAttemptedFileComponent;
  let fixture: ComponentFixture<UserTestAttemptedFileComponent>;
  let syllabusDataService: jasmine.SpyObj<SyllabusDataService>;

  beforeEach(async () => {
    syllabusDataService = jasmine.createSpyObj('SyllabusDataService', ['GetUserAttemptedTestResult']);
    syllabusDataService.GetUserAttemptedTestResult.and.returnValue(of([
      {
        testId: 1,
        testGuid: '2008119f-5e67-4915-bb43-963266620f97',
        testName: 'BPSC TRE4.0 Computer Science',
        attemptCount: 5,
        lastAttempt: '2026-08-07T15:03:26.453',
        syllabusAttempts: [
          {
            syllabusId: 1,
            syllabusName: 'Fundamental Of Computer',
            attemptCount: 5,
            chapterAttempts: [
              { chapterId: 6, chapterName: 'Internet of Things', attemptCount: 1, lastAttempt: '2026-07-11T17:14:55.797' },
              { chapterId: 3, chapterName: 'Data Structure', attemptCount: 2, lastAttempt: '2026-07-11T20:06:45.56' },
              { chapterId: 2, chapterName: 'Digital Electronics', attemptCount: 1, lastAttempt: '2026-07-26T17:57:00.113' },
              { chapterId: 1, chapterName: 'Multimedia', attemptCount: 1, lastAttempt: '2026-08-07T15:03:26.453' }
            ]
          }
        ]
      }
    ]));

    await TestBed.configureTestingModule({
      declarations: [UserTestAttemptedFileComponent],
      providers: [{ provide: SyllabusDataService, useValue: syllabusDataService }]
    }).compileComponents();

    fixture = TestBed.createComponent(UserTestAttemptedFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should map the raw API response into displayable test, syllabus and chapter data', () => {
    component.LoadUserAttemptedTestResult();

    expect(component.attemptedTests.length).toBe(1);
    expect(component.attemptedTests[0].testName).toBe('BPSC TRE4.0 Computer Science');
    expect(component.attemptedTests[0].syllabusData.length).toBe(1);
    expect(component.attemptedTests[0].syllabusData[0].syllabusName).toBe('Fundamental Of Computer');
    expect(component.attemptedTests[0].syllabusData[0].chapterData.length).toBe(4);
    expect(component.attemptedTests[0].syllabusData[0].chapterData[0].chapterName).toBe('Internet of Things');
    expect(component.attemptedTests[0].syllabusData[0].chapterData[0].attemptCount).toBe(1);
  });
});
