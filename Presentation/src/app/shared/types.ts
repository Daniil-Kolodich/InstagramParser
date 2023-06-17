import { FormControl, FormGroup } from '@angular/forms';
import { Observable, Subject } from 'rxjs';

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
