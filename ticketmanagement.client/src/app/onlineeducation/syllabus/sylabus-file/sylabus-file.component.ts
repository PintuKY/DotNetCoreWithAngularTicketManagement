import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Syllabus, TestItem } from 'src/app/model/onlineeducation/syllabus.model';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
@Component({
  selector: 'app-sylabus-file',
  templateUrl: './sylabus-file.component.html',
  styleUrls: ['./sylabus-file.component.css']
})
export class SylabusFileComponent implements OnInit {
  isLoading: boolean = false;
  syllabusdata: any[] = [];
  syllabusid: number | null = null;
  paymentModalOpen: boolean = false;
  selectedSyllabus: any = null;
  currentTest: TestItem | null = null;
  paymentPrice = 0;
  testName = '';
  TestGuid: string | null = null;
  paymentMethod = 'card';
  cardNumber = '';
  expiry = '';
  cvv = '';
  upiId = '';

  constructor(
    private syllabusDataService: SyllabusDataService,
    private route: ActivatedRoute,
    private http: HttpClient
  ) {
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      const testId = params.get('testid');
      const id = params.get('id');
      this.syllabusid = id ? parseInt(id, 10) : null;
      this.loadSyllabus(testId);
      this.loadTestPaymentDetails(this.syllabusid);
    });
  }
private loadSyllabus(testId: string | null) {

  if (!testId) {
    this.syllabusdata = [];
    return;
  }

  this.isLoading = true;

  this.syllabusDataService.getSyllabusForTest(testId)
    .subscribe({
      next: (res: Syllabus[]) => {
        console.log(res);
        this.syllabusdata = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
}

  private loadTestPaymentDetails(testId: number | null): void {
    if (!testId) {
      this.currentTest = null;
      this.TestGuid = null;
      this.paymentPrice = 0;
      this.testName = '';
      return;
    }

    this.http.get<TestItem[]>('/api/Tests').subscribe({
      next: (tests) => {
        const matchedTest = tests.find(item => item.id === testId) ?? null;
        this.currentTest = matchedTest;
        this.TestGuid = matchedTest?.testGuid || null;
        this.paymentPrice = matchedTest?.isPaid ? matchedTest.price : 0;
        this.testName = matchedTest?.testName || '';
      },
      error: () => {
        this.currentTest = null;
        this.TestGuid = null;
        this.paymentPrice = 0;
        this.testName = '';
      }
    });
  }

  openPaymentModal(syllabus: any): void {
    this.selectedSyllabus = syllabus;
    this.paymentPrice = this.currentTest?.isPaid ? this.currentTest.price : 0;
    this.testName = this.currentTest?.testName || '';
    this.TestGuid = this.currentTest?.testGuid || null;
    this.paymentMethod = 'card';
    this.cardNumber = '';
    this.expiry = '';
    this.cvv = '';
    this.upiId = '';
    this.paymentModalOpen = true;
  }

  closePaymentModal(): void {
    this.paymentModalOpen = false;
  }

  confirmPayment(): void {
    if (!this.TestGuid || !this.selectedSyllabus) {
      alert('Unable to submit payment because the test or syllabus details are missing.');
      return;
    }

    // Convert expiry MM/YY to ISO date (end of month). If input not in MM/YY, attempt Date parse.
    let expiryDate: string | null = null;
    if (this.expiry) {
      const mmYY = this.expiry.replace(/\s+/g, '');
      const mmYYMatch = mmYY.match(/^(\d{2})\/?(\d{2,4})$/);
      if (mmYYMatch) {
        let month = parseInt(mmYYMatch[1], 10);
        let year = parseInt(mmYYMatch[2], 10);
        if (year < 100) year += year >= 70 ? 1900 : 2000; // two-digit year heuristic
        // last day of month
        const lastDay = new Date(Date.UTC(year, month, 0, 0, 0, 0));
        expiryDate = lastDay.toISOString();
      } else {
        // Try direct parse
        const parsed = Date.parse(this.expiry);
        if (!isNaN(parsed)) expiryDate = new Date(parsed).toISOString();
      }
    }

    const syllabusDto = {
      syllGuid: this.selectedSyllabus?.syllabusGuid || null,
      syllabusID: String(this.selectedSyllabus?.syllabusID ?? ''),
      syllabusName: this.selectedSyllabus?.syllabusName ?? '',
      totalChapters: this.selectedSyllabus?.totalChapters ?? 0,
      totalQuestions: this.selectedSyllabus?.totalQuestions ?? 0
    };

    const paymentPayload = {
      testGuid: this.TestGuid,               // camelCase is fine (server is case-insensitive)
      testName: this.testName,
      paymentPrice: Number(this.paymentPrice),
      paymentMethod: this.paymentMethod,
      cardNumber: this.cardNumber || '',
      cvv: this.cvv || '',
      expiry: expiryDate,                    // ISO string or null
      upiId: this.upiId || '',
      syllabus: [ syllabusDto ]
    };

    console.log('Payment payload:', paymentPayload);

    this.syllabusDataService.SubmitTestPayment(paymentPayload)
      .subscribe({
        next: (response) => {
          console.log('Payment successful:', response);
          this.paymentModalOpen = false;
        },
        error: (error) => {
          console.error('Payment failed:', error);
          alert('Payment failed. Please try again.');
        }
      });
  }

}

