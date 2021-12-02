import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginGuard } from './guards/login.guard';
import { LogoutGuard } from './guards/logout.guard';

const routes: Routes = [
  {
    path:'',
    redirectTo: 'account',
    pathMatch:'full'
  },
  {
    path:'weather',
    loadChildren: () => import('./weather/weather.module').then(m => m.WeatherModule),
    canActivate:[LoginGuard]
  },
  {
    path:'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
    canActivate:[LogoutGuard]
  }  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
