import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FootersFilesComponent } from './footers-files.component';

describe('FootersFilesComponent', () => {
  let component: FootersFilesComponent;
  let fixture: ComponentFixture<FootersFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FootersFilesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FootersFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
