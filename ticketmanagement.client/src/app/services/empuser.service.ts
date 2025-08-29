import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface employeelist {
  empId: number;
  fullName: string;
  ticketId: number;
  department: string;
  designation: string;
  
}  

@Injectable({
  providedIn: 'root'
})
export class EmpuserService {


  private empbaseurl = '/api/empuser';
  constructor(private http: HttpClient) { }


  // Get all tickets
  FetchEmpUser(): Observable<employeelist[]> {
    return this.http.get<employeelist[]>(this.empbaseurl);
  }
  //getbyid
  GetEmpuserByID(id: number): Observable<employeelist> {
    return this.http.get<employeelist>(`${this.empbaseurl}/${id}`);
  }
}
