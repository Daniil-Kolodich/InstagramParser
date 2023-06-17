import { Component, inject, OnDestroy } from '@angular/core';
import { HeaderService } from '../services/header.service';
import { DestroyableComponent } from './destroyable.component';

@Component({
	template: '',
})
export abstract class TitledComponent extends DestroyableComponent implements OnDestroy {
	protected readonly headerService = inject(HeaderService);

	public override ngOnDestroy(): void {
		console.log('on destroy from titled one');
		this.headerService.reset();
		super.ngOnDestroy();
	}
}
