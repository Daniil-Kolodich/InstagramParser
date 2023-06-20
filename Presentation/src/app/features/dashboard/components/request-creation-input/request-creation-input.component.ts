import { Component, inject, Input, OnInit } from '@angular/core';
import {
	AbstractControl,
	FormArray,
	FormControl,
	NonNullableFormBuilder,
	ValidationErrors,
	ValidatorFn,
} from '@angular/forms';
import {
	catchError,
	debounceTime,
	EMPTY,
	filter,
	finalize,
	flatMap,
	of,
	skip,
	startWith,
	Subject,
	switchMap,
	takeUntil,
	mergeMap,
	tap,
} from 'rxjs';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import {
	GetInstagramAccountResponse,
	InstagramAccountService,
} from '../../../../shared/services/instagram-account.service';
import { ObservableResults } from '../../../../shared/types';
import { SubscriptionSource } from '../../../../shared/services/subscription.service';

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

	public readonly accountControls = new FormArray<FormControl<GetInstagramAccountResponse>>([]);
	public readonly searchInput = new FormControl<GetInstagramAccountResponse | string>('', [correctInstagramAccount]);
	public readonly SubscriptionSource = SubscriptionSource;

	private notFound$ = new Subject<unknown | null>();
	private loading$ = new Subject<boolean>();
	public account$: ObservableResults<GetInstagramAccountResponse> = {
		Value: of(null),
		Error: this.notFound$.asObservable(),
		Loading: this.loading$.asObservable(),
	};
	public ngOnInit(): void {
		this.accountControls.valueChanges
			.pipe(
				takeUntil(this.destroy$),
				tap(() => this.form.Accounts.clear()),
				mergeMap((value) => value)
			)
			.subscribe((value) => {
				this.form.Accounts.push(this.formBuilder.control(value.id));
			});

		this.addAccount({ userName: 'danon', id: '1dn', fullName: 'Daniil Kolodich' });
		this.addAccount({ userName: 'danissimo', id: '2do', fullName: 'Daniil Mishenin' });
		this.addAccount({ userName: 'koshkina', id: '3ka', fullName: 'Nastya Shiba' });
		this.addAccount({ userName: 'yarick', id: '4yk', fullName: '9rolsav Ozimok' });
		this.addAccount({ userName: 'kser', id: '5kr', fullName: 'Yura Chirko' });
		this.addAccount({ userName: 'gleb', id: '6gb', fullName: 'Gleb Punko' });
		this.addAccount({ userName: 'shevchik', id: '7sk', fullName: 'Masksim Shev4ik' });

		this.account$.Value = this.searchInput.valueChanges.pipe(
			takeUntil(this.destroy$),
			debounceTime(300),
			filter(isString),
			filter((value) => value.length > 0),
			switchMap((value) => {
				this.notFound$.next(null);
				this.loading$.next(true);

				return this.instagramAccountService.getByUserName(value).pipe(
					startWith({ userName: value, id: '111' + value, fullName: 'FullName of' + value }),
					finalize(() => this.loading$.next(false)),
					catchError(() => {
						this.notFound$.next({});
						return of(null);
					})
				);
			})
		);
	}

	public addAccount(account: GetInstagramAccountResponse | string | null) {
		if (account === null || isString(account)) {
			return;
		}

		this.account$.Value = this.account$.Value.pipe(skip(1));
		this.searchInput.reset('', { emitEvent: true });
		this.accountControls.push(this.formBuilder.control(account));
	}

	public removeControl(control: FormControl<GetInstagramAccountResponse>) {
		const index = this.accountControls.controls.indexOf(control);
		this.accountControls.removeAt(index, { emitEvent: true });
	}

	public displayWith(account: GetInstagramAccountResponse): string {
		return account.userName;
	}

	protected readonly isString = isString;
}

export type SourceAndAccountsControls = {
	Source: FormControl<SubscriptionSource>;
	Accounts: FormArray<FormControl<string>>;
};

function correctInstagramAccount(
	control: AbstractControl<GetInstagramAccountResponse | string>
): ValidationErrors | null {
	return isString(control.value) && control.value.length > 0 ? { invalidAccount: { value: control.value } } : null;
}

function isString<T>(value: T | string | null): value is string {
	return typeof value === 'string';
}
