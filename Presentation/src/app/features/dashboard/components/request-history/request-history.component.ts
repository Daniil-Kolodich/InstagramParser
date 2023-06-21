import { Component, inject, OnInit } from '@angular/core';
import {
	GetSubscriptionResponse,
	SubscriptionService,
	SubscriptionStatus,
} from '../../../../shared/services/subscription.service';
import { ObservableResults } from '../../../../shared/types';

@Component({
	selector: 'app-request-history',
	templateUrl: './request-history.component.html',
	styleUrls: ['./request-history.component.sass'],
})
export class RequestHistoryComponent implements OnInit {
	private readonly subscriptionService = inject(SubscriptionService);

	public subscriptions$!: ObservableResults<GetSubscriptionResponse[]>;

	public ngOnInit() {
		this.subscriptions$ = this.subscriptionService.subscriptions$();

		this.subscriptionService.getAll();
	}

	protected readonly SubscriptionStatus = SubscriptionStatus;
}
