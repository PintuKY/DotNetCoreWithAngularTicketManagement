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

chapters:any[]=[];
syllabusId:any;

  ngOnInit(): void {
      this.route.queryParams.subscribe(params => {
      this.syllabusId = params['id'];
      console.log("SyllabusID",this.syllabusId);
      this.loadChapters();
  });
  }

  loadChapters()
  {
    this.syllabusService.getSyllabusData().subscribe(res => {
    const syllabus = res.find((x:any)=>x.syllabusGuid == this.syllabusId);
    console.log("syllabus",syllabus);
    if(syllabus)
    {
        this.chapters = syllabus.chapters;
        //console.log("syllabus",this.chapters);
    }
  });
  }
}
