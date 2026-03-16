import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChapterFilesComponent } from './chapter-files.component';

describe('ChapterFilesComponent', () => {
  let component: ChapterFilesComponent;
  let fixture: ComponentFixture<ChapterFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChapterFilesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChapterFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
