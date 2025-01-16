export class RegisterForm {
    username: string;
    email: string;
    name: string;
    surname: string;
    gender: number;
    height: number;
    weight: number;
    isTrainer: boolean;
    password: string;
  
    constructor(
      username: string = '',
      email: string = '',
      name: string = '',
      surname: string = '',
      gender: number = 0,
      height: number = 0,
      weight: number = 0,
      isTrainer: boolean = false,
      password: string = ''
    ) {
      this.username = username;
      this.email = email;
      this.name = name;
      this.surname = surname;
      this.gender = gender;
      this.height = height;
      this.weight = weight;
      this.isTrainer = isTrainer;
      this.password = password;
    }
  }
  