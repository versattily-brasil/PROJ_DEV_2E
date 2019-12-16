import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { JwtHelperService  } from "@auth0/angular-jwt";
import { Grupo } from '../models/grupo.model';
import { AutenticacaoService } from '../autenticacao/autenticacao.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class GrupoService {

    constructor(private http: HttpClient, private auth:AutenticacaoService) { }

    public telaLista:boolean = true;

    public cdGrpVisualizar = 0;

    public getGrupos(tx_dsc:string, currentpage:number, pagesize:number, orderby:string, descending:boolean): Observable<any> {
    
        var params = new HttpParams()
            .set("tx_dsc",tx_dsc)
            .set("currentpage",currentpage.toString())
            .set("pagesize", pagesize.toString())
            .set("orderby",orderby)
            .set("descending",descending.toString());
        
        
        const userToken = sessionStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/grupo", { headers: reqHeader, params: params} );
    }

    
    public getGrupo(cd_usr:number): Observable<any> {

        const userToken = sessionStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.get(environment.baseUrl + "/api/v1/grupo/"+cd_usr, { headers: reqHeader});
    }

    public salvarGrupo(grupo:Grupo): Observable<any> {

        const userToken = sessionStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.put(environment.baseUrl + "/api/v1/grupo/" +grupo.CD_GRP, grupo, { headers: reqHeader});
    }

    public deletarGrupo(cd_usr:number): Observable<any> {

        const userToken = sessionStorage.getItem("token");
        var reqHeader = new HttpHeaders({ 
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + userToken
         });
     
        return this.http.delete(environment.baseUrl + "/api/v1/grupo/" +cd_usr, { headers: reqHeader});
    }
}