// Angular
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
	selector: 'versattily-seguranca',
	templateUrl: './seguranca.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class SegurancaComponent implements OnInit {

	constructor() {}

	/*
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
    */

	/**
	 * On init
	 */
	ngOnInit() {}
}
