// Angular
import { Injectable } from '@angular/core';
// RxJS
import { BehaviorSubject } from 'rxjs';
// Object path
import * as objectPath from 'object-path';
// Services
import { MenuConfigService } from './menu-config.service';
import { MenuService } from '../../../seguranca/menu.service';
import { AutenticacaoService } from '../../../autenticacao/autenticacao.service';

@Injectable()
export class MenuAsideService {
	// Public properties
	menuList$: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);

	/**
	 * Service constructor
	 *
	 * @param menuConfigService: MenuConfigService
	 */
	constructor(private menuConfigService: MenuConfigService,
		private menuService: MenuService,
		private auth: AutenticacaoService) {
		this.loadMenu();
	}

	/**
	 * Load menu list
	 */
	loadMenu() {

		if (localStorage.getItem("menus") && localStorage.getItem("menus") != "undefined") {

			console.log(localStorage.getItem("menus"));
			this.menuList$.next(JSON.parse(localStorage.getItem("menus")));
		} else {

			this.menuService.getPermissoes(this.auth.idUsuario).subscribe(menus => {
				this.menuList$.next(menus);
			});
		}


	}

	// loadMenu() {
	// 	// get menu list
	// 	const menuItems: any[] = objectPath.get(this.menuConfigService.getMenus(), 'aside.items');
	// 	this.menuList$.next(menuItems);
	// }
}
