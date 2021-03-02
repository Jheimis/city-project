import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Cidade } from 'src/app/models/Cidade';
import { Estado } from 'src/app/models/Estado';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  public tituloEstado = 'Estados';
  public tituloCidade = 'Cidade';

  public cidadeForm: FormGroup;
  public estados: Estado[];
  public cidades: Cidade[];
  public cidadeSubmit(){
    
  }

  constructor() { }

  ngOnInit(): void {
  }

}
