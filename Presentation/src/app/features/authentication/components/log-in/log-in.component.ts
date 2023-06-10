import { Component, inject, OnInit } from '@angular/core';
import { AuthenticationService, LoginUserRequest } from '../../../../shared/services/authentication.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ControlsOf } from '../../../../shared/shared.module';
import { filter, takeUntil } from 'rxjs';
import { tap } from 'rxjs/operators';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { Router } from '@angular/router';

@Component({
	selector: 'app-log-in',
	templateUrl: './log-in.component.html',
	styleUrls: ['./log-in.component.sass'],
})
export class LogInComponent extends DestroyableComponent implements OnInit {
	private readonly authenticationService = inject(AuthenticationService);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private readonly router: Router = inject(Router);

	public form: FormGroup<ControlsOf<LoginUserRequest>> | undefined;
	public loading = false;

	public ngOnInit(): void {
		this.authenticationService
			.authorized$()
			.pipe(
				tap(() => (this.loading = false)),
				filter((value) => value),
				takeUntil(this.destroy$)
			)
			.subscribe(() => this.router.navigate(['']));

		this.form = this.formBuilder.nonNullable.group({
			Email: '',
			Password: '',
		});
	}

	public onSubmit(): void {
		if (!this.form || this.form.invalid) {
			return;
		}

		this.loading = true;
		this.authenticationService.login(this.form.getRawValue());
	}
}
