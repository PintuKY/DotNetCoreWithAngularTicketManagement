import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylabusFileComponent } from './sylabus-file.component';

describe('SylabusFileComponent', () => {
  let component: SylabusFileComponent;
  let fixture: ComponentFixture<SylabusFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylabusFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SylabusFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
