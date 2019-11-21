// Angular
import { Component, OnInit, ElementRef, ViewChild, ChangeDetectionStrategy, OnDestroy, Input, AfterViewInit } from '@angular/core';
import { ServicoService } from '../../../../../core/seguranca/servico.service';
import { HttpParams } from '@angular/common/http';
import { ServicoDataSource } from '../../../../../core/seguranca/servico.datasource';
import { MatPaginator, MatSort } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { merge } from 'rxjs/internal/observable/merge';
import { fromEvent } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-servico-lista',
	templateUrl: './servico-lista.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServicoListaComponent implements OnInit, AfterViewInit {

	dataSource: ServicoDataSource;
	displayedColumns = ["TXT_DEC", "editar"];
	tamanho: number;

	salvouSucesso: boolean = false;
	excluidoSucesso: boolean = false;

	@ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
	@ViewChild(MatSort, { static: false }) sort: MatSort;
	@ViewChild('filtro', { static: false }) filtro: ElementRef;

	constructor(
		private servicoService: ServicoService,
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
		this.dataSource = new ServicoDataSource(this.servicoService);
		this.dataSource.loadServicos('', 1, 10, "TXT_DEC", false);
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
		this.dataSource.loadServicos(
			this.filtro.nativeElement.value,
			this.paginator.pageIndex + 1,
			this.paginator.pageSize,
			this.sort.active,
			this.sort.direction != 'asc');
	}

	adicionarServico() {
		this.router.navigateByUrl('/seguranca/servico/cadastro');
	}

	visualizarServico(cd_srv) {
		this.router.navigate(['/seguranca/servico/cadastro', { id: cd_srv }]);
	}
}
