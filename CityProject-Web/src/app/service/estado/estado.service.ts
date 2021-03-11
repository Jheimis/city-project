import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Estado } from 'src/app/models/Estado';

@Injectable({
  providedIn: 'root'
})
export class EstadoService {
  baseUrl = `${environment.baseUrl}/api/estado`;

  constructor(private http : HttpClient) {}
  
  getAll(): Observable<Estado[]>{
    return this.http.get<Estado[]>(`${this.baseUrl}`);
  }
  
  getById(id: number): Observable<Estado>{
    return this.http.get<Estado>(`${this.baseUrl}/${id}`);  
  }

  put(estado: Estado){
    return this.http.put(`${this.baseUrl}/${estado.id}`, estado);
  }
}
