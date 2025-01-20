import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../_services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ImageCropperComponent, ImageCroppedEvent } from 'ngx-image-cropper';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ImageCropperComponent]
})
export class PersonalComponent implements OnInit {
  profileForm: FormGroup;
  currentUser: any;
  isEditing = false;
  profilePicture: string = 'assets/images/default-avatar.png';
  imageChangedEvent: any = '';
  croppedImage: any = '';
  showCropper = false;

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
    this.loadProfilePicture();
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

  onFileChange(event: any): void {
    this.imageChangedEvent = event;
    this.showCropper = true;
  }

  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.blob;
  }

  saveCroppedImage() {
    console.log('Saving cropped image...');
    if (!this.croppedImage) {
      this.snackBar.open('No image selected', 'Close', { duration: 3000 });
      return;
    }

    this.userService.updateProfilePicture(this.croppedImage).subscribe({
      next: () => {
        console.log('Profile picture updated successfully');
        this.snackBar.open('Profile picture updated', 'Close', { duration: 3000 });
        this.loadProfilePicture();
        this.showCropper = false;
      },
      error: (err) => {
        console.error('Error updating profile picture:', err);
        this.snackBar.open('Error updating profile picture', 'Close', { duration: 3000 });
      }
    });
  }

  private base64ToFile(base64: string, filename: string): File {
    const byteString = atob(base64);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new File([ab], filename, { type: 'image/jpeg' });
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

  private loadProfilePicture(): void {
    this.userService.getProfilePicture().subscribe({
      next: (blob: Blob) => {
        if (blob.size > 0) {
          const reader = new FileReader();
          reader.onloadend = () => {
            this.profilePicture = reader.result as string;
          };
          reader.readAsDataURL(blob);
        }
      },
      error: () => {
        this.profilePicture = 'assets/images/default-avatar.png';
      }
    });
  }
}
