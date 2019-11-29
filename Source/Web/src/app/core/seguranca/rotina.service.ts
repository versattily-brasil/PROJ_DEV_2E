import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { AutenticacaoService } from '../autenticacao/autenticacao.service';
import { environment } from '../../../environments/environment';
import { Rotina } from '../models/rotina.model';

@Injectable()
export class RotinaService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public telaLista:boolean = true;

    public getRotinas(): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/rotina/todos" , { headers: reqHeader} );
    }

    public getRotinasPaginas(tx_nome:string, currentpage:number, pagesize:number, orderby:string, descending:boolean): Observable<any> {
    
        var params = new HttpParams()
            .set("descricao",tx_nome)
            .set("currentpage",currentpage.toString())
            .set("pagesize", pagesize.toString())
            .set("orderby",orderby)
            .set("descending",descending.toString());
        
        
        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/rotina", { headers: reqHeader, params: params} );
    }

    public getRotina(cd_rot:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/rotina/"+cd_rot, { headers: reqHeader});
    }

    public salvarRotina(rotina:Rotina): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.put(environment.baseUrl + "/api/v1/rotina/" +rotina.CD_ROT, rotina, { headers: reqHeader});
    }

    public deletarRotina(cd_rot:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.delete(environment.baseUrl + "/api/v1/rotina/" +cd_rot, { headers: reqHeader});
    }

    public getRotinasAssociadas(cd_rot:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/rotina/associadas/"+cd_rot, { headers: reqHeader});
    }

}