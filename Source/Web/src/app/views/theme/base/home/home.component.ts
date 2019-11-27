// Angular
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
	selector: 'versattily-home',
	templateUrl: './home.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit {

	constructor() {}

	/*
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
    */

	/**
	 * On init
	 */
	ngOnInit() {}
}
