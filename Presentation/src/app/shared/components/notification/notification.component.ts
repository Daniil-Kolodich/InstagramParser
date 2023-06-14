import { AfterViewInit, Component, inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { Notification } from '../../services/notification.service';
import { endWith, finalize, Observable, scan, takeWhile, timer } from 'rxjs';

@Component({
	selector: 'app-notification',
	templateUrl: './notification.component.html',
	styleUrls: ['./notification.component.sass'],
})
export class NotificationComponent implements AfterViewInit {
	private readonly snackBarRef: MatSnackBarRef<NotificationComponent> = inject(MatSnackBarRef);
	public readonly notification: Notification = inject(MAT_SNACK_BAR_DATA);
	public countdown$: Observable<number> | undefined;

	public ngAfterViewInit() {
		this.countdown$ = timer(0, 200).pipe(
			scan((acc) => acc - 5, 100),
			takeWhile((x) => x > 0),
			endWith(0),
			finalize(() => this.snackBarRef._dismissAfter(200))
		);
	}
}
