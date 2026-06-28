import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

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

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  submitLogin() {
    this.loginError = '';
    this.successMessage = '';

    if (!this.email.trim() || !this.password) {
      this.loginError = 'Email and password are required.';
      return;
    }

    this.isSubmitting = true;
    this.http.post<any>(this.loginApi, {
      email: this.email,
      password: this.password
    }).subscribe({
      next: response => {
        localStorage.setItem('currentUser', JSON.stringify(response.user));
        this.successMessage = response.message || 'Login successful.';
        this.isSubmitting = false;
        this.router.navigate(['/states']);
      },
      error: error => {
        this.loginError = error?.error?.message || 'Login failed. Please check your email and password.';
        this.isSubmitting = false;
      }
    });
  }
}
