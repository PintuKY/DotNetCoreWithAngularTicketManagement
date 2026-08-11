import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';

@Component({
  selector: 'app-user-course-file',
  templateUrl: './user-course-file.component.html',
  styleUrls: ['./user-course-file.component.css']
})
export class UserCourseFileComponent implements OnInit {

  userCourses: any[] = [];
  expandedCourseIndex: number | null = null;

  constructor(
    private syllabusDataService: SyllabusDataService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.loadUserCourseData();
  }

  loadUserCourseData(): void {
    this.syllabusDataService.UserCourseData().subscribe({
      next: (res: any) => {

        const data = Array.isArray(res)
          ? res
          : res?.data && Array.isArray(res.data)
            ? res.data
            : res
              ? [res]
              : [];

        this.userCourses = data.map((item: any) => this.mapUserCourse(item));

        console.log('Mapped User Courses', this.userCourses);
        console.log(this.userCourses);
        console.log(this.userCourses[0].testData[0].syllabusData);
        console.log(this.userCourses[0].testData[0].syllabusData[0].chapterData);
      },
      error: (err) => {
        console.error(err);
        this.userCourses = [];
      }
    });
  }

  // ===========================
  // Mapping
  // ===========================

  private mapUserCourse(item: any): any {

    const syllabus = this.toArray(
      item?.syllabusData ||
      item?.syllabus ||
      item?.syllabusList ||
      item?.syllabuses
    ).map((s: any) => this.mapSyllabusData(s));

    return {

      ...item,

      syllabusData: syllabus,

      testData: this.toArray(item?.testData).map((test: any) => ({

        ...test,

        syllabusData: syllabus

      }))
    };
  }

  private mapSyllabusData(syllabus: any): any {

    return {

      ...syllabus,

      chapterData: this.toArray(

        syllabus?.chapterData ||
        syllabus?.chapter ||
        syllabus?.chapters

      ).map((chapter: any) => this.mapChapterData(chapter))

    };

  }

  private mapChapterData(chapter: any): any {

    return {

      ...chapter

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

  // ===========================
  // Accordion
  // ===========================

  toggleCourse(index: number): void {

    this.expandedCourseIndex =
      this.expandedCourseIndex === index
        ? null
        : index;

  }

  trackByCourse(index: number, course: any): any {

    return course?.id ?? index;

  }

  // ===========================
  // Display Helpers
  // ===========================

  getCourseTitle(course: any): string {

    return course?.testData?.[0]?.testName || 'Untitled Test';

  }

  getTestTitle(test: any): string {

    return test?.testName || 'Untitled Test';

  }

  getSyllabusTitle(syllabus: any): string {

    return syllabus?.syllabusName || 'Untitled Syllabus';

  }

  getSyllabusId(syllabus: any): any {

    return syllabus?.syllabusID;

  }

  getSyllabusGuid(syllabus: any): any {

    return syllabus?.syllabusGuid;

  }

  getChapterTitle(chapter: any): string {

    return chapter?.chapterName || 'Untitled Chapter';

  }

  getChapterId(chapter: any): any {

    return chapter?.chapterId;

  }

  getChapterGuid(chapter: any): any {

    return chapter?.chapterGuid;

  }

}