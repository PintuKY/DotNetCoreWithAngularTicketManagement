import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  constructor(private translate: TranslateService) 
  {
     translate.addLangs(['en', 'hi']);

    const savedLanguage = localStorage.getItem('language');

    const defaultLanguage = savedLanguage || 'en';

    translate.setDefaultLang('en');
    translate.use(defaultLanguage);
   }
    changeLanguage(language: string): void {

    this.translate.use(language);

    localStorage.setItem('language', language);
  }

  getCurrentLanguage(): string {

    return this.translate.currentLang || 'en';
  }
}
