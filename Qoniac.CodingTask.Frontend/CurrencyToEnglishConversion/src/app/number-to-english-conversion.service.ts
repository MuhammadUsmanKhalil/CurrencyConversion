import { HttpClient, HttpHeaders } from  '@angular/common/http';
import { Injectable } from  '@angular/core';
import { environment } from 'src/environments/environment';

export class ConversionResult {
    data: string =  "";
    exception : string = "";
    success : boolean = false   
}


@Injectable({
providedIn:  'root'
})

export class CurrencyConverterHttpService {

private converToWordsUrl = environment.apiPath.concat('convert-to-words/');

constructor(private http: HttpClient) {

  console.log(environment.apiPath);
}

  getConvertedEnglishWords(currencyNumber : string) {
        
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });        
    headers.append("Access-Control-Allow-Origin", "*");

    headers.append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    headers.append('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    const options = { headers };

    return this.http.get(this.converToWordsUrl + '?currencyValue' + '=' + currencyNumber, options);
  }
}