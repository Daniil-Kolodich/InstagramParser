import { Component, inject, OnInit } from '@angular/core';
import {
	AuthenticationResponse,
	AuthenticationService,
	LoginUserRequest,
} from '../../../../shared/services/authentication.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { filter, tap } from 'rxjs';
import { NotificationService } from '../../../../shared/services/notification.service';
import { ControlsOf, ObservableResults } from '../../../../shared/types';
import { nonNull } from '../../../../shared/functions';

@Component({
	selector: 'app-log-in',
	templateUrl: './log-in.component.html',
	styleUrls: ['./log-in.component.sass'],
})
export class LogInComponent extends DestroyableComponent implements OnInit {
	private readonly authenticationService = inject(AuthenticationService);
	private readonly notificationService = inject(NotificationService);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);

	public form: FormGroup<ControlsOf<LoginUserRequest>> | undefined;
	public result$: ObservableResults<AuthenticationResponse> | undefined;
	public ngOnInit(): void {
		this.form = this.formBuilder.nonNullable.group<LoginUserRequest>({
			Email: '',
			Password: '',
		});

		this.result$ = this.authenticationService.authenticationResults$();

		this.result$.Error = this.result$.Error.pipe(
			filter(nonNull),
			tap(() => this.notificationService.error('Invalid Email or Password'))
		);
	}

	public onSubmit(): void {
		if (!this.form || this.form.invalid) {
			return;
		}

		this.authenticationService.login(this.form.getRawValue());
	}
}
