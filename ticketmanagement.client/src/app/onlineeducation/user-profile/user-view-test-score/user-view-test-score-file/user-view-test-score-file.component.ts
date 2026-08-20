import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import Chart from 'chart.js/auto';
import { SyllabusDataService } from '../../../../services/onlineeducation/syllabus/syllabus-data.service';

@Component({
  selector: 'app-user-view-test-score-file',
  templateUrl: './user-view-test-score-file.component.html',
  styleUrls: ['./user-view-test-score-file.component.css']
})
export class UserViewTestScoreFileComponent implements OnInit {
 // Loading
  isLoading = true;
  errorMessage = '';

  // GUID values coming from Angular/route/navigation
  testGuid: string = '';
  syllabusGuid: string = '';
  chapterGuid: string = '';

  // Complete API response
  dashboardData: any = null;

  // Current user
  currentUser: any = null;

  // Topper
  topper: any = null;

  // Leaderboard
  leaderboard: any[] = [];


  constructor(
    private syllabusDataService: SyllabusDataService,
    private http: HttpClient,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
     // If GUIDs are coming from query params
    this.route.queryParams.subscribe(params => {

      this.testGuid = params['testGuid'] || '';
      this.syllabusGuid = params['syllabusGuid'] || '';
      this.chapterGuid = params['chapterGuid'] || '';

      console.log('Test GUID:', this.testGuid);
      console.log('Syllabus GUID:', this.syllabusGuid);
      console.log('Chapter GUID:', this.chapterGuid);

      if (this.testGuid && this.syllabusGuid && this.chapterGuid
      ) {
        this.loadUserTestScoreDetails();
      }
    });
    // this.route.queryParamMap.subscribe(params => {
    //   this.testGuid = params.get('testGuid');
    //   this.syllabusGuid = params.get('syllabusGuid');
    //   this.chapterGuid = params.get('chapterGuid');

    //   console.log('testGuid:', this.testGuid);
    //   console.log('syllabusGuid:', this.syllabusGuid);
    //   console.log('chapterGuid:', this.chapterGuid);

    //   if (this.testGuid && this.syllabusGuid && this.chapterGuid) {
    //     this.loadScoreByGuids(this.testGuid, this.syllabusGuid, this.chapterGuid);
    //   }
    // });

   // this.LoadUserTestScoreDetails();
    // this.http.get<any>('/api/performance-report').subscribe({
    //   next: report => {
    //     this.report = report;
    //     setTimeout(() => this.renderDonutChart(), 0);
    //   },
    //   error: () => {
    //     setTimeout(() => this.renderDonutChart(), 0);
    //   }
    // });
  }
// =====================================================
  // GET USER TEST RESULT DASHBOARD
  // =====================================================
  loadUserTestScoreDetails(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.syllabusDataService
      .GetUserTestScoreDetailsByGuids(
        this.testGuid,
        this.syllabusGuid,
        this.chapterGuid
      )
      .subscribe({

        next: (response: any) => {

          console.log(
            'User Test Score API Response:',
            response
          );

          this.isLoading = false;

          // Your backend returns:
          // {
          //   success: true,
          //   data: { ... }
          // }

          if (response?.success) {

            this.dashboardData = response.data;

            // Current logged-in user
            this.currentUser =
              response.data.currentUserResult;

            // Topper
            this.topper =
              response.data.topperResult;

            // All ranking users
            this.leaderboard =
              response.data.leaderboard || [];

            console.log(
              'Current User:',
              this.currentUser
            );

            console.log(
              'Topper:',
              this.topper
            );

            console.log(
              'Leaderboard:',
              this.leaderboard
            );
          }
          else {
            this.errorMessage =
              response?.message ||
              'Unable to load test result.';
          }
        },

        error: (error) => {

          console.error(
            'Error loading test score details:',
            error
          );

          this.isLoading = false;

          this.errorMessage =
            error?.error?.message ||
            'Failed to load test result.';
        }
      });
  }

// =====================================================
  // FORMAT TIME
  // API TimeSpan may come as:
  // "00:00:24"
  // =====================================================
  formatTime(time: string | null | undefined): string {

    if (!time) {
      return '0 Min 0 Sec';
    }

    const parts = time.split(':');

    if (parts.length === 3) {

      const hours = Number(parts[0]);
      const minutes = Number(parts[1]);
      const seconds = Math.floor(Number(parts[2]));

      const totalMinutes =
        (hours * 60) + minutes;

      return `${totalMinutes} Min ${seconds} Sec`;
    }

    return time;
  }


  // =====================================================
  // FORMAT AVERAGE TIME
  // =====================================================
  formatAverageTime(seconds: number): string {

    if (!seconds || seconds <= 0) {
      return '0 Min 0 Sec';
    }

    const minutes = Math.floor(seconds / 60);
    const remainingSeconds =
      Math.round(seconds % 60);

    return `${minutes} Min ${remainingSeconds} Sec`;
  }


  // =====================================================
  // REFRESH DATA
  // =====================================================
  refreshResult(): void {
    this.loadUserTestScoreDetails();
  }


  // loadScoreByGuids(testGuid: string, syllabusGuid: string, chapterGuid: string): void 
  // {
  //     console.log('loadScoreByGuids testGuid:', this.testGuid);
  //     console.log('loadScoreByGuids syllabusGuid:', this.syllabusGuid);
  //     console.log('loadScoreByGuids chapterGuid:', this.chapterGuid);
  //   this.syllabusDataService.GetUserTestScoreDetailsByGuids(testGuid, syllabusGuid, chapterGuid)
  //     .subscribe({
  //       next: (response) => {
  //         console.log('Score details by GUIDs:', response);
  //       },
  //       error: (error) => {
  //         console.error('Error loading score details:', error);
  //       }
  //     });
  // }

  // private renderDonutChart(): void {
  //   // destroy previous chart if any
  //   if (this.donutChart) {
  //     try { this.donutChart.destroy(); } catch (e) { /* ignore */ }
  //     this.donutChart = null;
  //   }

  //   // ensure report exists and canvas is present
  //   if (!this.report) { return; }
  //   const canvas = document.getElementById('donutChart') as HTMLCanvasElement | null;
  //   if (!canvas) { return; }
  //   const ctx = canvas.getContext('2d');
  //   if (!ctx) { return; }

  //   // derive numbers from sections if available
  //   let totalQuestions = 0;
  //   let attemptedTotal = 0;
  //   let correctTotal = 0;
  //   for (const s of this.report.sections || []) {
  //     // expected formats like '2 / 30' or '0 / 40'
  //     const attemptedStr = (s.attempted || '').toString();
  //     const correctStr = (s.correct || '').toString();
  //     const totalStr = attemptedStr.includes('/') ? attemptedStr.split('/')[1] : (totalQuestions ? String(totalQuestions) : '0');

  //     const attempted = parseInt(attemptedStr.split('/')[0] ? attemptedStr.split('/')[0].trim() : '0', 10) || 0;
  //     const correct = parseInt(correctStr.split('/')[0] ? correctStr.split('/')[0].trim() : '0', 10) || 0;
  //     const total = parseInt(totalStr.trim(), 10) || 0;

  //     attemptedTotal += attempted;
  //     correctTotal += correct;
  //     totalQuestions += total;
  //   }

  //   // fallback if parsing failed — try simple stats fallback
  //   if (totalQuestions === 0) {
  //     totalQuestions = 100;
  //   }

  //   const notVisited = Math.max(0, totalQuestions - attemptedTotal);
  //   const wrong = Math.max(0, attemptedTotal - correctTotal);
  //   const unattempted = notVisited;

  //   const data = [correctTotal || 0, wrong || 0, unattempted || 0, notVisited || 0];

  //   this.donutChart = new Chart(ctx, {
  //     type: 'doughnut',
  //     data: {
  //       labels: ['Correct', 'Wrong', 'Unattempted', 'Not Visited'],
  //       datasets: [
  //         {
  //           data,
  //           backgroundColor: ['#16a34a', '#ef4444', '#f59e0b', '#94a3b8'],
  //           hoverOffset: 8,
  //           borderWidth: 0
  //         }
  //       ]
  //     },
  //     options: {
  //       responsive: true,
  //       maintainAspectRatio: false,
  //       cutout: '70%',
  //       plugins: {
  //         legend: {
  //           display: false
  //         },
  //         tooltip: {
  //           callbacks: {
  //             label: (context: any) => {
  //               const label = context.label || '';
  //               const value = context.parsed || 0;
  //               return `${label}: ${value}`;
  //             }
  //           }
  //         }
  //       }
  //     }
  //   });
  // }

  // ngOnDestroy(): void {
  //   if (this.donutChart) {
  //     try { this.donutChart.destroy(); } catch (e) { }
  //   }
  // }

  // LoadUserTestScoreDetails() {
  //   this.syllabusDataService.GetUserTestScoreDetails().subscribe({
  //     next: (response) => {
  //       console.log('User test score details fetched successfully:', response);
  //     },
  //     error: (error) => {
  //       console.error('Error fetching user test score details:', error);
  //     }
  //   });
  // }
}
