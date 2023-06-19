import { Component, inject, Input, OnInit } from '@angular/core';
import { SubscriptionSource } from '../../../../shared/services/dashboard.service';
import {
	AbstractControl,
	FormArray,
	FormControl,
	NonNullableFormBuilder,
	ValidationErrors,
	ValidatorFn,
} from '@angular/forms';
import { catchError, debounceTime, filter, finalize, of, startWith, Subject, switchMap, takeUntil } from 'rxjs';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import {
	GetInstagramAccountResponse,
	InstagramAccountService,
} from '../../../../shared/services/instagram-account.service';
import { ObservableResults } from '../../../../shared/types';

@Component({
	selector: 'app-request-creation-input[form][type]',
	templateUrl: './request-creation-input.component.html',
	styleUrls: ['./request-creation-input.component.sass'],
})
export class RequestCreationInputComponent extends DestroyableComponent implements OnInit {
	private readonly formBuilder = inject(NonNullableFormBuilder);
	private readonly instagramAccountService = inject(InstagramAccountService);

	@Input() public type!: string;
	@Input() public form!: [Source: FormControl<SubscriptionSource>, Accounts: FormArray<FormControl<string>>];

	public readonly searchInput = new FormControl<GetInstagramAccountResponse | string>('', [correctInstagramAccount]);
	public readonly SubscriptionSource = SubscriptionSource;
	public account$?: ObservableResults<GetInstagramAccountResponse>;
	private notFound$ = new Subject<unknown | null>();
	private loading$ = new Subject<boolean>();
	public ngOnInit(): void {
		this.account$ = {
			Error: this.notFound$,
			Loading: this.loading$,
			Value: this.searchInput.valueChanges.pipe(
				takeUntil(this.destroy$),
				debounceTime(300),
				filter(isString),
				switchMap((value) => {
					this.notFound$.next(null);
					this.loading$.next(true);

					return this.instagramAccountService.getByUserName(value).pipe(
						startWith({ userName: 'danon', id: '111' }),
						finalize(() => this.loading$.next(false)),
						catchError(() => {
							this.notFound$.next({});
							return of(null);
						})
					);
				})
			),
		};
	}

	public addAccount() {
		console.log('form value', this.searchInput.value, this.searchInput.value);
	}

	public displayWith(account: GetInstagramAccountResponse): string {
		return account.userName;
	}
}

function correctInstagramAccount(
	control: AbstractControl<GetInstagramAccountResponse | string>
): ValidationErrors | null {
	return isString(control.value) ? { invalidAccount: { value: control.value } } : null;
}

function isString<T>(value: T | string | null): value is string {
	return typeof value === 'string';
}
