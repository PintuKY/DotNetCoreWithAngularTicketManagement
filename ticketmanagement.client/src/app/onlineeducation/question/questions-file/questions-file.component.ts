import { Component, OnInit } from '@angular/core';
// import { QuestionService } from '../../../services/onlineeducation/question.service';
import { Question }   from '../../../model/onlineeducation/question';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
import { ExamserviceService } from 'src/app/services/onlineeducation/examservice/examservice.service';
import { timer, Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-questions-file',
  templateUrl: './questions-file.component.html',
  styleUrls: ['./questions-file.component.css']
})
export class QuestionsFileComponent implements OnInit {

questions: Question[] = [];
questionStatus: string[] = [];
currentIndex = 0;
answers: any = {};
attempted = 0;
marked = 0;
notvisited = 0;
not_answared = 0;
chapterGuId: any;
warningMessage: string = '';
isWarning: boolean = false;
chapterid:any;
private readonly MAIN_TIME = 60;
private countdownTime = this.MAIN_TIME;
displayTime = '01:00';
private timerSubscription: Subscription | null = null;

constructor(
  private route: ActivatedRoute,
  private syllabusService: SyllabusDataService,
  private examservice:ExamserviceService
) {}

ngOnInit(): void {

  this.route.queryParams.subscribe(params => {
    this.chapterGuId = params['chapterId'];
    this.loadQuestions();
  });

}

loadQuestions() {

this.syllabusService.getSyllabusData().subscribe(res => {

const chapter = res
  .flatMap((s:any)=>s.chapters)
  .find((c:any)=>c.chapterGuid == this.chapterGuId);

if(chapter){

  this.questions = chapter.questions;
  this.chapterid = chapter.chapterId;
  console.log("chapterID",this.chapterid);
  this.notvisited = this.questions.length;

  this.questionStatus = new Array(this.questions.length).fill('notvisited');

  this.startTimer();
}

});

}

selectOption(questionId:number, optionId:number){

this.answers[questionId] = optionId;

this.updateCounts();

}

updateCounts(){

this.marked = Object.keys(this.answers)
  .filter(k => this.answers[k] != null).length;

this.not_answared = this.questions.length - this.marked;

}

previous(){

if(this.currentIndex > 0){
  this.currentIndex--;
}

this.clearTimer();

}

next(){

const currentQuestion = this.questions[this.currentIndex];
const selectedOption = this.answers[currentQuestion.id];

if(selectedOption){
  this.questionStatus[this.currentIndex] = 'answered';
}else{
  this.questionStatus[this.currentIndex] = 'attempted';
}

if(this.currentIndex < this.questions.length - 1){
  this.currentIndex++;
}

this.updateCounts();

this.clearTimer();

}

clear(){

const qId = this.questions[this.currentIndex].id;

this.answers[qId] = null;

this.questionStatus[this.currentIndex] = 'attempted';

this.updateCounts();

}

Questionnext(index:number){

this.currentIndex = index;

this.clearTimer();

}

submit()
{
      const payload = {
        chapterid: this.chapterid,
        answers: this.answers
      };
      console.log("payload result:", payload);
      this.examservice.submitTest(payload).subscribe(res=>{
        console.log("Result",res);
        const result = res;
        console.log("Response result:", result);
      });
      //console.log("All Answers:", this.answers);
}

startTimer(){

this.stopSubscription();

this.countdownTime = this.MAIN_TIME;

this.timerSubscription = timer(0,1000).subscribe(()=>{

if(this.countdownTime > 0){

   this.countdownTime--;

   this.displayTime = this.formatTime(this.countdownTime);

   if(this.countdownTime <= 5){
      this.warningMessage = "⚠ Hurry up! Time almost over";
      this.isWarning = true;
   }

}else{

   this.autoNextQuestion();

}

});

}

// startTimer(){

// this.stopSubscription();
// this.countdownTime = this.MAIN_TIME;
// this.timerSubscription = timer(0,1000).subscribe(()=>{
// if(this.countdownTime > 0){
//   this.countdownTime--;
//   this.displayTime = this.formatTime(this.countdownTime);
// }else{

//    this.autoNextQuestion();
// }
// });
// }

autoNextQuestion(){

const currentQuestion = this.questions[this.currentIndex];
const selectedOption = this.answers[currentQuestion.id];

if(selectedOption){
   this.questionStatus[this.currentIndex] = "answered";
}else{
   this.questionStatus[this.currentIndex] = "attempted";
}

if(this.currentIndex < this.questions.length - 1){

   this.currentIndex++;
   this.clearTimer();

}else{

   console.log("Test completed");
   this.submit();

}

}

clearTimer(){

this.warningMessage = '';
this.isWarning = false;

this.startTimer();

}

stopSubscription(){

if(this.timerSubscription){
  this.timerSubscription.unsubscribe();
  this.timerSubscription = null;
}

}

formatTime(seconds:number){

const minutes = Math.floor(seconds/60);
const sec = seconds % 60;

return `${minutes.toString().padStart(2,'0')}:${sec.toString().padStart(2,'0')}`;

}

ngOnDestroy(){

this.stopSubscription();

}

}



// export class QuestionsFileComponent implements OnInit {
//   questions: Question[] = [];
//   questionStatus: string[] = [];
//   currentIndex = 0;
//   count:number=1;
//   attempted=0;
//   notvisited=0
//   marked=0;
//   not_answared=0;
//   selectedOption=0;
//   bgColor = 'white';
//   textColor = 'black';
//   answers: any = {}; // stores selected answers
//   private readonly MAIN_TIME = 60; // Initial time in seconds
//   private countdownTime = this.MAIN_TIME;
//   public displayTime: string = '01:00';
//   private timerSubscription: Subscription | null = null;
//   constructor(private route: ActivatedRoute,private syllabusService: SyllabusDataService) { }
//   ChapterId:any;
//   ngOnInit(): void 
//   {
//     this.route.queryParams.subscribe(params => {
//       this.ChapterId = params['chapterId'];
//       console.log("SyllabusID",this.ChapterId);
//       this.loadQuestions();
//     });    
//     this.startTimer();    
//   }
//   startTimer() {
    
//       const source = timer(0, 1000); // Create an observable that emits a value every 1000ms (1 second)
//       this.timerSubscription = source.subscribe(() => {
//       this.countdownTime--;
//       this.displayTime = this.formatTime(this.countdownTime);      
//     });
//   }
//   private clearTimer()
//   {
//     this.stopSubscription(); 
//     this.countdownTime = this.MAIN_TIME;// Reset variables to original state
//     this.displayTime = this.formatTime(this.MAIN_TIME); 
//     this.startTimer();
//   }
//   // Helper to purely stop the RxJS stream
//   private stopSubscription()
//   {
//     if (this.timerSubscription) {
//       this.timerSubscription.unsubscribe();
//       this.timerSubscription = null;
//     }
//   }
//   private formatTime(seconds: number): string 
//   {
//     const minutes: number = Math.floor(seconds / 60);
//     const remainingSeconds: number = seconds % 60;
//     // Add leading zeros if necessary
//     const minutesStr = `00${minutes}`.slice(-2);
//     const secondsStr = `00${remainingSeconds}`.slice(-2);
//     return `${minutesStr}:${secondsStr}`;
//   }

// loadQuestions() 
// {
//     //old code
//     // this.questionService.getQuestions().subscribe(res => {
//     //   this.questions = res;       
//     //   this.notvisited=this.questions.length;      
//     //   this.marked = this.questions.filter(q => this.answers[q.id] != null).length; 
//     //   this.not_answared = this.questions.length - this.marked;      
//     // });
//       this.syllabusService.getSyllabusData().subscribe(res => {
//       let selectedChapter:any = null;
//       res.forEach((s:any) => {
//       const chapter = s.chapters.find((c:any) => c.chapterGuid == this.ChapterId);       
//       if(chapter)
//       {
//         selectedChapter = chapter;
//       }
//       });
//       if(selectedChapter)
//       {
//         this.questions = selectedChapter.questions;
//         this.notvisited=this.questions.length;  
//         this.marked = this.questions.filter(q => this.answers[q.id] != null).length;
//         this.not_answared = this.questions.length - this.marked;   
//       }    
//   });
// }
// previous()
// {
//       const currentQuestion = this.questions[this.currentIndex];
//       console.log(currentQuestion.id);
//       if(this.currentIndex > 0)
//         {
//           this.currentIndex--;
//         }
//         this.clearTimer();      
// }
// next()
// {
    
//     const currentQuestion = this.questions[this.currentIndex];
//     const selectedOption = this.answers[currentQuestion.id];
//     console.log("Question ID:", currentQuestion.id);
//     console.log("Selected Option:",selectedOption);
//      if(currentQuestion && selectedOption)
//       {
//         this.questionStatus[this.currentIndex] = "answered";
//       }
//       else
//       {
//         this.questionStatus[this.currentIndex] = "attempted";
//       }
//       if(this.currentIndex < this.questions.length - 1)
//       {
//           this.currentIndex++;               
//           this.marked = this.questions.filter(q => this.answers[q.id] != null).length; 
//           console.log("marked Option:",this.marked); 
//       }
//       this.attempted = this.currentIndex;
//       console.log("total attempted:",this.attempted);
//     this.clearTimer();
// }
// clear()
// {
    
//     const qId = this.questions[this.currentIndex].id;
//     this.answers[qId] = null;
// }
// Questionnext(id:number)
// {
//     this.currentIndex =id;
//     console.log("Question ID",id);     
//     this.clearTimer();
// }
// submit()
// {
    
//     const currentQuestion = this.questions[this.currentIndex];
//     this.selectedOption = this.answers[currentQuestion.id];
//     //console.log("Current Question:", currentQuestion);
//     //console.log("Selected Option:", this.selectedOption);
//     console.log("All Answers:", this.answers);
//     //this.http.post("api/save", this.answers)
//   }
 
// ngOnDestroy()
// {
//   this.stopSubscription();
// }
// }
