import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../_services/account.service';
import { TextInputComponent } from "../../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../../_forms/date-picker/date-picker.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  private accountService = inject(AccountService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  cancelRegister = output<boolean>();
  responseError: string | undefined;
  registerForm: FormGroup = new FormGroup({});
  maxDate = new Date();

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: ['', Validators.required],
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      password: ['', [
        Validators.required, Validators.minLength(8), Validators.maxLength(32)
      ]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { isMatching: true }
    };
  }

  register() {
    const patchedForm = this.registerForm.value;
    patchedForm.dateOfBirth = this.convertDateFormat(this.registerForm.get('dateOfBirth')?.value);

    this.accountService.register(patchedForm).subscribe({
      next: _ => this.router.navigateByUrl('/'),
      error: e => this.responseError = e.error
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  private convertDateFormat(dob: string | undefined) {
    if (!dob) return;
    const date = new Date(dob);
    return date.getFullYear().toString() + '-' + (date.getMonth() + 1).toString() + '-' + date.getDate().toString();
  }
}
