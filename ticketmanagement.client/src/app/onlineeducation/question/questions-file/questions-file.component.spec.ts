import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionsFileComponent } from './questions-file.component';

describe('QuestionsFileComponent', () => {
  let component: QuestionsFileComponent;
  let fixture: ComponentFixture<QuestionsFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuestionsFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionsFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
