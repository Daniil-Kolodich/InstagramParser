import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ControlsOf } from '../../../../shared/types';
import {
	DashboardService,
	FollowCheckRequest,
	SubscriptionSource,
} from '../../../../shared/services/dashboard.service';
import { SourceAndAccountsControls } from '../request-creation-input/request-creation-input.component';

@Component({
	selector: 'app-request-creation',
	templateUrl: './request-creation.component.html',
	styleUrls: ['./request-creation.component.sass'],
})
export class RequestCreationComponent implements OnInit {
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private readonly dashboardService = inject(DashboardService);
	public form?: FormGroup<ControlsOf<FollowCheckRequest>>;

	public ngOnInit() {
		this.form = this.formBuilder.nonNullable.group({
			Target: this.formBuilder.nonNullable.array<string>([]),
			Source: this.formBuilder.nonNullable.array<string>([]),
			SubscriptionTarget: this.formBuilder.nonNullable.control<SubscriptionSource>(
				SubscriptionSource.AccountsList
			),
			SubscriptionSource: this.formBuilder.nonNullable.control<SubscriptionSource>(
				SubscriptionSource.AccountsList
			),
		});

		// this.form?.controls.Target.addValidators(Validators.minLength(1));
	}

	public getSourceForms(form: FormGroup<ControlsOf<FollowCheckRequest>>): SourceAndAccountsControls {
		return { Source: form.controls.SubscriptionSource, Accounts: form.controls.Source };
	}

	public getTargetForms(form: FormGroup<ControlsOf<FollowCheckRequest>>): SourceAndAccountsControls {
		return { Source: form.controls.SubscriptionTarget, Accounts: form.controls.Target };
	}
}
