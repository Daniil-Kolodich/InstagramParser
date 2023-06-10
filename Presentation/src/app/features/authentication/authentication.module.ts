import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogInComponent } from './components/log-in/log-in.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

const moduleName = 'authentication/';

@NgModule({
	declarations: [LogInComponent, SignUpComponent],
	imports: [
		CommonModule,
		MatButtonModule,
		MatIconModule,
		MatTooltipModule,
		RouterModule.forChild([
			{ path: moduleName + 'login', component: LogInComponent },
			{ path: moduleName + 'signup', component: SignUpComponent },
		]),
		MatCardModule,
		MatProgressBarModule,
		MatDividerModule,
		ReactiveFormsModule,
		MatFormFieldModule,
		MatInputModule,
	],
})
export class AuthenticationModule {}
