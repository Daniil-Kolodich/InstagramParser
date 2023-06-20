import { Component, inject, OnInit } from '@angular/core';
import {
	GetSubscriptionResponse,
	SubscriptionService,
	SubscriptionSource,
	SubscriptionStatus,
	SubscriptionType,
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

	public infos(request: GetSubscriptionResponse): { Title: string; Description: string }[] {
		return [
			{ Title: 'Source accounts type', Description: SubscriptionSource[request.source] },
			{ Title: 'Source accounts', Description: request.sourceAccounts.toString() },
			{ Title: 'Target accounts type', Description: SubscriptionSource[request.target] },
			{ Title: 'Target accounts', Description: request.target.toString() },
			{ Title: 'Request type', Description: SubscriptionType[request.type] },
			{ Title: 'Request status', Description: SubscriptionStatus[request.status] },
			{ Title: 'Selected winners', Description: request.selectedAccounts.length.toString() },
		];
	}

	protected readonly SubscriptionStatus = SubscriptionStatus;
}
