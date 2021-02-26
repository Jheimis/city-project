import { Component, OnInit, TemplateRef } from '@angular/core';
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

  modalRef: BsModalRef;
  public titulo = 'Cidades';
  public cidadeSelecionada : Cidade;
  public cidadeForm: FormGroup;
  public cadastroForm: FormGroup;
  public textSimple: string;
  public teste = "teste";

  public cidades: Cidade[];

   openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
  constructor(private fb: FormBuilder, private modalService: BsModalService, 
    private cidadeService: CidadeService) { 
    this.criarForm();
  }

  ngOnInit(): void {
    this.carregarCidades();
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

  public cidadeSelect(cidade: Cidade){
    this.cidadeSelecionada = cidade;
    this.cidadeForm.patchValue(cidade);
  }

  public cadastroCidade(){
    this.cidadeSelecionada = new Cidade();
    this.cadastroForm.patchValue(this.cidadeSelecionada);
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
    this.cidadeService.put(cidade.id, cidade).subscribe(
    (retorno: Cidade) => {
      console.log(retorno);
      this.carregarCidades();
    },
    (erro : any) => {
      console.log(erro);
    }
    );
  }

  public cidadeSubmit(){
    this.salvarCidade(this.cidadeForm.value);
  }

  public cadastroSubmit(){
    this.salvarCidade(this.cadastroForm.value);
  }

}
