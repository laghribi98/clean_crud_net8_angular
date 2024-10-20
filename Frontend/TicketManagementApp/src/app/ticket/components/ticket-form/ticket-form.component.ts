import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ticket } from '../../models/ticket.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ticket-form',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './ticket-form.component.html',
  styleUrl: './ticket-form.component.css'
})
export class TicketFormComponent implements OnChanges {
  ticketForm: FormGroup;

  @Input() ticketData?: Ticket; // Input to receive ticket data for updating
  @Output() submitTicket = new EventEmitter<Ticket>();
  @Output() cancel = new EventEmitter<void>();

  constructor(private fb: FormBuilder) {
    this.ticketForm = this.fb.group({
      ticketId: [null], // ticketId will only be set during update
      description: ['', Validators.required],
      status: ['', Validators.required]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['ticketData'] && this.ticketData) {
      let statusId = this.ticketData.status;
      console.log(statusId);

      this.ticketForm.patchValue({
        ticketId: this.ticketData.ticketId,
        description: this.ticketData.description,
        status: statusId
      });
    } else if (!this.ticketData) {
      // Reset the form if no ticket data is provided (for creating new ticket)
      this.ticketForm.reset();
    }
  }

  onSubmit(): void {
    if (this.ticketForm.valid) {
      // Convert the status back to string for frontend use
      const formValue = { ...this.ticketForm.value };
      formValue.status = formValue.status;
      this.submitTicket.emit(formValue);
      this.ticketForm.reset();
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
