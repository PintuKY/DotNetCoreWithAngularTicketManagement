import { Component, OnInit } from '@angular/core';
import { TicketService } from '../services/ticket.service';
import { NgForm, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.css']
})
export class CreateTicketComponent implements OnInit {
  ticketForm!: FormGroup;
  constructor(private ticketService: TicketService) { }

  ngOnInit(): void {

  }
  successMessage: string = '';
  errorMessage: string = '';
  onSubmit(form: NgForm) {

    //const formData = new FormData();
    //formData.append('Title', form.value.Title);
    //formData.append('Description', form.value.Description);
    //formData.append('Status', form.value.Status);
    //formData.append('Prioritys', form.value.Prioritys);
    //formData.append('Categorys', form.value.Categorys);
    //formData.append('CreatedBy', form.value.CreatedBy);
    //formData.append('AssignedTo', form.value.AssignedTo);
    //formData.append('CreatedDate', form.value.CreatedDate);
    //formData.append('DueDate', form.value.DueDate);

    const data = {
      TicketId: 0, // backend will auto-generate
      Title: form.value.Title,
      Description: form.value.Description,
      Status: Number(form.value.Status),       
      Prioritys: Number(form.value.Prioritys), 
      Categorys: Number(form.value.Categorys), 
      CreatedBy: form.value.CreatedBy,
      AssignedTo: form.value.AssignedTo,
      CreatedDate: new Date().toISOString(), 
      DueDate: form.value.DueDate,
      AttachmentPath: null
    };

    //const fileInput = (document.getElementById('AttachmentPath') as HTMLInputElement);
    //if (fileInput.files?.length)
    //{
    //  formData.append('Attachment', fileInput.files[0]);
    //}
    if (data)
    {
      console.log("form data:", data);

      this.ticketService.createTicket(data).subscribe(
        (result) => {
          console.log('Ticket created successfully', result);
          this.successMessage = "Ticket created successfully!";
          form.reset(); 
        },
        (error) => {
          console.error(error);
          this.errorMessage = "Failed to create ticket!";
        }
      );
    }
  }

}
