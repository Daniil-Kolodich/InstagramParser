import { finalize, Observable } from 'rxjs';
import { ObservableResults, SubjectResults } from './types';

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
	subject.Value.next(null);
	subject.Error.next(null);

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
