import { Component, inject, OnInit, output } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TextInputComponent } from '../../_forms/text-input/text-input.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  private accountService = inject(AccountService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  loginForm: FormGroup = new FormGroup({});
  error: string | undefined;
  cancelLogin = output<boolean>();

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      userNameOrEmail: '',
      password: ['', Validators.required]
    });
  }

  login() {
    this.error = undefined;
    this.accountService.login(this.loginForm.value).subscribe({
      next: _ => this.router.navigateByUrl('/'),
      error: _ => this.error = 'Bad credentials'
    });
  }

  cancel() {
    this.cancelLogin.emit(false);
  }
}
