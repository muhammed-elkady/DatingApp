import { AuthService } from './../../_services/auth.service';
import { environment } from './../../../environments/environment';

import { Photo } from './../../models/interfaces/photo';
import { Component, OnInit, Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() photos: Photo[];
  private baseUrl = environment.apiUrl;
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean = false;

  constructor(private authService: AuthService) { }

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
  }
}
