import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
import { UserCourseFileComponent } from '../user-course/user-course-file/user-course-file.component';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  isEditMode = false;
  isSidebarCollapsed = false;
  currentView: 'profile' | 'course' | 'attempts' | 'scores' = 'profile';
  activeSidebar: 'profile' | 'course' | 'attempts' | 'scores' | 'logout' = 'profile';
user: any = {
    firstName: 'Rakesh',
    lastName: 'Yadav',
    email: 'rakesh@example.com',
    phone: '+91-9876543210',
    address: '123 Education Street, Tech City, TC 12345',
    profileImage: '',    
    city: 'Tech City',
    state: 'TC',
    dob: '' // ISO date string or empty
  };  
  constructor(private router: Router, private syllabusService: SyllabusDataService) { }

  ngOnInit(): void {
    // Fetch user profile data from API
    //this.LoadUserProfile();
  }


  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
    console.log('Sidebar collapsed:', this.isSidebarCollapsed);
  }

  changeProfile() {
    console.log('Change profile clicked');
   this.activeSidebar = 'profile';
  this.router.navigate(['userprofile/profile']);
  }

  viewUserCourse() {
    console.log('View user course clicked');   
    this.activeSidebar = 'course';    
    this.router.navigate(['userprofile/course']);
  }

  viewTestAttempted() {
    console.log('View test attempted clicked');
   this.activeSidebar = 'attempts';
  this.router.navigate(['userprofile/attempted']);
  }

  viewTestScore() {
    console.log('View test score clicked');
    this.activeSidebar = 'scores';
    this.router.navigate(['userprofile/score']);
  }

  goBack() {
    this.currentView = 'profile';
    this.isEditMode = false;
    this.activeSidebar = 'profile';
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout() {
    console.log('Logout clicked');
    this.activeSidebar = 'logout';
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    this.router.navigate(['/login']);
  }
}
