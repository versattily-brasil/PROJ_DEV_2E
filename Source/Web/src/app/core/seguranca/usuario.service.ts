import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { JwtHelperService  } from "@auth0/angular-jwt";
import { Usuario } from '../models/usuario.model';
import { AutenticacaoService } from '../autenticacao/autenticacao.service';

@Injectable()
export class UsuarioService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public cd_usr:number = 0;

    public getUsuarios(tx_nome:string, currentpage:number, pagesize:number, orderby:string, descending:boolean): Observable<any> {
    
        var params = new HttpParams()
            .set("tx_nome",tx_nome)
            .set("currentpage",currentpage.toString())
            .set("pagesize", pagesize.toString())
            .set("orderby",orderby)
            .set("descending",descending.toString());
        
        
        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get("http://localhost:7010/api/v1/usuario", { headers: reqHeader, params: params} );
    }

    
    public getUsuario(cd_usr:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get("http://localhost:7010/api/v1/usuario/"+cd_usr, { headers: reqHeader});
    }

    public salvarUsuario(usuario:Usuario): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.put("http://localhost:7010/api/v1/usuario/"+usuario.CD_USR, usuario, { headers: reqHeader});
    }

    public deletarUsuario(cd_usr:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.delete("http://localhost:7010/api/v1/usuario/"+cd_usr, { headers: reqHeader});
    }
}