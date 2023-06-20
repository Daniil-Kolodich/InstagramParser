import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { delay, repeat, Subject, tap } from 'rxjs';
import { ObservableResults, SubjectResults } from '../types';
import { observe, observeOnce, process } from '../functions';

@Injectable({
	providedIn: 'root',
})
export class SubscriptionService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'subscription/';

	private readonly subscribed$ = new Subject<unknown>();

	private readonly _subscriptions$: SubjectResults<GetSubscriptionResponse[]> = {
		Value: new Subject<GetSubscriptionResponse[] | null>(),
		Error: new Subject<unknown | null>(),
		Loading: new Subject<boolean>(),
	};

	public subscriptions$(): ObservableResults<GetSubscriptionResponse[]> {
		return observe(this._subscriptions$);
	}

	public getAll() {
		const response = this.httpClient
			.get<GetSubscriptionResponse[]>(this.url + 'All')
			.pipe(repeat({ delay: () => this.subscribed$ }));
		process(response, this._subscriptions$);
	}

	public getById(id: number): ObservableResults<GetSubscriptionResponse> {
		console.log('get by id called!!!', id);
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
	Id: number;
	Source: SubscriptionSource;
	Target: SubscriptionSource;
	Status: SubscriptionStatus;
	Type: SubscriptionType;
	Accounts: string[];
};

export type FollowCheckRequest = {
	Source: string[];
	Target: string[];
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
