import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EncounterInfoComponent } from './encounter-info.component';

describe('EncounterInfoComponent', () => {
  let component: EncounterInfoComponent;
  let fixture: ComponentFixture<EncounterInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EncounterInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EncounterInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
