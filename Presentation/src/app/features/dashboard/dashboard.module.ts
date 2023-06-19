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
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ReactiveFormsModule } from '@angular/forms';
import { RequestCreationInputComponent } from './components/request-creation-input/request-creation-input.component';
import { MatInputModule } from '@angular/material/input';

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
	declarations: [
		DashboardComponent,
		RequestCreationComponent,
		RequestHistoryComponent,
		RequestStatisticsComponent,
		RequestCreationInputComponent,
	],
	imports: [
		CommonModule,
		SharedModule,
		MatCardModule,
		MatButtonModule,
		RouterLink,
		MatIconModule,
		MatFormFieldModule,
		MatSelectModule,
		MatAutocompleteModule,
		ReactiveFormsModule,
		MatInputModule,
	],
})
export class DashboardModule {}
