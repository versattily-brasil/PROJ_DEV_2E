import { MatPaginatorIntl } from '@angular/material';

export function PaginatorPortuguesConfig() {
    const customPaginatorIntl = new MatPaginatorIntl();

    customPaginatorIntl.itemsPerPageLabel = 'Registros por página:';
    customPaginatorIntl.nextPageLabel = 'Próxima';
    customPaginatorIntl.previousPageLabel = 'Anterior';
    customPaginatorIntl.firstPageLabel = 'Primeira Página';
    customPaginatorIntl.lastPageLabel = 'Última Página'

    return customPaginatorIntl;
}