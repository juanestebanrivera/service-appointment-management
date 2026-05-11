import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AdminNavbar } from './components/admin-navbar/admin-navbar';
import { AdminHeader } from './components/admin-header/admin-header';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterOutlet, AdminNavbar, AdminHeader],
  templateUrl: './admin-layout.html',
})
export class AdminLayout {}
