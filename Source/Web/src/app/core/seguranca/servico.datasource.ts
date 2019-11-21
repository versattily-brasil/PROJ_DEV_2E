import {CollectionViewer, DataSource} from "@angular/cdk/collections";
import { Servico } from '../models/servico.model';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { ServicoService } from './servico.service';
import { catchError, finalize } from 'rxjs/operators';

export class ServicoDataSource implements DataSource<Servico> {

    private servicoSubject = new BehaviorSubject<Servico[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private totalItemsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public totalItems$ = this.totalItemsSubject.asObservable();

    constructor(private servicoService: ServicoService) {}

    connect(collectionViewer: CollectionViewer): Observable<Servico[]> {
      return this.servicoSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
      this.servicoSubject.complete();
      this.loadingSubject.complete();
      this.totalItemsSubject.complete();
    }
  
    // loadServicos(courseId: number, filter: string, sortDirection: string, pageIndex: number, pageSize: number) {
      
    loadServicos(txt_dec:string,currentpage:number, pagesize:number, orderby:string, descending:boolean) {
    
        this.loadingSubject.next(true);

        this.servicoService.getServico(txt_dec, currentpage,pagesize,orderby,descending).pipe(
            
            catchError(() => of([])),
            finalize(() => this.loadingSubject.next(false))
        
        )
        .subscribe(servicos => {
            console.log(servicos); 
            this.servicoSubject.next(servicos.Items); 
            this.totalItemsSubject.next(servicos.TotalItems)
        });
    }  
}