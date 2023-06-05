import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogInComponent } from './components/log-in/log-in.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { RouterModule } from '@angular/router';

const moduleName = 'authentication/';

@NgModule({
	declarations: [LogInComponent, SignUpComponent],
	imports: [
		CommonModule,
		RouterModule.forChild([
			{ path: moduleName + 'login', component: LogInComponent },
			{ path: moduleName + 'signup', component: SignUpComponent },
		]),
	],
})
export class AuthenticationModule {}
