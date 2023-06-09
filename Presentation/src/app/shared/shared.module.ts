import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { NotificationComponent } from './components/notification/notification.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PasswordToggleDirective } from './directives/password-toggle.directive';
import { HeaderComponent } from './components/header/header.component';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FooterComponent } from './components/footer/footer.component';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { PrimarySubscriptionInfoPipe } from './pipes/primary-subscription-info.pipe';
import { SubscriptionStatisticsPipe } from './pipes/subscription-statistics.pipe';

@NgModule({
	declarations: [
		NotificationComponent,
		PasswordToggleDirective,
		HeaderComponent,
		FooterComponent,
		PrimarySubscriptionInfoPipe,
		SubscriptionStatisticsPipe,
	],
	imports: [
		CommonModule,
		MatButtonModule,
		MatProgressBarModule,
		MatSnackBarModule,
		MatCardModule,
		MatToolbarModule,
		MatIconModule,
		RouterLink,
	],
	providers: [DatePipe],
	exports: [
		PasswordToggleDirective,
		HeaderComponent,
		FooterComponent,
		PrimarySubscriptionInfoPipe,
		SubscriptionStatisticsPipe,
	],
})
export class SharedModule {}
