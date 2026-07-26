import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserViewTestScoreFileComponent } from './user-view-test-score-file.component';

describe('UserViewTestScoreFileComponent', () => {
  let component: UserViewTestScoreFileComponent;
  let fixture: ComponentFixture<UserViewTestScoreFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserViewTestScoreFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserViewTestScoreFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
