import { Component, Input, signal } from '@angular/core';
import { Register } from "../account/register/register";
import { User } from '../../types/user';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  //modo di input per passare dati da un componente padre al figlio tramite decorator (app.ts è padre, qui siamo nel figlio)
  @Input({ required: true }) membersFromApp: User[] = [];
  protected registerMode = signal(false);

  showRegister(value: boolean) {
    this.registerMode.set(value);
  }
}
