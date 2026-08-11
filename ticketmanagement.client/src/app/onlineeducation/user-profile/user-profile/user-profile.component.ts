import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
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
    profileImage: '/assets/images/default-user.png',    
    city: 'Tech City',
    state: 'TC',
    dob: '' // ISO date string or empty
  };  
  constructor(private router: Router, private route: ActivatedRoute, private syllabusService: SyllabusDataService) { }

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
    this.router.navigate(['profile'], { relativeTo: this.route });
  }

  viewUserCourse() {
    console.log('View user course clicked');   
    this.activeSidebar = 'course';    
    this.router.navigate(['course'], { relativeTo: this.route });
  }

  viewTestAttempted() {
    console.log('View test attempted clicked');
    this.activeSidebar = 'attempts';
    this.router.navigate(['attempted'], { relativeTo: this.route });
  }

  viewTestScore() {
    console.log('View test score clicked');
    this.activeSidebar = 'scores';
    this.router.navigate(['score'], { relativeTo: this.route });
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

  // Handle image load error and set default image
  onImageError(event: any) {
    event.target.src = '/assets/images/team-1.jpg';
  }

  // Update profile image
  updateProfileImage(imageUrl: string) {
    if (imageUrl && imageUrl.trim() !== '') {
      this.user.profileImage = imageUrl;
    } else {
      this.user.profileImage = '/assets/images/team-1.jpg';
    }
  }
}
