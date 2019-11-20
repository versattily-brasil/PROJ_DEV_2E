// Angular
import { Component, OnInit, ElementRef, ViewChild, TemplateRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { GrupoService } from '../../../../../core/seguranca/grupo.service';
import { share, tap } from 'rxjs/operators';
import { Modulo } from '../../../../../core/models/modulo.model';
import { Grupo } from '../../../../../core/models/grupo.model';
import { OperacaoService } from '../../../../../core/seguranca/operacao.service';
import { RotinaService } from '../../../../../core/seguranca/rotina.service';
import { ServicoService } from '../../../../../core/seguranca/servico.service';
import { Servico } from '../../../../../core/models/servico.model';
import { Rotina } from '../../../../../core/models/rotina.model';
import { Operacao } from '../../../../../core/models/operacao.model';
import { RotinaOperacoes } from '../../../../../core/models/rotina-operacoes.model';
import { RotinaGrupoOperacao } from '../../../../../core/models/rotina-grupo-operacao.model';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-grupo-form',
	templateUrl: './grupo-form.component.html'
})



export class GrupoFormComponent implements OnInit {

	titulo:string = "Visualizar Grupo";

	modoEdicao: boolean = false;

	grupoForm: FormGroup;
	hasFormErrors: boolean = false;

	loadingSalvar:boolean = false;

	grupo: Observable<Grupo>;
	listaServicos: Servico[] = [];
	listaRotinas: Rotina[] = [];
	listaOperacoes: Operacao[] = [];

	listaRotinaOperacoesSelecionadas: RotinaOperacoes[] = [];

	cdSrvSelecionado = 0;
	cdRotSelecionada = 0;

	@ViewChild('content8', {static: true}) private modalSalvando: TemplateRef<any>;
	@ViewChild('content12', {static: true}) private modalExcluindo: TemplateRef<any>;

	constructor(
		private modalService: NgbModal,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private grupoFB: FormBuilder,
		private grupoService: GrupoService,
		private operacaoService: OperacaoService,
		private rotinaService: RotinaService,
		private servicoService: ServicoService
	) { }

	ngOnInit() {

		this.grupoForm = this.grupoFB.group({
			CD_GRP: [''],
			TX_DSC: ['', Validators.required],
		});

		this.activatedRoute.params.subscribe(params => {
			let id = params['id'] && params['id'] > 0 ? params['id'] : 0;

			if(id == 0){
				this.modoEdicao = true;
				this.titulo = "Cadastrar Grupo";
			}

			this.grupo = this.grupoService.getGrupo(id).pipe(
				tap(grupo => {

					this.grupoForm.patchValue(grupo);
					this.montarTabelas(grupo);
				})
			);

		});

		this.servicoService.getServicos().subscribe(servicos => {
			this.listaServicos = servicos;
		});
		this.rotinaService.getRotinas().subscribe(rotinas => {
			this.listaRotinas = rotinas;
		});
		this.operacaoService.getOperacoes().subscribe(operacoes => {
			this.listaOperacoes = operacoes;
		});
	}


	getTitle() {
		return "Visualizar Grupo"
	}

	exibirModal(content) {
		this.modalService.open(content);
	}

	cancelar() {
		this.router.navigateByUrl('/seguranca/grupos');
		this.modalService.dismissAll();
	}


	onSumbit(withBack: boolean = false) {
		this.hasFormErrors = false;
		const controls = this.grupoForm.controls;

		/** check form */
		if (this.grupoForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);

			this.hasFormErrors = true;
			return;
		}

		return;
	}

	salvar() {

		this.modalService.dismissAll();

		let ngbModalOptions: NgbModalOptions = {
			backdrop : 'static',
			keyboard : false
	  	};
		this.modalService.open(this.modalSalvando,ngbModalOptions);

		let grupoSalvar: Grupo = this.grupoForm.value;
		grupoSalvar.RotinaGrupoOperacao = [];

		this.listaRotinaOperacoesSelecionadas.forEach(function (rotinaOp) {

			rotinaOp.operacoes.forEach(function (op) {

				if(op.selecionada){

					let rotinaGrupoOperacaoSalvar: RotinaGrupoOperacao = new RotinaGrupoOperacao();
					rotinaGrupoOperacaoSalvar.CD_GRP = grupoSalvar.CD_GRP;
					rotinaGrupoOperacaoSalvar.CD_OPR = op.CD_OPR;
					rotinaGrupoOperacaoSalvar.CD_ROT = rotinaOp.rotina.CD_ROT;
	
					grupoSalvar.RotinaGrupoOperacao.push(rotinaGrupoOperacaoSalvar);
				}
			});
		});

		this.grupoService.salvarGrupo(grupoSalvar).subscribe(result=>{
			
			this.modalService.dismissAll();
			this.router.navigate(['/seguranca/grupos', { sucesso: "true" }]);
		})

	}

	montarTabelas(u) {

		let comp = this;

		u.RotinaGrupoOperacao.forEach(function (item) {

			if (comp.listaRotinaOperacoesSelecionadas.filter(o => o.rotina && o.rotina.CD_ROT == item.CD_ROT).length == 0) {

				let novaRotinaOperacoes = new RotinaOperacoes();
				novaRotinaOperacoes.rotina = comp.listaRotinas.find(o => o.CD_ROT == item.CD_ROT);

				comp.listaOperacoes.forEach(function (op) {

					let operacaoAdicionar = new Operacao;
					operacaoAdicionar.CD_OPR = op.CD_OPR;
					operacaoAdicionar.TX_DSC = op.TX_DSC;
					operacaoAdicionar.selecionada = false;

					novaRotinaOperacoes.operacoes.push(operacaoAdicionar);
				})

				comp.listaRotinaOperacoesSelecionadas.push(novaRotinaOperacoes);
			}
		});

		this.listaRotinaOperacoesSelecionadas.forEach(function (item) {

			item.operacoes.forEach(function (op) {

				if (u.RotinaGrupoOperacao.filter(o => o.CD_ROT == item.rotina.CD_ROT && o.CD_OPR == op.CD_OPR).length > 0) {
					op.selecionada = true;
				}
			})
		})
	}

	filtrarRotinas(cdSrv) {
		return this.listaRotinas.filter(o => o.CD_SRV == cdSrv);
	}

	adicionarRotina() {

		let rotinaOperacoes = new RotinaOperacoes();
		rotinaOperacoes.rotina = this.listaRotinas.find(o => o.CD_ROT == this.cdRotSelecionada);

		this.listaOperacoes.forEach(function (op) {

			let operacaoAdicionar = new Operacao;
			operacaoAdicionar.CD_OPR = op.CD_OPR;
			operacaoAdicionar.TX_DSC = op.TX_DSC;
			operacaoAdicionar.selecionada = false;

			rotinaOperacoes.operacoes.push(operacaoAdicionar);
		})
		this.listaRotinaOperacoesSelecionadas.push(rotinaOperacoes);
	}

	habilitarEdicao(){
		this.modalService.dismissAll();
		this.modoEdicao = true;
		this.titulo = "Editar Grupo"
	}

	excluir(){

		this.modalService.dismissAll();

		let ngbModalOptions: NgbModalOptions = {
			backdrop : 'static',
			keyboard : false
	  	};
		this.modalService.open(this.modalExcluindo,ngbModalOptions);

		let grupoExcluir: Grupo = this.grupoForm.value;

		this.grupoService.deletarGrupo(grupoExcluir.CD_GRP).subscribe(result=>{
			
			this.modalService.dismissAll();
			this.router.navigate(['/seguranca/grupos', { excluido: "true" }]);
		})
	}
}