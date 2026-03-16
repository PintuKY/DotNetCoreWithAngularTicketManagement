import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BannerFileComponent } from './banner-file.component';

describe('BannerFileComponent', () => {
  let component: BannerFileComponent;
  let fixture: ComponentFixture<BannerFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BannerFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BannerFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
