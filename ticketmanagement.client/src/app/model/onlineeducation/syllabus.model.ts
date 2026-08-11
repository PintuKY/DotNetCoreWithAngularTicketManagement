export interface Syllabus {
  syllabusID: number;
  syllabusGuid: string;
  syllabusName: string;
  totalChapters: number,
  totalQuestions: number
}

export interface TestItem {
  id: number;
  testGuid: string;
  testName: string;
  description: string;
  price: number;
  isActive: boolean;
  createdOn: string;
  totalSyllabus: number;
  isPaid: boolean;
}

export interface Chapters
{
    chapterId: number;
    chapterGuid:string;
    syllabusId: number;
    chapterName: string;
    module: string;
    topic: string;
    isActive: boolean;
    createdOn: Date;
    syllabus: null;
    question:Question[];
}
export interface Question {
  id: number;
  chapterID:number;
  questionGuid:string;
  questionText: string;
  options: Option[];
}
export interface Option {
  optionId: number;
  optionText: string;
}

export interface UserCourseItem {
  id?: number;
  userCoursGuid?: string;
  userId?: number;
  testId?: number;
  testData?: UserCourseTestData[];
  [key: string]: any;
}

export interface UserCourseTestData {
  id?: number;
  testName?: string;
  title?: string;
  testGuid?: string;
  syllabusData?: UserCourseSyllabusData[];
  [key: string]: any;
}

export interface UserCourseSyllabusData {
  id?: number;
  syllabusName?: string;
  title?: string;
  chapterData?: UserCourseChapterData[];
  [key: string]: any;
}

export interface UserCourseChapterData {
  id?: number;
  chapterName?: string;
  title?: string;
  [key: string]: any;
}