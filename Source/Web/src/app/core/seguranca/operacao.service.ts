import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { AutenticacaoService } from '../autenticacao/autenticacao.service';

@Injectable()
export class OperacaoService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public getOperacoes(): Observable<any> {
    
        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get("http://localhost:7010/api/v1/operacao/todos", { headers: reqHeader} );
    }
}