import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterFileComponent } from './footer-file.component';

describe('FooterFileComponent', () => {
  let component: FooterFileComponent;
  let fixture: ComponentFixture<FooterFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FooterFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FooterFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
