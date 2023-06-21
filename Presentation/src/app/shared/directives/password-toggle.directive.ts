import { AfterContentInit, ContentChild, Directive, Renderer2 } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';

@Directive({
	selector: '[appPasswordToggle]',
})
export class PasswordToggleDirective implements AfterContentInit {
	@ContentChild(MatIcon, { static: false }) private icon: MatIcon | undefined;
	@ContentChild(MatInput, { static: false }) private input: MatInput | undefined;

	private passwordShown = false;

	public constructor(private readonly renderer: Renderer2) {}

	public ngAfterContentInit(): void {
		if (this.icon && this.input) {
			this.icon.fontIcon = 'visibility';
			this.input.type = 'password';

			this.renderer.listen(this.icon._elementRef.nativeElement, 'click', () => this.toggleVisibility());
		} else {
			console.error(
				`Unable to apply password toggling: icon ${this.icon ? 'exists' : 'missing'}, input ${
					this.input ? 'exists' : 'missing'
				}`
			);
		}
	}

	private toggleVisibility(): void {
		this.passwordShown = !this.passwordShown;
		this.icon!.fontIcon = this.passwordShown ? 'visibility_off' : 'visibility';
		this.input!.type = this.passwordShown ? 'text' : 'password';
	}
}
