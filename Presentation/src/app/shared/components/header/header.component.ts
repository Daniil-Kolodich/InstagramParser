import { Component, inject, OnInit } from '@angular/core';
import { HeaderService } from '../../services/header.service';
import { Observable, of } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.sass'],
})
export class HeaderComponent implements OnInit {
	private readonly headerService = inject(HeaderService);
	private readonly authService = inject(AuthenticationService);
	public readonly defaultTitle = 'Instagram Assistant';

	public userName$?: Observable<string>;
	public title$?: Observable<string | null>;
	public subtitle$?: Observable<string | null>;
	public authenticated$?: Observable<boolean>;

	public ngOnInit() {
		this.title$ = this.headerService.title$();
		this.subtitle$ = this.headerService.subtitle$();
		this.userName$ = of('Danon');
		this.authenticated$ = this.authService.authenticated$();
	}

	public logout() {
		this.authService.logout();
	}
}
