import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthenticationModule } from './features/authentication/authentication.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
	bootstrap: [AppComponent],
	declarations: [AppComponent],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		RouterOutlet,
		RouterModule.forRoot([]),
		AuthenticationModule,
		NgbModule,
		HttpClientModule,
	],
	providers: [],
})
export class AppModule {}
