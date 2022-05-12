import { NgModule } from '@angular/core';

import { FilterByCategory } from './filterByCategory.filter';
import { CustomerByName } from './customerByName.filter';
import { FilterMenuItemName } from './FilterMenuItemName.filter';

CustomerByName

@NgModule({
    declarations: [
        FilterByCategory,
        CustomerByName,
        FilterMenuItemName
    ],
    exports: [
        FilterByCategory,
        CustomerByName,
        FilterMenuItemName
    ]
})
export class FiltersModule{}