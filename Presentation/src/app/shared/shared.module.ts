import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup } from '@angular/forms';

@NgModule({
	declarations: [],
	imports: [CommonModule],
})
export class SharedModule {}

export type ControlsOf<T extends Record<string, unknown>> = {
	[K in keyof T]: T[K] extends Record<any, unknown> ? FormGroup<ControlsOf<T[K]>> : FormControl<T[K]>;
};
