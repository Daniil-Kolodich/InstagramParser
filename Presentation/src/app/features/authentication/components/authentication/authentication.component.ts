import { Component, inject, OnInit } from '@angular/core';
import { DestroyableComponent } from '../../../../shared/components/destroyable.component';
import { AuthenticationService } from '../../../../shared/services/authentication.service';
import { Router } from '@angular/router';
import { filter, takeUntil } from 'rxjs';

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
			.authorized$()
			.pipe(
				filter((value) => value),
				takeUntil(this.destroy$)
			)
			.subscribe(() => this.router.navigate(['']));
	}
}
