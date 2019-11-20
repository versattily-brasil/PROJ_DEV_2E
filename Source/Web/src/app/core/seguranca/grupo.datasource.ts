import {CollectionViewer, DataSource} from "@angular/cdk/collections";
import { Grupo } from '../models/grupo.model';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { GrupoService } from './grupo.service';
import { catchError, finalize } from 'rxjs/operators';

export class GrupoDataSource implements DataSource<Grupo> {

    private grupoSubject = new BehaviorSubject<Grupo[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private totalItemsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public totalItems$ = this.totalItemsSubject.asObservable();

    constructor(private grupoService: GrupoService) {}

    connect(collectionViewer: CollectionViewer): Observable<Grupo[]> {
      return this.grupoSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
      this.grupoSubject.complete();
      this.loadingSubject.complete();
      this.totalItemsSubject.complete();
    }
  
    // loadGrupos(courseId: number, filter: string, sortDirection: string, pageIndex: number, pageSize: number) {
      
    loadGrupos(tx_nome:string,currentpage:number, pagesize:number, orderby:string, descending:boolean) {
    
        this.loadingSubject.next(true);

        this.grupoService.getGrupos(tx_nome, currentpage,pagesize,orderby,descending).pipe(
            
            catchError(() => of([])),
            finalize(() => this.loadingSubject.next(false))
        
        )
        .subscribe(grupos => {
            console.log(grupos); 
            this.grupoSubject.next(grupos.Items); 
            this.totalItemsSubject.next(grupos.TotalItems)
        });
    }  
}