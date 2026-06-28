import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Syllabus } from 'src/app/model/onlineeducation/syllabus.model';
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
  constructor(private syllabusDataService: SyllabusDataService, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      const testId = params.get('testid');
      const id = params.get('id');
      this.syllabusid = id ? parseInt(id, 10) : null;
      this.loadSyllabus(testId);
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

}

