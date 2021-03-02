import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Cidade } from 'src/app/models/Cidade';
import { Estado } from 'src/app/models/Estado';
import { CidadeService } from 'src/app/service/cidade/cidade.service';
import { Dolar } from 'src/app/service/dolar';
import { DolarService } from 'src/app/service/dolar.service';
import { EstadoService } from 'src/app/service/estado/estado.service';

@Component({
  selector: 'tabelaCidades',
  templateUrl: './tabela-cidades.component.html',
  styleUrls: ['./tabela-cidades.component.css']
})
export class TabelaCidadesComponent implements OnInit {

  public titulo = 'Cidades';
  public modo: string;

  public dolarHoje = new Dolar;
  public cidadeDolar: FormGroup;

  public cidades: Cidade[];
  public cidade : Cidade;
  public estados: Estado[];

  
  constructor(
    private fb: FormBuilder, 
    private cidadeService: CidadeService, 
    private estadosService: EstadoService, 
    private dolarService: DolarService,
    private toastr: ToastrService ) { 
  }
    
  ngOnInit(): void {
    this.carregarCidades();
    this.setDolar();
  }

  carregarCidades(){
    this.cidadeService.getAll().subscribe(
      (cidades: Cidade[]) => {
        this.cidades = cidades;
      },
      (erro: any) => {
        console.error(erro);
      }
      
    );

    this.estadosService.getAll().subscribe(
      (estados: Estado[]) => {
        this.estados = estados;
      },
      (erro: any) => {
        console.error(erro);
      }
      
    );
  }

  convertDolarCidade(cidade: Cidade) {

    if (cidade.custoCidadeUS && this.dolarHoje?.USD) {

      return cidade.custoCidadeUS * (this.dolarHoje.USD.ask || 1);

    }
    return 0
  }

  private setDolar() {
    this.dolarService.getDolar()
      .then(res => {
        this.dolarHoje = res as Dolar

      })
      .catch(err => console.log(err))
  }
  
}


