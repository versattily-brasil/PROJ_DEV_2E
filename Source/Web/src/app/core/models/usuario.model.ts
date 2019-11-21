import { Grupo } from './grupo.model';
import { Modulo } from './modulo.model';
import { UsuarioGrupo } from './usuario-grupo.model';
import { UsuarioModulo } from './usuario-modulo.model';
import { RotinaUsuarioOperacao } from './rotina-usuario-operacao.model';

export class Usuario {
    CD_USR: number;
    TX_LOGIN: string;
    TX_NOME: string;
    TX_SENHA: string;
    CONFIRMA_SENHA: string;
    OP_STATUS: string;
    API_TOKEN: string;

    //Todos os módulos e usuários para preencher a tabela de escolha
    Grupo:Grupo[];
    Modulo:Modulo[];

    //Propriedades de relacionamento no banco
    UsuarioGrupo:UsuarioGrupo[] = [];
    UsuarioModulo:UsuarioModulo[] = [];
    RotinaUsuarioOperacao:RotinaUsuarioOperacao[] = [];

    NomeStatus:string; 
}

export class UsuarioStatus{

    TX_DESC: string;
    OP_STATUS: string;
}