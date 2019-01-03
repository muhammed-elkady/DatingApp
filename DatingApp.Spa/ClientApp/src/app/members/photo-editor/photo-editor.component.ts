import { AlertifyService } from './../../_services/alertify.service';
import { UserService } from './../../_services/user.service';
import { AuthService } from './../../_services/auth.service';
import { environment } from './../../../environments/environment';

import { Photo } from './../../models/interfaces/photo';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Output() mainPhotoChanged = new EventEmitter<string>();
  @Input() photos: Photo[];
  private baseUrl = environment.apiUrl;
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean = false;

  constructor(private authService: AuthService, private userService: UserService, private alertifyService: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();

  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${this.baseUrl}/users/${this.authService.decodedToken.nameid}/photos`,
      authToken: `Bearer ${this.authService.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 10 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem =
      (item, response, status, headers) => {
        if (response) {
          const photo: Photo = JSON.parse(response) as Photo;
          this.photos.push(photo);
        }
      }
  }

  setMainPhoto(photo: Photo) {
    let userId = this.authService.decodedToken.nameid;
    this.userService.setMainPhoto(userId, photo.id)
      .subscribe(
        () => {
          let currentMain = this.photos.filter(c => c.isMain)[0]
          currentMain.isMain = false;
          photo.isMain = true;
          // this.mainPhotoChanged.emit(photo.url);
          this.authService.changeMemberPhoto(photo.url);
          this.authService.currentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
          this.alertifyService.success('Image were set to main');
        },
        err => this.alertifyService.error(err)
      );
  }

  deletePhoto(photoId: number) {
    let userId = this.authService.decodedToken.nameid;
    this.alertifyService.confirm('Are you sure you want to delete this photo?',
      () => this.userService.deletePhoto(userId, photoId)
        .subscribe(
          () => {
            let photoIndex = this.photos.findIndex(p => p.id === photoId);
            this.photos.splice(photoIndex, 1);
            this.alertifyService.success('Photo was deleted successfully!');
          },
          (err) => { this.alertifyService.error(err); }
        )
    );
  }



}
