import { ParceiroModuloServico } from './parceiro-modulo-servico.model';

export class ParceiroNegocio {
    CD_PAR: number;
    TXT_RZSOC: string;
    CNPJ: string;
    TX_EMAIL: string;
    ParceiroModuloServico: ParceiroModuloServico[] = [];
}

