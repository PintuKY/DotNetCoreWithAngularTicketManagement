import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';

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

  // added fields: city, state, dob
  user: any = {
    firstName: 'Rakesh',
    lastName: 'Yadav',
    email: 'rakesh@example.com',
    phone: '+91-9876543210',
    address: '123 Education Street, Tech City, TC 12345',
    profileImage: 'assets/onlinestudey/img/hero-img.png',    
    city: 'Tech City',
    state: 'TC',
    dob: '' // ISO date string or empty
  };

  // Drag state
  isDragging = false;

  // Change password UI model
  showChangePassword = false;
  passwordModel = {
    current: '',
    new: '',
    confirm: ''
  };

  constructor(private router: Router, private syllabusService: SyllabusDataService) { }

  ngOnInit(): void {
    // Fetch user profile data from API
    this.LoadUserProfile();
  }
LoadUserProfile() {

  // Fetch user profile data from API
    this.syllabusService.UserProfileData().subscribe(
      response => {
        console.log('User profile data fetched successfully:', response);
        this.user = response;
      },
      error => {
        console.error('Error fetching user profile data:', error);
        alert('Failed to fetch user profile data. Please try again later.');
      }
    );
}
  toggleEditMode() {
    this.isEditMode = true;
  }

  saveProfile() {
    //Basic password validation if user requested a password change
    if (this.showChangePassword) {
      if (!this.passwordModel.current || !this.passwordModel.new || !this.passwordModel.confirm) {
        alert('Please fill all password fields to change password.');
        return;
      }
      if (this.passwordModel.new !== this.passwordModel.confirm) {
        alert('New password and confirm password do not match.');
        return;
      }
      // call change password API here (placeholder)
      this.changePassword(this.passwordModel.current, this.passwordModel.new,this.passwordModel.confirm);
    }
    this.syllabusService.SubmitUserProfileData(this.user).subscribe(
        response => {
          console.log('Profile updated successfully:', response);
          alert('Profile updated successfully.');
        },
        error => {
          console.error('Error updating profile:', error);
          alert('Failed to update profile. Please try again later.');
        }
      );
    // Save profile (call API) - placeholder
    console.log('Profile saved:', this.user);
    this.isEditMode = false;
    this.showChangePassword = false;
    // reset password model
    this.passwordModel = { current: '', new: '', confirm: '' };
  }

  cancelEdit() {
    this.isEditMode = false;
    this.showChangePassword = false;
    // Optionally reload user data from server to discard local edits
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

  // Image handlers
  onProfileImageSelected(evt: Event) {
    const input = evt.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.readAndPreviewFile(input.files[0]);
    }
  }

  onDragOver(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.isDragging = false;
  }

  onDrop(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.isDragging = false;
    const files = evt.dataTransfer?.files;
    if (files && files.length) {
      const file = files[0];
      this.readAndPreviewFile(file);
    }
  }

  private readAndPreviewFile(file: File) {
    const maxSizeMB = 5;
    if (file.size > maxSizeMB * 1024 * 1024) {
      alert('File is too large. Max 5MB allowed.');
      return;
    }

    const allowed = ['image/png', 'image/jpeg', 'image/jpg', 'image/webp'];
    if (!allowed.includes(file.type)) {
      alert('Unsupported file type. Use PNG or JPG.');
      return;
    }

    const reader = new FileReader();
    reader.onload = (e) => {
      this.user.profileImage = e.target?.result as string;
      // You should upload the file to server here and replace preview with server URL.
      console.log('Profile image preview set');
    };
    reader.readAsDataURL(file);
  }

  toggleChangePassword() {
    this.showChangePassword = !this.showChangePassword;
    if (!this.showChangePassword) {
      this.passwordModel = { current: '', new: '', confirm: '' };
    }
  }

  private changePassword(current: string, newPassword: string, confirm: string) {
    // Placeholder: call change password API here.
    console.log('Change password requested', { current, newPassword, confirm });
    this.syllabusService.UpdatePassword(current, newPassword, confirm).subscribe(
      response => {
        console.log('Password changed successfully:', response);
        // Simulate success
        alert('Password changed successfully.');
      },
      error => {
        console.error('Error changing password:', error);
        alert('Failed to change password.');
      }
    );
  }
}
