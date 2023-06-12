import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, shareReplay, Subject, tap } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ObservableResults, observe, process, SubjectResults } from '../shared.module';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService {
	private readonly url = 'authentication/';
	private readonly httpClient = inject(HttpClient);
	private readonly _authenticated$ = new BehaviorSubject<boolean>(false);
	private readonly _authenticationResults$: SubjectResults<AuthenticationResponse> = {
		Value: new Subject<AuthenticationResponse | null>(),
		Error: new Subject<unknown>(),
		Loading: new Subject<boolean>(),
	};

	public authenticated$(): Observable<boolean> {
		return this._authenticated$.pipe(shareReplay(1, 60));
	}

	public authenticationResults$(): ObservableResults<AuthenticationResponse> {
		return observe(this._authenticationResults$);
	}

	public setToken(value: AuthenticationResponse): void {
		localStorage.setItem('auth_token', value.Token);
		this._authenticated$.next(true);
	}

	public getToken(): string | null {
		const token = localStorage.getItem('auth_token');

		if (token === null) {
			this._authenticated$.next(false);
			return null;
		}

		return token;
	}
	public login(request: LoginUserRequest): void {
		const query = new HttpParams({ fromObject: request });
		const response = this.httpClient.get<AuthenticationResponse>(this.url + 'Get', { params: query });
		process(response, this._authenticationResults$);
	}

	public register(request: RegisterUserRequest): void {
		const response = this.httpClient.post<AuthenticationResponse>(this.url + 'Post', request);
		process(response, this._authenticationResults$);
	}
}
// LoginUserRequest(string Email, string Password);
export type LoginUserRequest = {
	Email: string;
	Password: string;
};

// RegisterUserRequest(string UserName, string Email, string Password)
export type RegisterUserRequest = {
	UserName: string;
	Email: string;
	Password: string;
};

export type AuthenticationResponse = {
	Token: string;
};
