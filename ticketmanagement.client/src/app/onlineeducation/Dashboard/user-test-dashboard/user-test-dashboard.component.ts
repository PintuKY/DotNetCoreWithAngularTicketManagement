import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import Chart from 'chart.js/auto';

interface StatCard {
  label: string;
  value: string;
  accent: string;
  icon: string;
}

interface LeaderboardItem {
  rank: number;
  name: string;
  marks: string;
}

interface ComparisonRow {
  label: string;
  icon: string;
  you: string;
  topper: string;
}

interface SectionRow {
  section: string;
  attempted: string;
  correct: string;
  accuracy: string;
  time: string;
}

interface HighlightCard {
  title: string;
  subject: string;
  icon: string;
  tone: string;
}

interface EfficiencyCard {
  title: string;
  value: string;
  icon: string;
  tone: string;
}

interface PerformanceReport {
  examTitle: string;
  stats: StatCard[];
  leaderboard: LeaderboardItem[];
  comparison: ComparisonRow[];
  highlights: HighlightCard[];
  sections: SectionRow[];
  efficiency: EfficiencyCard[];
}

@Component({
  selector: 'app-user-test-dashboard',
  templateUrl: './user-test-dashboard.component.html',
  styleUrls: ['./user-test-dashboard.component.css']
})
export class UserTestDashboardComponent implements OnInit {

  private donutChart: any = null;
  report?: PerformanceReport;
    //if data not came list by default data property of report to fallbackReport to avoid template errors and show default values
   // readonly fallbackReport: PerformanceReport = {
  //   examTitle: 'Bihar Higher Secondary Computer Science Mock Test - 10'    
 // };
  constructor(private readonly http: HttpClient) { }

  ngOnInit(): void {
     this.http.get<PerformanceReport>('/api/performance-report')
      .subscribe({
        next: report => {
          this.report = report;
          console.log('Fetched performance report:', report);
          setTimeout(() => this.renderDonutChart(), 0);
        },
        error: () => {
          //if data not came list by default data property of report to fallbackReport to avoid template errors and show default values
          //this.report = this.fallbackReport;
          setTimeout(() => this.renderDonutChart(), 0);
        }
      });
  }

  private renderDonutChart(): void {
    // destroy previous chart if any
    if (this.donutChart) {
      try { this.donutChart.destroy(); } catch (e) { /* ignore */ }
      this.donutChart = null;
    }

    // ensure report exists and canvas is present
    if (!this.report) { return; }
    const canvas = document.getElementById('donutChart') as HTMLCanvasElement | null;
    if (!canvas) { return; }
    const ctx = canvas.getContext('2d');
    if (!ctx) { return; }

    // derive numbers from sections if available
    let totalQuestions = 0;
    let attemptedTotal = 0;
    let correctTotal = 0;
    for (const s of this.report.sections || []) {
      // expected formats like '2 / 30' or '0 / 40'
      const attemptedStr = (s.attempted || '').toString();
      const correctStr = (s.correct || '').toString();
      const totalStr = attemptedStr.includes('/') ? attemptedStr.split('/')[1] : (totalQuestions ? String(totalQuestions) : '0');

      const attempted = parseInt(attemptedStr.split('/')[0] ? attemptedStr.split('/')[0].trim() : '0', 10) || 0;
      const correct = parseInt(correctStr.split('/')[0] ? correctStr.split('/')[0].trim() : '0', 10) || 0;
      const total = parseInt(totalStr.trim(), 10) || 0;

      attemptedTotal += attempted;
      correctTotal += correct;
      totalQuestions += total;
    }

    // fallback if parsing failed — try simple stats fallback
    if (totalQuestions === 0) {
      totalQuestions = 100;
    }

    const notVisited = Math.max(0, totalQuestions - attemptedTotal);
    const wrong = Math.max(0, attemptedTotal - correctTotal);
    const unattempted = notVisited;

    const data = [correctTotal || 0, wrong || 0, unattempted || 0, notVisited || 0];

    this.donutChart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['Correct', 'Wrong', 'Unattempted', 'Not Visited'],
        datasets: [
          {
            data,
            backgroundColor: ['#16a34a', '#ef4444', '#f59e0b', '#94a3b8'],
            hoverOffset: 8,
            borderWidth: 0
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '70%',
        plugins: {
          legend: {
            display: false
          },
          tooltip: {
            callbacks: {
              label: (context: any) => {
                const label = context.label || '';
                const value = context.parsed || 0;
                return `${label}: ${value}`;
              }
            }
          }
        }
      }
    });
  }

  ngOnDestroy(): void {
    if (this.donutChart) {
      try { this.donutChart.destroy(); } catch (e) { }
    }
  }

}
