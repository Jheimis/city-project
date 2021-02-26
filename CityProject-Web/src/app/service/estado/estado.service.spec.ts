/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { EstadoService } from './estado.service';

describe('Service: Estado', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EstadoService]
    });
  });

  it('should ...', inject([EstadoService], (service: EstadoService) => {
    expect(service).toBeTruthy();
  }));
});
