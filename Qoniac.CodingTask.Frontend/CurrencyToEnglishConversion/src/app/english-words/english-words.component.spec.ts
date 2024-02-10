import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnglishWordsComponent } from './english-words.component';

describe('EnglishWordsComponent', () => {
  let component: EnglishWordsComponent;
  let fixture: ComponentFixture<EnglishWordsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnglishWordsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EnglishWordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
