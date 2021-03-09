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
//ordem
import { DashboardComponent, NgbdSortableHeader } from './components/dashboard/dashboard.component';
import { TituloComponent } from './components/titulo/titulo.component';
import { EstadosComponent } from './components/estados/estados.component';
import { CidadesComponent } from './components/cidades/cidades.component';

import { OnlyNumberDirective } from './directive/only-number.directive';

import {LOCALE_ID, DEFAULT_CURRENCY_CODE} from '@angular/core';
import localePt from '@angular/common/locales/pt';
import {registerLocaleData} from '@angular/common';
//pesquisa
import { ListFilterPipe } from './components/dashboard/listFilterPipe';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

registerLocaleData(localePt, 'pt');

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    TituloComponent,
    EstadosComponent,
    CidadesComponent,
    NgbdSortableHeader,
    OnlyNumberDirective,
    ListFilterPipe
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
    NgbModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }
    ),
 
  ],
  exports: [DashboardComponent],
  providers: [
  {
      provide: LOCALE_ID,
      useValue: 'pt'
  },

  /* if you don't provide the currency symbol in the pipe, 
  this is going to be the default symbol (R$) ... */
  {
      provide:  DEFAULT_CURRENCY_CODE,
      useValue: 'BRL'
  },
    ],
    
  bootstrap: [AppComponent]
})
export class AppModule { }

