import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogInComponent } from './components/log-in/log-in.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { RouterModule, Routes } from '@angular/router';
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
import { MatSnackBarModule } from '@angular/material/snack-bar';

export const authenticationRoutes: Routes = [
	{ path: 'login', component: LogInComponent },
	{ path: 'signup', component: SignUpComponent },
];

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
		MatSnackBarModule,
	],
})
export class AuthenticationModule {}
