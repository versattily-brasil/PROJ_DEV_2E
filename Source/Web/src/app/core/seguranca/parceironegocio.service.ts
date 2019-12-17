import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { JwtHelperService  } from "@auth0/angular-jwt";
import { ParceiroNegocio } from '../models/parceironegocio.model';
import { AutenticacaoService } from '../autenticacao/autenticacao.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class ParceiroNegocioService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public telaLista:boolean = true;
    public cdPrnVisualizar = 0;

    public getParceiroNegocios(txt_rzsoc:string, currentpage:number, pagesize:number, orderby:string, descending:boolean): Observable<any> {
    
        var params = new HttpParams()
            .set("razaosocial",txt_rzsoc)
            .set("currentpage",currentpage.toString())
            .set("pagesize", pagesize.toString())
            .set("orderby",orderby)
            .set("descending",descending.toString());
        
        
        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/parceironegocio", { headers: reqHeader, params: params} );
    }

    
    public getParceiroNegocio(cd_par:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/parceironegocio/"+cd_par, { headers: reqHeader});
    }

    public salvarParceiroNegocio(parceironegocio:ParceiroNegocio): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.put(environment.baseUrl + "/api/v1/parceironegocio/" +parceironegocio.CD_PAR, parceironegocio, { headers: reqHeader});
    }

    public deletarParceiroNegocio(cd_par:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.delete(environment.baseUrl + "/api/v1/parceironegocio/" +cd_par, { headers: reqHeader});
    }
}