import {CollectionViewer, DataSource} from "@angular/cdk/collections";
import { ParceiroNegocio } from '../models/parceironegocio.model';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { ParceiroNegocioService } from './parceironegocio.service';
import { catchError, finalize } from 'rxjs/operators';

export class ParceiroNegocioDataSource implements DataSource<ParceiroNegocio> {

    private parceironegocioSubject = new BehaviorSubject<ParceiroNegocio[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private totalItemsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public totalItems$ = this.totalItemsSubject.asObservable();

    constructor(private parceironegocioService: ParceiroNegocioService) {}

    connect(collectionViewer: CollectionViewer): Observable<ParceiroNegocio[]> {
      return this.parceironegocioSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
      this.parceironegocioSubject.complete();
      this.loadingSubject.complete();
      this.totalItemsSubject.complete();
    }
  
    // loadParceiroNegocios(courseId: number, filter: string, sortDirection: string, pageIndex: number, pageSize: number) {
      
    loadParceiroNegocios(tx_rzsoc:string,currentpage:number, pagesize:number, orderby:string, descending:boolean) {
    
        this.loadingSubject.next(true);

        this.parceironegocioService.getParceiroNegocios(tx_rzsoc, currentpage,pagesize,orderby,descending).pipe(
            
            catchError(() => of([])),
            finalize(() => this.loadingSubject.next(false))
        
        )
        .subscribe(parceironegocios => {
            console.log(parceironegocios); 
            this.parceironegocioSubject.next(parceironegocios.Items); 
            this.totalItemsSubject.next(parceironegocios.TotalItems)
        });
    }  
}