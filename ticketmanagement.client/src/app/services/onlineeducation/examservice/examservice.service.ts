import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExamserviceService
{
  private apiUrl="api/ExamSubmit/examsubmits";
  constructor(private http:HttpClient) { }
  
  submitTest(data:any)
  {
    return this.http.post(this.apiUrl, data, { responseType: 'text' });
  }
}

