import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { AutenticacaoService } from '../autenticacao/autenticacao.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class ServicoService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public getServicos(): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/servico/todos", { headers: reqHeader} );
    }
}