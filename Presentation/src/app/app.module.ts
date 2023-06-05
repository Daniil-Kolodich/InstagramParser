import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthenticationModule } from './features/authentication/authentication.module';

@NgModule({
	declarations: [AppComponent],
	imports: [BrowserModule, RouterOutlet, RouterModule.forRoot([]), AuthenticationModule],
	providers: [],
	bootstrap: [AppComponent],
})
export class AppModule {}
