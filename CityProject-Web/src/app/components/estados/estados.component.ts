import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Cidade } from 'src/app/models/Cidade';
import { Estado } from 'src/app/models/Estado';
import { CidadeService } from 'src/app/service/cidade/cidade.service';
import { EstadoService } from 'src/app/service/estado/estado.service';



@Component({
  selector: 'app-estados',
  templateUrl: './estados.component.html',
  styleUrls: ['./estados.component.css']
})
export class EstadosComponent implements OnInit {

  modalRef: BsModalRef;
  @ViewChild('consultaCidade')consultaCidade;

  public titulo = 'Estados';
  public tituloCidades = 'Cidades';
  public estadoSelecionado : Estado;

  public estados: Estado[];
  public estado: Estado;
  public cidades: Cidade[];
  
   openConsultModal(estado) {
    this.estadoSelecionado = estado;

    this.modalRef = this.modalService.show(this.consultaCidade);

    this.cidadesService.getCidadeByEstadoId(this.estadoSelecionado.id).subscribe(
      (cidades: Cidade[]) => {
        this.cidades = cidades;
      },
      (erro: any) => {
        console.error(erro);
      }
    );
  }
  constructor(private fb: FormBuilder, private modalService: BsModalService, 
    private estadoService: EstadoService, private cidadesService: CidadeService ) { 

  }

  ngOnInit(): void {
    this.carregarEstados();
  }

  carregarEstados(){
    this.estadoService.getAll().subscribe(
      (estados: Estado[]) => {
        this.estados = estados;
      },
      (erro: any) => {
        console.error(erro);
      }
    );
  }


  public voltar(){
    this.estadoSelecionado = null;
    console.log(this.estadoSelecionado);
    this.modalRef.hide();
  }

}
