import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { delay, repeat, ReplaySubject, Subject, tap } from 'rxjs';
import { ObservableResults, SubjectResults } from '../types';
import { observe, observeOnce, process } from '../functions';
import { GetInstagramAccountResponse, InstagramAccountRequest } from './instagram-account.service';

@Injectable({
	providedIn: 'root',
})
export class SubscriptionService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'subscription/';

	private readonly subscribed$ = new Subject<unknown>();

	private readonly _subscriptions$: SubjectResults<GetSubscriptionResponse[]> = {
		Value: new ReplaySubject<GetSubscriptionResponse[] | null>(1),
		Error: new ReplaySubject<unknown | null>(1),
		Loading: new ReplaySubject<boolean>(1),
	};

	public subscriptions$(): ObservableResults<GetSubscriptionResponse[]> {
		return observe(this._subscriptions$);
	}

	public getAll() {
		const response = this.httpClient
			.get<GetSubscriptionResponse[]>(this.url + 'All')
			.pipe(repeat({ delay: () => this.subscribed$ }), delay(2500));

		process(response, this._subscriptions$);
	}

	public getById(id: number): ObservableResults<GetSubscriptionResponse> {
		const query = new HttpParams({ fromObject: { id: id } });
		const response = this.httpClient
			.get<GetSubscriptionResponse>(this.url + 'Id', { params: query })
			.pipe(delay(2500));
		return observeOnce(response);
	}

	public followCheck(request: FollowCheckRequest): ObservableResults<GetSubscriptionResponse> {
		const response = this.httpClient.post<GetSubscriptionResponse>(this.url + 'FollowCheck', request);
		const result = observeOnce(response);

		result.Value = result.Value.pipe(
			tap((value) => {
				if (value !== null) {
					this.subscribed$.next({});
				}
			})
		);

		return result;
	}
}

export type GetSubscriptionResponse = {
	id: number;
	source: SubscriptionSource;
	target: SubscriptionSource;
	status: SubscriptionStatus;
	type: SubscriptionType;
	sourceAccounts: GetInstagramAccountResponse[];
	targetAccounts: GetInstagramAccountResponse[];
	selectedAccounts: GetInstagramAccountResponse[];
	createdAt: Date;
};

export type FollowCheckRequest = {
	Source: InstagramAccountRequest[];
	Target: InstagramAccountRequest[];
	SubscriptionSource: SubscriptionSource;
	SubscriptionTarget: SubscriptionSource;
};

export enum SubscriptionSource {
	AccountsFollowers,
	AccountsFollowings,
	AccountsList,
}

export enum SubscriptionStatus {
	Pending,
	ReadyForProcessing,
	Active,
	Completed,
}

export enum SubscriptionType {
	Follow = 0b1,
}
