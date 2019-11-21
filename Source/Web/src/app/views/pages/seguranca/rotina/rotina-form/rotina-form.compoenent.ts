// Angular
import { Component, OnInit, ElementRef, ViewChild, TemplateRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RotinaService } from '../../../../../core/seguranca/rotina.service';
import { share, tap } from 'rxjs/operators';
import { Modulo } from '../../../../../core/models/modulo.model';
import { Grupo } from '../../../../../core/models/grupo.model';
import { OperacaoService } from '../../../../../core/seguranca/operacao.service';
import { ServicoService } from '../../../../../core/seguranca/servico.service';
import { Servico } from '../../../../../core/models/servico.model';
import { Rotina } from '../../../../../core/models/rotina.model';
import { Operacao } from '../../../../../core/models/operacao.model';
import { RotinaOperacoes } from '../../../../../core/models/rotina-operacoes.model';
import { RotinaAssociada } from '../../../../../core/models/rotina-associada.model';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-rotina-form',
	templateUrl: './rotina-form.component.html'
})



export class RotinaFormComponent implements OnInit {

	titulo:string = "Visualizar Rotina";

	modoEdicao: boolean = false;

	rotinaForm: FormGroup;
	hasFormErrors: boolean = false;

	loadingSalvar:boolean = false;

	rotina: Observable<Rotina>;
	listaServicos: Servico[] = [];
	listaRotinas: Rotina[] = [];
	listaOperacoes: Operacao[] = [];

    listaMenu: string[] = ["usuario","rotina","grupo","parceiroNegocio","servico","detalhencm","programacaobot","moeda"];
	listaRotinaAssociadas: Rotina[] = [];

	cdSrvSelecionado = 0;
	cdRotSelecionada = 0;

	@ViewChild('content8', {static: true}) private modalSalvando: TemplateRef<any>;
	@ViewChild('content12', {static: true}) private modalExcluindo: TemplateRef<any>;

	constructor(
		private modalService: NgbModal,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private rotinaFB: FormBuilder,
		private rotinaService: RotinaService,
		private servicoService: ServicoService
	) { }

	ngOnInit() {

		this.rotinaForm = this.rotinaFB.group({
			CD_ROT: [''],
			TX_NOME: ['', Validators.required],
			TX_DSC: ['', Validators.required],
			CD_SRV: ['', Validators.required],
			TX_URL: ['', Validators.required]
		});

		this.activatedRoute.params.subscribe(params => {
			let id = params['id'] && params['id'] > 0 ? params['id'] : 0;

			if(id == 0){
				this.modoEdicao = true;
				this.titulo = "Cadastrar Rotina";
			}

			this.rotina = this.rotinaService.getRotina(id).pipe(
				tap(rotina => {

					this.rotinaForm.patchValue(rotina);

					this.rotinaService.getRotinasAssociadas(rotina.CD_ROT).subscribe(rotinasAssociadas=>{

						this.rotinaService.getRotinas().subscribe(rotinas => {
							this.listaRotinas = rotinas;
							this.montarTabelas(rotinasAssociadas);
						});

						
					});
				})
			);

		});

		this.servicoService.getServicos().subscribe(servicos => {
			this.listaServicos = servicos;
		});

	}


	getTitle() {
		return "Visualizar Rotina"
	}

	exibirModal(content) {
		this.modalService.open(content);
	}

	cancelar() {
		this.router.navigateByUrl('/seguranca/rotinas');
		this.modalService.dismissAll();
	}


	onSumbit(withBack: boolean = false) {
		this.hasFormErrors = false;
		const controls = this.rotinaForm.controls;

		/** check form */
		if (this.rotinaForm.invalid) {
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

		let rotinaSalvar: Rotina = this.rotinaForm.value;
		rotinaSalvar.RotinasAssociadas = [];

		this.listaRotinaAssociadas.forEach(function (rot) {

			let rotinaAssociadaSalvar: RotinaAssociada = new RotinaAssociada();
			rotinaAssociadaSalvar.CD_ROT_PRINCIPAL = rotinaSalvar.CD_ROT;
			rotinaAssociadaSalvar.CD_ROT_ASS = rot.CD_ROT;
			rotinaSalvar.RotinasAssociadas.push(rotinaAssociadaSalvar);
		});

		this.rotinaService.salvarRotina(rotinaSalvar).subscribe(result=>{
			
			this.modalService.dismissAll();
			this.router.navigate(['/seguranca/rotinas', { sucesso: "true" }]);
		})
	}

	montarTabelas(rotinasAssociadas) {

		let comp = this;
		
		rotinasAssociadas.forEach(function (item){

			comp.listaRotinaAssociadas.push(comp.listaRotinas.find(o=>o.CD_ROT == item.CD_ROT_ASS));
		});
	}
	adicionarRotina() {

		this.listaRotinaAssociadas.push(this.listaRotinas.find(o => o.CD_ROT == this.cdRotSelecionada));
	}

	habilitarEdicao(){
		this.modalService.dismissAll();
		this.modoEdicao = true;
		this.titulo = "Editar Rotina"
	}

	excluir(){

		this.modalService.dismissAll();

		let ngbModalOptions: NgbModalOptions = {
			backdrop : 'static',
			keyboard : false
	  	};
		this.modalService.open(this.modalExcluindo,ngbModalOptions);

		let rotinaExcluir: Rotina = this.rotinaForm.value;

		this.rotinaService.deletarRotina(rotinaExcluir.CD_ROT).subscribe(result=>{
			
			this.modalService.dismissAll();
			this.router.navigate(['/seguranca/rotinas', { excluido: "true" }]);
		})
    }
    
    removerRotina(rotina, index){
		this.listaRotinaAssociadas.splice(index, 1);
    }    
}