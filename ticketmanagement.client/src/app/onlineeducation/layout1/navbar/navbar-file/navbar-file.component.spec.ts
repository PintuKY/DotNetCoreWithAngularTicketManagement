import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarFileComponent } from './navbar-file.component';

describe('NavbarFileComponent', () => {
  let component: NavbarFileComponent;
  let fixture: ComponentFixture<NavbarFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NavbarFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
