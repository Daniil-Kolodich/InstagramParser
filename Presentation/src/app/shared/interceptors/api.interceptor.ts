import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
	public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
		const updatedRequest = request.clone({
			url: `https://localhost:7057/${request.url}`,
			headers: request.headers.append('Access-Control-Allow-Origin', '*'),
		});

		return next.handle(updatedRequest);
	}
}
