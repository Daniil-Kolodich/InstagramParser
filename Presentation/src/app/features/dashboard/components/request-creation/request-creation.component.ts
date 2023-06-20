import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { ControlsOf } from '../../../../shared/types';
import { DashboardService } from '../../../../shared/services/dashboard.service';
import { SourceAndAccountsControls } from '../request-creation-input/request-creation-input.component';
import {
	FollowCheckRequest,
	SubscriptionService,
	SubscriptionSource,
} from '../../../../shared/services/subscription.service';

@Component({
	selector: 'app-request-creation',
	templateUrl: './request-creation.component.html',
	styleUrls: ['./request-creation.component.sass'],
})
export class RequestCreationComponent implements OnInit {
	private readonly formBuilder = inject(NonNullableFormBuilder);
	private readonly dashboardService = inject(DashboardService);
	private readonly subscriptionService = inject(SubscriptionService);

	public form?: FormGroup<ControlsOf<FollowCheckRequest>>;

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

		this.subscriptionService.followCheck(this.form.getRawValue());
	}

	public getSourceForms(form: FormGroup<ControlsOf<FollowCheckRequest>>): SourceAndAccountsControls {
		return { Source: form.controls.SubscriptionSource, Accounts: form.controls.Source };
	}

	public getTargetForms(form: FormGroup<ControlsOf<FollowCheckRequest>>): SourceAndAccountsControls {
		return { Source: form.controls.SubscriptionTarget, Accounts: form.controls.Target };
	}
}
