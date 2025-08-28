import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router  } from '@angular/router';
import { TicketService, Ticketlist } from '../../services/ticket.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {
  ticketId!: number;
  ticket!: Ticketlist;
  editForm!: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private ticketService: TicketService,
    private fb: FormBuilder,
    private router: Router
  ) { }

  ngOnInit(): void {

    // initialize form with validation rules
    this.editForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.maxLength(250)]],
      status: ['', Validators.required],
      priority: ['', Validators.required],
      category: ['', Validators.required],
      createdBy: ['', Validators.required],
      assignedTo: [''],
      createdDate: ['', Validators.required],
      dueDate: [''],
      attachmentPath: [null]
    });

    
    
    this.ticketId = Number(this.route.snapshot.paramMap.get('id'));//get ID from url
    console.log("Editing ticket with id:", this.ticketId);
    //get data from backend ByID
    this.ticketService.GetByID(this.ticketId).subscribe(
      (data) => {
        this.ticket = data;
        console.log(this.ticket);
        // patch values into show data on form
        this.editForm.patchValue({
          title: this.ticket.title,
          description: this.ticket.description,
          status: this.ticket.status,
          priority: this.ticket.prioritys,
          category: this.ticket.categorys,
          createdBy: this.ticket.createdBy,
          assignedTo: this.ticket.assignedTo,
          createdDate: this.formatDate(this.ticket.createdDate),
          dueDate: this.formatDate(this.ticket.dueDate),
          attachmentPath: this.ticket.attachmentPath
        });
      },
      (error) => {
        console.error("Ticket featching error", error);
      }
    );
  }

  // return yyyy-MM-dd
  formatDate(dateString: any): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }
  //for file image
  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.editForm.patchValue({ attachmentPath: file });
    }
  }
  //for form submit
  onSubmit() {
    if (this.editForm.valid)
    {
      const updatedTicket: Ticketlist = {
        ticketId: this.ticketId, 
        ...this.editForm.value //...this.editForm.value = a shortcut to copy all form field values into the object.
      };
      this.ticketService.updateTicket(this.ticketId, updatedTicket).subscribe(() => {
        alert('Ticket updated successfully');
        this.router.navigate(['/tickets']);
      });
    } else {
      this.editForm.markAllAsTouched();
    }
  }
  get f() {
    return this.editForm.controls;
  }
  
}
