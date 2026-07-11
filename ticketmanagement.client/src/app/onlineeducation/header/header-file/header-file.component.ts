import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header-file',
  templateUrl: './header-file.component.html',
  styleUrls: ['./header-file.component.css']
})
export class HeaderFileComponent implements OnInit {
  isLoggedIn: boolean = false;

  constructor() { }

  ngOnInit(): void {
    // Check if user is logged in (from localStorage or auth service)
    const token = localStorage.getItem('token');
    this.isLoggedIn = !!token;
  }
// isLoggedIn(): boolean {
//   return !!localStorage.getItem('token');
// }
}
