// Angular
import { Component, OnDestroy, OnInit, ViewEncapsulation, ViewChild, ChangeDetectorRef } from '@angular/core';
// RxJS
import { Observable, Subscription } from 'rxjs';
// Object-Path
import * as objectPath from 'object-path';
// Layout
import { LayoutConfigService, MenuConfigService, PageConfigService } from '../../../core/_base/layout';
import { HtmlClassService } from '../html-class.service';
import { LayoutConfig } from '../../../core/_config/layout.config';
import { MenuConfig } from '../../../core/_config/menu.config';
import { PageConfig } from '../../../core/_config/page.config';
// User permissions
import { NgxPermissionsService } from 'ngx-permissions';
// import { currentUserPermissions, Permission } from '../../../core/auth';
import { select, Store } from '@ngrx/store';
import { AppState } from '../../../core/reducers';
import { TelaAtualService } from '../../../core/tela-atual.service';
import { AbasService } from '../../../core/seguranca/abas.service';
import { MatTabGroup } from '@angular/material';

@Component({
	selector: 'kt-base',
	templateUrl: './base.component.html',
	styleUrls: ['./base.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class BaseComponent implements OnInit, OnDestroy {
	// Public variables
	selfLayout: string;
	asideDisplay: boolean;
	asideSecondary: boolean;
	subheaderDisplay: boolean;
	desktopHeaderDisplay: boolean;
	fitTop: boolean;
	fluid: boolean;

	selectedTab = 0;

	public get AbasAbertas(){
		return this.abas.listAbasAbertas;
	}
	
	@ViewChild('tabs', {static: false}) tabGroup: MatTabGroup;


	// Private properties
	private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
	// private currentUserPermissions$: Observable<Permission[]>;

	/**
	 * Component constructor
	 *
	 * @param layoutConfigService: LayoutConfigService
	 * @param menuConfigService: MenuConfifService
	 * @param pageConfigService: PageConfigService
	 * @param htmlClassService: HtmlClassService
	 * @param store
	 * @param permissionsService
	 */
	constructor(
		private abas: AbasService,
		private tela: TelaAtualService,

		private layoutConfigService: LayoutConfigService,
		private menuConfigService: MenuConfigService,
		private pageConfigService: PageConfigService,
		private htmlClassService: HtmlClassService,
		private cd: ChangeDetectorRef,
		private store: Store<AppState>,
		private permissionsService: NgxPermissionsService) {
		this.loadRolesWithPermissions();

		// register configs by demos
		this.layoutConfigService.loadConfigs(new LayoutConfig().configs);
		this.menuConfigService.loadConfigs(new MenuConfig().configs);
		this.pageConfigService.loadConfigs(new PageConfig().configs);

		// setup element classes
		this.htmlClassService.setConfig(this.layoutConfigService.getConfig());

		const subscr = this.layoutConfigService.onConfigUpdated$.subscribe(layoutConfig => {
			// reset body class based on global and page level layout config, refer to html-class.service.ts
			document.body.className = '';
			this.htmlClassService.setConfig(layoutConfig);
		});
		this.unsubscribe.push(subscr);
	}

	/**
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
	 */

	/**
	 * On init
	 */
	ngOnInit(): void {

		this.tela.TelaAtual == "usuario-lista";
		this.abas.listAbasAbertas = [];

		const config = this.layoutConfigService.getConfig();
		this.selfLayout = objectPath.get(config, 'self.layout');
		this.asideDisplay = objectPath.get(config, 'aside.self.display');
		this.subheaderDisplay = objectPath.get(config, 'subheader.display');
		this.desktopHeaderDisplay = objectPath.get(config, 'header.self.fixed.desktop');
		this.fitTop = objectPath.get(config, 'content.fit-top');
		this.fluid = objectPath.get(config, 'content.width') === 'fluid';

		// let the layout type change
		const subscr = this.layoutConfigService.onConfigUpdated$.subscribe(cfg => {
			setTimeout(() => {
				this.selfLayout = objectPath.get(cfg, 'self.layout');
			});
		});
		this.unsubscribe.push(subscr);

		this.abas.tabSubject.subscribe(aba=>{

			this.tabGroup.selectedIndex = aba;
			// this.selectedTab = aba;
			// this.cd.detectChanges();
		});
	}

	/**
	 * On destroy
	 */
	ngOnDestroy(): void {
		this.unsubscribe.forEach(sb => sb.unsubscribe());
	}

	/**
	 * NGX Permissions, init roles
	 */
	loadRolesWithPermissions() {
		// this.currentUserPermissions$ = this.store.pipe(select(currentUserPermissions));
		// const subscr = this.currentUserPermissions$.subscribe(res => {
		// 	if (!res || res.length === 0) {
		// 		return;
		// 	}

		// 	this.permissionsService.flushPermissions();
		// 	res.forEach((pm: Permission) => this.permissionsService.addPermission(pm.name));
		// });
		// this.unsubscribe.push(subscr);
	}

	mudarTela(tela) {
		this.tela.TelaAtual = tela;
	}

	public get AbaUsuario() {
		return this.abas.AbaUsuario;
	}
	public get AbaGrupo() {

		return this.abas.AbaGrupos;
	}
	public get AbaServico() {
		return this.abas.AbaServico;
	}
	public get AbaParceironegocio() {
		return this.abas.AbaParceironegocio;
	}
	public get AbaRotina() {
		return this.abas.AbaRotina;
	}

	fecharAbaUsuario() {
		this.abas.listAbasAbertas.splice(this.abas.listAbasAbertas.indexOf('Usuário'),1);
		// this.abas.AbaUsuario = false;
	}
	fecharAbaGrupo() {
		this.abas.listAbasAbertas.splice(this.abas.listAbasAbertas.indexOf('Grupos de Usuários'),1);
		// this.abas.AbaGrupos = false;
	}
	fecharAbaServico() {
		this.abas.listAbasAbertas.splice(this.abas.listAbasAbertas.indexOf('Serviços'),1);
		// this.abas.AbaServico = false;
	}
	fecharAbaParceironegocio() {

		this.abas.listAbasAbertas.splice(this.abas.listAbasAbertas.indexOf('Parceiro de Negócio'),1);
		// this.abas.AbaParceironegocio = false;
	}
	fecharAbaRotina() {
		this.abas.listAbasAbertas.splice(this.abas.listAbasAbertas.indexOf('Rotinas'),1);
		// this.abas.AbaRotina = false;
	}
}
