import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';

@Component({
  selector: 'app-user-profile-details',
  templateUrl: './user-profile-details.component.html',
  styleUrls: ['./user-profile-details.component.css']
})
export class UserProfileDetailsComponent implements OnInit {
  // added fields: city, state, dob
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
    this.LoadUserProfile();
  }

  LoadUserProfile() {
    // Fetch user profile data from API
    this.syllabusService.UserProfileData().subscribe(
      response => {
        console.log('User profile data fetched successfully:', response);
        // Normalize response naming differences:
        // server returns ProfileImageUrl or profileImageUrl; use first available
        const img = (response.profileImageUrl ?? response.ProfileImageUrl ?? response.profileImage ?? response.ProfileImage) || null;
        this.user = {
          ...response,
          profileImage: img
        };
        console.log('Profile Image URL:', img);
        console.log('Final URL (built):', this.getProfileImage(this.user.profileImage));
      },
      error => {
        console.error('Error fetching user profile data:', error);
        // keep local fallback user
      }
    );
  }

  toggleEditMode() {
    this.isEditMode = true;
  }

  saveProfile() {
    // Basic password validation if user requested a password change
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
      this.changePassword(this.passwordModel.current, this.passwordModel.new, this.passwordModel.confirm);
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

    console.log('Profile saved:', this.user);
    this.isEditMode = false;
    this.showChangePassword = false;
    this.passwordModel = { current: '', new: '', confirm: '' };
  }

  cancelEdit() {
    this.isEditMode = false;
    this.showChangePassword = false;
  }

  goBack() {
    this.currentView = 'profile';
    this.isEditMode = false;
    this.activeSidebar = 'profile';
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
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
    console.log('Change password requested', { current, newPassword, confirm });
    this.syllabusService.UpdatePassword(current, newPassword, confirm).subscribe(
      response => {
        console.log('Password changed successfully:', response);
        alert('Password changed successfully.');
      },
      error => {
        console.error('Error changing password:', error);
        alert('Failed to change password.');
      }
    );
  }

  // Build profile image URL safely:
  public getProfileImage(src: string | null | undefined): string {
    // Use an existing asset in the project; the placeholder file was missing.
    const clientPlaceholder = window.location.origin.replace(/\/$/, '') + '/assets/images/team-1.jpg';

    if (!src) {
      return clientPlaceholder;
    }

    // already absolute
    if (/^https?:\/\//i.test(src)) {
      return src;
    }

    // If src is a data URL (preview), return it as-is
    if (src.startsWith('data:')) {
      return src;
    }

    // src likely server relative path (e.g. "/uploads/profileimages/xxx.png")
    const apiBase = (environment && environment.apiBaseUrl) ? environment.apiBaseUrl : null;

    // If apiBase provided, use it; otherwise assume server files served from same origin as client
    if (apiBase) {
      return apiBase.replace(/\/$/, '') + (src.startsWith('/') ? src : '/' + src);
    }

    // Fallback: resolve relative to client origin (useful when dev server proxies)
    return window.location.origin.replace(/\/$/, '') + (src.startsWith('/') ? src : '/' + src);
  }

  public onImageError(event: Event) {
    const target = event.target as HTMLImageElement;
    target.onerror = null;
    target.src = window.location.origin.replace(/\/$/, '') + '/assets/images/team-1.jpg';
  }
}


