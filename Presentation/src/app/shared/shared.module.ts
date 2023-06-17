import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { NotificationComponent } from './components/notification/notification.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PasswordToggleDirective } from './directives/password-toggle.directive';

@NgModule({
	declarations: [NotificationComponent, PasswordToggleDirective],
	imports: [CommonModule, MatButtonModule, MatProgressBarModule, MatSnackBarModule],
	exports: [PasswordToggleDirective],
})
export class SharedModule {}
