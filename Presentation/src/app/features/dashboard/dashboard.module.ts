import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { RouterLink, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { authenticatedGuard } from '../../shared/guards/authenticated.guard';
import { RequestCreationComponent } from './components/request-creation/request-creation.component';
import { RequestHistoryComponent } from './components/request-history/request-history.component';
import { RequestStatisticsComponent } from './components/request-statistics/request-statistics.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

export const dashboardRoutes: Routes = [
	{
		path: 'dashboard',
		component: DashboardComponent,
		canActivate: [authenticatedGuard],
	},
	{
		path: 'dashboard/create',
		component: RequestCreationComponent,
		canActivate: [authenticatedGuard],
	},
];

@NgModule({
	declarations: [DashboardComponent, RequestCreationComponent, RequestHistoryComponent, RequestStatisticsComponent],
	imports: [CommonModule, SharedModule, MatCardModule, MatButtonModule, RouterLink],
})
export class DashboardModule {}
