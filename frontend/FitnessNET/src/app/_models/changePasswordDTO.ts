export class ChangePasswordDTO {
    newPassword: string;
    oldPassword: string;
  
    constructor(
      newPassword: string = '',
      oldPassword: string = '',
    ) {
      this.newPassword = newPassword;
      this.oldPassword = oldPassword;
    }
  }
  