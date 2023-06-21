import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class InstagramAccountService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'instagramAccount/';

	public getByUserName(userName: string): Observable<InstagramAccountRequest> {
		const query = new HttpParams({ fromObject: { userName: userName } });
		return of();
		// TODO work here to handle lower and upper case plz
		// return this.httpClient.get<GetInstagramAccountResponse>(this.url + 'GetByName', { params: query });
	}
}

type GetInstagramAccountResponse = {
	userName: string;
	id: string;
	fullName: string;
	followersCount: number;
	followingsCount: number;
};

export type InstagramAccountRequest = {
	Id: string;
	UserName: string;
	FullName: string;
	FollowersCount: number;
	FollowingsCount: number;
};
