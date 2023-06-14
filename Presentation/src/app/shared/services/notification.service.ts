import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationComponent } from '../components/notification/notification.component';

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	private readonly snackBar = inject(MatSnackBar);

	public info(message: string): void {
		this.snackBar.openFromComponent<NotificationComponent, Notification>(NotificationComponent, {
			horizontalPosition: 'right',
			verticalPosition: 'top',
			data: { Message: message, Type: 'Info' },
			panelClass: ['mat-toolbar', 'mat-primary'],
		});
	}

	public error(message: string): void {
		this.snackBar.openFromComponent<NotificationComponent, Notification>(NotificationComponent, {
			horizontalPosition: 'right',
			verticalPosition: 'top',
			data: { Message: message, Type: 'Error' },
			panelClass: ['mat-toolbar', 'mat-warn'],
		});
	}

	public success(message: string): void {
		this.snackBar.openFromComponent<NotificationComponent, Notification>(NotificationComponent, {
			horizontalPosition: 'right',
			verticalPosition: 'top',
			data: { Message: message, Type: 'Success' },
			panelClass: ['mat-toolbar', 'mat-accent'],
		});
	}
}

export type Notification = {
	Message: string;
	Type: NotificationType;
};

export type NotificationType = 'Success' | 'Info' | 'Error';
