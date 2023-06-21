import { Pipe, PipeTransform } from '@angular/core';
import {
	GetSubscriptionResponse,
	SubscriptionSource,
	SubscriptionStatus,
	SubscriptionType,
} from '../services/subscription.service';

@Pipe({
	name: 'primarySubscriptionInfo',
})
export class PrimarySubscriptionInfoPipe implements PipeTransform {
	public transform(request: GetSubscriptionResponse, ...args: unknown[]): { Title: string; Description: string }[] {
		const infos = [
			{ Title: 'Source accounts type', Description: SubscriptionSource[request.source] },
			{ Title: 'Source accounts', Description: request.sourceAccounts.length.toString() },
			{ Title: 'Target accounts type', Description: SubscriptionSource[request.target] },
			{ Title: 'Target accounts', Description: request.targetAccounts.length.toString() },
			{ Title: 'Request type', Description: SubscriptionType[request.type] },
			{ Title: 'Request status', Description: SubscriptionStatus[request.status] },
		];

		if (request.status === SubscriptionStatus.Completed) {
			infos.push({ Title: 'Selected winners', Description: request.selectedAccounts.length.toString() });
		}

		return infos;
	}
}
