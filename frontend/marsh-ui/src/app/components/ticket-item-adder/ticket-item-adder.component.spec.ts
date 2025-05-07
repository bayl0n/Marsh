import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketItemAdderComponent } from './ticket-item-adder.component';

describe('TicketItemAdderComponent', () => {
  let component: TicketItemAdderComponent;
  let fixture: ComponentFixture<TicketItemAdderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketItemAdderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketItemAdderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
