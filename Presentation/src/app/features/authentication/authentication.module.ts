import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogInComponent } from './components/log-in/log-in.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { Route, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthenticationComponent } from './components/authentication/authentication.component';
import { SharedModule } from '../../shared/shared.module';

export const authenticationRoute: Route = {
	path: 'auth',
	component: AuthenticationComponent,
	children: [
		{ path: 'login', component: LogInComponent },
		{ path: 'signup', component: SignUpComponent },
		{ path: '**', redirectTo: 'login' },
		{ path: '', redirectTo: 'login', pathMatch: 'full' },
	],
};

@NgModule({
	declarations: [LogInComponent, SignUpComponent, AuthenticationComponent],
	imports: [
		CommonModule,
		MatButtonModule,
		MatIconModule,
		MatTooltipModule,
		RouterModule.forChild([]),
		MatCardModule,
		MatProgressBarModule,
		MatDividerModule,
		ReactiveFormsModule,
		MatFormFieldModule,
		MatInputModule,
		SharedModule,
	],
})
export class AuthenticationModule {}
