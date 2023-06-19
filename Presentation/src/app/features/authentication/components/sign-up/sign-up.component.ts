import { Component, inject, OnInit } from '@angular/core';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import {
	AuthenticationResponse,
	AuthenticationService,
	RegisterUserRequest,
} from '../../../../shared/services/authentication.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ControlsOf, ObservableResults } from '../../../../shared/types';
@Component({
	selector: 'app-sign-up',
	templateUrl: './sign-up.component.html',
	styleUrls: ['./sign-up.component.sass'],
})
export class SignUpComponent extends DestroyableComponent implements OnInit {
	private readonly authenticationService = inject(AuthenticationService);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);

	public form: FormGroup<ControlsOf<RegisterUserRequest>> | undefined;
	public result$: ObservableResults<AuthenticationResponse> | undefined;

	public ngOnInit(): void {
		this.form = this.formBuilder.nonNullable.group<RegisterUserRequest>({
			UserName: '',
			Email: '',
			Password: '',
		});

		this.result$ = this.authenticationService.authenticationResults$();
	}

	public onSubmit(): void {
		if (!this.form || this.form.invalid) {
			return;
		}

		this.authenticationService.register(this.form.getRawValue());
	}
}
