import { Component, OnInit } from '@angular/core';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
@Component({
  selector: 'app-header-file',
  templateUrl: './header-file.component.html',
  styleUrls: ['./header-file.component.css']
})
export class HeaderFileComponent implements OnInit {
  isLoggedIn: boolean = false;
  user: any = null;
  constructor(private syllabusService: SyllabusDataService) { }

  ngOnInit(): void {

    this.syllabusService.userProfile$.subscribe(profile => {

      if (profile) {
        this.user = profile;
      }

    });
    // Check if user is logged in (from localStorage or auth service)
    const token = localStorage.getItem('token');
    this.isLoggedIn = !!token;
  }
// isLoggedIn(): boolean {
//   return !!localStorage.getItem('token');
// }
}
