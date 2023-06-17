import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Route } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { authenticatedGuard } from '../../shared/guards/authenticated.guard';

export const dashboardRoute: Route = {
	path: 'dashboard',
	component: DashboardComponent,
	canActivate: [authenticatedGuard],
};

@NgModule({
	declarations: [DashboardComponent],
	imports: [CommonModule, SharedModule],
})
export class DashboardModule {}
