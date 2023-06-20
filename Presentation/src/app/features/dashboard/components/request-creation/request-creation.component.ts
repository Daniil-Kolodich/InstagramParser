import { Component, inject, OnInit } from '@angular/core';
import { FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { ControlsOf, ObservableResults } from '../../../../shared/types';
import { SourceAndAccountsControls } from '../request-creation-input/request-creation-input.component';
import {
	FollowCheckRequest,
	GetSubscriptionResponse,
	SubscriptionService,
	SubscriptionSource,
} from '../../../../shared/services/subscription.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../../../shared/services/notification.service';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { filter, map, takeUntil } from 'rxjs';
import { nonNull } from '../../../../shared/functions';

@Component({
	selector: 'app-request-creation',
	templateUrl: './request-creation.component.html',
	styleUrls: ['./request-creation.component.sass'],
})
export class RequestCreationComponent extends DestroyableComponent implements OnInit {
	private readonly router = inject(Router);
	private readonly route = inject(ActivatedRoute);
	private readonly formBuilder = inject(NonNullableFormBuilder);
	private readonly subscriptionService = inject(SubscriptionService);
	private readonly notificationService = inject(NotificationService);

	public form?: FormGroup<ControlsOf<FollowCheckRequest>>;
	public request$?: ObservableResults<GetSubscriptionResponse>;

	public ngOnInit() {
		this.form = this.formBuilder.group({
			Target: this.formBuilder.array<string>([], [Validators.required]),
			Source: this.formBuilder.array<string>([], [Validators.required]),
			SubscriptionTarget: this.formBuilder.control<SubscriptionSource>(SubscriptionSource.AccountsList, [
				Validators.required,
			]),
			SubscriptionSource: this.formBuilder.control<SubscriptionSource>(SubscriptionSource.AccountsList, [
				Validators.required,
			]),
		});
	}

	public onSubmit(): void {
		if (!this.form || this.form.invalid) {
			return;
		}

		this.request$ = this.subscriptionService.followCheck(this.form.getRawValue());

		this.request$.Value.pipe(
			takeUntil(this.destroy$),
			filter(nonNull),
			map((v) => v.id)
		).subscribe((id) => this.router.navigate(['/dashboard', id], { relativeTo: this.route }));

		this.request$.Error.pipe(takeUntil(this.destroy$), filter(nonNull)).subscribe(() =>
			this.notificationService.error('Unable to create request')
		);
	}

	public getSourceForms(form: FormGroup<ControlsOf<FollowCheckRequest>>): SourceAndAccountsControls {
		return { Source: form.controls.SubscriptionSource, Accounts: form.controls.Source };
	}

	public getTargetForms(form: FormGroup<ControlsOf<FollowCheckRequest>>): SourceAndAccountsControls {
		return { Source: form.controls.SubscriptionTarget, Accounts: form.controls.Target };
	}
}
