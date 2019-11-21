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

    public cd_srv:number = 0;

    public getServico(txt_dec:string, currentpage:number, pagesize:number, orderby:string, descending:boolean): Observable<any> {
    
        var params = new HttpParams()
            .set("txt_dec",txt_dec)
            .set("currentpage",currentpage.toString())
            .set("pagesize", pagesize.toString())
            .set("orderby",orderby)
            .set("descending",descending.toString());
        
        
        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/servico", { headers: reqHeader, params: params} );
    }

    
    public getServicoCD(cd_srv:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/servico/"+cd_srv, { headers: reqHeader});
    }

    

    public deletarServico(cd_par:number): Observable<any> {

        const userToken = localStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.delete(environment.baseUrl + "/api/v1/servico/" +cd_par, { headers: reqHeader});
    }
}