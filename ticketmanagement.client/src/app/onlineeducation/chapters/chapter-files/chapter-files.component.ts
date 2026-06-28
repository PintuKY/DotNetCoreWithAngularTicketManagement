import { Component, OnInit } from '@angular/core';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-chapter-files',
  templateUrl: './chapter-files.component.html',
  styleUrls: ['./chapter-files.component.css']
})
export class ChapterFilesComponent implements OnInit {

  
constructor(private route: ActivatedRoute,private syllabusService: SyllabusDataService) {}

chapters: any[] = [];
syllabusGuid: string | null = null;
syllabusID: number | null = null;

  ngOnInit(): void {
      this.route.queryParams.subscribe(params => {
      this.syllabusGuid = params['id'];
      this.syllabusID = params['SyID'];
      console.log("SyllabusGuid",this.syllabusGuid);
      console.log("SyllabusID",this.syllabusID);
      if (this.syllabusGuid) {
        this.loadChapters(this.syllabusGuid);
      } else {
        console.warn('No syllabus id in query params');
      }
  });
  }

  loadChapters(syllabusGuid: string | null)
  {
    if (!syllabusGuid) {
      console.warn('loadChapters called with empty syllabusGuid');
      this.chapters = [];
      return;
    }
    this.syllabusService.getChapterForSyllabusTest(syllabusGuid).subscribe((res: any) => {
      console.log('syllabus response', res);
      // API may return either an array of chapters or an object with a `chapters` property.
      if (Array.isArray(res)) {
        this.chapters = res;
        console.log('Chapters (array)', this.chapters);
      } else if (res && res.chapters && Array.isArray(res.chapters)) {
        this.chapters = res.chapters;
        console.log('Chapters (from object)', this.chapters);
      } else {
        console.warn('Unexpected chapters payload for syllabusGuid', syllabusGuid, res);
        this.chapters = [];
      }
    }, err => {
      console.error('Failed to load chapters', err);
      this.chapters = [];
    });
  }
}
