import { Component, OnInit } from '@angular/core';
import { TitledComponent } from '../../../../shared/components/titled.component';

@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.sass'],
})
export class DashboardComponent extends TitledComponent implements OnInit {
	public ngOnInit(): void {
		this.headerService.setTitle(null, 'Dashboard');
	}
}
