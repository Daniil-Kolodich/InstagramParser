import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})

// TODO i might need to use Subjects with like { Value<T>, Error, Loading } type to get full control
export class AuthenticationService {
	private readonly url = 'authentication/';
	private readonly httpClient = inject(HttpClient);
	private readonly _authorized$ = new BehaviorSubject<boolean>(false);

	// TODO: should this be handled with destroy?
	public authorized$(): Observable<boolean> {
		return this._authorized$.asObservable();
	}

	public login(request: LoginUserRequest): void {
		const query = new HttpParams({ fromObject: request });
		this.httpClient.get(this.url + 'Get', { params: query }).subscribe(
			(result) => this._authorized$.next(true),
			(error) => this._authorized$.next(false)
		);
	}

	public register(request: RegisterUserRequest): void {
		this.httpClient.post(this.url + 'Post', request).subscribe(
			(result) => this._authorized$.next(true),
			(error) => this._authorized$.next(false)
		);
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
