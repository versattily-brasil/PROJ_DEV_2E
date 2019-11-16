import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AutenticacaoService } from './autenticacao.service';


@Injectable()
export class LogadoGuard {

    constructor(private autenticacaoService: AutenticacaoService, private router: Router) {
    }

    canActivate() {
        if(!this.autenticacaoService.logado()){
            this.router.navigate(["/auth/login"]);
            return false;
        }
        return true;
    }
}