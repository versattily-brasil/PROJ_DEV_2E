import { Injectable } from "@angular/core";
import { Subject } from 'rxjs';

@Injectable()
export class AbasService {

    constructor() { }

    public AbaUsuario: boolean = false;
    public AbaGrupos: boolean = false;
    public AbaServico: boolean = false;
    public AbaParceironegocio: boolean = false;
    public AbaRotina: boolean = false;


    public SelectedTab: number = 0;

    tabSubject: Subject<number> = new Subject<number>();

    changeAbaUsuario() {
        if (this.listAbasAbertas.indexOf('Usuário') < 0) {
            this.listAbasAbertas.push('Usuário');
        }



        // this.AbaUsuario = true;
        this.tabSubject.next(this.listAbasAbertas.indexOf('Usuário'));
    }

    changeAbaGrupos() {
        if (this.listAbasAbertas.indexOf('Grupos de Usuários') < 0) {
            this.listAbasAbertas.push('Grupos de Usuários');
        }
        // this.AbaGrupos = true;
        this.tabSubject.next(this.listAbasAbertas.indexOf('Grupos de Usuários'));
    }

    changeAbaServico() {
        if (this.listAbasAbertas.indexOf('Serviços') < 0) {
            this.listAbasAbertas.push('Serviços');
        }
        // this.AbaServico = true;
        this.tabSubject.next(this.listAbasAbertas.indexOf('Serviços'));
    }

    changeAbaParceirnoNegocio() {
        if (this.listAbasAbertas.indexOf('Parceiro de Negócio') < 0) {
            this.listAbasAbertas.push('Parceiro de Negócio');
        }
        // this.AbaParceironegocio = true;
        this.tabSubject.next(this.listAbasAbertas.indexOf('Parceiro de Negócio'));
    }

    changeAbaRotina() {
        if (this.listAbasAbertas.indexOf('Rotinas') < 0) {
            this.listAbasAbertas.push('Rotinas');
        }
        // this.AbaRotina = true;
        this.tabSubject.next(this.listAbasAbertas.indexOf('Rotinas'));
    }

    limparAbas() {
        this.AbaUsuario = false;
        this.AbaGrupos = false;
        this.AbaServico = false;
        this.AbaParceironegocio = false;
        this.AbaRotina = false;
    }

    public listAbasAbertas = [];
    // public listAbasAbertas = ['Grupos de Usuários','Serviços','Rotinas','Parceiro de Negócio','Usuário'];
}