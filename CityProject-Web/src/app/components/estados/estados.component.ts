import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Estado } from 'src/app/models/Estado';
import { EstadoService } from 'src/app/service/estado/estado.service';



@Component({
  selector: 'app-estados',
  templateUrl: './estados.component.html',
  styleUrls: ['./estados.component.css']
})
export class EstadosComponent implements OnInit {

  modalRef: BsModalRef;
  public titulo = 'Estados';
  public estadoSelecionado : Estado;

  public textSimple: string;

  public estados: Estado[];

   openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
  constructor(private fb: FormBuilder, private modalService: BsModalService, 
    private estadoService: EstadoService) { 
  
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

  public estadoSelect(estado: Estado){
    this.estadoSelecionado = estado;
  
  }

  public voltar(){
    this.estadoSelecionado = null;
  }


}
