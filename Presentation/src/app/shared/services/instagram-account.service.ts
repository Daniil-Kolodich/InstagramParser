import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class InstagramAccountService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'instagramAccount/';

	public getByUserName(userName: string): Observable<GetInstagramAccountResponse> {
		const query = new HttpParams({ fromObject: { userName: userName } });
		return of();
		// return this.httpClient.get<GetInstagramAccountResponse>(this.url + 'GetByName', { params: query });
	}
}

export type GetInstagramAccountResponse = {
	userName: string;
	id: string;
	fullName?: string;
	followersCount: number;
	followingsCount: number;
};
