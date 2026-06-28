import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-bodybanners-file',
  templateUrl: './bodybanners-file.component.html',
  styleUrls: ['./bodybanners-file.component.css']
})
export class BodybannersFileComponent implements OnInit, OnDestroy {
  activeSlide = 0;
  private slideTimer?: number;
  private readonly slideCount = 3;

  ngOnInit(): void {
    this.startAutoSlide();
  }

  ngOnDestroy(): void {
    this.stopAutoSlide();
  }

  setSlide(index: number): void {
    this.activeSlide = index;
    this.resetAutoSlide();
  }

  prevSlide(): void {
    this.activeSlide = (this.activeSlide + this.slideCount - 1) % this.slideCount;
    this.resetAutoSlide();
  }

  nextSlide(): void {
    this.activeSlide = (this.activeSlide + 1) % this.slideCount;
    this.resetAutoSlide();
  }

  private startAutoSlide(): void {
    this.slideTimer = window.setInterval(() => {
      this.nextSlide();
    }, 6000);
  }

  private stopAutoSlide(): void {
    if (this.slideTimer !== undefined) {
      window.clearInterval(this.slideTimer);
      this.slideTimer = undefined;
    }
  }

  private resetAutoSlide(): void {
    this.stopAutoSlide();
    this.startAutoSlide();
  }
}
