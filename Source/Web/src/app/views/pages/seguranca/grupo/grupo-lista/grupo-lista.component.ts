// Angular
import { Component, OnInit, ElementRef, ViewChild, ChangeDetectionStrategy, OnDestroy, Input, AfterViewInit } from '@angular/core';
import { GrupoService } from '../../../../../core/seguranca/grupo.service';
import { HttpParams } from '@angular/common/http';
import { GrupoDataSource } from '../../../../../core/seguranca/grupo.datasource';
import { MatPaginator, MatSort } from '@angular/material';
import { tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { merge } from 'rxjs/internal/observable/merge';
import { fromEvent } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';

import { PermissaoService } from '../../../../../core/seguranca/permissao.service';
import { AutenticacaoService } from '../../../../../core/autenticacao/autenticacao.service';
import { Permissao } from '../../../../../core/models/permissao.model';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-grupo-lista',
	templateUrl: './grupo-lista.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class GrupoListaComponent implements OnInit, AfterViewInit {

	nomeRotina : string =  "Grupos de Usuários";
	permissoes : Array<Permissao>;

	dataSource: GrupoDataSource;
	displayedColumns = ["TX_DSC", "editar"];
	tamanho: number;

	salvouSucesso: boolean = false;
	excluidoSucesso: boolean = false;

	@ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
	@ViewChild(MatSort, { static: false }) sort: MatSort;
	@ViewChild('filtro', { static: false }) filtro: ElementRef;

	constructor(
		private grupoService: GrupoService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private permissaoService: PermissaoService,
		private auth:AutenticacaoService) {

	}


	ngOnInit(): void {



		this.activatedRoute.params.subscribe(params => {
			this.salvouSucesso = params['sucesso'] && params['sucesso'] == 'true' ? true : false;
		});
		this.activatedRoute.params.subscribe(params => {
			this.excluidoSucesso = params['excluido'] && params['excluido'] == 'true' ? true : false;
		});


		this.tamanho = 20;
		this.dataSource = new GrupoDataSource(this.grupoService);
		this.dataSource.loadGrupos('', 1, 10, "TX_DSC", false);

		this.carregarPermissoes();

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
		this.dataSource.loadGrupos(
			this.filtro.nativeElement.value,
			this.paginator.pageIndex + 1,
			this.paginator.pageSize,
			this.sort.active,
			this.sort.direction != 'asc');
	}

	adicionarGrupo() {
		this.grupoService.telaLista = false;
		// this.router.navigateByUrl('/seguranca/grupos/cadastro');
	}

	visualizarGrupo(cd_grp) {
		this.grupoService.telaLista = false;
		// this.router.navigate(['/seguranca/grupos/cadastro', { id: cd_grp }]);
	}

	//-------------------------------------------------------------------------------------------------
	// Método para carregar as permissões da página----------------------------------------------------
	//-------------------------------------------------------------------------------------------------
	carregarPermissoes(){
		this.permissaoService.getPermissoes(this.auth.idUsuario, this.nomeRotina).subscribe(permissao => {
			this.permissoes = permissao;
			console.log(this.permissoes);
		});
	}

	//-------------------------------------------------------------------------------------------------
	// Método para verificar a permissão sobre componente----------------------------------------------
	//-------------------------------------------------------------------------------------------------
	verificarPermissao(acao:string){

		if(this.permissoes === undefined || this.permissoes === null || this.permissoes.length === 0)
		{
			return false;
		}

		var encontrou = this.permissoes.filter(filtro => filtro.TX_DSC === acao);


		if(encontrou === undefined || encontrou === null || encontrou.length === 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
