import { Component, OnInit } from '@angular/core';
import { SyllabusDataService } from '../../../services/onlineeducation/syllabus/syllabus-data.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-user-test-attempted-file',
  templateUrl: './user-test-attempted-file.component.html',
  styleUrls: ['./user-test-attempted-file.component.css']
})
export class UserTestAttemptedFileComponent implements OnInit {
  attemptedTests: any[] = [];
  expandedCourseIndex: number | null = null;
  // testGuid: string | null = null;
  // syllabusGuid: string | null = null;
  // chapterGuid: string | null = null;
  constructor(private syllabusDataService: SyllabusDataService,private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.LoadUserAttemptedTestResult();
    // this.route.queryParamMap.subscribe(params => {
    //   this.testGuid = params.get('testGuid');
    //   this.syllabusGuid = params.get('syllabusGuid');
    //   this.chapterGuid = params.get('chapterGuid');

    //   console.log('testGuid:', this.testGuid);
    //   console.log('syllabusGuid:', this.syllabusGuid);
    //   console.log('chapterGuid:', this.chapterGuid);
    // });
  }

  LoadUserAttemptedTestResult(): void {
    this.syllabusDataService.GetUserAttemptedTestResult().subscribe({
      next: (response) => {
        const rawData = Array.isArray(response) ? response : response ? [response] : [];
        this.attemptedTests = rawData.map((test: any) => this.mapAttemptedTest(test));
        console.log('Mapped attempted tests:', this.attemptedTests);
      },
      error: (error) => {
        console.error('Error fetching user attempted test result:', error);
        this.attemptedTests = [];
      }
    });
  }

  private mapAttemptedTest(test: any): any {
    const syllabusData = this.toArray(test?.syllabusAttempts).map((syllabus: any) => ({
      ...syllabus,
      chapterData: this.toArray(syllabus?.chapterAttempts).map((chapter: any) => ({
        ...chapter,
        chapterName: chapter?.chapterName || 'Untitled Chapter',
        attemptCount: Number(chapter?.attemptCount ?? 0),
        lastAttempt: chapter?.lastAttempt || null
      }))
    }));

    return {
      ...test,
      attemptCount: Number(test?.attemptCount ?? 0),
      testName: test?.testName || 'Untitled Test',
      lastAttempt: test?.lastAttempt || null,
      syllabusData
    };
  }

  private toArray(value: any): any[] {
    if (Array.isArray(value)) {
      return value;
    }

    if (value) {
      return [value];
    }

    return [];
  }

  toggleCourse(index: number): void {
    this.expandedCourseIndex = this.expandedCourseIndex === index ? null : index;
  }

  trackByCourse(index: number, course: any): any {
    return course?.testId ?? course?.testGuid ?? index;
  }

  getCourseTitle(test: any): string {
    return test?.testName || 'Untitled Test';
  }

  getSyllabusTitle(syllabus: any): string {
    return syllabus?.syllabusName || 'Untitled Syllabus';
  }
}
