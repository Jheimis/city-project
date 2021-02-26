import { Cidade } from './../../models/Cidade';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-tabelaCidade',
  templateUrl: './cidadesEstado.component.html',
  styleUrls: ['./cidadesEstado.component.css']
})
export class CidadesEstadoComponent implements OnInit {

  @Input('cidade') cidades: string;
  constructor() { }

  ngOnInit() {
  }

}
