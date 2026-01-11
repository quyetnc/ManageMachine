import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminSettingsComponent } from './admin-settings/admin-settings.component';

const routes: Routes = [
    { path: '', component: AdminSettingsComponent }
];

@NgModule({
    declarations: [
        AdminSettingsComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild(routes)
    ]
})
export class SettingsModule { }
