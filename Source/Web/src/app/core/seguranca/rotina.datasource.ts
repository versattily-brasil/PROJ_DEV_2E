import {CollectionViewer, DataSource} from "@angular/cdk/collections";
import { Rotina } from '../models/rotina.model';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { RotinaService } from './rotina.service';
import { catchError, finalize } from 'rxjs/operators';
import { ServicoService } from './servico.service';

export class RotinaDataSource implements DataSource<Rotina> {

    private rotinaSubject = new BehaviorSubject<Rotina[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private totalItemsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public totalItems$ = this.totalItemsSubject.asObservable();

    constructor(private rotinaService: RotinaService, private servicoService: ServicoService) {}

    connect(collectionViewer: CollectionViewer): Observable<Rotina[]> {
      return this.rotinaSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
      this.rotinaSubject.complete();
      this.loadingSubject.complete();
      this.totalItemsSubject.complete();
    }
  
    // loadRotinas(courseId: number, filter: string, sortDirection: string, pageIndex: number, pageSize: number) {
      
    loadRotinas(tx_nome:string,currentpage:number, pagesize:number, orderby:string, descending:boolean) {
    
        this.loadingSubject.next(true);

        this.rotinaService.getRotinasPaginas(tx_nome, currentpage,pagesize,orderby,descending).pipe(
            
            catchError(() => of([])),
            finalize(() => this.loadingSubject.next(false))
        
        )
        .subscribe(rotinas => {

            this.servicoService.getServicos().subscribe(servicos =>{

              rotinas.Items.forEach(r => {
                

                r.descricaoServico = servicos.find(o=>o.CD_SRV == r.CD_SRV).TXT_DEC;
              });

              
            console.log(rotinas); 
            this.rotinaSubject.next(rotinas.Items); 
            this.totalItemsSubject.next(rotinas.TotalItems);

            })

        });
    }  
}