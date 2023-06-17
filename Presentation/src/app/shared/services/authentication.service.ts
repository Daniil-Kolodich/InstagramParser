import { inject, Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, shareReplay, Subject } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ObservableResults, SubjectResults } from '../types';
import { observe, process } from '../functions';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
	providedIn: 'root',
})
export class AuthenticationService implements OnDestroy {
	private _jwtHelperService = new JwtHelperService();
	private _tokenHandlerId: ReturnType<typeof setInterval> | undefined;

	private readonly url = 'authentication/';
	private readonly httpClient = inject(HttpClient);
	private readonly _token$ = new BehaviorSubject<string | null>(this.retrieveToken());
	private readonly _authenticationResults$: SubjectResults<AuthenticationResponse> = {
		Value: new Subject<AuthenticationResponse | null>(),
		Error: new Subject<unknown>(),
		Loading: new Subject<boolean>(),
	};

	public constructor() {
		console.log('ctor called for auth service');
		this._tokenHandlerId = setInterval(() => this.getToken(), 30000);
	}

	public ngOnDestroy() {
		clearTimeout(this._tokenHandlerId);
	}

	public token$(): Observable<string | null> {
		return this._token$.pipe(shareReplay(1, 120));
	}

	public authenticationResults$(): ObservableResults<AuthenticationResponse> {
		return observe(this._authenticationResults$);
	}

	private saveToken(token: string) {
		localStorage.setItem('auth_token', token);
	}

	private retrieveToken(): string | null {
		return localStorage.getItem('auth_token');
	}

	public setToken(value: AuthenticationResponse): void {
		this.saveToken(value.token);
		this._token$.next(value.token);
	}

	public getToken(): string | null {
		const token = this.retrieveToken();
		if (token === null || this._jwtHelperService.isTokenExpired(token)) {
			this._token$.next(null);
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
	token: string;
};
