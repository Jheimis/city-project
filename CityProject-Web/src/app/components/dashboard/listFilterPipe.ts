import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'listFilter'})
export class ListFilterPipe implements PipeTransform {
    // if(filterText != null){
    transform(list: any[], filterText: string): any {
        return list ? list.filter(item => item.nome.search(new RegExp(filterText, 'i')) > -1) : [];
    }
    // }
}
