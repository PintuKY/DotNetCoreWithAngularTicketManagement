import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { TicketsComponent } from './tickets/tickets/tickets.component';
import { ReportsComponent } from './tickets/reports/reports.component';


const routes: Routes = [
  { path: 'tickets', component: TicketsComponent },
  //{ path: '', redirectTo: '/tickets', pathMatch: 'full' },
  { path: 'reports', component: ReportsComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    TicketsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
