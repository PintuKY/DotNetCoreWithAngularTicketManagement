import { Component, OnInit } from '@angular/core';
import { TicketService, Ticketlist } from '../../services/ticket.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})

export class TicketsComponent implements OnInit {
  public resultticket: Ticketlist[] = [];
  constructor(private ticketService: TicketService) { }

  ngOnInit(): void {
    this.getTickets();
  }
  getTickets() {
    this.ticketService.FetchTickets().subscribe(
      (result) => {
        this.resultticket = result;
      },
      (error) => {
        console.error(error);
      }
    );
   
  }
}
