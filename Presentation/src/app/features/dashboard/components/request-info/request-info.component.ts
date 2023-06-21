import { Component, inject, OnInit } from '@angular/core';
import {
	GetSubscriptionResponse,
	SubscriptionService,
	SubscriptionStatus,
} from '../../../../shared/services/subscription.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ObservableResults } from '../../../../shared/types';
import { filter, map, take, tap } from 'rxjs';
import { isString } from '../../../../shared/functions';
import { GetInstagramAccountResponse } from '../../../../shared/services/instagram-account.service';
import { NotificationService } from '../../../../shared/services/notification.service';

@Component({
	selector: 'app-request-info',
	templateUrl: './request-info.component.html',
	styleUrls: ['./request-info.component.sass'],
})
export class RequestInfoComponent implements OnInit {
	private readonly route = inject(ActivatedRoute);
	private readonly router = inject(Router);
	private readonly notificationService = inject(NotificationService);
	private readonly subscriptionService = inject(SubscriptionService);

	public subscription$?: ObservableResults<GetSubscriptionResponse>;
	public lists?: {
		Title: string;
		Accounts: GetInstagramAccountResponse[];
	}[];
	public ngOnInit() {
		this.route.paramMap
			.pipe(
				map((v) => v.get('id')),
				filter(isString),
				take(1)
			)
			.subscribe((id) => {
				this.subscription$ = this.subscriptionService.getById(+id);
				this.subscription$.Value = this.subscription$.Value.pipe(
					tap((value) => this.getExpandableLists(value))
				);
				this.subscription$.Error = this.subscription$.Error.pipe(
					tap(() => {
						this.notificationService.error('Unable to load request data');
						this.router.navigate(['/dashboard']);
					})
				);
			});
	}

	private getExpandableLists(response: GetSubscriptionResponse | null): void {
		if (response === null) {
			return;
		}

		const result = [
			{ Title: 'Source accounts', Accounts: response.sourceAccounts },
			{ Title: 'Target accounts', Accounts: response.targetAccounts },
		];

		if (response.status === SubscriptionStatus.Completed) {
			result.push({ Title: 'Selected winners', Accounts: response.selectedAccounts });
		}

		this.lists = result;
	}

	protected readonly SubscriptionStatus = SubscriptionStatus;
}
