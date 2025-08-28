import { HttpClientModule } from '@angular/common/http'; //this for service call api backend
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; //this for form submit ngForm, ngModel, (ngSubmit) all belong to FormsModule
import { AppComponent } from './app.component';
import { TicketsComponent } from './tickets/tickets/tickets.component';
import { ReportsComponent } from './tickets/reports/reports.component';
import { CreateTicketComponent } from './create-ticket/create-ticket.component';
import { TruncatePipe } from './truncate.pipe';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateComponent } from './tickets/update/update.component';

const routes: Routes = [
  { path: 'tickets', component: TicketsComponent },
  //{ path: '', redirectTo: '/tickets', pathMatch: 'full' },
  { path: 'reports', component: ReportsComponent },
  { path: 'addticket', component: CreateTicketComponent },
  { path: 'edit/:id', component: UpdateComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    TicketsComponent,
    CreateTicketComponent,
    TruncatePipe,
    UpdateComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    ReactiveFormsModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
