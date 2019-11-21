export class MenuConfig {
	public defaults: any = {
		header: {
			self: {},
			items: [

			]
		},
		aside: {
			self: {},
			items: [
				// {
				// 	title: 'Dashboard',
				// 	root: true,
				// 	icon: 'flaticon2-architecture-and-city',
				// 	page: '/dashboard',
				// 	translate: 'MENU.DASHBOARD',
				// 	bullet: 'dot',
				// },
				{
					title: 'Segurança',
					root: true,
					bullet: 'dot',
					icon: 'flaticon2-user-outline-symbol',
					submenu: [
						{
							title: 'Parceiros de Negócio',
							page: '/seguranca/parceironegocio'
						},
						{
							title: 'Usuários',
							page: '/seguranca/usuarios'
						},						{
							title: 'Rotinas',
							page: '/seguranca/rotinas'
						},
						{
							title: 'Grupos de Usuários',
							page: '/seguranca/grupos'
						},						{
							title: 'Serviços',
							page: '/seguranca/servico'
						}
					]
				},
				{
					title: 'Importação',
					root: true,
					bullet: 'dot',
					icon: 'flaticon2-browser-2',
					submenu: [
						{
							title: 'Detalhes NCM',
							page: '/seguranca/parceironegocio'
						},
						{
							title: 'Moeda',
							page: '/seguranca/usuarios'
						}
					]
				},
				{
					title: 'Administrativos',
					root: true,
					bullet: 'dot',
					icon: 'flaticon2-architecture-and-city',
					submenu: [
						{
							title: 'Teste de Rotina - Leo',
							page: '/seguranca/parceironegocio'
						}
					]
				}
			]
		},
	};

	public get configs(): any {
		return this.defaults;
	}
}
