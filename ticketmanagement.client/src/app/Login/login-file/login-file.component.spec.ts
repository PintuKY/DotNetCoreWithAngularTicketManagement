import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginFileComponent } from './login-file.component';

describe('LoginFileComponent', () => {
  let component: LoginFileComponent;
  let fixture: ComponentFixture<LoginFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
