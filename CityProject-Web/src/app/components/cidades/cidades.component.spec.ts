import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CidadesComponent } from './cidades.component';

describe('CidadesComponent', () => {
  let component: CidadesComponent;
  let fixture: ComponentFixture<CidadesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CidadesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
