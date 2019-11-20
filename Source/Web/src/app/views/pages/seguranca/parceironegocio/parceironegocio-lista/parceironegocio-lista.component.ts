// Angular
import { Component, OnInit, ElementRef, ViewChild, ChangeDetectionStrategy, OnDestroy, Input, AfterViewInit } from '@angular/core';
import { ParceiroNegocioService } from '../../../../../core/seguranca/parceironegocio.service';
import { HttpParams } from '@angular/common/http';
import { ParceiroNegocioDataSource } from '../../../../../core/seguranca/parceironegocio.datasource';
import { MatPaginator, MatSort } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { merge } from 'rxjs/internal/observable/merge';
import { fromEvent } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-parceironegocio-lista',
	templateUrl: './parceironegocio-lista.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class ParceiroNegocioListaComponent implements OnInit, AfterViewInit {

	dataSource: ParceiroNegocioDataSource;
	displayedColumns = ["TX_RZSOC","CNPJ","TX_EMAIL", "editar"];
	tamanho: number;

	salvouSucesso: boolean = false;
	excluidoSucesso: boolean = false;

	@ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
	@ViewChild(MatSort, { static: false }) sort: MatSort;
	@ViewChild('filtro', { static: false }) filtro: ElementRef;

	constructor(
		private parceironegocioService: ParceiroNegocioService,
		private router: Router,
		private activatedRoute: ActivatedRoute) {

	}


	ngOnInit(): void {



		this.activatedRoute.params.subscribe(params => {
			this.salvouSucesso = params['sucesso'] && params['sucesso'] == 'true' ? true : false;
		});
		this.activatedRoute.params.subscribe(params => {
			this.excluidoSucesso = params['excluido'] && params['excluido'] == 'true' ? true : false;
		});


		this.tamanho = 20;
		this.dataSource = new ParceiroNegocioDataSource(this.parceironegocioService);
		this.dataSource.loadParceiroNegocios('', 1, 10, "TX_RZSOC", false);
	}

	ngAfterViewInit() {

		// server-side search
		fromEvent(this.filtro.nativeElement, 'keyup')
			.pipe(
				debounceTime(150),
				distinctUntilChanged(),
				tap(() => {
					this.paginator.pageIndex = 0;
					this.loadLessonsPage();
				})
			)
			.subscribe();

		// reset the paginator after sorting
		this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

		// on sort or paginate events, load a new page
		merge(this.sort.sortChange, this.paginator.page)
			.pipe(
				tap(() => this.loadLessonsPage())
			)
			.subscribe();
	}

	loadLessonsPage() {
		this.dataSource.loadParceiroNegocios(
			this.filtro.nativeElement.value,
			this.paginator.pageIndex + 1,
			this.paginator.pageSize,
			this.sort.active,
			this.sort.direction != 'asc');
	}

	adicionarParceiroNegocio() {
		this.router.navigateByUrl('/seguranca/parceironegocios/cadastro');
	}

	visualizarParceiroNegocio(cd_par) {
		this.router.navigate(['/seguranca/parceironegocios/cadastro', { id: cd_par }]);
	}
}
