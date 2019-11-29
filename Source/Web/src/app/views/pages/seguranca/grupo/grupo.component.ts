// Angular
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { GrupoService } from '../../../../core/seguranca/grupo.service';

@Component({
	selector: 'versattily-grupo',
	templateUrl: './grupo.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class GrupoComponent implements OnInit {

	constructor(private grupoService:GrupoService) {}

	public get TelaLista(){
		return this.grupoService.telaLista;
	}

	/*
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
    */

	/**
	 * On init
	 */
	ngOnInit() {}
}
