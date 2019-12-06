// Anglar
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Layout Directives
// Services
import {
	ContentAnimateDirective,
	FirstLetterPipe,
	GetObjectPipe,
	HeaderDirective,
	JoinPipe,
	MenuDirective,
	OffcanvasDirective,
	SafePipe,
	ScrollTopDirective,
	SparklineChartDirective,
	StickyDirective,
	TabClickEventDirective,
	TimeElapsedPipe,
	ToggleDirective
} from './_base/layout';

import { AutenticacaoService } from './autenticacao/autenticacao.service';
import { LogadoGuard } from './autenticacao/logado.guard';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { UsuarioService } from './seguranca/usuario.service';
import { RotinaService } from './seguranca/rotina.service';
import { OperacaoService } from './seguranca/operacao.service';
import { ServicoService } from './seguranca/servico.service';
import { TelaAtualService } from './tela-atual.service';
import { GrupoService } from './seguranca/grupo.service';
import { MenuService } from './seguranca/menu.service';
import { ParceiroNegocioService } from './seguranca/parceironegocio.service';
import { ModuloService } from './seguranca/modulo.service';
import { PermissaoService } from './seguranca/permissao.service';
import { AbasService } from './seguranca/abas.service';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
	imports: [
		CommonModule,      
		HttpClientModule
		],
	declarations: [
		// directives
		ScrollTopDirective,
		HeaderDirective,
		OffcanvasDirective,
		ToggleDirective,
		MenuDirective,
		TabClickEventDirective,
		SparklineChartDirective,
		ContentAnimateDirective,
		StickyDirective,
		// pipes
		TimeElapsedPipe,
		JoinPipe,
		GetObjectPipe,
		SafePipe,
		FirstLetterPipe,
	],
	exports: [
		// directives
		ScrollTopDirective,
		HeaderDirective,
		OffcanvasDirective,
		ToggleDirective,
		MenuDirective,
		TabClickEventDirective,
		SparklineChartDirective,
		ContentAnimateDirective,
		StickyDirective,
		// pipes
		TimeElapsedPipe,
		JoinPipe,
		GetObjectPipe,
		SafePipe,
		FirstLetterPipe
	],
	providers: [
		AutenticacaoService,
		UsuarioService,
		RotinaService,
		OperacaoService,
		ServicoService,
		LogadoGuard,
		JwtHelperService,
		TelaAtualService,
		GrupoService,
		MenuService,
		GrupoService,
		ParceiroNegocioService,
		ModuloService,
		ServicoService,
		PermissaoService,
		AbasService
	]
})
export class CoreModule {
}
