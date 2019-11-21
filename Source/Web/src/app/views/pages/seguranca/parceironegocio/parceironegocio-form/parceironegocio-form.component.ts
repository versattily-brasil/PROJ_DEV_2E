// Angular
import { Component, OnInit, ElementRef, ViewChild, TemplateRef, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ParceiroNegocioService } from '../../../../../core/seguranca/parceironegocio.service';
import { share, tap } from 'rxjs/operators';
import { Modulo } from '../../../../../core/models/modulo.model';
import { ServicoService } from '../../../../../core/seguranca/servico.service';
import { Servico } from '../../../../../core/models/servico.model';
import { ParceiroNegocio } from '../../../../../core/models/parceironegocio.model';
import { ModuloService } from '../../../../../core/seguranca/modulo.service';
import { ParceiroModuloServico } from '../../../../../core/models/parceiro-modulo-servico.model';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-parceironegocio-form',
	templateUrl: './parceironegocio-form.component.html'
})



export class ParceiroNegocioFormComponent implements OnInit {

	titulo:string = "Visualizar ParceiroNegocio";

	modoEdicao: boolean = false;

	parceironegocioForm: FormGroup;
	hasFormErrors: boolean = false;

	loadingSalvar:boolean = false;

	parceironegocio: Observable<ParceiroNegocio>;
    listaServicos: Servico[] = [];
    listaModulos: Modulo[]=[];

	listaParceiroServicoModulo:ParceiroModuloServico[]=[];	

	cdSrvSelecionado = 0;
	cdModSelecionado = 0;

	@ViewChild('content8', {static: true}) private modalSalvando: TemplateRef<any>;
	@ViewChild('content12', {static: true}) private modalExcluindo: TemplateRef<any>;

	constructor(
		private modalService: NgbModal,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private parceironegocioFB: FormBuilder,
		private parceironegocioService: ParceiroNegocioService,
        private servicoService: ServicoService,
        private moduloService:ModuloService
	) { }

	ngOnInit() {

		this.parceironegocioForm = this.parceironegocioFB.group({
			CD_PAR: [''],
			TXT_RZSOC: ['', Validators.required],
			CNPJ: ['', Validators.required],
			TX_EMAIL: ['', Validators.required]
		});

		this.activatedRoute.params.subscribe(params => {
			let id = params['id'] && params['id'] > 0 ? params['id'] : 0;

			if(id == 0){
				this.modoEdicao = true;
				this.titulo = "Cadastrar ParceiroNegocio";
			}

			this.parceironegocio = this.parceironegocioService.getParceiroNegocio(id).pipe(
				tap(parceironegocio => {

					this.parceironegocioForm.patchValue(parceironegocio);

					this.servicoService.getServicos().subscribe(servicos=>{
                        this.listaServicos=servicos;

						this.moduloService.getModulos().subscribe(modulos => {
                            this.listaModulos=modulos;							
							 this.montarTabelas(parceironegocio.ParceiroNegocioServicoModulo);
						});						
					});
				})
			);
		});
	}


	getTitle() {
		return "Visualizar ParceiroNegocio"
	}

	exibirModal(content) {
		this.modalService.open(content);
	}

	cancelar() {
		this.router.navigateByUrl('/seguranca/parceironegocio');
		this.modalService.dismissAll();
	}


	onSumbit(withBack: boolean = false) {
		this.hasFormErrors = false;
		const controls = this.parceironegocioForm.controls;

		/** check form */
		if (this.parceironegocioForm.invalid) {
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

		let parceironegocioSalvar: ParceiroNegocio = this.parceironegocioForm.value;
		parceironegocioSalvar.ParceiroNegocioServicoModulo=this.listaParceiroServicoModulo;
		
		this.parceironegocioService.salvarParceiroNegocio(parceironegocioSalvar).subscribe(result=>{
			
			this.modalService.dismissAll();
			this.router.navigate(['/seguranca/parceironegocio', { sucesso: "true" }]);
		})
	}

	montarTabelas(ParceiroModuloServicos) {

		let comp = this;
		
		ParceiroModuloServicos.forEach(function (item){

			let novo : ParceiroModuloServico = new ParceiroModuloServico();
			novo.CD_MOD=item.CD_MOD;
			novo.CD_SRV=item.CD_SRV;
			novo.NomeModulo = comp.listaModulos.find(o=>o.CD_MOD==item.CD_MOD).TX_DSC;
			novo.NomeServico = comp.listaServicos.find(o=>o.CD_SRV==item.CD_SRV).TXT_DEC;	

			comp.listaParceiroServicoModulo.push(novo);
		});
	}
	adicionarParceiroModuloServico() {

		let novo : ParceiroModuloServico = new ParceiroModuloServico();
			novo.CD_MOD=this.cdModSelecionado;
			novo.CD_SRV=this.cdSrvSelecionado;
			novo.NomeModulo = this.listaModulos.find(o=>o.CD_MOD==this.cdModSelecionado).TX_DSC;
			novo.NomeServico = this.listaServicos.find(o=>o.CD_SRV==this.cdSrvSelecionado).TXT_DEC;	

			this.listaParceiroServicoModulo.push(novo);
	}

	habilitarEdicao(){
		this.modalService.dismissAll();
		this.modoEdicao = true;
		this.titulo = "Editar Parceiro Negocio"
	}

	excluir(){

		this.modalService.dismissAll();

		let ngbModalOptions: NgbModalOptions = {
			backdrop : 'static',
			keyboard : false
	  	};
		this.modalService.open(this.modalExcluindo,ngbModalOptions);

		let parceironegocioExcluir: ParceiroNegocio = this.parceironegocioForm.value;

		this.parceironegocioService.deletarParceiroNegocio(parceironegocioExcluir.CD_PAR).subscribe(result=>{
			
			this.modalService.dismissAll();
			this.router.navigate(['/seguranca/parceironegocio', { excluido: "true" }]);
		})
    }
    
    removerParceiroServicoModulo(parceironegocio, index){
		this.listaParceiroServicoModulo.splice(index, 1);
    }    
}