import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCourseFileComponent } from './user-course-file.component';

describe('UserCourseFileComponent', () => {
  let component: UserCourseFileComponent;
  let fixture: ComponentFixture<UserCourseFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserCourseFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserCourseFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
