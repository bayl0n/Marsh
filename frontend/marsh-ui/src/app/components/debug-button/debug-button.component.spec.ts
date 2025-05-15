import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DebugButtonComponent } from './debug-button.component';

describe('DebugButtonComponent', () => {
  let component: DebugButtonComponent;
  let fixture: ComponentFixture<DebugButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DebugButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DebugButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
