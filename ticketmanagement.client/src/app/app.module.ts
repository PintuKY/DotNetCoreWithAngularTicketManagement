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
import { LayoutNoBannerComponent } from './onlineeducation/layout/layout-no-banner.component';
import { QuestionsFileComponent } from './onlineeducation/question/questions-file/questions-file.component';
import { Layout1Component } from './onlineeducation/layout1/layout1.component';
import { NavbarFileComponent } from './onlineeducation/layout1/navbar/navbar-file/navbar-file.component';
import { BannerFileComponent } from './onlineeducation/layout1/banner/banner-file/banner-file.component';
import { FootersFilesComponent } from './onlineeducation/layout1/footer/footers-files/footers-files.component';
import { StatesFilesComponent } from './onlineeducation/states/states-files/states-files.component';
import { SylabusFileComponent } from './onlineeducation/syllabus/sylabus-file/sylabus-file.component';
import { ChapterFilesComponent } from './onlineeducation/chapters/chapter-files/chapter-files.component';
import { TestinstructionComponent } from './onlineeducation/testinstruction/testinstruction.component';
import { UserTestDashboardComponent } from './onlineeducation/Dashboard/user-test-dashboard/user-test-dashboard.component';
import { LoginFileComponent } from './Login/login-file/login-file.component';
import { RegistrationFileComponent } from './Registration/registration-file/registration-file.component';
import { UserProfileComponent } from './onlineeducation/user-profile/user-profile/user-profile.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthGuard } from './guards/auth.guard';
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
  { path: 'login', component: LoginFileComponent },
  { path: 'user-profile', redirectTo: 'userprofile', pathMatch: 'full' },
  { path: 'chapter', redirectTo: 'chapters', pathMatch: 'full' },
  { path: 'chapters', component: ChapterFilesComponent, canActivate: [AuthGuard] },
  { path: 'questions', redirectTo: 'question', pathMatch: 'full' },
  { path: 'test', redirectTo: 'states', pathMatch: 'full' },
  { path: 'tests', redirectTo: 'states', pathMatch: 'full' },
  {
    path: '',
    component: LayoutComponent,
    children:
    [
      { path: '', component: MainbodyFileComponent },
      { path: 'states',
         component: StatesFilesComponent, 
        //canActivate: [AuthGuard]
      },
      { path: 'syllabus',
         component: SylabusFileComponent,
        canActivate: [AuthGuard]
      },
      
      { path: 'testinstruction', component: TestinstructionComponent },
      { path: 'User-performance-reports', 
        component: UserTestDashboardComponent,
        canActivate: [AuthGuard]
      }
    ]
  },
  { path: 'registration', component: RegistrationFileComponent },
  {
    path: '',
    component: LayoutNoBannerComponent,
    children:
    [
      { path: 'userprofile',
        component: UserProfileComponent,
        canActivate: [AuthGuard]
       }
    ]
  },
  {
    path: 'question', 
    component: Layout1Component,
    canActivate: [AuthGuard],
    children:
    [
      { path: '', 
        component: QuestionsFileComponent,
        canActivate: [AuthGuard]
      }
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
    LayoutNoBannerComponent,
    QuestionsFileComponent,
    Layout1Component,
    TestinstructionComponent,
    NavbarFileComponent,
    BannerFileComponent,
    FootersFilesComponent,
    StatesFilesComponent,
    SylabusFileComponent,
    ChapterFilesComponent,
    UserTestDashboardComponent,
    LoginFileComponent,
    RegistrationFileComponent,
    UserProfileComponent
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
  providers: [
    {
      provide:HTTP_INTERCEPTORS,
      useClass:AuthInterceptor,
      multi:true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
 