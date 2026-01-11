import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from './users-routing.module';
import { UserListComponent } from './user-list/user-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    UserListComponent,
    UserFormComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    SharedModule
  ]
})
export class UsersModule { }
