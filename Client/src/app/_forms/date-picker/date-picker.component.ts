import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [MatDatepickerModule, MatNativeDateModule, MatInputModule, MatFormFieldModule, ReactiveFormsModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss',
})
export class DatePickerComponent implements ControlValueAccessor {
  label = input<string>('');

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(value: any): void {
    
  }

  registerOnChange(fn: any): void {
    
  }

  registerOnTouched(fn: any): void {
    
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
