import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { NotificationComponent } from './components/notification/notification.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { PasswordToggleDirective } from './directives/password-toggle.directive';
import { HeaderComponent } from './components/header/header.component';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FooterComponent } from './components/footer/footer.component';

@NgModule({
	declarations: [NotificationComponent, PasswordToggleDirective, HeaderComponent, FooterComponent],
	imports: [CommonModule, MatButtonModule, MatProgressBarModule, MatSnackBarModule, MatCardModule, MatToolbarModule],
	exports: [PasswordToggleDirective, HeaderComponent, FooterComponent],
})
export class SharedModule {}
