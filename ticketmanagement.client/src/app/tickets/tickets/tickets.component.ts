import { Component, OnInit } from '@angular/core';
import { TicketService, Ticketlist } from '../../services/ticket.service';


@Component({
  selector: 'app-tickets', //This is the custom HTML tag name you use in templates
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})

export class TicketsComponent implements OnInit {
  public resultticket: Ticketlist[] = [];
  //enumMap
  statusMap: { [key: number]: string } = {
    0: 'Open',
    1: 'InProgress',
    2: 'Hold',
    3: 'Closed',
    4: 'Rejected'
  };

  priorityMap: { [key: number]: string } = {
    0: 'Low',
    1: 'High',
    2: 'Medium',
    3: 'Critical'
  };

  categoryMap: { [key: number]: string } = {
    0: 'Bug',
    1: 'Feature',
    2: 'Support'
  };
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
  deleteTicket(id: number) {
    if (confirm("Are you sure you want to delete this ticket?")) {
      this.ticketService.deleteTicket(id).subscribe(() => {
        this.resultticket = this.resultticket.filter(t => t.ticketId !== id);
        alert("Ticket deleted successfully!");
      }, error => {
        console.error(error);
        alert("Error deleting ticket");
      });
    }
  }
}
