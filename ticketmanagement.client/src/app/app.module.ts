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
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateComponent } from './tickets/update/update.component';
import { HeaderFileComponent } from './onlineeducation/header/header-file/header-file.component';
import { FooterFileComponent } from './onlineeducation/footer/footer-file/footer-file.component';
import { MainbodyFileComponent } from './onlineeducation/mainbody/mainbody-file/mainbody-file.component';
import { BodybannersFileComponent } from './onlineeducation/banner/bodybanners-file/bodybanners-file.component';
import { LayoutComponent } from './onlineeducation/layout/layout.component';
import { QuestionsFileComponent } from './onlineeducation/question/questions-file/questions-file.component';
import { Layout1Component } from './onlineeducation/layout1/layout1.component';
import { NavbarFileComponent } from './onlineeducation/layout1/navbar/navbar-file/navbar-file.component';
import { BannerFileComponent } from './onlineeducation/layout1/banner/banner-file/banner-file.component';
import { FootersFilesComponent } from './onlineeducation/layout1/footer/footers-files/footers-files.component';
import { StatesFilesComponent } from './onlineeducation/states/states-files/states-files.component';
import { SylabusFileComponent } from './onlineeducation/syllabus/sylabus-file/sylabus-file.component';
import { ChapterFilesComponent } from './onlineeducation/chapters/chapter-files/chapter-files.component';

// const routes: Routes = [
  //{ path: 'tickets', component: TicketsComponent },
//   { path: '', redirectTo: '/tickets', pathMatch: 'full' },
//   { path: 'reports', component: ReportsComponent },
//   { path: 'addticket', component: CreateTicketComponent },
//   { path: 'edit/:id', component: UpdateComponent }
// ];

// const routes: Routes = [
//     { path: '', component: MainbodyFileComponent },
//     { path: 'question', component: QuestionsFileComponent }, 
//  // { path: '', redirectTo: '/Question', pathMatch: 'full' },
// ];
const routes: Routes = [

{
  path: '',
  component: LayoutComponent,
  children:
  [
    { path: '', component: MainbodyFileComponent },
    { path: 'states', component: StatesFilesComponent },
    { path: 'syllabus', component: SylabusFileComponent },
    { path: 'chapters', component: ChapterFilesComponent }


  ]
},

{
  path: 'question',component: Layout1Component,
  children:
  [
    { path: '', component: QuestionsFileComponent }
  ]
}

];


@NgModule({
  declarations: [
    AppComponent,
    TicketsComponent,
    CreateTicketComponent,
    TruncatePipe,
    UpdateComponent,
    HeaderFileComponent,
    FooterFileComponent,
    MainbodyFileComponent,
    BodybannersFileComponent,
    LayoutComponent,
    QuestionsFileComponent,
    Layout1Component,
    NavbarFileComponent,
    BannerFileComponent,
    FootersFilesComponent,
    StatesFilesComponent,
    SylabusFileComponent,
    ChapterFilesComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,    
    RouterModule.forRoot(routes),
    ReactiveFormsModule

  ],
  exports: [
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
