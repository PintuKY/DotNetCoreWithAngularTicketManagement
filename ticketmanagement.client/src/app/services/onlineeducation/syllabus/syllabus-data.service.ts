import { Injectable } from '@angular/core';
import { Syllabus } from 'src/app/model/onlineeducation/syllabus.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SyllabusDataService 
{
  private apiUrl="/api/syllabus/syllabusname"
  constructor(private http:HttpClient)
  {

  }
  getSyllabusData(): Observable<Syllabus[]>
  {
    return this.http.get<Syllabus[]>(this.apiUrl);
  }
}
