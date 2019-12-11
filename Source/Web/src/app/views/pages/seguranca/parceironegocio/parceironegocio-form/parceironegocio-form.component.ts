// Angular
import { Component, OnInit, ElementRef, ViewChild, TemplateRef, ChangeDetectionStrategy, OnDestroy, ChangeDetectorRef } from '@angular/core';
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
import { PermissaoService } from '../../../../../core/seguranca/permissao.service';
import { AutenticacaoService } from '../../../../../core/autenticacao/autenticacao.service';
import { Permissao } from '../../../../../core/models/permissao.model';
import { ToastrService } from 'ngx-toastr';


@Component({
	// tslint:disable-next-line:component-selector
	selector: 'versattily-parceironegocio-form',
	templateUrl: './parceironegocio-form.component.html'
})



export class ParceiroNegocioFormComponent implements OnInit {

	titulo: string = "Visualizar Parceiro de Negócio";

	nomeRotina: string = "Parceiro de Negócio";
	permissoes: Array<Permissao>;

	modoEdicao: boolean = false;

	parceironegocioForm: FormGroup;
	hasFormErrors: boolean = false;

	loadingSalvar: boolean = false;

	parceironegocio: Observable<ParceiroNegocio>;
	listaServicos: Servico[] = [];
	listaModulos: Modulo[] = [];

	listaParceiroServicoModulo: ParceiroModuloServico[] = [];

	cdSrvSelecionado = 0;
	cdModSelecionado = 0;

	public mask = [/[0-9]/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/];

	@ViewChild('content8', { static: true }) private modalSalvando: TemplateRef<any>;
	@ViewChild('content12', { static: true }) private modalExcluindo: TemplateRef<any>;

	constructor(
		private modalService: NgbModal,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private parceironegocioFB: FormBuilder,
		private parceironegocioService: ParceiroNegocioService,
		private servicoService: ServicoService,
		private permissaoService: PermissaoService,
		private auth: AutenticacaoService,
		private moduloService: ModuloService,
		private toast: ToastrService,
		private cd: ChangeDetectorRef,
	) { }

	ngOnInit() {

		this.parceironegocioForm = this.parceironegocioFB.group({
			CD_PAR: [''],
			TXT_RZSOC: ['', Validators.required],
			CNPJ: ['', Validators.required],
			TX_EMAIL: ['', Validators.required]
		});

		let id = this.parceironegocioService.cdPrnVisualizar;

		if (id == 0) {
			this.modoEdicao = true;
			this.titulo = "Cadastrar Parceiro de Negócio";
		}

		this.parceironegocio = this.parceironegocioService.getParceiroNegocio(id).pipe(
			tap(parceironegocio => {

				this.parceironegocioForm.patchValue(parceironegocio);

				this.servicoService.getServicos().subscribe(servicos => {
					this.listaServicos = servicos;

					this.moduloService.getModulos().subscribe(modulos => {
						this.listaModulos = modulos;
						this.montarTabelas(parceironegocio.ParceiroNegocioServicoModulo);
					});
				});
			})
		);

		this.carregarPermissoes();
	}


	getTitle() {
		return "Visualizar Parceiro de Negócio"
	}

	exibirModal(content) {
		this.modalService.open(content);
	}

	exibirModalSalvar(content) {

		this.hasFormErrors = false;
		const controls = this.parceironegocioForm.controls;

		/** check form */
		if (this.parceironegocioForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			this.toast.error("Realize as correções no formulário e tente novamente.", 'Não é possível salvar o parceiro de negócio');
			this.hasFormErrors = true;
			return;
		}else{
			this.modalService.open(content);
		}

		
	}

	cancelar() {
		this.parceironegocioService.telaLista = true;
		// this.router.navigateByUrl('/seguranca/parceironegocio');
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
			backdrop: 'static',
			keyboard: false
		};
		this.modalService.open(this.modalSalvando, ngbModalOptions);

		let parceironegocioSalvar: ParceiroNegocio = this.parceironegocioForm.value;
		parceironegocioSalvar.ParceiroNegocioServicoModulo = this.listaParceiroServicoModulo;

		this.parceironegocioService.salvarParceiroNegocio(parceironegocioSalvar).subscribe(result => {

			this.modalService.dismissAll();
			this.parceironegocioService.telaLista = true;
			this.toast.success("Registro salvo com sucesso!", 'Notificação');
			this.cd.markForCheck();
			// this.router.navigate(['/seguranca/parceironegocio', { sucesso: "true" }]);
		})
	}

	montarTabelas(ParceiroModuloServicos) {

		let comp = this;

		ParceiroModuloServicos.forEach(function (item) {

			let novo: ParceiroModuloServico = new ParceiroModuloServico();
			novo.CD_MOD = item.CD_MOD;
			novo.CD_SRV = item.CD_SRV;
			novo.NomeModulo = comp.listaModulos.find(o => o.CD_MOD == item.CD_MOD).TX_DSC;
			novo.NomeServico = comp.listaServicos.find(o => o.CD_SRV == item.CD_SRV).TXT_DEC;

			comp.listaParceiroServicoModulo.push(novo);
		});
	}
	adicionarParceiroModuloServico() {
		if (this.cdModSelecionado && this.cdSrvSelecionado) {
			let novo: ParceiroModuloServico = new ParceiroModuloServico();
			novo.CD_MOD = this.cdModSelecionado;
			novo.CD_SRV = this.cdSrvSelecionado;
			novo.NomeModulo = this.listaModulos.find(o => o.CD_MOD == this.cdModSelecionado).TX_DSC;
			novo.NomeServico = this.listaServicos.find(o => o.CD_SRV == this.cdSrvSelecionado).TXT_DEC;

			this.listaParceiroServicoModulo.push(novo);
		}

	}

	habilitarEdicao() {
		this.modalService.dismissAll();
		this.modoEdicao = true;
		this.titulo = "Editar Parceiro de Negócio"
	}

	excluir() {

		this.modalService.dismissAll();

		let ngbModalOptions: NgbModalOptions = {
			backdrop: 'static',
			keyboard: false
		};
		this.modalService.open(this.modalExcluindo, ngbModalOptions);

		let parceironegocioExcluir: ParceiroNegocio = this.parceironegocioForm.value;

		this.parceironegocioService.deletarParceiroNegocio(parceironegocioExcluir.CD_PAR).subscribe(result => {

			this.modalService.dismissAll();
			this.parceironegocioService.telaLista = true;
			this.toast.success("Registro excluído com sucesso!", 'Notificação');
			this.cd.markForCheck();
			// this.router.navigate(['/seguranca/parceironegocio', { excluido: "true" }]);
		})
	}

	removerParceiroServicoModulo(parceironegocio, index) {
		this.listaParceiroServicoModulo.splice(index, 1);
	}

	//-------------------------------------------------------------------------------------------------
	// Método para carregar as permissões da página----------------------------------------------------
	//-------------------------------------------------------------------------------------------------
	carregarPermissoes() {
		this.permissaoService.getPermissoes(this.auth.idUsuario, this.nomeRotina).subscribe(permissao => {
			this.permissoes = permissao;
			this.cd.markForCheck();
			console.log(this.permissoes);
		});
	}

	//-------------------------------------------------------------------------------------------------
	// Método para verificar a permissão sobre componente----------------------------------------------
	//-------------------------------------------------------------------------------------------------
	verificarPermissao(acao: string) {

		if (this.permissoes === undefined || this.permissoes === null || this.permissoes.length === 0) {
			return false;
		}

		var encontrou = this.permissoes.filter(filtro => filtro.TX_DSC === acao);


		if (encontrou === undefined || encontrou === null || encontrou.length === 0) {
			return false;
		}
		else {
			return true;
		}
	}
}