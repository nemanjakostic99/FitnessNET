import { RegisterForm } from "../_models/registerForm";

export class RegisterValidator {
    static validate(user: RegisterForm
    ): string | null {
      // Username validation
      if (!user.username || user.username.trim().length < 3) {
        return 'Username must be at least 3 characters long.';
      }
  
      // Email validation (basic regex)
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!user.email || !emailRegex.test(user.email)) {
        return 'Please provide a valid email address.';
      }
  
      // Name and surname validation
      if (!user.name || user.name.trim().length === 0) {
        return 'First name cannot be empty.';
      }
      if (!user.surname || user.surname.trim().length === 0) {
        return 'Last name cannot be empty.';
      }
  
      // Gender validation
      const validGenders = [0, 1];
      if (!user.gender || !validGenders.includes(user.gender)) {
        return 'Gender must be either Male or Female.';
      }
  
      // Height and weight validation
      if (user.height <= 0) {
        return 'Height must be a positive number.';
      }
      if (user.weight <= 0) {
        return 'Weight must be a positive number.';
      }
  
      // Password validation
      if (!user.password || user.password.length < 6) {
        return 'Password must be at least 6 characters long.';
      }
  
      // If everything is valid, return null
      return null;
    }
  }
  