import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup } from '@angular/forms';
import { finalize, Observable, Subject } from 'rxjs';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { NotificationComponent } from './components/notification/notification.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
	declarations: [NotificationComponent],
	imports: [CommonModule, MatSnackBarModule, MatButtonModule, MatProgressBarModule],
	exports: [],
})
export class SharedModule {}

export type ControlsOf<T extends Record<string, unknown>> = {
	[K in keyof T]: T[K] extends Record<any, unknown> ? FormGroup<ControlsOf<T[K]>> : FormControl<T[K]>;
};

export type SubjectResults<T> = {
	Value: Subject<T | null>;
	Error: Subject<unknown | null>;
	Loading: Subject<boolean>;
};

export type ObservableResults<T> = {
	Value: Observable<T | null>;
	Error: Observable<unknown | null>;
	Loading: Observable<boolean>;
};

export function observe<T>(subjects: SubjectResults<T>): ObservableResults<T> {
	return {
		Value: subjects.Value.asObservable(),
		Error: subjects.Error.asObservable(),
		Loading: subjects.Loading.asObservable(),
	};
}

export function nonNull<T>(value: T | null): value is T {
	return value !== null;
}

export function process<T>(request: Observable<T>, subject: SubjectResults<T>): void {
	subject.Loading.next(true);

	request.pipe(finalize(() => subject.Loading.next(false))).subscribe({
		next: (result) => {
			subject.Value.next(result);
			subject.Error.next(null);
		},
		error: () => {
			subject.Value.next(null);
			subject.Error.next({});
		},
	});
}
