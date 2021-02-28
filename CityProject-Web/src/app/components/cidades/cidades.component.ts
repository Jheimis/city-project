import { EstadoService } from './../../service/estado/estado.service';
import { Estado } from './../../models/Estado';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Cidade } from 'src/app/models/Cidade';
import { CidadeService } from 'src/app/service/cidade/cidade.service';

@Component({
  selector: 'app-cidades',
  templateUrl: './cidades.component.html',
  styleUrls: ['./cidades.component.css']
})
export class CidadesComponent implements OnInit {

  deleteModalRef: BsModalRef;
  message: string;
  @ViewChild('deleteModal')deleteModal;

  public titulo = 'Cidades';
  public cidadeSelecionada : Cidade;
  public deleteSelecionado : Cidade;
  public cidadeForm: FormGroup;
  public textSimple: string;
  public modo: string;

  public cidades: Cidade[];
  public cidade : Cidade;
  public estados: Estado[];
  
  openDeleteModal(cidade) {
    this.deleteSelecionado = cidade;
    this.deleteModalRef = this.modalService.show(this.deleteModal, {class: 'modal-sm'});
  }

  confirmDelete(): void {
    this.cidadeService.delete(this.deleteSelecionado.id).subscribe(
      (model : any) => {
        console.log(model);
        this.carregarCidades();
        this.deleteModalRef.hide();
      },
      (erro : any) => {console.log(erro);}
    );
  }
 
  declineDelete(): void {
    this.message = 'Cancelado!';
    this.deleteModalRef.hide();
  }
  constructor(private fb: FormBuilder, private modalService: BsModalService, 
    private cidadeService: CidadeService, private estadosService: EstadoService) { 
    this.criarForm();
  }

  ngOnInit(): void {
    this.carregarCidades();
    this.carregarEstados();
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
  }

  carregarEstados(){
    this.estadosService.getAll().subscribe(
      (estados: Estado[]) => {
        this.estados = estados;
      },
      (erro: any) => {
        console.error(erro);
      }
      
    );
  }

  public cidadeSelect(cidade: Cidade){
    this.cidadeSelecionada = cidade;
    this.cidadeForm.patchValue(cidade);
  }

  public cadastroCidade(){
    this.cidadeSelecionada = new Cidade();
    this.cidadeForm.patchValue(this.cidadeSelecionada);
  }

  public voltar(){
    this.cidadeSelecionada = null;
  }

  public criarForm(){
    this.cidadeForm = this.fb.group({
      id: [""],
      nome: ['', Validators.required],
      estadoId: ['', Validators.required],
      populacao: ['', Validators.required]
    });
  }

  public salvarCidade (cidade: Cidade){
    (cidade.id === 0) ? this.modo = 'post' : this.modo = 'put';
    this.cidadeService[this.modo](cidade).subscribe(
    (retorno: Cidade) => {
      console.log(retorno);
      this.carregarCidades();
    },
    (erro : any) => {
      console.log(erro);
    }
    );
  }

  public deletarCidade(){
    
  }

  public cidadeSubmit(){
    this.salvarCidade(this.cidadeForm.value);
  }
}
