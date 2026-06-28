import { Injectable } from '@angular/core';
import { Syllabus } from 'src/app/model/onlineeducation/syllabus.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SyllabusDataService 
{
//  private syllabusUrl = '/api/Tests/syllabus';
   private questionsUrl = '/api/listquestions/questions';
  private testGuidUrl = '/api/Tests';

  constructor(private http:HttpClient)
  {

  }

  getQuestionDataByChapterGuid(chapterGuid: string | null): Observable<any>
  {
    let url = this.questionsUrl;
    if (chapterGuid) {
      url = `${this.questionsUrl}/${encodeURIComponent(chapterGuid)}`;
    }
    this.lastRequestUrl = url;
    console.log('Calling question API:', url);
    return this.http.get<any>(url);
  }

  // Use this when you only want the syllabus for a specific test GUID.
  private lastRequestUrl: string | null = null;

getSyllabusForTest(testGuid: string): Observable<Syllabus[]>
{
  const url = `${this.testGuidUrl}/${encodeURIComponent(testGuid)}/syllabus`;

  this.lastRequestUrl = url;

  console.log('Calling syllabus API:', url);

  return this.http.get<Syllabus[]>(url);
}
  getChapterForSyllabusTest(testGuid: string): Observable<Syllabus>
  {
    const url = `${this.testGuidUrl}/${encodeURIComponent(testGuid)}/chapters`;
    this.lastRequestUrl = url;
    console.log('Calling Chappter API:', url);
    return this.http.get<Syllabus>(url);
  }

  getLastRequestUrl(): string | null {
    return this.lastRequestUrl;
  }
}
