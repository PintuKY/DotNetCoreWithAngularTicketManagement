import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

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
  user = {
    firstName: 'Rakesh',
    lastName: 'Yadav',
    email: 'rakesh@example.com',
    phone: '+91-9876543210',
    address: '123 Education Street, Tech City, TC 12345',
    profileImage: 'assets/onlinestudey/img/hero-img.png'
  };

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  toggleEditMode() {
    this.isEditMode = true;
  }

  saveProfile() {
    console.log('Profile saved:', this.user);
    this.isEditMode = false;
    // Call API to save profile here
  }

  cancelEdit() {
    this.isEditMode = false;
    // Reset user data if needed
  }

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
    console.log('Sidebar collapsed:', this.isSidebarCollapsed);
  }

  changeProfile() {
    console.log('Change profile clicked');
    this.currentView = 'profile';
    this.isEditMode = true;
    this.activeSidebar = 'profile';
  }

  viewUserCourse() {
    console.log('View user course clicked');
    this.currentView = 'course';
    this.activeSidebar = 'course';
  }

  viewTestAttempted() {
    console.log('View test attempted clicked');
    this.currentView = 'attempts';
    this.activeSidebar = 'attempts';
  }

  viewTestScore() {
    console.log('View test score clicked');
    this.currentView = 'scores';
    this.activeSidebar = 'scores';
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
