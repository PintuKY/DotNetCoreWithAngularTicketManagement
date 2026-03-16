import { Component, OnInit } from '@angular/core';
import { Syllabus } from 'src/app/model/onlineeducation/syllabus.model';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
@Component({
  selector: 'app-sylabus-file',
  templateUrl: './sylabus-file.component.html',
  styleUrls: ['./sylabus-file.component.css']
})
export class SylabusFileComponent implements OnInit {
syllabusdata:Syllabus[]=[];
  constructor(private syllabusDataService:SyllabusDataService)
  {

  }

  ngOnInit(): void {
    this.loadSyllabus();
  }

loadSyllabus() 
{
    this.syllabusDataService.getSyllabusData().subscribe(res => {
      this.syllabusdata = res;       
      console.log("SyllabusData",this.syllabusdata);
    });
}
}
