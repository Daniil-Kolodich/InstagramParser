import { Component, inject, Input, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormControl, NonNullableFormBuilder, ValidationErrors } from '@angular/forms';
import {
	catchError,
	debounceTime,
	filter,
	finalize,
	map,
	of,
	skip,
	startWith,
	Subject,
	switchMap,
	takeUntil,
} from 'rxjs';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import {
	InstagramAccountService,
	InstagramAccountRequest,
	toRequest,
} from '../../../../shared/services/instagram-account.service';
import { ObservableResults } from '../../../../shared/types';
import { SubscriptionSource } from '../../../../shared/services/subscription.service';
import { isString } from '../../../../shared/functions';

@Component({
	selector: 'app-request-creation-input[form][type]',
	templateUrl: './request-creation-input.component.html',
	styleUrls: ['./request-creation-input.component.sass'],
})
export class RequestCreationInputComponent extends DestroyableComponent implements OnInit {
	private readonly formBuilder = inject(NonNullableFormBuilder);
	private readonly instagramAccountService = inject(InstagramAccountService);

	@Input() public type!: string;
	@Input() public form!: SourceAndAccountsControls;

	public readonly searchInput = new FormControl<InstagramAccountRequest | string>('', [correctInstagramAccount]);
	public readonly SubscriptionSource = SubscriptionSource;

	private notFound$ = new Subject<unknown | null>();
	private loading$ = new Subject<boolean>();
	public account$: ObservableResults<InstagramAccountRequest> = {
		Value: of(null),
		Error: this.notFound$.asObservable(),
		Loading: this.loading$.asObservable(),
	};
	public ngOnInit(): void {
		this.account$.Value = this.searchInput.valueChanges.pipe(
			takeUntil(this.destroy$),
			debounceTime(1000),
			filter(isString),
			filter((value) => value.length > 0),
			switchMap((value) => {
				this.notFound$.next(null);
				this.loading$.next(true);

				return this.instagramAccountService.getByUserName(value).pipe(
					map(toRequest),
					finalize(() => this.loading$.next(false)),
					catchError(() => {
						this.notFound$.next({});
						return of(null);
					})
				);
			})
		);
	}
	public addAccount(account: InstagramAccountRequest | string | null) {
		if (account === null || isString(account)) {
			return;
		}

		this.account$.Value = this.account$.Value.pipe(skip(1));
		this.searchInput.reset('', { emitEvent: true });

		this.form.Accounts.push(this.formBuilder.control(account));
	}

	public removeControl(control: FormControl<InstagramAccountRequest>) {
		const index = this.form.Accounts.controls.indexOf(control);
		this.form.Accounts.removeAt(index, { emitEvent: true });
	}

	public displayWith(account: InstagramAccountRequest): string {
		return account.UserName;
	}

	public getLinkedAccounts(): number {
		let sum = 0;
		for (const control of this.form.Accounts.controls) {
			sum +=
				this.form.Source.value === SubscriptionSource.AccountsFollowers
					? control.value.FollowersCount
					: control.value.FollowingsCount;
		}

		return sum;
	}

	public isString = isString;
}

export type SourceAndAccountsControls = {
	Source: FormControl<SubscriptionSource>;
	Accounts: FormArray<FormControl<InstagramAccountRequest>>;
};

function correctInstagramAccount(control: AbstractControl<InstagramAccountRequest | string>): ValidationErrors | null {
	return isString(control.value) && control.value.length > 0 ? { invalidAccount: { value: control.value } } : null;
}
