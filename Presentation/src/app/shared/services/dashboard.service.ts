import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class DashboardService {}

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
