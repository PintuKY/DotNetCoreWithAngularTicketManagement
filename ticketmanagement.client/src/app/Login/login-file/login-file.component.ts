import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router,ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login-file',
  templateUrl: './login-file.component.html',
  styleUrls: ['./login-file.component.css']
})
export class LoginFileComponent implements OnInit {
  email = '';
  password = '';
  loginError = '';
  successMessage = '';
  isSubmitting = false;

  private readonly loginApi = '/api/Login/login';

  constructor(private route: ActivatedRoute,private http: HttpClient, private router: Router,) { }

  ngOnInit(): void {
  }

  private normalizeRedirectState(state: string | null | undefined): string {
    const normalized = (state || '').toString().trim().toLowerCase();

    switch (normalized) {
      case 'userprofile':
      case 'user-profile':
      case 'profile':
        return 'userprofile';
      case 'chapter':
      case 'chapters':
        return 'chapters';
      case 'question':
      case 'questions':
        return 'question';
      case 'test':
      case 'tests':
      case 'testseries':
      case 'states':
        return 'states';
      case 'syllabus':
        return 'syllabus';
      case 'testinstruction':
      case 'instruction':
        return 'testinstruction';
      case 'user-performance-reports':
      case 'reports':
        return 'User-performance-reports';
      default:
        return 'states';
    }
  }

  submitLogin() {
    this.loginError = '';
    this.successMessage = '';

    if (!this.email.trim() || !this.password) {
      this.loginError = 'Email and password are required.';
      return;
    }

    this.isSubmitting = true;
    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || null;

    this.http.post<any>(this.loginApi, {
      email: this.email,
      password: this.password
    }).subscribe({
      next: response => {
        const token = response?.token || response?.user?.token || response?.user?.accessToken || null;
        const redirectState = this.normalizeRedirectState(response?.user?.state || response?.state || returnUrl || 'userprofile');

        if (token) {
          localStorage.setItem('token', token);
        }

        if (response?.user) {
          localStorage.setItem('currentUser', JSON.stringify(response.user));
        }

        this.successMessage = response.message || 'Login successful.';
        this.isSubmitting = false;

        if (returnUrl) {
          this.router.navigateByUrl(returnUrl);
        } else {
          this.router.navigate([redirectState]);
        }
      },
      error: error => {
        this.loginError = error?.error?.message || 'Login failed. Please check your email and password.';
        this.isSubmitting = false;
      }
    });
  }
}
