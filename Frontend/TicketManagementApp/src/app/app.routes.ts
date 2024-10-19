import { Routes } from '@angular/router';
import { TicketAddComponent } from './ticket/components/ticket-add/ticket-add.component';
import { TicketEditComponent } from './ticket/components/ticket-edit/ticket-edit.component';
import { TicketListComponent } from './ticket/components/ticket-list/ticket-list.component';

export const routes: Routes = [
  { path: 'tickets', component: TicketListComponent },         
  { path: 'tickets/add', component: TicketAddComponent },         
  { path: 'tickets/edit/:id', component: TicketEditComponent },  
];
