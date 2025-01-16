import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../_services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class PersonalComponent implements OnInit {
  profileForm: FormGroup;
  currentUser: any;
  isEditing = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) {
    this.profileForm = this.fb.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      gender: [0],
      height: ['', [Validators.min(0), Validators.max(300)]],
      weight: ['', [Validators.min(0), Validators.max(500)]],
      description: ['']
    });
  }

  ngOnInit() {
    this.loadUserData();
  }

  loadUserData() {
    this.userService.getProfile().subscribe({
      next: (user) => {
        this.currentUser = user;
        this.profileForm.patchValue({
          name: user.name,
          surname: user.surname,
          email: user.email,
          gender: user.gender,
          height: user.height,
          weight: user.weight,
          description: user.description
        });
      },
      error: (error) => {
        this.snackBar.open('Error loading profile data', 'Close', { duration: 3000 });
      }
    });
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.loadUserData(); // Reset form if canceling edit
    }
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.userService.updateProfile(this.profileForm.value).subscribe({
        next: () => {
          this.snackBar.open('Profile updated successfully', 'Close', { duration: 3000 });
          this.isEditing = false;
          this.loadUserData();
        },
        error: (error) => {
          this.snackBar.open('Error updating profile', 'Close', { duration: 3000 });
        }
      });
    }
  }

  getInitials(): string {
    if (this.currentUser?.name && this.currentUser?.surname) {
      return (this.currentUser.name[0] + this.currentUser.surname[0]).toUpperCase();
    }
    return 'U';
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file && file.type.match(/image\/jpe?g/)) {
      this.userService.updateProfilePicture(file).subscribe({
        next: () => {
          this.snackBar.open('Profile picture updated', 'Close', { duration: 3000 });
          this.loadUserData();
        },
        error: (error) => {
          this.snackBar.open('Error updating profile picture', 'Close', { duration: 3000 });
        }
      });
    } else {
      this.snackBar.open('Please select a JPEG image', 'Close', { duration: 3000 });
    }
  }

  removePhoto() {
    this.userService.removeProfilePicture().subscribe({
      next: () => {
        this.snackBar.open('Profile picture removed', 'Close', { duration: 3000 });
        this.loadUserData();
      },
      error: (error) => {
        this.snackBar.open('Error removing profile picture', 'Close', { duration: 3000 });
      }
    });
  }
}
