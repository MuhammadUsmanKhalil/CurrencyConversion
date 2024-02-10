import { CUSTOM_ELEMENTS_SCHEMA, Component, Input, OnInit } from '@angular/core';

import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ConversionResult, CurrencyConverterHttpService } from '../number-to-english-conversion.service';
import { Observable } from 'rxjs';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'english-words',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './english-words.component.html',
  styleUrl: './english-words.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EnglishWordsComponent implements OnInit {

   dataSource: ConversionResult | any;
  
  constructor(private currencyConverterService : CurrencyConverterHttpService )
  {

  }

  @Input('dataSource')
  set dataSourceForWords(value: ConversionResult) {   
      this.dataSource = value;
  }

  ngOnInit(): void {
    this.dataSource = "";
  }  
}
