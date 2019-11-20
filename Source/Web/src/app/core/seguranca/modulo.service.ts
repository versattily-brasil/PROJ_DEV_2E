import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { JwtHelperService  } from "@auth0/angular-jwt";
import { Modulo } from '../models/modulo.model';
import { AutenticacaoService } from '../autenticacao/autenticacao.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class ModuloService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public getModulos(tx_nome:string, currentpage:number, pagesize:number, orderby:string, descending:boolean): Observable<any> {
    
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
     
        return this.http.get(environment.baseUrl + "/api/v1/modulo/todos", { headers: reqHeader, params: params} );
    }
}