// Angular
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UsuarioService } from '../../../../core/seguranca/usuario.service';

@Component({
	selector: 'versattily-usuario',
	templateUrl: './usuario.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsuarioComponent implements OnInit {

	constructor(private usuarioService:UsuarioService) {}

	public get TelaLista(){
		return this.usuarioService.telaLista;
	}

	/*
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
    */

	/**
	 * On init
	 */
	ngOnInit() {}
}
