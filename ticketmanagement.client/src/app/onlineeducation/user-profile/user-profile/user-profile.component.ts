import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  isEditMode = false;
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

  changeProfile() {
    console.log('Change profile clicked');
    this.toggleEditMode();
  }

  viewTestAttempted() {
    console.log('View test attempted clicked');
    // Navigate to test attempted page or show modal
  }

  viewTestScore() {
    console.log('View test score clicked');
    // Navigate to test score page
  }

  logout() {
    console.log('Logout clicked');
    // Clear session/token and navigate to login
    this.router.navigate(['/login']);
  }
}
