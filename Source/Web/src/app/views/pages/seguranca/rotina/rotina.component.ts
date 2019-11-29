// Angular
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { RotinaService } from '../../../../core/seguranca/rotina.service';

@Component({
	selector: 'versattily-rotina',
	templateUrl: './rotina.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class RotinaComponent implements OnInit {

	constructor(private rotinaService:RotinaService) {}

	public get TelaLista(){
		return this.rotinaService.telaLista;
	}

	/*
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
    */

	/**
	 * On init
	 */
	ngOnInit() {}
}
