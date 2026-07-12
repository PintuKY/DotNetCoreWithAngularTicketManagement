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
   private userprofileUrl = '/api/UserProfile/updateuserprofile';
   private pws = '/api/UserProfile/changepassword';
   private getprofile = '/api/UserProfile/getmeprofile';
  constructor(private http:HttpClient)
  {

  }
UserProfileData(): Observable<any> {
    const url = this.getprofile;
    console.log('Fetching user profile data from API:', url);
    return this.http.get<any>(url);
  }

  UpdatePassword(currentPassword: string, newPassword: string,confirmPassword:string): Observable<any> {
    const url = this.pws;
    const payload = {
      currentPassword: currentPassword,
      newPassword: newPassword,
      confirmPassword: confirmPassword
    };
    console.log('Submitting password change request to API:', url);
    return this.http.post<any>(url, payload);
  }
  SubmitUserProfileData(userProfileData: any): Observable<any> {
    const url = this.userprofileUrl;
    console.log('Submitting user profile data to API:', url, userProfileData);
    return this.http.post<any>(url, userProfileData);
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
