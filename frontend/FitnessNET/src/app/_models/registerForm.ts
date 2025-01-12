export class RegisterForm {
    username: string;
    email: string;
    name: string;
    surname: string;
    gender: string;
    height: number;
    weight: number;
    password: string;
  
    constructor(
      username: string = '',
      email: string = '',
      name: string = '',
      surname: string = '',
      gender: string = 'Male',
      height: number = 0,
      weight: number = 0,
      password: string = ''
    ) {
      this.username = username;
      this.email = email;
      this.name = name;
      this.surname = surname;
      this.gender = gender;
      this.height = height;
      this.weight = weight;
      this.password = password;
    }
  }
  