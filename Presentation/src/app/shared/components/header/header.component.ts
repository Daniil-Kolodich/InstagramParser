import { Component, inject, OnInit } from '@angular/core';
import { HeaderService } from '../../services/header.service';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.sass'],
})
export class HeaderComponent implements OnInit {
	private readonly headerService = inject(HeaderService);
	public readonly defaultTitle = 'Instagram Assistant';

	public title$?: Observable<string | null>;
	public subtitle$?: Observable<string | null>;

	public ngOnInit() {
		this.title$ = this.headerService.title$();
		this.subtitle$ = this.headerService.subtitle$();
	}
}
