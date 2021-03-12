import { Component, OnInit, ViewChild, Directive, EventEmitter, Input, Output, QueryList, ViewChildren  } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Cidade } from 'src/app/models/Cidade';
import { Estado } from 'src/app/models/Estado';
import { CidadeService } from 'src/app/service/cidade/cidade.service';
import { Dolar } from 'src/app/service/dolar';
import { DolarService } from 'src/app/service/dolar.service';
import { EstadoService } from 'src/app/service/estado/estado.service';

//orddem
export type SortDirection = 'asc' | 'desc' | '';
const rotate: {[key: string]: SortDirection} = { 'asc': 'desc', 'desc': '', '': 'asc' };
const compare = (v1: string | number, v2: string | number) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

export interface SortEvent {
  column: string;
  direction: SortDirection;
}

@Directive({
  selector: 'th[sortable]',
  host: {
    '[class.asc]': 'direction === "asc"',
    '[class.desc]': 'direction === "desc"',
    '(click)': 'rotate()'
  }
})
export class NgbdSortableHeader {
  
  @Input() sortable: string = '';
  @Input() direction: SortDirection = '';
  @Output() sort = new EventEmitter<SortEvent>();
  
  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({column: this.sortable, direction: this.direction});
  }
}
//fim orddem

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
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
  
  public titulo = 'Cidades';
  public cidadeSelecionada : Cidade;
  public deleteSelecionado : Cidade;
  public cidadeForm: FormGroup;
  public imgEstado: string;
  formData = new FormData();
  
  public dolarHoje = new Dolar;
  public cidadeDolar: FormGroup;
  
  public cidades: Cidade[];
  public cidade : Cidade;
  public estados: Estado[];
  public estadoSelect: Estado[];
  public estado: Estado[];
  selectedValue: number  = 2;
  
  //pesquisa
  searchTerm: string;
  searchVar = 0;
  
  //
  
  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService, 
    private cidadeService: CidadeService, 
    private estadosService: EstadoService, 
    private dolarService: DolarService,
    private toastr: ToastrService,
    private router: Router, 
  )
  { this.criarForm(); }
    
  ngOnInit(): void {
    this.carregarEstado();
    this.carregarEstadoSelect();
    this.setDolar();
    this.carregarCidades()
    this.imgEstado = "assets/img/SantaCatarina.png";
  }
    
  //ordem
  onSort({column, direction}: SortEvent) {
      
    // resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });
      
    // sorting countries
    if (direction === '' || column === '') {
      return this.cidades

    } else {
        this.cidades.sort((a, b) => {
          const res = compare(a[column], b[column]);
          return direction === 'asc' ? res : -res;
        });
    }
  }

  search(value: string): void {
    this.searchVar = value.length;
    this.cidades.filter((val) => val.nome.toLowerCase().includes(value));  
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
          this.carregarCidades();
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
    this.criarForm();
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
  
  carregarEstadoSelect(){
    this.estadosService.getAll().subscribe(
      (estados: Estado[]) => {
        this.estadoSelect = estados;
      },
      (erro: any) => {
        console.error(erro);
      } 
    );
  }
          
  carregarCidades(){
    this.cidadeService.getCidadeByEstadoId(this.selectedValue).subscribe(
      (cidades: Cidade[]) => {
        this.cidades = cidades;
      },
      (erro: any) => {
        console.error(erro);
      }
    );
  }
  
  // excel
  selectFiles(input: HTMLInputElement) {
    const files = input.files;
    if (files) this.formData.append('file', files[0], files[0].name);
  }
            
  saveCidadeFile() {

    this.cidadeService
      .saveCidadeFromFile(this.formData)
      .then((response) => {
        console.log('cidades salva com sucesso'+response);
      })
      .then(() => {
        this.router.navigate([`/`]);
      })
      .catch((err) => {
        console.log('error: ' + err);
      });
  }
            
  public changeEstado(event : any ){
              
    console.log(event);
    console.log(this.selectedValue);
    this.selectedValue = event;
    if(event == 1){
      this.imgEstado = "assets/img/RioGrandeDoSul.png";
    }
    if(event == 2){
      this.imgEstado = "assets/img/SantaCatarina.png";
    }
    if(event == 3){
      this.imgEstado = "assets/img/Parana.png";
    }
              
    this.carregarCidades();
              
  }
            
  convertDolarCidade(cidade: Cidade) {        
    if (cidade.custoCidadeUS && this.dolarHoje?.USD) {         
      return cidade.custoCidadeUS * (this.dolarHoje.USD.ask || 1);        
    }
    return 0
  }
            
  convertDolarEstado(estado: Estado) {        
    if (estado.custoEstadoUS && this.dolarHoje?.USD) {
      return estado.custoEstadoUS * (this.dolarHoje.USD.ask || 1);
    }
    return (0);
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
    nome: ['', Validators.required],
    populacao: ['', Validators.required],
    estadoId: [this.selectedValue || '']
    });
  }
            
  public salvarCidade(cidade: Cidade) {
    this.cidadeService.post(this.cidade).subscribe(
      (retorno: Cidade) => {
        console.log(retorno);
        this.carregarCidades();
        this.carregarEstado();
        this.cadastroModalRef.hide();
        return this.toastr.success('Cidade cadastrada com sucesso', 'Sucesso');
      },
      (erro : any) => {
        console.log(erro);
      }
    );   
  }
              
  public cidadeSubmit(){
    this.cidade = this.cidadeForm.value;
    if(this.cidade.populacao <= 0){
      this.toastr.error('Número de população deve ser maior que 0', 'Erro');
    }
    else{
      var validado: boolean = true;
      this.cidadeService.getCidadeByEstadoId(this.selectedValue).subscribe(
        (cidades: Cidade[]) => {
          for(let item of cidades){
            if(item.nome.toUpperCase() == this.cidade.nome.toUpperCase() ){
              validado= false;
              break;
            }
          }
          if (validado) {
            this.salvarCidade(this.cidadeForm.value); 
          }
          else {
            this.toastr.error('Essa cidade já existe nesse estado', 'Erro');
          }
        }
      );
    }
  }              
}
              