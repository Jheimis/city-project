import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cidade } from 'src/app/models/Cidade';

@Injectable({
  providedIn: 'root'
})
export class CidadeService {

  baseUrl = `${environment.baseUrl}/api/cidade`;

  constructor(private http : HttpClient) {}
  
  getAll(): Observable<Cidade[]>{
    return this.http.get<Cidade[]>(`${this.baseUrl}`);
  }
  
  getById(id: number): Observable<Cidade>{
    return this.http.get<Cidade>(`${this.baseUrl}/${id}`);
  }

  getCidadeByEstadoId(id: number): Observable<Cidade[]>{
    return this.http.get<Cidade[]>(`${this.baseUrl}/ByEstado/${id}`);
  }

  post(cidade: Cidade){
    return this.http.post(`${this.baseUrl}`, cidade);
  }

  put(cidade: Cidade){
    return this.http.put(`${this.baseUrl}/${cidade.id}`, cidade);
  }

  delete(id: number ){
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
  
}
