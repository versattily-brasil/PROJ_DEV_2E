import {CollectionViewer, DataSource} from "@angular/cdk/collections";
import { Usuario } from '../models/usuario.model';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { UsuarioService } from './usuario.service';
import { catchError, finalize } from 'rxjs/operators';

export class UsuarioDataSource implements DataSource<Usuario> {

    private usuarioSubject = new BehaviorSubject<Usuario[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private totalItemsSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public totalItems$ = this.totalItemsSubject.asObservable();

    constructor(private usuarioService: UsuarioService) {}

    connect(collectionViewer: CollectionViewer): Observable<Usuario[]> {
      return this.usuarioSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
      this.usuarioSubject.complete();
      this.loadingSubject.complete();
      this.totalItemsSubject.complete();
    }
  
    // loadUsuarios(courseId: number, filter: string, sortDirection: string, pageIndex: number, pageSize: number) {
      
    loadUsuarios(tx_nome:string,currentpage:number, pagesize:number, orderby:string, descending:boolean) {
    
        this.loadingSubject.next(true);

        this.usuarioService.getUsuarios(tx_nome, currentpage,pagesize,orderby,descending).pipe(
            
            catchError(() => of([])),
            finalize(() => this.loadingSubject.next(false))
        
        )
        .subscribe(usuarios => {
            console.log(usuarios); 
            this.usuarioSubject.next(usuarios.Items); 
            this.totalItemsSubject.next(usuarios.TotalItems)
        });
    }  
}