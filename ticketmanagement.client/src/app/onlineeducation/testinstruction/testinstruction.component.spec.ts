import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestinstructionComponent } from './testinstruction.component';

describe('TestinstructionComponent', () => {
  let component: TestinstructionComponent;
  let fixture: ComponentFixture<TestinstructionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TestinstructionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TestinstructionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
