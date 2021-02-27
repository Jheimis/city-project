import { Estado } from './Estado';
export class Cidade {
    
    constructor() {
        
        this.id = 0;
        this.nome = '';
        this.populacao = 0;
        this.custoCidadeUS = 0;
        this.estadoId = 2;
        
    }
    
    id: number;
    nome: string;
    populacao: number;
    custoCidadeUS: number;
    estadoId: number;
    estado: Estado;
    
}
