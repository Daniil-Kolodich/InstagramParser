import { CanActivateFn } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';
import { map } from 'rxjs';

export const authenticatedGuard: CanActivateFn = (route, state) => {
	const authService = inject(AuthenticationService);

	return authService.token$().pipe(
		map((token) => {
			if (token === null) {
				authService.logout();

				return false;
			}

			return true;
		})
	);
};
