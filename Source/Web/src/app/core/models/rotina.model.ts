import { RotinaServico } from './rotina-servico.model';
import { RotinaAssociada } from './rotina-associada.model';

export class Rotina {
    CD_ROT:number;
    TX_NOME:string;
    TX_DSC:string;
    OP_TIPO:number;
    
    CD_SRV:number;
    TX_URL:string;

    descricaoServico:string;

    RotinaServico:RotinaServico[] = [];
    RotinasAssociadas:RotinaAssociada[] = [];
}
