import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CidadesComponent } from './components/cidades/cidades.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EstadosComponent } from './components/estados/estados.component';


const routes: Routes = [
  {path: '', redirectTo: 'dashboard' , pathMatch: 'full'},
  {path:'dashboard', component: DashboardComponent},
  {path:'estado', component: EstadosComponent},
  {path:'cidade', component: CidadesComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
