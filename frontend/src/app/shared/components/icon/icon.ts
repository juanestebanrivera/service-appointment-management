import { Component, input } from '@angular/core';

@Component({
  selector: 'app-icon',
  templateUrl: './icon.html',
})
export class Icon {
  name = input.required<string>();
}
