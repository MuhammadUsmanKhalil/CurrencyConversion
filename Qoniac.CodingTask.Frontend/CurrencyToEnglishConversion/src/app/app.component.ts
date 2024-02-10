import { Component, OnDestroy, OnInit } from '@angular/core';  
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ConversionResult, CurrencyConverterHttpService } from './number-to-english-conversion.service';
import { Subscription } from 'rxjs';
  
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],  
})
export class AppComponent implements OnInit, OnDestroy {
  
  dataSource: ConversionResult | any;
  isLoading: boolean = false;  
   
  errorMessage : string = "";

  form: FormGroup = new FormGroup({});
  private static subscriptions: Subscription[] = [];

  constructor(private formBuilder: FormBuilder, private currencyConverterService : CurrencyConverterHttpService ) {
      
  }
  
  ngOnInit(): void {

    this.errorMessage = "input amount must be in currency format(e.g. 4545,15  or 45789. Supported dollar range (0-999 999 999 ) and cents range (0-99))";
    this.form = this.formBuilder.group({
      currency: ['', [Validators.pattern(/^\d+(,(0?\d|[0-9][0-9]))?$/)]],
    })
  }  

  public clearContents()
  {
    this.form.controls['currency'].setValue('');
    this.dataSource = { data : '', exception: null, success : true };
  }
   
  public submit()
  {
    this.isLoading = true;
    var currencyNumber = this.form.controls['currency'].value;
    var currencyHttpSubscription= this.currencyConverterService.getConvertedEnglishWords(currencyNumber)
                                 .subscribe(payload =>{
                                    setTimeout( ()=> {
                                      this.dataSource = payload;
                                      this.isLoading = false; 
                                    }, 500);                                    
                                 },
                                 (error) => {
                                      this.isLoading = false;

                                      const formControl = this.form.controls['currency'];
                                      
                                      if (formControl) {

                                        formControl.setErrors({
                                        serverError: error.error
                                      });

                                      this.dataSource = { data : '', exception: null, success : true };
                                    }
                                 });

    AppComponent.subscriptions.push(currencyHttpSubscription);                                          
  }

  ngOnDestroy(): void {

    AppComponent.subscriptions.forEach(subscription => subscription ? subscription.unsubscribe() : 0);
    AppComponent.subscriptions = [];

  }
}