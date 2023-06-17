import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authenticationRoute } from './features/authentication/authentication.module';
import { dashboardRoute } from './features/dashboard/dashboard.module';

const routes: Routes = [authenticationRoute, dashboardRoute];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
