import { Component, inject, Input, OnInit } from '@angular/core';
import { SubscriptionSource } from '../../../../shared/services/dashboard.service';
import { FormArray, FormControl } from '@angular/forms';
import { map, Observable } from 'rxjs';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-request-creation-input[form][type]',
	templateUrl: './request-creation-input.component.html',
	styleUrls: ['./request-creation-input.component.sass'],
})
export class RequestCreationInputComponent extends DestroyableComponent implements OnInit {
	private readonly httpClient = inject(HttpClient);
	@Input() public type!: string;
	@Input() public form!: [Source: FormControl<SubscriptionSource>, Accounts: FormArray<FormControl<string>>];

	public readonly searchInput = new FormControl<InstagramAccount | string>('');
	public readonly SubscriptionSource = SubscriptionSource;
	public accounts$?: Observable<InstagramAccount[]>;

	public onSearchSubmit($event: any) {
		console.log('event', $event);
	}

	public ngOnInit(): void {
		this.accounts$ = this.searchInput.valueChanges.pipe(
			map((value) => {
				const name = typeof value === 'string' ? value : value?.UserName ?? '';
				// may be includes instead of startsWith
				return accounts.filter((a) => a.UserName.startsWith(name));
			})
		);
	}

	public displayWith(account: InstagramAccount): string {
		return account.UserName;
	}
}

const accounts: InstagramAccount[] = [
	{ Id: '1', Picture: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Texas.svg', UserName: 'danon' },
	{
		Id: '2',
		Picture: 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Flag_of_Florida.svg',
		UserName: 'danissimo',
	},
	{
		Id: '3',
		Picture: 'https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_California.svg',
		UserName: 'denchick',
	},
	{ Id: '4', Picture: 'https://upload.wikimedia.org/wikipedia/commons/9/9d/Flag_of_Arkansas.svg', UserName: 'hello' },
];

type InstagramAccount = { Picture: string; UserName: string; Id: string };
