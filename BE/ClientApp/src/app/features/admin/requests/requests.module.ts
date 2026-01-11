import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { RequestListComponent } from './request-list/request-list.component';

const routes: Routes = [
    { path: '', component: RequestListComponent }
];

@NgModule({
    declarations: [
        RequestListComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild(routes)
    ]
})
export class RequestsModule { }
