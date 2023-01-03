import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate
{
  constructor (
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  canActivate(): boolean | UrlTree
  {
    if (this.accountService.isLoggedIn()) {
      return true;
    } else {
      this.toastr.error("You need to log in.")
      return this.router.parseUrl('login')
    }
  }

}
