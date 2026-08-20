import { Injectable } from '@angular/core';
import { Syllabus, SyllabusResponse } from 'src/app/model/onlineeducation/syllabus.model';
import { HttpClient ,HttpParams} from '@angular/common/http';
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
   private getusercoursedata = '/api/UserPurchageTestCourse/usercoursedata';
   private getuserattemptedtestResult = '/api/UserTestAttempted/usertest-attempted';
   private getuserTestScoreDetails = '/api/UsersTestScoreDetails/usrs-testscore-details';
   
  constructor(private http:HttpClient)
  {

  }
// GetUserTestScoreDetails(): Observable<any> {
//     const url = this.getuserTestScoreDetails;
//     console.log('Fetching user test score details from API:', url);
//     return this.http.get<any>(url);
//   }
   GetUserTestScoreDetailsByGuids(testGuid: string, syllabusGuid: string, chapterGuid: string): Observable<any> {
    const payload = { testGuid, syllabusGuid, chapterGuid };
    const url = this.getuserTestScoreDetails;
    console.log('Fetching user test score details from API:', url, payload);
    return this.http.post<any>(url, payload);
  }
  
  GetUserAttemptedTestResult(): Observable<any> {
    const url = this.getuserattemptedtestResult;
    console.log('Fetching user attempted test result from API:', url);
    return this.http.get<any>(url);
  }

  SubmitTestPayment(payload: any): Observable<any> {
    const url = '/api/UserPurchageTestCourse/TestPayment';
    console.log('Submitting test payment to API:', url, payload);
    return this.http.post<any>(url, payload);
  }

  private buildSyllabusArray(syllabus: any): any[] {
    if (!syllabus) {
      return [];
    }

    return [
      {
        syllGuid: syllabus.syllabusGuid,
        syllabusID: syllabus.syllabusID?.toString() ?? '',
        syllabusName: syllabus.syllabusName,
        totalChapters: syllabus.totalChapters ?? 0,
        totalQuestions: syllabus.totalQuestions ?? 0
      }
    ];
  }

  private parseInteger(value: any): number {
    if (value === null || value === undefined || value === '') {
      return 0;
    }

    const digits = value.toString().replace(/\D/g, '');
    return digits ? Number(digits) : 0;
  }

  private parseExpiry(expiry: any): string | null {
    if (!expiry) {
      return null;
    }

    const expiryStr = expiry.toString().trim();
    const parts = expiryStr.split('/');
    if (parts.length !== 2) {
      return null;
    }

    const month = parts[0].padStart(2, '0');
    let year = parts[1].trim();

    if (year.length === 2) {
      year = `20${year}`;
    }

    if (!/^\d{2}$/.test(month) || !/^\d{4}$/.test(year)) {
      return null;
    }

    return `${year}-${month}-01T00:00:00`;
  }

UserCourseData(): Observable<any> {
    const url = this.getusercoursedata;
    console.log('Fetching user course data from API:', url);
    return this.http.get<any>(url);
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

getSyllabusForTest(testGuid: string): Observable<SyllabusResponse | Syllabus[]>
{
  const url = `${this.testGuidUrl}/${encodeURIComponent(testGuid)}/syllabus`;

  this.lastRequestUrl = url;

  console.log('Calling syllabus API:', url);

  return this.http.get<SyllabusResponse | Syllabus[]>(url);
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
