import { Component, OnInit } from '@angular/core';
// import { QuestionService } from '../../../services/onlineeducation/question.service';
import { Question }   from '../../../model/onlineeducation/question';
import { SyllabusDataService } from 'src/app/services/onlineeducation/syllabus/syllabus-data.service';
import { ExamserviceService } from 'src/app/services/onlineeducation/examservice/examservice.service';
import { timer, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-questions-file',
  templateUrl: './questions-file.component.html',
  styleUrls: ['./questions-file.component.css']
})
export class QuestionsFileComponent implements OnInit {

isLoading: boolean = false;
questions: Question[] = [];
questionStatus: string[] = [];
currentIndex = 0;
answers: any = {};
attempted = 0;
marked = 0;
notvisited = 0;
not_answared = 0;
chapterGuId: any;
syllabusID: number | null = null;
warningMessage: string = '';
isWarning: boolean = false;
chapterid:any;
confirmSubmitOpen = false;
reportModalOpen = false;
reportMessage = '';
readonly reportPlaceholder = 'Please describe the issue or comment you want to send to the exam team.';
private readonly MAIN_TIME = 60;
private countdownTime = this.MAIN_TIME;
displayTime = '01:00';
private timerSubscription: Subscription | null = null;

// Timing tracking properties
private startDateTime: Date | null = null;
private endDateTime: Date | null = null;
private questionTimings: { [questionId: number]: { startTime: Date; endTime: Date | null; timeSpentSeconds: number } } = {};
private currentQuestionStartTime: Date | null = null;

constructor(
  private route: ActivatedRoute,
  private router: Router,
  private syllabusService: SyllabusDataService,
  private examservice:ExamserviceService
) {}

ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      this.chapterGuId = params['id'] || params['chapterId'] || null;
      const syParam = params['syllabusID'] || params['SyID'] || params['SyId'] || null;
      this.syllabusID = syParam != null ? Number(syParam) : null;
      console.log('Query params', params);
      console.log('Resolved chapter GUID', this.chapterGuId);
      console.log('Resolved syllabusID', this.syllabusID);
      this.loadQuestions(this.chapterGuId);
    });

  }

  private normalizeQuestionResponse(response: any): any[] {
    if (!response) {
      return [];
    }

    if (Array.isArray(response)) {
      return response;
    }

    if (response.data) {
      return Array.isArray(response.data) ? response.data : [response.data];
    }

    if (response.result) {
      return Array.isArray(response.result) ? response.result : [response.result];
    }

    return [response];
  }

  loadQuestions(chapterGuid: string | null) {
    if (!chapterGuid) {
      console.warn('No chapter GUID provided in query params.');
      this.questions = [];
      this.isLoading = false;
      return;
    }

    this.isLoading = true;
    this.startDateTime = new Date();

    this.syllabusService.getQuestionDataByChapterGuid(chapterGuid).subscribe(
      res => {
        this.isLoading = false;

        const questions = this.normalizeQuestionResponse(res);

        console.log('Question API URL:', this.syllabusService.getLastRequestUrl ? this.syllabusService.getLastRequestUrl() : 'unknown');
        console.log('Question response payload', res);
        console.log('Normalized questions array', questions);

        if (Array.isArray(questions) && questions.length) {
          this.questions = questions;
          this.chapterid = questions[0].chapterID ?? null;
          this.currentIndex = 0;
          this.notvisited = this.questions.length;
          this.marked = 0;
          this.not_answared = this.questions.length;
          this.questionStatus = new Array(this.questions.length).fill('notvisited');

          this.questions.forEach(q => {
            this.questionTimings[q.id] = {
              startTime: new Date(),
              endTime: null,
              timeSpentSeconds: 0
            };
          });

          this.currentQuestionStartTime = new Date();
          this.startTimer();
        } else {
          console.warn('No questions returned for chapter GUID', chapterGuid, res);
          this.questions = [];
          this.questionStatus = [];
          this.notvisited = 0;
          this.marked = 0;
          this.not_answared = 0;
        }
      },
      err => {
        this.isLoading = false;
        console.error('Failed to load chapter questions', err);
      }
    );
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
  // Record time spent on current question
  this.recordQuestionTime();
  this.currentIndex--;
  // Start timing for new question
  this.currentQuestionStartTime = new Date();
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
  // Record time spent on current question
  this.recordQuestionTime();
  this.currentIndex++;
  // Start timing for new question
  this.currentQuestionStartTime = new Date();
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

  // Record time spent on current question before moving
  this.recordQuestionTime();
  this.currentIndex = index;
  // Start timing for new question
  this.currentQuestionStartTime = new Date();

  this.clearTimer();

}

submit(): void {
  this.openSubmitConfirm();
}

submitTest(): void {
  this.stopSubscription();
  
  // Record time spent on the last question
  this.recordQuestionTime();
  
  // Set end time
  this.endDateTime = new Date();
  
  // Build comprehensive payload with timing data
  const payload = this.buildTestSubmissionPayload();
  
  console.log("payload result:", payload);
  this.examservice.submitTest(payload).subscribe(
    res => {
      console.log("Result", res);
      const result = res;
      console.log("Response result:", result);
      this.router.navigate(['/userprofile']);
    },
    err => {
      console.error('Submit error', err);
      // still navigate or show error if needed
      //this.router.navigate(['/User-performance-reports']);
    }
  );
}

openSubmitConfirm(): void {
  this.confirmSubmitOpen = true;
}

closeSubmitConfirm(): void {
  this.confirmSubmitOpen = false;
}

confirmSubmit(): void {
  this.closeSubmitConfirm();
  this.submitTest();
}

openReportModal(): void {
  this.reportMessage = '';
  this.reportModalOpen = true;
}

closeReportModal(): void {
  this.reportModalOpen = false;
}

sendReport(): void {
  console.log('Report sent:', this.reportMessage);
  // TODO: connect to API or service to save the report
  this.closeReportModal();
}

startTimer(){

this.stopSubscription();

this.countdownTime = this.MAIN_TIME;

this.timerSubscription = timer(0,1000).subscribe(()=>{

if(this.countdownTime > 0)
{

   this.countdownTime--;

   this.displayTime = this.formatTime(this.countdownTime);

   if(this.countdownTime <= 5)
   {
      this.warningMessage = "⚠ Hurry up! Time almost over";
      this.isWarning = true;      
   }
   else if(this.countdownTime == 1)
   {
    this.stopSubscription();
   }

}
else
{
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

if(this.currentIndex < this.questions.length + 1){

   this.currentIndex++;
   this.clearTimer();

}else{

   console.log("Timer auto Next Question Submit Function");
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

// Record time spent on the current question
private recordQuestionTime(): void {
  if (this.currentIndex >= 0 && this.currentIndex < this.questions.length && this.currentQuestionStartTime) {
    const currentQuestion = this.questions[this.currentIndex];
    const endTime = new Date();
    const timeSpentMs = endTime.getTime() - this.currentQuestionStartTime.getTime();
    const timeSpentSeconds = Math.floor(timeSpentMs / 1000);
    
    if (this.questionTimings[currentQuestion.id]) {
      this.questionTimings[currentQuestion.id].endTime = endTime;
      this.questionTimings[currentQuestion.id].timeSpentSeconds = timeSpentSeconds;
    }
  }
}

// Get option character (A, B, C, D) based on index
getOptionChar(index: number): string {
  return String.fromCharCode(65 + index); // 65 is ASCII for 'A'
}

// Get option character based on optionId
getOptionCharByOptionId(optionId: number): string {
  if (!this.questions[this.currentIndex] || !this.questions[this.currentIndex].options) {
    return '';
  }
  const index = this.questions[this.currentIndex].options.findIndex(opt => opt.optionId === optionId);
  return index >= 0 ? this.getOptionChar(index) : '';
}

// Build comprehensive payload with all timing and question data
private buildTestSubmissionPayload(): any {
  const questionMetadata: any[] = [];
  const answersWithCharacters: any = {};
  
  // Build detailed data for each question
  this.questions.forEach((question, index) => {
    const timing = this.questionTimings[question.id];
    const selectedOptionId = this.answers[question.id];
    const isAnswered = selectedOptionId != null;
    const isSkipped = this.questionStatus[index] === 'notvisited';
    const isAttempted = this.questionStatus[index] === 'attempted';
    
    // Convert optionId to option character (A, B, C, D)
    let selectedOptionChar = null;
    if (isAnswered && question.options) {
      const selectedIndex = question.options.findIndex(opt => opt.optionId === selectedOptionId);
      if (selectedIndex >= 0) {
        selectedOptionChar = String.fromCharCode(65 + selectedIndex); // A, B, C, D
      }
    }
    
    answersWithCharacters[question.id] = selectedOptionChar;
    
    questionMetadata.push({
      questionId: question.id,
      questionText: question.questionText || '',
      selectedAnswer: selectedOptionChar || null,  // Now sends character like A, B, C, D
      selectedOptionId: selectedOptionId || null,  // Keep original optionId for reference
      isAnswered: isAnswered,
      isSkipped: isSkipped,
      isAttempted: isAttempted,
      timeSpentSeconds: timing ? timing.timeSpentSeconds : 0,
      startedAt: timing ? timing.startTime.toISOString() : null,
      endedAt: timing ? (timing.endTime ? timing.endTime.toISOString() : null) : null, 
    });
  });
  
  // Calculate total time spent
  const totalTimeSpentMs = this.endDateTime && this.startDateTime 
    ? this.endDateTime.getTime() - this.startDateTime.getTime() 
    : 0;
  const totalTimeSpentSeconds = Math.floor(totalTimeSpentMs / 1000);
  
  return {
    chapterid: this.chapterid,
    syllabusID: this.syllabusID,
    answers: answersWithCharacters,  // Send character answers (A, B, C, D)
    answersWithOptionIds: this.answers,  // Keep original optionIds for reference
    startDateTime: this.startDateTime ? this.startDateTime.toISOString() : null,
    endDateTime: this.endDateTime ? this.endDateTime.toISOString() : null,
    totalTimeSpentSeconds: totalTimeSpentSeconds,
    questionsAttempted: this.marked,
    questionsSkipped: this.notvisited - this.marked,
    questionsAnswered: this.marked,
    questionsNotAnswered: this.not_answared,
    questionMetadata: questionMetadata,
    // Summary statistics
    summary: {
      totalQuestions: this.questions.length,
      answered: this.marked,
      attempted: this.attempted,
      skipped: this.notvisited - this.marked,
      notAnswered: this.not_answared
    }
  };
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
