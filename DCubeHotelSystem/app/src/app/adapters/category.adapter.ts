import { EntityState, createEntityAdapter }     from '@ngrx/entity';
import { Category }                             from '../models/category.model';

// Entity adapter
export const CategoryAdapter = createEntityAdapter<Category>({
    selectId: (category: Category) => category.Id,
    sortComparer: false
});

export interface State extends EntityState<Category> {}

export const InitialState: State = CategoryAdapter.getInitialState();