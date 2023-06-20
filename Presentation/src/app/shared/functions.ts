import { catchError, finalize, ignoreElements, map, merge, Observable, of, share, take } from 'rxjs';
import { ObservableResults, SubjectResults } from './types';

export function observe<T>(subjects: SubjectResults<T>): ObservableResults<T> {
	return {
		Value: subjects.Value.asObservable(),
		Error: subjects.Error.asObservable(),
		Loading: subjects.Loading.asObservable(),
	};
}

export function observeOnce<T>(request: Observable<T>): ObservableResults<T> {
	const observable = request.pipe(share());

	return {
		Value: observable,
		Error: observable.pipe(
			ignoreElements(),
			catchError(() => of({}))
		),
		Loading: merge(
			of(true),
			observable.pipe(
				take(1),
				catchError(() => of(false)),
				map(() => false)
			)
		),
	};
}

export function nonNull<T>(value: T | null): value is T {
	return value !== null;
}

export function process<T>(request: Observable<T>, subject: SubjectResults<T>): void {
	subject.Loading.next(true);
	subject.Value.next(null);
	subject.Error.next(null);

	request
		.pipe(
			take(1),
			finalize(() => subject.Loading.next(false))
		)
		.subscribe({
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

export function isString<T>(value: T | string | null): value is string {
	return typeof value === 'string';
}
