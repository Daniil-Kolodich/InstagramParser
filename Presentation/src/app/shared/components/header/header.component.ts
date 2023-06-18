import { Component, inject, OnInit } from '@angular/core';
import { HeaderService } from '../../services/header.service';
import { Observable, tap } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';
import { GetUserResponse, UserService } from '../../services/user.service';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.sass'],
})
export class HeaderComponent implements OnInit {
	private readonly headerService = inject(HeaderService);
	private readonly authService = inject(AuthenticationService);
	private readonly userService = inject(UserService);
	public readonly defaultTitle = 'Instagram Assistant';

	public user$?: Observable<GetUserResponse | null>;
	public title$?: Observable<string | null>;
	public subtitle$?: Observable<string | null>;
	public authenticated$?: Observable<boolean>;

	public ngOnInit() {
		this.title$ = this.headerService.title$();
		this.subtitle$ = this.headerService.subtitle$();
		this.authenticated$ = this.authService
			.authenticated$()
			.pipe(tap((value) => value && this.userService.getUserById()));
		this.user$ = this.userService.user$().Value;
	}

	public logout() {
		this.authService.logout();
	}
}
