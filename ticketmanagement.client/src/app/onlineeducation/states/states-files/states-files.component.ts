import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface TestItem {
  id: number;
  testGuid: string;
  testName: string;
  description: string;
  price: number;
  isActive: boolean;
  createdOn: string;
  totalSyllabus: number;
  isPaid: boolean;
}

@Component({
  selector: 'app-states-files',
  templateUrl: './states-files.component.html',
  styleUrls: ['./states-files.component.css']
})
export class StatesFilesComponent implements OnInit {

  tests: TestItem[] = [];
  loading = false;
  error: string | null = null;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadTests();
  }

  loadTests(): void {
    console.log('Loading tests...');
    this.loading = true;
    this.error = null;
    this.http.get<TestItem[]>('/api/Tests').subscribe({
      next: data => {
        this.tests = data || [];
        this.loading = false;
      },
      error: err => {
        this.error = (err && err.message) ? err.message : 'Failed to load tests.';
        this.tests = [];
        this.loading = false;
      }
    });
  }

}
