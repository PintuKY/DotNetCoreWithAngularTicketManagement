import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration-file',
  templateUrl: './registration-file.component.html',
  styleUrls: ['./registration-file.component.css']
})
export class RegistrationFileComponent implements OnInit {
  firstName = '';
  lastName = '';
  email = '';
  emailOtp = '';
  password = '';
  confirmPassword = '';
  emailTouched = false;
  emailValid = false;
  emailVerified = false;
  emailVerificationSent = false;
  emailError = '';
  emailMessage = '';
  passwordError = '';
  formError = '';
  enterOTP='';
  successMessage = '';
  isSubmitting = false;
  isVerifyingEmail = false;
  confirmSubmitOpen = false;
  reportModalOpen = false;
  reportMessage = '';
  private readonly registrationApi = '/api/Login/registration';
  private readonly emailVerificationApi = '/api/Login/emailvarifcation';
 // /api/Login/emailvarifcation  emailvarifcation
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  // onEmailInput(value: string) {
  //   this.email = value;
  //   this.emailTouched = true;
  //   this.emailVerified = false;
  //   this.emailVerificationSent = false;
  //   this.emailOtp = '';
  //   this.emailMessage = '';
  //   this.validateEmail();
  // }
  onEmailInput(value: string): void {

  this.email = value;

  this.emailTouched = true;

  this.emailVerified = false;

  this.emailVerificationSent = false;

  this.enterOTP = '';

  this.emailMessage = '';

  this.emailError = '';

  this.validateEmail();
}

  validateEmail() {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    this.emailValid = emailPattern.test(this.email);
    if (this.emailTouched && !this.emailValid) {
      this.emailError = 'Please enter a valid email address.';
    } else {
      this.emailError = '';
    }
  }

  // verifyEmail() {
  //   this.emailError = '';
  //   this.emailMessage = '';
  //   if (!this.emailValid) {
  //     this.emailError = 'Enter a valid email address before verification.';
  //     return;
  //   }

  //   if (this.emailVerificationSent && !this.emailOtp.trim()) {
  //     this.emailError = 'Please enter OTP.';
  //     return;
  //   }    

  //   const payload = {
  //     email: this.email,
  //     Otp:this.enterOTP
  //   };
  //   console.log("OTPMail",this.email);
  //   this.isVerifyingEmail = true;
  //   this.http.post<{ message: string; emailSent?: boolean; devOtp?: string }>(this.emailVerificationApi, payload).subscribe({
  //     next: response => {
  //       this.isVerifyingEmail = false;
  //       if (!this.emailVerificationSent) {
  //         this.emailVerificationSent = true;
  //         this.emailMessage = response.devOtp
  //           ? `${response.message} OTP: ${response.devOtp}`
  //           : response.message || 'OTP sent to your email.';
  //         return;
  //       }

  //       this.emailVerified = true;
  //       this.emailMessage = response.message || 'Email verified successfully.';
  //     },
  //     error: error => {
  //       this.isVerifyingEmail = false;
  //       this.emailError = error?.error?.message || 'Email verification failed. Please try again.';
  //     }
  //   });
  // }

verifyEmail(): void {

  this.emailError = '';
  this.emailMessage = '';

  if (!this.emailValid) {
    this.emailError = 'Enter a valid email address before verification.';
    return;
  }

  const payload = {
    email: this.email
  };

  this.isVerifyingEmail = true;

  this.http.post<any>(this.emailVerificationApi, payload)
    .subscribe({
      next: response => {

        this.isVerifyingEmail = false;

        this.emailVerificationSent = true;

        this.emailMessage =
          response.message || 'OTP sent to your email.';

        // Open popup ONLY after OTP is successfully sent
        this.openSubmitConfirm();
      },

      error: error => {

        this.isVerifyingEmail = false;

        this.emailError =
          error?.error?.message ||
          'Unable to send OTP. Please try again.';
      }
    });
}

confirmSubmit(): void {

  this.emailError = '';

  if (!this.enterOTP || this.enterOTP.trim().length !== 6) {
    this.emailError = 'Please enter a valid 6-digit OTP.';
    return;
  }

  const payload = {
    email: this.email,
    otp: this.enterOTP.trim()
  };

  this.isVerifyingEmail = true;

  this.http.post<any>(this.emailVerificationApi, payload)
    .subscribe({

      next: response => {

        this.isVerifyingEmail = false;

        this.emailVerified = true;

        this.emailMessage =
          response.message || 'Email verified successfully.';

        this.closeSubmitConfirm();

        this.enterOTP = '';
      },

      error: error => {

        this.isVerifyingEmail = false;

        this.emailError =
          error?.error?.message ||
          'OTP verification failed.';
      }
    });
}

  validatePassword(): boolean {
    if (!this.password) {
      this.passwordError = 'Password is required.';
      return false;
    }

    if (this.password.length > 10) {
      this.passwordError = 'Password must be maximum 10 characters.';
      return false;
    }

    const passwordPattern = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9\s])\S{1,10}$/;
    if (!passwordPattern.test(this.password)) {
      this.passwordError = 'Password must include one capital letter, one small letter, one number and one special symbol.';
      return false;
    }

    this.passwordError = '';
    return true;
  }

  //open model for email otp varification 
  openReportModal(): void {
  this.reportMessage = '';
  this.reportModalOpen = true;
}
closeReportModal(): void {
  this.reportModalOpen = false;
}
closeSubmitConfirm(): void {
  this.confirmSubmitOpen = false;
}
// confirmSubmit(): void {
//   this.closeSubmitConfirm();
  
// }
openSubmitConfirm(): void {
  this.confirmSubmitOpen = true;
}

  submitRegistration() {
    this.formError = '';
    this.passwordError = '';
    this.successMessage = '';
    this.validateEmail();
    if (!this.firstName.trim() || !this.lastName.trim()) {
      this.formError = 'First name and last name are required.';
      return;
    }
    if (!this.emailVerified) {
      this.emailError = 'Please verify your email before continuing.';
      return;
    }
    if (!this.validatePassword()) {
      return;
    }
    if (this.password !== this.confirmPassword) {
      this.passwordError = 'Password and confirm password do not match.';
      return;
    }

    const payload = {
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword
    };

    this.isSubmitting = true;
    this.http.post<{ message: string }>(this.registrationApi, payload).subscribe({
      next: response => {
        this.successMessage = response.message || 'Registration successful.';
        this.isSubmitting = false;
        setTimeout(() => this.router.navigate(['/login']), 600);
      },
      error: error => {
        this.formError = error?.error?.message || 'Registration failed. Please try again.';
        this.isSubmitting = false;
      }
    });
  }
}
