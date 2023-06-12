import { Component, inject, OnInit } from '@angular/core';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { AuthenticationResponse, AuthenticationService } from '../../../../shared/services/authentication.service';
import { Router } from '@angular/router';
import { filter, takeUntil } from 'rxjs';
import { nonNull } from '../../../../shared/shared.module';

@Component({
	selector: 'app-authentication',
	templateUrl: './authentication.component.html',
	styleUrls: ['./authentication.component.sass'],
})
export class AuthenticationComponent extends DestroyableComponent implements OnInit {
	private readonly authenticationService = inject(AuthenticationService);
	private readonly router: Router = inject(Router);

	public ngOnInit(): void {
		this.authenticationService
			.authenticationResults$()
			.Value.pipe(filter(nonNull), takeUntil(this.destroy$))
			.subscribe(this.onAuthentication);
	}

	private onAuthentication(value: AuthenticationResponse) {
		this.authenticationService.setToken(value);
		this.router.navigate(['']);
	}
}
