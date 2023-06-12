import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthenticationModule, authenticationRoutes } from './features/authentication/authentication.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationComponent } from './features/authentication/components/authentication/authentication.component';
import { ApiInterceptor } from './shared/interceptors/api.interceptor';

@NgModule({
	bootstrap: [AppComponent],
	declarations: [AppComponent],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		RouterOutlet,
		RouterModule.forRoot([{ path: 'auth', component: AuthenticationComponent, children: authenticationRoutes }]),
		AuthenticationModule,
		NgbModule,
		HttpClientModule,
	],
	providers: [
		{
			provide: HTTP_INTERCEPTORS,
			useClass: ApiInterceptor,
			multi: true,
		},
	],
})
export class AppModule {}
