// Angular
import { Component, OnInit, ElementRef, ViewChild, ChangeDetectionStrategy, OnDestroy, Input, AfterViewInit } from '@angular/core';
import { UsuarioService } from '../../../../../core/seguranca/usuario.service';
import { HttpParams } from '@angular/common/http';
import { UsuarioDataSource } from '../../../../../core/seguranca/usuario.datasource';
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
	selector: 'versattily-usuario-lista',
	templateUrl: './usuario-lista.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsuarioListaComponent implements OnInit, AfterViewInit {

	nomeRotina : string =  "Usuário";
	permissoes : Array<Permissao>;

	dataSource: UsuarioDataSource;
	displayedColumns = ["TX_NOME", "TX_LOGIN", "NomeStatus", "editar"];
	tamanho: number;

	get salvouSucesso(){
		return this.usuarioService.sucessoSalvar;
	}
	get excluidoSucesso(){
		return this.usuarioService.sucessoExcluir;
	}

	@ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
	@ViewChild(MatSort, { static: false }) sort: MatSort;
	@ViewChild('filtro', { static: false }) filtro: ElementRef;

	constructor(
		private usuarioService: UsuarioService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private permissaoService: PermissaoService,
		private auth:AutenticacaoService) {

	}


	ngOnInit(): void {

		this.tamanho = 20;
		this.dataSource = new UsuarioDataSource(this.usuarioService);
		this.dataSource.loadUsuarios('', 1, 10, "TX_NOME", false);

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
		this.dataSource.loadUsuarios(
			this.filtro.nativeElement.value,
			this.paginator.pageIndex + 1,
			this.paginator.pageSize,
			this.sort.active,
			this.sort.direction != 'asc');
	}

	adicionarUsuario() {
		this.usuarioService.telaLista = false;
		this.usuarioService.sucessoExcluir = false;
		this.usuarioService.sucessoSalvar = false;
		this.usuarioService.cdUserVisualizar = 0;
		//this.router.navigateByUrl('/seguranca/usuarios/cadastro');
	}

	visualizarUsuario(cd_usr) {
		this.usuarioService.telaLista = false;
		this.usuarioService.sucessoExcluir = false;
		this.usuarioService.sucessoSalvar = false;
		this.usuarioService.cdUserVisualizar = cd_usr;
		// this.router.navigate(['/seguranca/usuarios/cadastro', { id: cd_usr }]);
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
		console.log('ação: ' + acao);

		if(this.permissoes === undefined || this.permissoes === null || this.permissoes.length === 0)
		{
			return false;
		}

		var encontrou = this.permissoes.filter(filtro => filtro.TX_DSC === acao);

		console.log(encontrou);

		if(encontrou === undefined || encontrou === null || encontrou.length === 0)
		{
			console.log('não encontrou ' + acao);
			return false;
		}
		else
		{
			console.log('encontrou ' + acao);
			return true;
		}
	}
}
