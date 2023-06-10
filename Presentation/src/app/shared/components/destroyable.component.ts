import { Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
	template: '',
})
export abstract class DestroyableComponent implements OnDestroy {
	protected readonly destroy$ = new Subject<unknown>();
	public ngOnDestroy(): void {
		this.destroy$.next(undefined);
		this.destroy$.complete();
	}
}
