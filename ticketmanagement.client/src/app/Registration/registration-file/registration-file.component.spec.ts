import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationFileComponent } from './registration-file.component';

describe('RegistrationFileComponent', () => {
  let component: RegistrationFileComponent;
  let fixture: ComponentFixture<RegistrationFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegistrationFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrationFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
