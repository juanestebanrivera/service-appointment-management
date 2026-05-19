import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { APP_ROUTES } from '@core/constants';
import { Icon } from '@shared/components/icon/icon';

@Component({
  selector: 'app-home',
  imports: [RouterLink, Icon],
  templateUrl: './home.html',
})
export class Home {
  readonly ROUTES = APP_ROUTES;
}
