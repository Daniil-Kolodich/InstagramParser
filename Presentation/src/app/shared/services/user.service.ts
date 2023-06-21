import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReplaySubject } from 'rxjs';
import { SubjectResults, ObservableResults } from '../types';
import { observe, process } from '../functions';

@Injectable({
	providedIn: 'root',
})
export class UserService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'user/';

	private readonly _user$: SubjectResults<GetUserResponse> = {
		Value: new ReplaySubject<GetUserResponse | null>(),
		Error: new ReplaySubject<unknown | null>(),
		Loading: new ReplaySubject<boolean>(),
	};

	public user$(): ObservableResults<GetUserResponse> {
		return observe(this._user$);
	}

	public getUserById(): void {
		const response = this.httpClient.get<GetUserResponse>(this.url + 'GetById');
		process(response, this._user$);
	}
}

export type GetUserResponse = {
	userName: string;
};
