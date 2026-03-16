import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BodybannersFileComponent } from './bodybanners-file.component';

describe('BodybannersFileComponent', () => {
  let component: BodybannersFileComponent;
  let fixture: ComponentFixture<BodybannersFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BodybannersFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BodybannersFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
