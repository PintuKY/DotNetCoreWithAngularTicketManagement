import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Ticketlist {
  ticketId: number;
  title: string;
  description: string;
  status: string;
  prioritys?: string;
  categorys?: string;
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
  // Get all tickets
  FetchTicketsForecast(): Observable<Ticketlist[]>
  {
    return this.http.get<Ticketlist[]>(this.baseUrl);
  }
  FetchTickets(): Observable<Ticketlist[]> {
    return this.http.get<Ticketlist[]>(this.baseeurl);
  }
}
