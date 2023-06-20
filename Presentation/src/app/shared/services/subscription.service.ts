import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})
export class SubscriptionService {
	private readonly httpClient = inject(HttpClient);
	private readonly url = 'subscription/';

	public followCheck(request: FollowCheckRequest) {
		this.httpClient.post<GetSubscriptionResponse>(this.url + 'FollowCheck', request).subscribe();
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
