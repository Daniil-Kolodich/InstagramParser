import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ControlsOf } from '../../../../shared/types';
import {
	DashboardService,
	FollowCheckRequest,
	SubscriptionSource,
} from '../../../../shared/services/dashboard.service';

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
	}

	public getSourceForms(
		form: FormGroup<ControlsOf<FollowCheckRequest>>
	): [FormControl<SubscriptionSource>, FormArray<FormControl<string>>] {
		return [form.controls.SubscriptionSource, form.controls.Source];
	}

	public getTargetForms(
		form: FormGroup<ControlsOf<FollowCheckRequest>>
	): [FormControl<SubscriptionSource>, FormArray<FormControl<string>>] {
		return [form.controls.SubscriptionTarget, form.controls.Target];
	}
}
