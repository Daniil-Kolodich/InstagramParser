import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Route } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { authenticatedGuard } from '../../shared/guards/authenticated.guard';
import { RequestCreationComponent } from './components/request-creation/request-creation.component';
import { RequestHistoryComponent } from './components/request-history/request-history.component';
import { RequestStatisticsComponent } from './components/request-statistics/request-statistics.component';

export const dashboardRoute: Route = {
	path: 'dashboard',
	component: DashboardComponent,
	canActivate: [authenticatedGuard],
};

@NgModule({
	declarations: [DashboardComponent, RequestCreationComponent, RequestHistoryComponent, RequestStatisticsComponent],
	imports: [CommonModule, SharedModule],
})
export class DashboardModule {}
