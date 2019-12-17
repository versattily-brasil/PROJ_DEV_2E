import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import { JwtHelperService  } from "@auth0/angular-jwt";
import { Usuario } from '../models/usuario.model';
import { environment } from '../../../environments/environment';
@Injectable()
export class AutenticacaoService {  

    constructor(private jwtHelper: JwtHelperService , private http: HttpClient) { }

    public login(usuario: Usuario): Observable<any> {
        //return this.http.post(environment.baseUrl + "/sso/v1/usuario/login", usuario);
        return this.http.post(environment.baseUrl + "/api/v1/usuario/login", usuario);        
    }

    public logout(){
        localStorage.removeItem("token");
        localStorage.removeItem("nome");
    }

    public armazenaInfoLogin(usuario:Usuario){
        localStorage.setItem("token", usuario.API_TOKEN);
        localStorage.setItem("nome", usuario.TX_NOME);
        localStorage.setItem("cd_usr", usuario.CD_USR.toString());
    }

    public logado() {
        var token = localStorage.getItem("token");
        if (token != "undefined" && !this.jwtHelper.isTokenExpired(token)) {
            return true;
        }
        return false;
    }

    public get token(){
        return localStorage.getItem("token");
    }

    public get nomeLogado(){
        return localStorage.getItem("nome");
    }

    
    public get idUsuario(){
        return localStorage.getItem("cd_usr");
    }
}