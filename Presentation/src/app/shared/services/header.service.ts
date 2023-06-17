import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class HeaderService {
	private readonly _title$ = new Subject<string | null>();
	private readonly _subtitle$ = new Subject<string | null>();

	public title$() {
		return this._title$.asObservable();
	}

	public subtitle$() {
		return this._subtitle$.asObservable();
	}

	public setTitle(title: string | null, subtitle: string | null) {
		this._title$.next(title);
		this._subtitle$.next(subtitle);
	}

	public reset() {
		this._title$.next(null);
		this._subtitle$.next(null);
	}
}
