import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds, User } from '../../../types/user';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  //modo di input per passare dati da un componente padre al figlio tramite signal (home.ts è il padre, qui siamo nel figlio)
  membersFromHome = input.required<User[]>();
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register() {
    console.log(this.creds);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
