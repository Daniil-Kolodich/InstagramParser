import { Component, inject, OnInit } from '@angular/core';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { AuthenticationService, RegisterUserRequest } from '../../../../shared/services/authentication.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ControlsOf } from '../../../../shared/shared.module';

@Component({
	selector: 'app-sign-up',
	templateUrl: './sign-up.component.html',
	styleUrls: ['./sign-up.component.sass'],
})
export class SignUpComponent extends DestroyableComponent implements OnInit {
	private readonly authenticationService = inject(AuthenticationService);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);

	public form: FormGroup<ControlsOf<RegisterUserRequest>> | undefined;
	public loading = false;

	public ngOnInit(): void {
		this.form = this.formBuilder.nonNullable.group({
			UserName: '',
			Email: '',
			Password: '',
		});
	}

	public onSubmit(): void {
		if (!this.form || this.form.invalid) {
			return;
		}

		this.loading = true;
		this.authenticationService.register(this.form.getRawValue());
	}
}
