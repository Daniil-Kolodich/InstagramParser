import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService {
	public ymp: string | undefined = undefined;
}

// LoginUserRequest(string Email, string Password);
type LoginUserRequest = {
	Email: string;
	Password: string;
};

// RegisterUserRequest(string UserName, string Email, string Password)
type RegisterUserRequest = {
	UserName: string;
	Email: string;
	Password: string;
};
