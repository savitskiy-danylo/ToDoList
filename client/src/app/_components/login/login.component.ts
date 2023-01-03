import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit
{
  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });
  constructor (
    private toastr: ToastrService,
    private accountService: AccountService,
    private router: Router
  )
  {
  }

  ngOnInit(): void
  {
  }

  isValid(control: FormControl)
  {
    if ((control.dirty || control.touched) &&
      control.errors) {
      return false;
    } else {
      return true;
    }
  }

  login()
  {
    if (this.loginForm.valid)
      this.accountService.login({
        userName: this.loginForm.controls['username'].value,
        Password: this.loginForm.controls['password'].value
      }).subscribe({
        next: (response) =>
        {

          let user: User | undefined;
          this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: currentUser =>
            {
              if (currentUser)
                user = currentUser;
            }
          })
          if (this.accountService.isLoggedIn() && user) {
            this.toastr.success(`Welcome back ${user.userName}!`);
            this.router.navigateByUrl('');
          }
        }
      })
  }
}
