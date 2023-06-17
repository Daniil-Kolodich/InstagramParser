import { CanActivateFn, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';
import { map } from 'rxjs';

export const authenticatedGuard: CanActivateFn = (route, state) => {
	const authService = inject(AuthenticationService);
	const router = inject(Router);
	return authService.token$().pipe(
		map((token) => {
			if (token === null) {
				console.log('token expired mate');
				router.navigate(['auth/login']);

				return false;
			}

			return true;
		})
	);
};
