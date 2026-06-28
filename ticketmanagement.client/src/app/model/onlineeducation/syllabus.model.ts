export interface Syllabus {
  syllabusID: number;
  syllabusGuid: string;
  syllabusName: string;
  totalChapters: number,
  totalQuestions: number
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