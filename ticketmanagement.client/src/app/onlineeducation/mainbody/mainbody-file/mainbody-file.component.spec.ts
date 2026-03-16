import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainbodyFileComponent } from './mainbody-file.component';

describe('MainbodyFileComponent', () => {
  let component: MainbodyFileComponent;
  let fixture: ComponentFixture<MainbodyFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainbodyFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainbodyFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
