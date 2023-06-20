import { Component, inject, OnInit } from '@angular/core';
import { GetSubscriptionResponse, SubscriptionService } from '../../../../shared/services/subscription.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ObservableResults } from '../../../../shared/types';
import { filter, map, take, tap } from 'rxjs';
import { isString } from '../../../../shared/functions';

@Component({
	selector: 'app-request-info',
	templateUrl: './request-info.component.html',
	styleUrls: ['./request-info.component.sass'],
})
export class RequestInfoComponent implements OnInit {
	private readonly route = inject(ActivatedRoute);
	private readonly router = inject(Router);
	private readonly subscriptionService = inject(SubscriptionService);

	public subscription$?: ObservableResults<GetSubscriptionResponse>;
	public ngOnInit() {
		this.route.paramMap
			.pipe(
				map((v) => v.get('id')),
				filter(isString),
				take(1)
			)
			.subscribe((id) => (this.subscription$ = this.subscriptionService.getById(+id)));
	}
}
