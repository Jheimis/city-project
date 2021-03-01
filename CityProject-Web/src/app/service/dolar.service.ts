import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Dolar } from './dolar';

@Injectable({
  providedIn: 'root'
})
export class DolarService {

  dolar: Dolar | undefined

  constructor(private http: HttpClient) {
    this.getDolar()
      .then(res => {
        this.dolar = res as Dolar
        console.log(this.dolar);
        
      })
      .catch(err => console.log(err))
  }

  getDolar() {
    return this.http.get(environment.apiDolar).toPromise()
  }
}
