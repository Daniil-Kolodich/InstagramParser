import { inject, Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
	private readonly authService = inject(AuthenticationService);

	public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		const updatedRequest = request.clone({
			url: `https://localhost:7057/${request.url}`,
			headers: request.headers
				.append('Access-Control-Allow-Origin', '*')
				.append('Authorization', `Bearer ${this.authService.getToken()}`),
		});

		return next.handle(updatedRequest);
	}
}
