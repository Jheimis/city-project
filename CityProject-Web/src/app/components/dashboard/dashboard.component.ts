import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Cidade } from 'src/app/models/Cidade';
import { Estado } from 'src/app/models/Estado';
import { CidadeService } from 'src/app/service/cidade/cidade.service';
import { Dolar } from 'src/app/service/dolar';
import { DolarService } from 'src/app/service/dolar.service';
import { EstadoService } from 'src/app/service/estado/estado.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  deleteModalRef: BsModalRef;
  cadastroArquivoModalRef: BsModalRef;
  cadastroModalRef: BsModalRef;
  @ViewChild('deleteModal')deleteModal;
  @ViewChild('cadastroArquivoModal')cadastroArquivoModal;
  @ViewChild('cadastroModal')cadastroModal;

  public titulo = 'Cidades';
  public cidadeSelecionada : Cidade;
  public deleteSelecionado : Cidade;
  public cidadeForm: FormGroup;
  public modo: string;
  public imgEstado: string;
  public estadoSelecionado: Estado;

  public dolarHoje = new Dolar;
  public cidadeDolar: FormGroup;

  public cidades: Cidade[];
  public cidade : Cidade;
  public estados: Estado[];
  public estado: Estado[];
  
  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService, 
    private cidadeService: CidadeService, 
    private estadosService: EstadoService, 
    private dolarService: DolarService,
    private toastr: ToastrService ) { 
      this.criarForm();
  }
    
  ngOnInit(): void {
    this.carregarEstado();
    this.setDolar();
  }
  openDeleteModal(cidade) {
    this.deleteSelecionado = cidade;
    this.deleteModalRef = this.modalService.show(this.deleteModal, {class: 'modal-sm'});
  }

  confirmDelete(): void {
    if(this.deleteSelecionado.estadoId != 1){
      this.toastr.success('Cidade deletada com Sucesso', 'Deletada');
      this.cidadeService.delete(this.deleteSelecionado.id).subscribe(
        (model : any) => {
          console.log(model);
          this.carregarEstado();
          this.deleteModalRef.hide();
        },
        (erro : any) => {console.log(erro);} 
      );
    }
    else{
      this.toastr.error('Cidades que pertencem ao estado do Rio Grande do Sul não podem ser deletadas', 'Erro');
      this.deleteModalRef.hide();
    }
    
  }
   
  declineDelete(): void {
    this.deleteModalRef.hide();
  }

  openCadastroModal(){
    this.cadastroModalRef = this.modalService.show(this.cadastroModal);
  }

  openCadastroArquivoModal(){
    this.cadastroArquivoModalRef = this.modalService.show(this.cadastroArquivoModal);
  }

  carregarEstado(){
    
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
    this.imgEstado = "assets/img/SantaCatarina.png";
    this.cidadeSelecionada = new Cidade();
    this.cidadeForm.patchValue(this.cidadeSelecionada);
  }

  public changeEstado(event : any ){

    console.log(event);
    if(event == 1){
     this.imgEstado = "assets/img/RioGrandeDoSul.png";
    }
    if(event == 2 ){
     this.imgEstado = "assets/img/SantaCatarina.png";
    }
    if(event == 3){
     this.imgEstado = "assets/img/Parana.png";
    }
    this.cidadeService.getCidadeByEstadoId(event).subscribe(
      (cidades: Cidade[]) => {
        this.cidades = cidades;
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

  public criarForm(){
    this.cidadeForm = this.fb.group({
      id: [""],
      nome: ['', Validators.required],
      estadoId: ['', Validators.required],
      populacao: ['', Validators.required]
    });
  }
  
  public salvarCidade (cidade: Cidade){
    if(cidade.populacao <= 0){
      this.toastr.error('Número de população deve ser maior que 0', 'Erro');
    }
    else{
      this.cidadeService.getAll().subscribe(
        (cidades: Cidade[]) => {
          cidades.forEach(item => {
            if(item.nome.toUpperCase() != cidade.nome.toUpperCase() && item.estadoId != cidade.estadoId){

              (cidade.id === 0) ? this.modo = 'post' : this.modo = 'put';
              this.cidadeService[this.modo](cidade).subscribe(
                (retorno: Cidade) => {
                  console.log(retorno);
                  this.carregarEstado();
                },
                (erro : any) => {
                  console.log(erro);
                }
                );
            }
            else {
              this.toastr.error('Essa cidade já existe nesse estado', 'Erro');
            }
          });
        }
      );
    }
  }
    
  public cidadeSubmit(){
    this.salvarCidade(this.cidadeForm.value);
  }

  public cancelarCadastroArquivo(){
    this.cadastroArquivoModal.hide();
  }
  public cancelarCadastro(){
    this.cadastroModal.hide();
  }

}
