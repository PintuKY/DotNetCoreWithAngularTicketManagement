import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Ticketlist {
  ticketId: number;
  title: string;
  description: string;
  status: number;
  prioritys: number;
  categorys: number;
  createdBy: string;
  assignedTo?: string;
  createdDate: Date;
  updatedDate?: Date;
  dueDate?: Date;
  attachmentPath?: string;
}

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private baseUrl = '/weatherforecast'; // backend API URL
  private baseeurl = '/api/ticket';
  constructor(private http: HttpClient) { }
  
  FetchTicketsForecast(): Observable<Ticketlist[]>
  {
    return this.http.get<Ticketlist[]>(this.baseUrl);
  }
  // Get all tickets
  FetchTickets(): Observable<Ticketlist[]> {
    return this.http.get<Ticketlist[]>(this.baseeurl);
  }
 //getbyid
  GetByID(id: number): Observable<Ticketlist> {
    return this.http.get<Ticketlist>(`${this.baseeurl}/${id}`);
  }
  //submit form
  createTicket(ticketData: any): Observable<any> {
    return this.http.post(this.baseeurl, ticketData);
  }
  //update
  updateTicket(id: number, ticket: Ticketlist): Observable<Ticketlist> {
    return this.http.post<Ticketlist>(`${this.baseeurl}/${id}`, ticket);
  }
  //delete ticket
  deleteTicket(id: number): Observable<any> {
    return this.http.delete(`${this.baseeurl}/${id}`);
  }
}
