import { Injectable } from "@angular/core";
import { Subject } from 'rxjs';

@Injectable()
export class AbasService {

    constructor() { }

    public AbaUsuario:boolean = false;
    public AbaGrupos:boolean = false;
    public AbaServico:boolean = false;
    public AbaParceironegocio:boolean = false;
    public AbaRotina:boolean = false;


    public SelectedTab:number = 0;

    tabSubject : Subject<number> = new Subject<number>();

    changeAbaUsuario(){
        this.AbaUsuario = true;
        this.tabSubject.next(1);
    }

    changeAbaGrupos(){
        this.AbaGrupos = true;
        this.tabSubject.next(3);
    }

    changeAbaServico(){
        this.AbaServico = true;
        this.tabSubject.next(4);
    }

    changeAbaParceirnoNegocio(){
        this.AbaParceironegocio = true;
        this.tabSubject.next(0);
    }

    changeAbaRotina(){
        this.AbaRotina = true;
        this.tabSubject.next(1);
    }

    limparAbas(){
        this.AbaUsuario = false;
        this.AbaGrupos = false;
        this.AbaServico = false;
        this.AbaParceironegocio = false;
        this.AbaRotina = false;
    }
}