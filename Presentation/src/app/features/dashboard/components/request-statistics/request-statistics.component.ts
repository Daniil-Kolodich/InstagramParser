import { Component, inject, OnInit } from '@angular/core';
import { GetSubscriptionResponse, SubscriptionService } from '../../../../shared/services/subscription.service';
import { ObservableResults } from '../../../../shared/types';

@Component({
	selector: 'app-request-statistics',
	templateUrl: './request-statistics.component.html',
	styleUrls: ['./request-statistics.component.sass'],
})
export class RequestStatisticsComponent implements OnInit {
	private readonly subscriptionService = inject(SubscriptionService);

	public subscriptions$?: ObservableResults<GetSubscriptionResponse[]>;

	public ngOnInit() {
		this.subscriptions$ = this.subscriptionService.subscriptions$();
	}
}
