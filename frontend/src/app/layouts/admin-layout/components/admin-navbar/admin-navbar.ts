import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { APP_ROUTES } from '@core/constants';

@Component({
  selector: 'app-admin-navbar',
  imports: [RouterModule],
  templateUrl: './admin-navbar.html',
})
export class AdminNavbar {
  readonly ROUTES = APP_ROUTES.ADMIN;
}
