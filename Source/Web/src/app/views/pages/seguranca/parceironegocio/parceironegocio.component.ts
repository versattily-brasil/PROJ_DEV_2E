// Angular
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ParceiroNegocioService } from '../../../../core/seguranca/parceironegocio.service';

@Component({
	selector: 'versattily-parceironegocio',
	templateUrl: './parceironegocio.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class ParceiroNegocioComponent implements OnInit {

	constructor(private parceiroNegocioService:ParceiroNegocioService) {}

	public get TelaLista(){
		return this.parceiroNegocioService.telaLista;
	}

	/*
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
    */

	/**
	 * On init
	 */
	ngOnInit() {
		this.parceiroNegocioService.telaLista = true;
	}
}
