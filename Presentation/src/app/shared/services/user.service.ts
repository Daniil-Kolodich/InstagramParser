import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { SubjectResults, ObservableResults } from '../types';
import { observe, process } from '../functions';

@Injectable({
	providedIn: 'root',
})
export class UserService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'users/';

	private readonly _user$: SubjectResults<GetUserResponse> = {
		Value: new Subject<GetUserResponse | null>(),
		Error: new Subject<unknown | null>(),
		Loading: new Subject<boolean>(),
	};

	public user$(): ObservableResults<GetUserResponse> {
		return observe(this._user$);
	}

	public getUserById(): void {
		console.log('getting value');
		const response = this.httpClient.get<GetUserResponse>(this.url + 'GetById');
		process(response, this._user$);
	}
}

export type GetUserResponse = {
	userName: string;
};
