import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Question }   from '../../model/onlineeducation/question';

@Injectable({
  providedIn: 'root'
})

export class QuestionService
{
  private apiUrl = '/api/listquestions/questions';
  constructor(private http: HttpClient)
  {

  }
  getQuestions(): Observable<Question[]>
  {
    return this.http.get<Question[]>(this.apiUrl);
  }
}
