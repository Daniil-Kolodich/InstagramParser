import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { Observable, ReplaySubject, Subject } from 'rxjs';

export type ControlsOf<T extends Record<string, unknown>> = {
	[K in keyof T]: T[K] extends Record<any, unknown>
		? FormGroup<ControlsOf<T[K]>>
		: T[K] extends Array<infer O>
		? FormArray<FormControl<O>>
		: FormControl<T[K]>;
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

export type ExpandRecursively<T> = T extends object
	? T extends infer O
		? { [K in keyof O]: ExpandRecursively<O[K]> }
		: never
	: T;

export type Expand<T> = T extends infer O ? { [K in keyof O]: O[K] } : never;
