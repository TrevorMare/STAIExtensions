import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';


const routes: Routes = [
  // {
  //   path: 'pages/home-page',
  //   component: HomePageComponent
  // },
  {
    path: 'pages',
    loadChildren: () => import('./pages/pages.module')
      .then(m => m.PagesModule),
  },
  // { path: '', redirectTo: 'pages', pathMatch: 'full' },
  // { path: '**', redirectTo: 'pages' },
];

const config: ExtraOptions = {
  useHash: false,
  enableTracing: false
};

@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
