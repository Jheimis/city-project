import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabelaCidadesComponent } from './tabela-cidades.component';

describe('TabelaCidadesComponent', () => {
  let component: TabelaCidadesComponent;
  let fixture: ComponentFixture<TabelaCidadesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabelaCidadesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TabelaCidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
