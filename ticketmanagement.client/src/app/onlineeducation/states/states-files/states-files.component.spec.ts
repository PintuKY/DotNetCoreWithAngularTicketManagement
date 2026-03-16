import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatesFilesComponent } from './states-files.component';

describe('StatesFilesComponent', () => {
  let component: StatesFilesComponent;
  let fixture: ComponentFixture<StatesFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatesFilesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatesFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
