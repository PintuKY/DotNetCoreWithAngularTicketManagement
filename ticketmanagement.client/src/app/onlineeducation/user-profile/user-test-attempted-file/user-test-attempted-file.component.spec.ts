import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTestAttemptedFileComponent } from './user-test-attempted-file.component';

describe('UserTestAttemptedFileComponent', () => {
  let component: UserTestAttemptedFileComponent;
  let fixture: ComponentFixture<UserTestAttemptedFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserTestAttemptedFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserTestAttemptedFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
