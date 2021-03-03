import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { CommonModule } from '@angular/common';

import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TituloComponent } from './components/titulo/titulo.component';
import { EstadosComponent } from './components/estados/estados.component';
import { CidadesComponent } from './components/cidades/cidades.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    TituloComponent,
    EstadosComponent,
    CidadesComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ModalModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    TooltipModule.forRoot(),
    CommonModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }
    ),
 
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
