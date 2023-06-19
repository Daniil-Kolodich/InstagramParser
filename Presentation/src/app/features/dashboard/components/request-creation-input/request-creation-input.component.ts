import { Component, inject, Input, OnInit } from '@angular/core';
import { SubscriptionSource } from '../../../../shared/services/dashboard.service';
import { FormArray, FormControl } from '@angular/forms';
import { catchError, debounceTime, filter, Observable, of, switchMap, takeUntil } from 'rxjs';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { HttpClient } from '@angular/common/http';
import {
	GetInstagramAccountResponse,
	InstagramAccountService,
} from '../../../../shared/services/instagram-account.service';

@Component({
	selector: 'app-request-creation-input[form][type]',
	templateUrl: './request-creation-input.component.html',
	styleUrls: ['./request-creation-input.component.sass'],
})
export class RequestCreationInputComponent extends DestroyableComponent implements OnInit {
	private readonly httpClient = inject(HttpClient);
	private readonly instagramAccountService = inject(InstagramAccountService);

	@Input() public type!: string;
	@Input() public form!: [Source: FormControl<SubscriptionSource>, Accounts: FormArray<FormControl<string>>];

	public readonly searchInput = new FormControl<GetInstagramAccountResponse | string>('');
	public readonly SubscriptionSource = SubscriptionSource;
	public account$?: Observable<GetInstagramAccountResponse | undefined>;

	public ngOnInit(): void {
		this.account$ = this.searchInput.valueChanges.pipe(
			takeUntil(this.destroy$),
			debounceTime(300),
			filter(isString),
			switchMap((value) =>
				this.instagramAccountService.getByUserName(value).pipe(catchError(() => of(undefined)))
			)
		);
	}

	public displayWith(account: GetInstagramAccountResponse): string {
		return account.userName;
	}
}

function isString<T>(value: T | string | null): value is string {
	return typeof value === 'string';
}
