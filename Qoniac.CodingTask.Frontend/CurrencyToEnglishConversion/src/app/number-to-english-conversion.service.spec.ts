import { TestBed } from '@angular/core/testing';

import { NumberToEnglishConversionService } from './number-to-english-conversion.service';

describe('NumberToEnglishConversionService', () => {
  let service: NumberToEnglishConversionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NumberToEnglishConversionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
