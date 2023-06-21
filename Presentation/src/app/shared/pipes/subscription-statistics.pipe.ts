import { inject, Pipe, PipeTransform } from '@angular/core';
import { GetSubscriptionResponse, SubscriptionStatus } from '../services/subscription.service';
import { DatePipe } from '@angular/common';

@Pipe({
	name: 'subscriptionStatistics',
})
export class SubscriptionStatisticsPipe implements PipeTransform {
	private readonly datePipe = inject(DatePipe);
	public transform(request: GetSubscriptionResponse[], ...args: unknown[]): { Title: string; Description: string }[] {
		return [
			{
				Title: 'Total requests',
				Description: request.length.toString(),
			},
			{
				Title: 'Last request',
				Description: this.datePipe.transform(request[0]?.createdAt, 'medium') ?? 'Not create yet',
			},
			{
				Title: 'Completed requests',
				Description: request.filter((r) => r.status === SubscriptionStatus.Completed).length.toString(),
			},
			{
				Title: 'Pending requests',
				Description: request
					.filter(
						(r) =>
							r.status === SubscriptionStatus.Pending ||
							r.status === SubscriptionStatus.ReadyForProcessing
					)
					.length.toString(),
			},
			{
				Title: 'Active requests',
				Description: request.filter((r) => r.status === SubscriptionStatus.Active).length.toString(),
			},
			// { Title: 'Average source accounts count', Description: SubscriptionType[request.type] },
			// { Title: 'Request status', Description: SubscriptionStatus[request.status] },
		];
	}
}
